using System;
using System.Diagnostics.Contracts;

namespace NetSteps.Core.Cache
{
	/// <summary>
	/// A delegate for resolving cache items.
	/// </summary>
	/// <typeparam name="K">key type K</typeparam>
	/// <typeparam name="R">representation type R</typeparam>
	/// <param name="key">the key</param>
	/// <param name="value">a reference to a variable that will receive the value upon success.</param>
	/// <returns><em>true</em> if successful; otherwise <em>false</em>.</returns>
	public delegate bool CacheItemResolverDelegate<K, R>(K key, out R value);

	/// <summary>
	/// Cache item resolver that delegates the resolve to a delegate.
	/// </summary>
	/// <typeparam name="K">key type K</typeparam>
	/// <typeparam name="R">representation type R</typeparam>
	public sealed class DelegatedDemuxCacheItemResolver<K, R> : DemuxCacheItemResolver<K, R>
	{
		readonly CacheItemResolverDelegate<K, R> _delegate;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="dlg">a resolver delegate</param>
		public DelegatedDemuxCacheItemResolver(CacheItemResolverDelegate<K, R> dlg)
		{
			Contract.Requires<ArgumentNullException>(dlg != null);

			_delegate = dlg;
		}

		/// <summary>
		/// Performs the resolve.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected override bool DemultiplexedTryResolve(K key, out R value)
		{
			return _delegate(key, out value);
		}
	}
}
