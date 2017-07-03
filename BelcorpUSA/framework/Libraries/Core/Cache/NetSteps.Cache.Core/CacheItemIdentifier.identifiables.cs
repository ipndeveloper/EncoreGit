
namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Gets the identity key from identifiable items.
	/// </summary>
	/// <typeparam name="I">item type I</typeparam>
	/// <typeparam name="IK">key type K</typeparam>
	public abstract class IdentifiableCacheItemIdentifier<I, IK> : CacheItemIdentifier<I, IK>
		where I: IIdentifiable<IK>
	{
		/// <summary>
		/// Gets the identifiable item's identity key.
		/// </summary>
		/// <param name="item">the item</param>
		/// <returns>the item's key</returns>
		protected override IK PeformGetIdentityKey(I item)
		{
			return item.GetIdentity();
		}
	}
}
