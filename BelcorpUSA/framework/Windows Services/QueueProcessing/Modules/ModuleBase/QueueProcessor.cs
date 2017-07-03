namespace NetSteps.QueueProcessing.Modules.ModuleBase
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using NetSteps.Encore.Core.IoC;
    using NetSteps.QueueProcessing.Common;

    #region QueueSyncEvents
    public class QueueSyncEvents
    {
        private EventWaitHandle _itemsExistInQueueEvent;
        public EventWaitHandle ItemsExistInQueueEvent { get { return this._itemsExistInQueueEvent; } }

        private EventWaitHandle _exitThreadEvent;
        public EventWaitHandle ExitThreadEvent { get { return this._exitThreadEvent; } }

        private EventWaitHandle _enqueueEvent;
        public EventWaitHandle EnqueueEvent { get { return this._enqueueEvent; } }

        private WaitHandle[] _exitOrItemExistEvents;
        public WaitHandle[] ExitOrItemExistEvents { get { return this._exitOrItemExistEvents; } }

        private WaitHandle[] _exitOrEnqueueEvents;
        public WaitHandle[] ExitOrEnqueueEvents { get { return this._exitOrEnqueueEvents; } }

        public QueueSyncEvents()
        {
            this._itemsExistInQueueEvent = new ManualResetEvent(false);
            this._exitThreadEvent = new ManualResetEvent(false);
            this._enqueueEvent = new AutoResetEvent(true); //this is set to true so that we get an initial poll without waiting for the interval

            this._exitOrItemExistEvents = new WaitHandle[2];
            this._exitOrItemExistEvents[0] = this._exitThreadEvent; // the order is important. When WaitAny runs it will return the index of the first item signaled.
            // because we exit when the ExitThreadEvent is signaled, be need to get that index back first if it
            // is signaled, therefore it needs to be first in the array.
            this._exitOrItemExistEvents[1] = this._itemsExistInQueueEvent;

            this._exitOrEnqueueEvents = new WaitHandle[2];
            this._exitOrEnqueueEvents[0] = this._exitThreadEvent;
            this._exitOrEnqueueEvents[1] = this._enqueueEvent;
        }

    }
    #endregion QueueSyncEvents

    public class Enqueuer<T>
    {
        IQueueProcessorLogger _logger;

        private Queue<T> queue;
        private QueueSyncEvents syncEvents;
        private Action<int> createQueueItems;
        private int pollingIntervalMs;
        private int maxNumberToPoll;

        public Enqueuer(Queue<T> queue, QueueSyncEvents syncEvents, Action<int> createQueueItems, int pollingIntervalMs, int maxNumberToPoll)
        {
            this._logger = Create.New<IQueueProcessorLogger>();

            this.queue = queue;
            this.syncEvents = syncEvents;
            this.createQueueItems = createQueueItems;
            this.pollingIntervalMs = pollingIntervalMs;
            this.maxNumberToPoll = maxNumberToPoll;
        }

        public void ThreadRun()
        {
            // loop until the ExitThreadEvent is signaled
            while (WaitHandle.WaitAny(this.syncEvents.ExitOrEnqueueEvents, this.pollingIntervalMs) != 0) // 0 means we received an exit event
            {
                // all threads need a try catch, if a thread throws and exception it terminates the program
                try
                {
                    int queueCount = 0;

                    lock (this.queue)
                    {
                        queueCount = this.queue.Count;
                    }

                    // This makes sure that if the queue doesn't just grow endlessly by polling continuously when the items aren't being serviced fast enough.
                    // If the queue is bigger than the maxNumberToPoll then just skip and wait for another interval or the EqueueEvent to signal.
                    if (queueCount < this.maxNumberToPoll)
                        this.createQueueItems(this.maxNumberToPoll); // passed in as delegate in the constructor
                    else
                        this._logger.Debug("Too many items in queue, not polling: {0}", queueCount);
                }
                catch (Exception e)
                {
                    this._logger.Error("Error in Enquerer thread, probably callingCreateQueueItems: {0}", e.Message);
                }
            }
        }
    }

    public class QueueItemWorkerThread<T>
    {
        IQueueProcessorLogger _logger;

        protected Queue<T> queue;
        private QueueSyncEvents syncEvents;
        private Action<T> processQueueItem;


        public QueueItemWorkerThread(Queue<T> queue, QueueSyncEvents syncEvents, Action<T> processQueueItem)
        {
            this._logger = Create.New<IQueueProcessorLogger>();

            this.queue = queue;
            this.syncEvents = syncEvents;
            this.processQueueItem = processQueueItem;
        }

        public void ThreadRun()
        {
            // loop when an item exists, exit if an ExitEvent is signaled ( != 1)
            while (WaitHandle.WaitAny(this.syncEvents.ExitOrItemExistEvents) != 0) // 0 means we received an exit event
            {
                // all threads need a try catch, if a thread throws and exception it terminates the program
                try
                {
                    // I thought of using null as the indicator that nothing was found, but I don't know if the datatype T will support null (like int)
                    // so I went with the surefire approach
                    bool itemFound;
                    T item = this.DequeueItem(out itemFound);

                    // because multiple threads are all waiting for a signal that something exists, all threads will release simultaneously 
                    // to try and service the queue. If there are not enough items on the queue for all threads, there may not be an item returned
                    // so we just loop back to wait for another signal that there is something to do
                    if (itemFound)
                    {
                        this.processQueueItem(item); // passed in as delegate in the constructor
                    }
                }
                catch (Exception e)
                {
                    this._logger.Error("Error in QueueItemWorkerThread thread, probably processQueueItem, {0}", e);
                }
            }
        }

        public T DequeueItem(out bool itemFound)
        {
            itemFound = false;

            lock (this.queue)
            {
                if (this.queue.Count == 0)
                    return default(T); // should be a null unless it is a value type param

                T item = this.queue.Dequeue();
                itemFound = true;

                // if the queue is now empty reset the event so threads will wait until there is something to process
                if (this.queue.Count == 0)
                {
                    // This unsignals the event that says there are items waiting for processing. This will allow the worker threads to wait
                    // until something new is added to the queue. It prevents the workers from spinning (repeatadly polling) for work where there isn't any
                    this.syncEvents.ItemsExistInQueueEvent.Reset();

                    // because the last item was just grabbed off the queue, immediatly signal the enqueuer thread to try and fetch more work
                    // this helps keep the queue running as fast as possible by grabbing more work as soon as it finishes it's list.
                    this._logger.Debug("Queue Empty, signaling poller");
                    this.syncEvents.EnqueueEvent.Set();
                }

                return item;
            }
        }
    }

    public abstract class QueueProcessor<T> : IQueueProcessor
    {
        private Thread enqueuerThread = null;
        private Thread[] queueItemWorkerThreads = null;
        QueueSyncEvents syncEvents = null;
        Queue<T> queue = null;

        protected IQueueProcessorLogger Logger { get; private set; }

        protected int _workerThreads = 1;
        protected int _pollingIntervalMs = 5000;
        protected int _maxNumberToPoll = 2000;

        public QueueProcessor()
        {
            this.Logger = Create.New<IQueueProcessorLogger>();
        }

        public string Name { get; set; }

        public bool IsRunning { get; set; }

        public int WorkerThreads
        {
            get
            {
                return this._workerThreads;
            }
            set
            {
                this._workerThreads = value;
            }
        }

        // delay between polls. As soon as the queue becomes empty it immediately polls without waiting for the interval to elapse
        public int PollingIntervalMs
        {
            get
            {
                return this._pollingIntervalMs;
            }
            set
            {
                this._pollingIntervalMs = value;
            }
        }

        public int MaxNumberToPoll
        {
            get
            {
                return this._maxNumberToPoll;
            }
            set
            {
                this._maxNumberToPoll = value;
            }
        }

        public void Start()
        {
            this.IsRunning = true;
            this.queue = new Queue<T>();
            this.syncEvents = new QueueSyncEvents();

            // Creating Enqueuer thread
			Enqueuer<T> enqueuer = new Enqueuer<T>(this.queue, this.syncEvents, this.FillQueue, this.PollingIntervalMs, this.MaxNumberToPoll); 
			this.enqueuerThread = new Thread(enqueuer.ThreadRun);
            this.enqueuerThread.Start();

            // Creating worker threads
            this.queueItemWorkerThreads = new Thread[this.WorkerThreads];

            for (int i = 0; i < this.WorkerThreads; i++)
            {
                QueueItemWorkerThread<T> queueItemWorkerThread = new QueueItemWorkerThread<T>(this.queue, this.syncEvents, this.ProcessQueueItem);
                this.queueItemWorkerThreads[i] = new Thread(queueItemWorkerThread.ThreadRun);
                this.queueItemWorkerThreads[i].Start();
            }
        }

        public void Stop()
        {
            this.IsRunning = false;
            this.Logger.Debug("QueueProcessor.Stop(), waiting on child threads to terminate");

            // Signaling threads to terminate
            if (this.syncEvents != null)
                this.syncEvents.ExitThreadEvent.Set();

            // Main thread waiting for all child threads to terminate (join makes the current thread wait until the child thread terminates)
            if (this.enqueuerThread != null)
                this.enqueuerThread.Join();

            if (this.queueItemWorkerThreads != null)
            {
                foreach (Thread thread in this.queueItemWorkerThreads)
                    thread.Join();
            }

            this.Logger.Debug("QueueProcessor.Stop(), child threads terminated");
        }

		public void FillQueue(int maxNumberToPoll)
		{
			int queueCount = this.queue.Count;
			int count = Math.Max(0, maxNumberToPoll - queueCount);

			if (count > 0)
			{
				this.Logger.Info("Attempting to add {0} items to the queue", count);
				this.CreateQueueItems(count);
			}
			else
			{
				this.Logger.Info("Queue is full, not adding additional items");
			}
		}

        /// <summary>
        /// Implement these method to create/process queue items. This is where you would check (i.e. poll) the database
        /// for items and then Enqueue them. Call EnqueueItem to add items to the queue.
        /// </summary>
        public abstract void CreateQueueItems(int maxNumberToPoll);

        // I debated for a while if I should make the processing of a queue item a method of the pooler or a method of the queueitem itself.
        // If I made the queue item implement a IQueueItem interface with a Process method, then you could queue items of multiple types
        // and each item would know how to process itself.
        //
        // I eventually decided to go this direction because it made the implementation a little easier to follow, but also I couldn't justify
        // a use case where we would have a single queue populate that added items of different types. It seemed that the better story was
        // that you should create a different queueprocessor for each type, so that you didn't enqueue multiple items in a single serialized 
        // populater. I can see reasons for putting it on the queueitem but the queue based processor seemed more likely.
        public abstract void ProcessQueueItem(T item);

        public void EnqueueItem(T item)
        {
            lock (this.queue)
            {
                this.queue.Enqueue(item);
				this.Logger.Debug("Enqueued item - total item count: {0}", this.queue.Count);

                // Signal that an item exists in queue, This will instantly free all waiting threads to work on items
                // this signal must be done within the lock so that if a thread is waiting to dequeue they don't get the item and clear the signal
                // and then this set a signal again
                this.syncEvents.ItemsExistInQueueEvent.Set();
            }
        }
    }
}
