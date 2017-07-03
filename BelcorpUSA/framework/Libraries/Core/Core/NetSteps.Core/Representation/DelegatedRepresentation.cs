using System;
using System.Diagnostics.Contracts;


namespace NetSteps.Encore.Core.Representation
{
	/// <summary>
	/// Delegated representation transform. Transforms target type T into representation R, and upon
	/// restore, restores type C.
	/// 
	/// </summary>
	/// <typeparam name="T">delegated target type T</typeparam>
	/// <typeparam name="C">target type C</typeparam>
	/// <typeparam name="R">representation type R</typeparam>
	public class DelegatedRepresentation<T, C, R> : IRepresentation<T, R>
		where C: class, T
	{
		readonly IRepresentation<C, R> _transform;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="transform">delegate target transform for type C to R</param>
		public DelegatedRepresentation(IRepresentation<C, R> transform)
		{
			Contract.Requires<ArgumentNullException>(transform != null);

			_transform = transform;
		}

		/// <summary>
		/// Produces representation type R from an item.
		/// </summary>
		/// <param name="item">the item</param>
		/// <returns>a representation of the item</returns>
		public R TransformItem(T item)
		{
			return _transform.TransformItem(item as C);
		}
		/// <summary>
		/// Restores an item from a representation
		/// </summary>
		/// <param name="representation">the representation</param>
		/// <returns>the restored item</returns>
		public T RestoreItem(R representation)
		{
			return _transform.RestoreItem(representation);
		}
	}
}
