using System;
using System.Collections.Generic;
using NetSteps.Encore.Core.Parallel;
using NetSteps.Encore.Core;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Interface for cache eviction monitors.
	/// </summary>
	public interface ICacheEvictionMonitor : IDisposable
	{
		/// <summary>
		/// Gets the monitor's registration key.
		/// </summary>
		Guid RegistrationKey { get; }
		/// <summary>
		/// Gets the context keys identifying what the monitor is 
		/// monitoring.
		/// </summary>
		IEnumerable<string> ContextKeys { get; }
		/// <summary>
		/// Invoked by the framework when cache evictions arrive.
		/// </summary>
		/// <param name="contextKey">a context key</param>
		/// <param name="evictionInfo">the eviction info</param>
		void OnEvictionNotification(string contextKey, object evictionInfo);
	}

	/// <summary>
	/// Default eviction monitor implementation.
	/// </summary>
	/// <typeparam name="K">key type K</typeparam>
	public class ReactiveCacheEvictionMonitor<K> : Disposable, ICacheEvictionMonitor
	{
		readonly Reactor<K> _reactor;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="contextKeys">context keys to be monitored</param>
		/// <param name="reactor">reactor method that recieves eviction notification</param>
		public ReactiveCacheEvictionMonitor(IEnumerable<string> contextKeys, Action<Reactor<K>, K> reactor)
		{
			_reactor = new Reactor<K>(reactor);
			RegistrationKey = Guid.NewGuid();
			ContextKeys = contextKeys.ToReadOnly();
		}

		/// <summary>
		/// Gets the monitor's registration key.
		/// </summary>
		public Guid RegistrationKey { get; private set; }
		/// <summary>
		/// Gets the context keys identifying what the monitor is 
		/// monitoring.
		/// </summary>
		public IEnumerable<string> ContextKeys { get; private set; }

		/// <summary>
		/// Invoked by the framework when cache evictions arrive.
		/// </summary>
		/// <param name="contextKey">a context key</param>
		/// <param name="evictionInfo">the eviction info</param>		
		public void OnEvictionNotification(string contextKey, object evictionInfo)
		{
			if (contextKey == null) return;

			var keys = evictionInfo as IEnumerable<K>;
			if (keys != null)
			{
				foreach (var k in keys)
				{
					_reactor.Push(k);
				}
			}
			else if (evictionInfo is K)
			{
				_reactor.Push((K)evictionInfo);
			}
			else
			{
				HandleUnrecognizedEvictionInfo(contextKey, evictionInfo);
			}
		}

		/// <summary>
		/// Invoked when unrecognized eviction info is received.
		/// </summary>
		/// <param name="contextKey"></param>
		/// <param name="evictionInfo"></param>
		protected virtual void HandleUnrecognizedEvictionInfo(string contextKey, object evictionInfo)
		{
		}

		/// <summary>
		/// Performs disposal for the instance.
		/// </summary>
		/// <param name="disposing"></param>
		/// <returns></returns>
		protected override bool PerformDispose(bool disposing)
		{
			_reactor.Cancel();
			return disposing;
		}
	}

}
