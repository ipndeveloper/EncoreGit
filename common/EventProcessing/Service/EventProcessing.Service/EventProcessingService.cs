using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NetSteps.Common.Extensions;
using NetSteps.Encore.Core.IoC;
using System.Threading;
using NetSteps.Encore.Core.Parallel;
using NetSteps.Encore.Core.Log;
using NetSteps.EventProcessing.Common;
using NetSteps.EventProcessing.Common.Data;
using System.Configuration;
using NetSteps.Encore.Core;
using NetSteps.EventProcessing.Common.Models;


namespace NetSteps.EventProcessing.Service
{
	[ContainerRegister(typeof(IEventProcessingService), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class EventProcessingService : Disposable, IEventProcessingService
	{
		private IEventRepository _eventRepository;
		private IEventTypeRepository _eventTypeRepository;
		private IRecurringEventRepository _recurringEventRepository;
		private static readonly ILogSink __log = typeof(EventProcessingService).GetLogSink();
		private IExceptionLogger _exceptionLogger;

		Semaphore _runTickets;
		int _currentlyExecutingTasks = 0;
		int _signaledToStop = 0;
		/// <summary>
		/// Indicates the amount of time to wait for a run-ticket before cycling
		/// through the logic.
		/// </summary>
		TimeSpan _busyPulseTimeout = TimeSpan.FromSeconds(5);
		/// <summary>
		/// Indicates the amount of time to idle when there are no events.
		/// </summary>
		TimeSpan _idlingTimeout = TimeSpan.FromSeconds(1);

		private int _numberToProcess;
		private Dictionary<int, IEventType> _eventTypes;

		private IEventTypeRegistry _eventTypeRegistry;

		public EventProcessingService(int numberOfThreads, int numberOfEventsToProcess, string assemblyFolders, bool useOnlyNamedFiles, string namedFiles)
		{
			Initialize(numberOfThreads, assemblyFolders, numberOfEventsToProcess, useOnlyNamedFiles, namedFiles);
		}

		public EventProcessingService()
		{
			string assemblyFolders = ConfigurationManager.AppSettings["assemblyFolders"];
			string numberOfThreads = ConfigurationManager.AppSettings["numberOfThreads"];
			string onlyProcessNamedFiles = ConfigurationManager.AppSettings["onlyProcessNamedFiles"] ?? "";
			string namedFiles = ConfigurationManager.AppSettings["namedFiles"] ?? "";
			bool useNamedFiles = onlyProcessNamedFiles.ToLower() == "true" || onlyProcessNamedFiles == "1";

			if (string.IsNullOrEmpty(numberOfThreads) && !numberOfThreads.IsNumberOnly(false))
			{
				throw new Exception("NumberOfThreads configuration value is incorrect. Need postive, non-zero integer.");
			}
			int numThreads = Convert.ToInt32(numberOfThreads);
			if (numThreads < 1)
			{
				throw new Exception("NumberOfThreads configuration value is incorrect. Need positive, non-zero integer.");
			}

			string numberOfEventsToProcess = ConfigurationManager.AppSettings["numberToProcess"];
			if (string.IsNullOrEmpty(numberOfEventsToProcess) && !numberOfEventsToProcess.IsNumberOnly(false))
			{
				throw new Exception("NumberOfEventsToProcess configuration value is incorrect. Must be positive, non-zero integer.");
			}
			int numToProcess = Convert.ToInt32(numberOfEventsToProcess);

			if (numToProcess < 1)
			{
				throw new Exception("NumberOfEventsToProcess configuration value is incorrect. Must be positive, non-zero integer.");
			}
			Initialize(numThreads, assemblyFolders, numToProcess, useNamedFiles, namedFiles);
		}


		private void Initialize(int numberOfThreads, string assemblyFolders, int numberOfEventsToProcess, bool onlyUseNamedFiles, string namedFiles)
		{
			_eventTypes = new Dictionary<int, IEventType>();

			_runTickets = new Semaphore(numberOfThreads, numberOfThreads);

			_numberToProcess = numberOfEventsToProcess;

			_eventTypeRegistry = Create.New<IEventTypeRegistry>();
			_eventRepository = Create.New<IEventRepository>();
			_eventTypeRepository = Create.New<IEventTypeRepository>();
			_exceptionLogger = Create.New<IExceptionLogger>();
			_recurringEventRepository = Create.New<IRecurringEventRepository>();

			populateServiceWithAssemblies(assemblyFolders, onlyUseNamedFiles ? namedFiles : null);
		}

		private void populateServiceWithAssemblies(string assemblyFolderString, string namedFiles)
		{
			var allAssemblyFolders = assemblyFolderString.Split(';');			
			string[] splitFiles = namedFiles == null ? new string[0] : namedFiles.Split(';');


			var assemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach(var assembly in assemblies)
			{
				_eventTypeRegistry.AddEventHandlerTypesFromAssembly(assembly);
			}
		}

		public void BeginMultiThreaded()
		{
			while (!StopSignaled)
			{
				Queue<Common.Models.IEvent> eventsToProcess = getEventQueue();
				Queue<IRecurringEvent> recurringEventsToProcess = getRecurringEventQueue();
				
				if ((recurringEventsToProcess == null || !recurringEventsToProcess.Any()) && (eventsToProcess == null || !eventsToProcess.Any()))
				{
					Thread.Sleep(_idlingTimeout);
					continue;
				}

				while (!StopSignaled && eventsToProcess != null && eventsToProcess.Any())
				{
					if (!_runTickets.WaitOne(_busyPulseTimeout))
					{
						continue;
					}

					// We got the semaphore, run one task...
					var currentEvent = eventsToProcess.Dequeue();

					var eventHandlerProcess = new EventHandlerProcess<IEvent>(incrementSemaphoreUsage, processEventAndReleaseSemaphore,
					                                                  currentEvent, decrementSemaphoreUsage);

					eventHandlerProcess.PreExecution.Invoke();
					eventHandlerProcess.QueueExecutionAndPostExecution();
				}

				while(!StopSignaled && recurringEventsToProcess != null && recurringEventsToProcess.Any())
				{
					if(!_runTickets.WaitOne(_busyPulseTimeout))
					{
						continue;
					}

					// We got the semaphore, run one task
					var currentEvent = recurringEventsToProcess.Dequeue();

					var eventHandlerProcess = new EventHandlerProcess<IRecurringEvent>(incrementSemaphoreUsage,
					                                                                   processRecurringEventAndReleaseSemaphore,
					                                                                   currentEvent, decrementSemaphoreUsage);

					eventHandlerProcess.PreExecution.Invoke();
					eventHandlerProcess.QueueExecutionAndPostExecution();
				}

				waitForThreadsToFinish();
			}
		}

		public Queue<IEvent> getEventQueue()
		{
			try
			{
				return new Queue<IEvent>(_eventRepository.GetTopEvents(_numberToProcess));
			}
			catch (Exception ex)
			{
				_exceptionLogger.LogException(ex);
				return null;
			}
		}

		public Queue<IRecurringEvent> getRecurringEventQueue()
		{
			try
			{
				return new Queue<IRecurringEvent>(_recurringEventRepository.GetActiveRecurringEvents(_numberToProcess));
			}
			catch (Exception ex)
			{
				_exceptionLogger.LogException(ex);
				return null;
			}
		}

		public void StopProcessing()
		{
			Interlocked.Increment(ref _signaledToStop);
			waitForThreadsToFinish();
		}

		private bool StopSignaled
		{
			get { return Thread.VolatileRead(ref _signaledToStop) > 0; }
		}

		private bool IsIdle
		{
			get { return Thread.VolatileRead(ref _currentlyExecutingTasks) == 0; }
		}

		private void processEventAndReleaseSemaphore(IEvent state)
		{
			var eventToProcess = state;
			try
			{
				var eventType = GetEventType(eventToProcess.EventTypeID);
				var result = ExecuteEventAssembly(eventType.EventHandler, eventToProcess.EventID);
				if (result == null)
				{
					throw new Exception(
                            String.Format("Unable to create eventType.EventTypeRegistry. Type not registered.  Attempted EventTypeID: [{0}]"
                                            , eventToProcess.EventTypeID));
				}

				if (result.IsFaulted || !result.Value)
				{
					eventToProcess.RetryCount++;
				}
				else
				{
					eventToProcess.CompletedDateUTC = DateTime.UtcNow;
				}
				_eventRepository.Save(eventToProcess);
			}
			catch (Exception ex)
			{
				_exceptionLogger.LogException(ex);
			}
		}

		private void processRecurringEventAndReleaseSemaphore(IRecurringEvent state)
		{
			var eventToProcess = state;
			try
			{
				var eventType = GetEventType(eventToProcess.EventTypeID);
				var result = ExecuteEventAssembly(eventType.EventHandler, eventToProcess.RecurringEventID);
				if(result == null)
				{
                    throw new Exception(
                            String.Format("Unable to create eventType.EventTypeRegistry. Type not registered.  Attempted EventTypeID: [{0}]"
                                            , eventToProcess.EventTypeID));
				}

				if (!result.IsFaulted && result.Value)
				{
					markRecurringEventAsCompleted(state);
				}
			}
			catch(Exception ex)
			{
				_exceptionLogger.LogException(ex);
			}
		}

		public void markRecurringEventAsCompleted(IRecurringEvent recurringEvent)
		{
			if (!recurringEvent.LastRunDateUTC.HasValue)
			{
				_recurringEventRepository.MarkRecurringEventAsCompleted(recurringEvent, DateTime.UtcNow);
				return;
			}

			var lastRunDate = getLastRunTimeThatSyncsWithInterval(recurringEvent.LastRunDateUTC.Value, recurringEvent.IntervalInMinutes, DateTime.UtcNow);
			_recurringEventRepository.MarkRecurringEventAsCompleted(recurringEvent, lastRunDate);
		}

		public DateTime getLastRunTimeThatSyncsWithInterval(DateTime lastRunTimeUTC, int intervalInMinutes, DateTime currentTimeUTC)
		{
			var timeSpan = currentTimeUTC - lastRunTimeUTC;
			int intervalsToProceed = (int)timeSpan.TotalMinutes / intervalInMinutes;
			return lastRunTimeUTC.AddMinutes(intervalInMinutes * intervalsToProceed);
		}


		public Future<bool> ExecuteEventAssembly(string typeName, int eventID)
		{
			var eventType = _eventTypeRegistry.GetType(typeName);
			if (eventType == null)
			{
				return null;
			}

			var handler = (IEventHandler)Activator.CreateInstance(eventType);
			var result = new Future<bool>();

			try
			{
				__log.Information("Started event");
				result.MarkCompleted(handler.Execute(eventID));
			}
			catch (Exception e)
			{
				_exceptionLogger.LogException(e);
				__log.Error("Unexpected exception: {0}: {1}", e.GetType(), e.Message);
				result.MarkFaulted(e);
			}

			return result;
		}


		private void waitForThreadsToFinish()
		{
			while (!IsIdle)
			{
				Thread.Sleep(1000);
			}
		}

		public IEventType GetEventType(int eventTypeID)
		{
			if (!_eventTypes.ContainsKey(eventTypeID))
			{
				lock (_eventTypes)
				{
					if (!_eventTypes.ContainsKey(eventTypeID))
					{
						_eventTypes.Add(eventTypeID, _eventTypeRepository.GetByEventTypeID(eventTypeID));
					}
				}
			}
			return _eventTypes[eventTypeID];
		}

		protected override bool PerformDispose(bool disposing)
		{
			Util.Dispose(ref _runTickets);
			return true;
		}

		private void incrementSemaphoreUsage()
		{
			Interlocked.Increment(ref _currentlyExecutingTasks);
		}

		private void decrementSemaphoreUsage()
		{
			Interlocked.Decrement(ref _currentlyExecutingTasks);
			_runTickets.Release();
		}
	}
}