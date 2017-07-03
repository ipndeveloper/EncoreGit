using System.Collections.Generic;
using System.Linq;
using System;

namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Cache item resolver that delegates the resolve to a delegate.
	/// </summary>
	/// <typeparam name="K">key type K</typeparam>
	/// <typeparam name="R">representation type R</typeparam>
	public sealed class DelegatedDemuxCacheManyItemResolver<K, R> : DemuxCacheManyItemResolver<K, R>
	{
		readonly CacheItemResolverDelegate<K, R> _delegate;
		readonly CacheItemResolverDelegate<IEnumerable<K>, IEnumerable<KeyValuePair<K, R>>> _manyDelegate;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="dlg">a resolver delegate</param>
		/// <param name="manyDlg">a many resolver delegate</param>
		public DelegatedDemuxCacheManyItemResolver(CacheItemResolverDelegate<K, R> dlg, CacheItemResolverDelegate<IEnumerable<K>, IEnumerable<KeyValuePair<K, R>>> manyDlg)
		{
			_delegate = dlg;
			_manyDelegate = manyDlg;
		}

		/// <summary>
		/// Performs the resolve.
		/// </summary>
		/// <param name="keys"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		protected override bool DemultiplexedTryResolveMany(IEnumerable<K> keys, out IEnumerable<KeyValuePair<K, R>> values)
		{
			if (_manyDelegate != null)
				return _manyDelegate(keys, out values);
			else
			{
				List<KeyValuePair<K, R>> results = new List<KeyValuePair<K, R>>();
				R item;
				foreach (var k in keys)
					if (_delegate(k, out item)) results.Add(new KeyValuePair<K, R>(k, item));

				values = results;
				return keys.Count() == results.Count;
			}
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
