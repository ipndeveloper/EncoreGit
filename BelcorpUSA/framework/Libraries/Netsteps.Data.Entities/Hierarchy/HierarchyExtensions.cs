using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

namespace NetSteps.Data.Entities.Hierarchy
{
	/// <summary>
	/// Extension methods for working with hierarchies.
	/// </summary>
	public static class HierarchyExtensions
	{
		/// <summary>
		/// Filters a sequence of <see cref="SponsorHierarchy"/> values within the hierarchy
		/// of a specified root account ID.
		/// </summary>
		/// <param name="source">An IQueryable{T} to filter.</param>
		/// <param name="rootAccountId">An account ID indicating the root node of the hierarchy.</param>
		/// <param name="maxLevels">If specified, limits the result to a specified depth.</param>
		/// <returns>An IQueryable{T} that contains elements from the input sequence
		/// that are contained in the hierarchy of the root account.</returns>
		public static IQueryable<SponsorHierarchy> WhereInHierarchy(this IQueryable<SponsorHierarchy> source, int rootAccountId, int? maxLevels = null)
		{
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Requires<ArgumentOutOfRangeException>(rootAccountId > 0);
			Contract.Requires<ArgumentOutOfRangeException>(maxLevels == null || maxLevels >= 0);
			Contract.Ensures(Contract.Result<IQueryable<SponsorHierarchy>>() != null);

			return source.WhereInHierarchy(x => x.AccountId == rootAccountId, maxLevels: maxLevels);
		}

		/// <summary>
		/// Filters a sequence of <see cref="INestedSet"/> values in a hierarchy
		/// based on a root predicate and an optional maximum depth.
		/// </summary>
		/// <typeparam name="T">The type of <see cref="INestedSet"/> values to filter.</typeparam>
		/// <param name="source">An IQueryable{T} to filter.</param>
		/// <param name="rootPredicate">A function to indicate the root node(s) in hierarchy.</param>
		/// <param name="maxLevels">If specified, limits the result to a specified depth.</param>
		/// <returns>An IQueryable{T} that contains elements from the input sequence
		/// that are contained in the hierarchy of the root node(s).</returns>
		public static IQueryable<T> WhereInHierarchy<T>(this IQueryable<T> source, Expression<Func<T, bool>> rootPredicate, int? maxLevels = null)
			where T : class, INestedSet
		{
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Requires<ArgumentNullException>(rootPredicate != null);
			Contract.Requires<ArgumentOutOfRangeException>(maxLevels == null || maxLevels >= 0);
			Contract.Ensures(Contract.Result<IQueryable<T>>() != null);

			// This may look weird to do a Where + SelectMany,
			// but it generates a nice INNER JOIN in SQL which is
			// just what we want. - Lundy

			var query = source
				.Where(rootPredicate);

			// For optimal SQL, we use different SelectMany expressions
			// depending on whether maxLevels is specified.
			if (maxLevels.HasValue)
			{
				// SelectMany with maxLevels
				query = query
					.SelectMany(root => source
						.Where(x =>
							x.LeftAnchor >= root.LeftAnchor
							&& x.RightAnchor <= root.RightAnchor
							&& x.TreeLevel <= root.TreeLevel + maxLevels.Value
						)
					);
			}
			else
			{
				// SelectMany without maxLevels
				query = query
					.SelectMany(root => source
						.Where(x =>
							x.LeftAnchor >= root.LeftAnchor
							&& x.RightAnchor <= root.RightAnchor
						)
					);
			}
			return query;
		}

		public static IDictionary<TKey, T> AddChildrenToParents<TKey, T>(
			this IDictionary<TKey, T> nodes,
			Func<T, TKey> parentKeySelector,
			Func<T, ICollection<T>> childCollectionSelector)
		{
			Contract.Requires<ArgumentNullException>(nodes != null);
			Contract.Requires<ArgumentNullException>(parentKeySelector != null);
			Contract.Requires<ArgumentNullException>(childCollectionSelector != null);

			foreach (var node in nodes.Values)
			{
				TKey parentKey = parentKeySelector(node);
				if (!parentKey.Equals(default(TKey)))
				{
					T parentNode;
					if (nodes.TryGetValue(parentKeySelector(node), out parentNode))
					{
						childCollectionSelector(parentNode).Add(node);
					}
				}
			}
			return nodes;
		}
	}
}
