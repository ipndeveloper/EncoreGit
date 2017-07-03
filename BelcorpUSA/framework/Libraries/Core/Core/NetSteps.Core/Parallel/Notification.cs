using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace NetSteps.Encore.Core.Parallel
{
	/// <summary>
	/// Utility for notify
	/// </summary>
	public sealed class Notification
	{
		internal struct NotifyRecord
		{
			public IAsyncResult Async;
			public Action Handler;
		}
		readonly Reactor<NotifyRecord> _invoker;
		readonly ConcurrentDictionary<int, Notifier> _notifiers = new ConcurrentDictionary<int, Notifier>();
		readonly ConcurrentQueue<Notifier> _availableNotifiers = new ConcurrentQueue<Notifier>();

		private Notification()
		{
			_invoker = new Reactor<NotifyRecord>((self, rec) => rec.Handler());
		}

		/// <summary>
		/// Schedules a continuation action for after an async result
		/// completes.
		/// </summary>
		/// <param name="async">the async result</param>
		/// <param name="continuation">the continuation</param>
		public void ContinueWith(IAsyncResult async, Action continuation)
		{
			var record = new NotifyRecord { Async = async, Handler = continuation };

			Notifier candidate;
			while (_availableNotifiers.TryDequeue(out candidate))
			{
				if (candidate.Add(record))
				{
					return;
				}
			}
			var notifier = new Notifier(this);
			_notifiers.TryAdd(notifier.ID, notifier);
			notifier.Add(record);
		}

		void MakeAvailable(Notifier notifier)
		{
			_availableNotifiers.Enqueue(notifier);
		}

		void PerformNotify(NotifyRecord item)
		{
			_invoker.Push(item);
		}

		internal class Notifier : Disposable
		{
			static readonly int CRecordsPerWaitPeriod = 63;
			static int __identityCount = 0;
			enum State
			{
				Initial = 0,
				Loading = 1,
				Waiting = 1 << 2,
				Notifying = 1 << 3,
				Disposing = 1 << 4,
			}
			readonly Status<State> _status = new Status<State>(State.Initial);
			readonly int _id;
			readonly Notification _owner;
			Thread _waiter;
			ManualResetEvent _signal;
			int _count = 0;
			readonly ConcurrentQueue<NotifyRecord> _incomming = new ConcurrentQueue<NotifyRecord>();

			internal Notifier(Notification owner)
			{
				_owner = owner;
				_id = Interlocked.Increment(ref __identityCount);
				_waiter = new Thread(ListenForSyncObjects);
				_waiter.Start();
				_signal = new ManualResetEvent(false);
			}

			internal int ID { get { return _id; } }

			internal bool Add(NotifyRecord record)
			{
				if (!IsDisposed && _status.IsLessThan(State.Disposing))
				{
					var count = Thread.VolatileRead(ref _count);
					if (count + _incomming.Count < CRecordsPerWaitPeriod)
					{ // appears to have room for more waithandles...
						var double_checked = Interlocked.CompareExchange(ref _count, count + 1, count);
						if (double_checked == count)
						{ // we won the potential race-condition, truly has room...
							_incomming.Enqueue(record);
							_signal.Set();
							if (!IsFull)
							{
								_owner.MakeAvailable(this);
							}
						}
					}
				}
				return false;
			}

			internal bool IsFull { get { return Thread.VolatileRead(ref _count) == CRecordsPerWaitPeriod; } }

			void ListenForSyncObjects()
			{
				const int wake = 0;
				const int offset = -1;

				var records = new List<NotifyRecord>();
				var handles = new List<WaitHandle>();
				handles.Add(_signal);

				while (_status.SetStateIfLessThan(State.Loading, State.Disposing))
				{
					NotifyRecord item;
					while (records.Count < CRecordsPerWaitPeriod
						&& _incomming.TryDequeue(out item))
					{
						records.Add(item);
						handles.Add(item.Async.AsyncWaitHandle);
					}

					Thread.VolatileWrite(ref _count, handles.Count);
					var waiting = handles.ToArray();
					if (_status.SetStateIfLessThan(State.Waiting, State.Disposing))
					{
						int signaled = WaitHandle.WaitAny(waiting);

						if (signaled > wake)
						{
							_status.SetStateIfLessThan(State.Notifying, State.Disposing);
							handles.RemoveAt(signaled);
							_owner.PerformNotify(records[signaled + offset]);
							records.RemoveAt(signaled + offset);
						}
						else
						{
							_signal.Reset();
						}
					}
				}
			}

			protected override bool PerformDispose(bool disposing)
			{
				if (_waiter != null && _waiter.IsAlive)
				{
					_status.SetStateIfLessThan(State.Disposing, State.Disposing);
					_signal.Set();
					_waiter.Join();
					Util.Dispose(ref _signal);
					Util.Dispose(ref _waiter);
				}
				return true;
			}
		}
	}
}
