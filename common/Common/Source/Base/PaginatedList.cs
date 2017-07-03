using System;
using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Common.Base
{
    public interface IPaginatedList<T> : IList<T>
    {
        int PageIndex { get; }
        int? PageSize { get; }
        int TotalCount { get; }
        int TotalPages { get; }
        bool HasPreviousPage{ get; }
        bool HasNextPage { get; }
    }

    /// <summary>
    /// Author: John Egbert
    /// Description: A helper class to work with large lists in paged manner.
    /// Created: 05-07-2010
    /// </summary>
    [Serializable]
    public class PaginatedList<T> : List<T>, IPaginatedList<T>
    {
        public int PageIndex { get; set; }
        public int? PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages
        {
            get
            {
                if (PageSize.HasValue)
                {
                    return (int)Math.Ceiling(TotalCount / (double)PageSize.Value);
                }
                else
                {
                    return 1;
                }
            }
        }

        public PaginatedList()
        {
        }
        public PaginatedList(IPaginatedListParameters pagedListParameters)
        {
            PageIndex = pagedListParameters.PageIndex;
            PageSize = pagedListParameters.PageSize;
        }

        /// <summary>
        /// Creates a new paginated list from the source given
        /// </summary>
        /// <param name="source">The source used to create the paginated list</param>
        /// <param name="pageIndex">Index of the page to retrieve</param>
        /// <param name="pageSize">Number of records desired for the page</param>
        /// <param name="applyPagination">Set to "true" if paginating the records is desired; or set to "false" if you don't want to apply pagination.</param>
        public PaginatedList(IQueryable<T> source, int pageIndex, int? pageSize, bool applyPagination = true)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = source.Count();

            if (PageSize.HasValue && applyPagination)
            {
                this.AddRange(source.Skip(PageIndex * PageSize.Value).Take(PageSize.Value));
            }
            else
            {
                this.AddRange(source);
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex + 1 < TotalPages);
            }
        }
    }

    public static class PaginatedListExtensions
    {
        public static PaginatedList<T> ToPaginatedList<T>(
            this IEnumerable<T> items,
            IPaginatedListParameters parameters,
            int totalCount)
        {
            var paginatedList = new PaginatedList<T>
            {
                PageIndex = parameters.PageIndex,
                PageSize = parameters.PageSize,
                TotalCount = totalCount
            };
            paginatedList.AddRange(items);

            return paginatedList;
        }
    }
}
