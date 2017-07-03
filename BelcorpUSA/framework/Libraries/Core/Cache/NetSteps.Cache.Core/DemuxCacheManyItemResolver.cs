using System;
using System.Linq;
using System.Collections.Generic;

using NetSteps.Encore.Core.Parallel;


namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Implementation of the ICacheManyItemResolver interface that demultiplexes
	/// resolution requests.
	/// </summary>
	/// <typeparam name="K">key type K</typeparam>
	/// <typeparam name="R">representation type R</typeparam>
	public abstract class DemuxCacheManyItemResolver<K, R> : CacheManyItemResolver<K, R>
	{
		internal class CacheDemuxProducer : DemuxProducer<K, R>
		{
			DemuxCacheManyItemResolver<K, R> _parent;

			public CacheDemuxProducer(DemuxCacheManyItemResolver<K, R> parent)
			{
				_parent = parent;
			}

			protected override bool ProduceResult(K arg, out R value)
			{
				return _parent.DemultiplexedTryResolve(arg, out value);
			}
		}

		class CacheDemuxManyProducer : DemuxProducer<IEnumerable<K>, IEnumerable<KeyValuePair<K, R>>>
		{
			DemuxCacheManyItemResolver<K, R> _parent;

			public CacheDemuxManyProducer(DemuxCacheManyItemResolver<K, R> parent)
			{
				_parent = parent;
			}

			protected override bool ProduceResult(IEnumerable<K> arg, out IEnumerable<KeyValuePair<K, R>> value)
			{
				return _parent.DemultiplexedTryResolveMany(arg, out value);
			}
		}

		CacheDemuxProducer _producer;
		CacheDemuxManyProducer _manyProducer;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public DemuxCacheManyItemResolver()
		{
			_producer = new CacheDemuxProducer(this);
			_manyProducer = new CacheDemuxManyProducer(this);
		}

		/// <summary>
		/// Overridden to demultiplex resolve attempts by multiple threads.
		/// If requests are received for the same key within a request period,
		/// all requests wait for the first caller to succeed and get the result
		/// multiplexed back to each concurrent caller.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="value">variable to hold the item upon success</param>
		/// <returns>the resolution kind</returns>
		/// <see cref="ResolutionKind"/>
		protected override sealed ResolutionKind PerformTryResolve(K key, out R value)
		{
			var kind = _producer.TryConsume(key, out value);
			// Rely on the fact that DemuxResultKind and ResolutionKind have the same values:
			return (ResolutionKind)(int)kind;
		}

		/// <summary>
		/// Tries to resolve an item's representation.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="value">variable to hold the item's value upon success</param>
		/// <returns>true if successful; otherwise false</returns>
		protected abstract bool DemultiplexedTryResolve(K key, out R value);

		/// <summary>
		/// Overridden to demultiplex resolve attempts by multiple threads.
		/// If requests are received for the same key within a request period,
		/// all requests wait for the first caller to succeed and get the result
		/// multiplexed back to each concurrent caller.
		/// </summary>
		/// <param name="keys">the item's key</param>
		/// <param name="values">variable to hold the item upon success</param>
		/// <returns>the resolution kind</returns>
		/// <see cref="ResolutionManyKind"/>
		protected override ResolutionManyKind PerformTryResolveMany(IEnumerable<K> keys, out IEnumerable<KeyValuePair<K, R>> values)
		{
			ResolutionManyKind kind = (ResolutionManyKind)(int)_manyProducer.TryConsume(keys, out values);
			if (!kind.HasFlag(ResolutionManyKind.Resolved) && values != null && values.Any())
			{
				kind = ResolutionManyKind.PartiallyResolved;
			}
			return kind;
		}

		/// <summary>
		/// Tries to resolve many items representation.
		/// </summary>
		/// <param name="keys">the items keys</param>
		/// <param name="values">variable to hold the item values upon success</param>
		/// <returns>true if successful; otherwise false</returns>
		protected abstract bool DemultiplexedTryResolveMany(IEnumerable<K> keys, out IEnumerable<KeyValuePair<K, R>> values);
	}
}
