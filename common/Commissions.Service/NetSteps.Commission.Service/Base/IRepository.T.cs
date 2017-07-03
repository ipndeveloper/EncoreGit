using System.Collections.Generic;

namespace NetSteps.Commissions.Service.Base
{
	/// <summary>
	/// Base repository
	/// </summary>
	/// <typeparam name="TObject">The type of the entity.</typeparam>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	public interface IRepository<TObject, in TKey> 
	{
		/// <summary>
		/// Fetches all.
		/// </summary>
		/// <returns></returns>
		IList<TObject> FetchAll();
		
        /// <summary>
		/// Fetches 1 by id.
		/// </summary>
		/// <returns></returns>
		TObject Fetch(TKey id);

		/// <summary>
		/// Fetches 1..n by id.
		/// </summary>
		/// <returns></returns>
		IEnumerable<TObject> Fetch(IEnumerable<TKey> ids);
		
        /// <summary>
		/// Adds the specified object.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns></returns>
		TObject Add(TObject obj);

        /// <summary>
		/// Updates the specified object.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns></returns>
		TObject Update(TObject obj);

        /// <summary>
        /// Updates the object or adds it if it doesn't exist
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns></returns>
	    TObject AddOrUpdate(TObject obj);
		
        /// <summary>
		/// Deletes the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		bool Delete(TKey id);
	}
}
