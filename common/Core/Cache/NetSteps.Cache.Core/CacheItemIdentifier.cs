using System.Diagnostics.Contracts;

namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Abstract base class implementation of ICacheItemIdentifier&lt;I,K>
	/// </summary>
	/// <typeparam name="I">item type I</typeparam>
	/// <typeparam name="K">key type K</typeparam>
	public abstract class CacheItemIdentifier<I, K> : ICacheItemIdentifier<I, K>
	{
		/// <summary>
		/// Gets an item's identity key.
		/// </summary>
		/// <param name="item">the item</param>
		/// <returns>the item's key</returns>
		public K GetIdentityKey(I item)
		{
			Contract.Assert(item != null);
			return PeformGetIdentityKey(item);
		}

		/// <summary>
		/// Overriden by subclasses to get an item's identity key.
		/// </summary>
		/// <param name="item">the item</param>
		/// <returns>the item's key</returns>
		protected abstract K PeformGetIdentityKey(I item);
	}
}
