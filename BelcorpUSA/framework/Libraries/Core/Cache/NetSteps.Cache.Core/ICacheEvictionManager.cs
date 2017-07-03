using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Net;
using System.Threading;
using NetSteps.Encore.Core.Buffers;
using NetSteps.Core.Cache.CacheNet;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Log;
using NetSteps.Encore.Core.Net;
using NetSteps.Encore.Core.Parallel;
using NetSteps.Encore.Core.Reflection;
using NetSteps.Encore.Core;

namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Interface for cache eviction managers.
	/// </summary>
	public interface ICacheEvictionManager
	{
		/// <summary>
		/// Adds an eviction monitor.
		/// </summary>
		/// <param name="monitor">the monitor</param>
		void AddEvictionMonitor(ICacheEvictionMonitor monitor);
		/// <summary>
		/// Removes an eviction monitor.
		/// </summary>
		/// <param name="key">the monitor's key</param>
		void RemoveEvictionMonitor(Guid key);

		/// <summary>
		/// Adds an eviction callback to be run on then next
		/// eviction cycle.
		/// </summary>
		/// <param name="callback">Callback function taking a boolean
		/// and returning a boolean. The parameter indicates whether
		/// the call is synchrounous, the result indicates whether
		/// the callback should be rescheduled.
		/// </param>
		void AddEvictionCallback(Func<bool, bool> callback);
	}

	/// <summary>
	/// Default eviction manager implementation.
	/// </summary>
	[ContainerRegister(typeof(ICacheEvictionManager), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class CacheEvictionManager : Disposable, ICacheEvictionManager
	{
		static readonly ILogSink __log = typeof(CacheEvictionManager).GetLogSink();
		static readonly int DefaultMaxConcurrentEvictionWorkers = Math.Max(Environment.ProcessorCount / 2, 1);

		class MonitorRegistration
		{
			public bool Removed { get; set; }
			public ICacheEvictionMonitor Monitor { get; set; }
		}

		readonly ConcurrentDictionary<Guid, MonitorRegistration> _monitorRegistrations = new ConcurrentDictionary<Guid, MonitorRegistration>();
		readonly ConcurrentDictionary<string, ConcurrentBag<MonitorRegistration>> _monitors = new ConcurrentDictionary<string, ConcurrentBag<MonitorRegistration>>();
		readonly ConcurrentQueue<Func<bool, bool>> _callbacks = new ConcurrentQueue<Func<bool, bool>>();
		readonly PerformanceCounter __evictionWorkers = new PerformanceCounter(CacheStats.CPerformanceCounterCategory, CacheStats.CPerformanceCounterEvictionWorkers, String.Empty, false);
		readonly Reactor<CacheNetMessage> _notificationReactor;
		readonly int _maxConcurrentEvictionWorkers;

		CacheNetProtocol _protocol;
		UdpProtocolEndpoint<CacheNetMessage> _endpoint;

		int _evictionWorkers = 0;
		int _evictionWorkersExecuting = 0;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public CacheEvictionManager()
			: this(DefaultMaxConcurrentEvictionWorkers, null)
		{
		}

		/// <summary>
		/// Creates a new instance with a maximum number of eviction workers, on the
		/// endpoint given.
		/// </summary>
		/// <param name="maxConcurrentEvictionWorkers"></param>
		/// <param name="endpoint"></param>
		public CacheEvictionManager(int maxConcurrentEvictionWorkers, IPEndPoint endpoint)
		{
			_maxConcurrentEvictionWorkers = maxConcurrentEvictionWorkers;
			_notificationReactor = new Reactor<CacheNetMessage>(this.ReactorCallback);
			if (endpoint != null)
			{
				_protocol = new CacheNetProtocol(this);
				_endpoint = new UdpProtocolEndpoint<CacheNetMessage>(_protocol).ParallelReceive(endpoint);
			}
		}

		/// <summary>
		/// Adds an eviction monitor.
		/// </summary>
		/// <param name="monitor">the monitor</param>
		public void AddEvictionMonitor(ICacheEvictionMonitor monitor)
		{
			Contract.Assert(monitor != null);

			var key = monitor.RegistrationKey;
			var reg = new MonitorRegistration { Monitor = monitor };
			if (_monitorRegistrations.TryAdd(key, reg))
			{
				foreach (var ctx in monitor.ContextKeys)
				{
					var bag = _monitors.GetOrAdd(ctx, c => new ConcurrentBag<MonitorRegistration>());
					bag.Add(reg);
				}
			}
		}

		/// <summary>
		/// Removes an eviction monitor.
		/// </summary>
		/// <param name="key">the monitor's key</param>
		public void RemoveEvictionMonitor(Guid key)
		{
			MonitorRegistration reg;
			if (_monitorRegistrations.TryRemove(key, out reg))
			{
				reg.Removed = true;
			}
		}

		/// <summary>
		/// Adds an eviction callback to be run on then next
		/// eviction cycle.
		/// </summary>
		/// <param name="callback"></param>
		public void AddEvictionCallback(Func<bool, bool> callback)
		{
			Contract.Assert(callback != null);

			_callbacks.Enqueue(callback);
			if (Thread.VolatileRead(ref _evictionWorkers) < _maxConcurrentEvictionWorkers)
			{
				ThreadPool.QueueUserWorkItem(Background_ProcessEvictions, null);
				Interlocked.Increment(ref _evictionWorkers);
				__evictionWorkers.Increment();
			}
		}

		string What
		{
			get
			{
				return Util.NonBlockingLazyInitializeVolatile(ref _what, () => this.GetType().GetReadableFullName());
			}
		}
		string _what;

		void Background_ProcessEvictions(object state)
		{
			try
			{
				if (__log.IsLogging(SourceLevels.Verbose))
				{
					__log.Verbose(String.Concat(this.What, ".Background_ProcessEvictions: worker started"));
				}
				Interlocked.Increment(ref _evictionWorkersExecuting);
				Func<bool, bool> callback;
				while (_callbacks.TryDequeue(out callback))
				{
					if (callback(false))
					{
						_callbacks.Enqueue(callback);
					}
				}
			}
			finally
			{
				Interlocked.Decrement(ref _evictionWorkers);
				Interlocked.Decrement(ref _evictionWorkersExecuting);
				__evictionWorkers.Decrement();
				if (__log.IsLogging(SourceLevels.Verbose))
				{
					__log.Verbose(String.Concat(this.What, ".Background_ProcessEvictions: worker done"));
				}
			}
		}

		internal void OnEvictionNotification(CacheNetMessage message)
		{
			_notificationReactor.Push(message);
		}
		void ReactorCallback(Reactor<CacheNetMessage> reactor, CacheNetMessage info)
		{
			foreach (var ctx in info.ContextKeys)
			{
				ConcurrentBag<MonitorRegistration> registrations;
				if (_monitors.TryGetValue(ctx, out registrations))
				{
					foreach (var reg in registrations)
					{
						if (!reg.Removed)
						{
							reg.Monitor.OnEvictionNotification(ctx, info);
						}
					}
				}
			}
		}

		/// <summary>
		/// Performs the instance's disposal.
		/// </summary>
		/// <param name="disposing"></param>
		/// <returns></returns>
		protected override bool PerformDispose(bool disposing)
		{
			Util.Dispose(ref _endpoint);
			return disposing;
		}
	}

}
