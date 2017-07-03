using NetSteps.Encore.Core.Parallel;

namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Implementation of the ICacheItemResolver interface that demultiplexes
	/// resolution requests.
	/// </summary>
	/// <typeparam name="K">key type K</typeparam>
	/// <typeparam name="R">representation type R</typeparam>
	public abstract class DemuxCacheItemResolver<K, R> : CacheItemResolver<K, R>
	{
		internal class CacheDemuxProducer : DemuxProducer<K, R>
		{
			DemuxCacheItemResolver<K, R> _parent;

			public CacheDemuxProducer(DemuxCacheItemResolver<K, R> parent)
			{
				_parent = parent;
			}

			protected override bool ProduceResult(K arg, out R value)
			{
				return _parent.DemultiplexedTryResolve(arg, out value);
			}
		}

		CacheDemuxProducer _producer;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public DemuxCacheItemResolver()
		{
			_producer = new CacheDemuxProducer(this);
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
	}
}
