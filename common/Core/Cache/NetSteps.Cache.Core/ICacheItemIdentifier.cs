
namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Gets an identity key from a cache item. An identity key
	/// uniquely identifies an item among items of the same type.
	/// </summary>
	/// <typeparam name="I">item type I</typeparam>
	/// <typeparam name="K">key type K</typeparam>
	public interface ICacheItemIdentifier<I, K>
	{
		/// <summary>
		/// Gets an item's identity key.
		/// </summary>
		/// <param name="item">the item</param>
		/// <returns>the item's key</returns>
		K GetIdentityKey(I item);
	}
}
