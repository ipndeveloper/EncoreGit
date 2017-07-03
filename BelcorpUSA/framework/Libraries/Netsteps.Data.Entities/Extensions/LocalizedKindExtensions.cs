using System;
using System.Linq;
using System.Linq.Expressions;

namespace NetSteps.Data.Entities.Extensions
{
    public class OuterJoinResult<TOuter, TInner>
    {
        public TOuter OuterItem { get; set; }
        public TInner InnerItem { get; set; }
    }

    public static class LocalizedKindExtensions
    {
        public static IQueryable<OuterJoinResult<TOuter, TInner>> OuterJoin<TOuter, TInner, TKey>(
            this IQueryable<TOuter> outer,
            IQueryable<TInner> inner,
            Expression<Func<TOuter, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector)
        {
            return outer
                .GroupJoin(
                    inner,
                    outerKeySelector,
                    innerKeySelector,
                    (o, i) => new { o, i }
                )
                .SelectMany(
                    g => g.i.DefaultIfEmpty(),
                    (g, i) => new OuterJoinResult<TOuter, TInner> { OuterItem = g.o, InnerItem = i }
                );
        }

        public static IQueryable<T> OrderByLocalizedKind<T>(
            this IQueryable<T> query,
            int localizedKindTableId,
            int languageId,
            Expression<Func<T, int>> kindKeySelector,
            Constants.SortDirection sortDirection,
            NetStepsEntities context)
        {
			// This used to be an outer join but some of the SQL queries it generated were too slow.
			// Changed to an inner join for better performance. This will only be a problem if the
			// database is missing values in the Locale.LocalizedKinds table. - Lundy
			var joinQuery = query
				.Join(
					context.LocalizedKinds.Where(lk => lk.LocalizedKindTableId == localizedKindTableId && lk.LanguageId == languageId),
					kindKeySelector,
					lk => lk.KindId,
					(Item, LocalizedKind) => new { Item, LocalizedKind }
				);
            return (
                sortDirection == Constants.SortDirection.Ascending
                    ? joinQuery.OrderBy(x => x.LocalizedKind.SortIndex)
                    : joinQuery.OrderByDescending(x => x.LocalizedKind.SortIndex)
                )
                .Select(x => x.Item);
        }
    }
}
