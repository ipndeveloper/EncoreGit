using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Foundation.Common
{
    /// <summary>
    /// A list of items that includes pagination info.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPaginatedList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
    {
        /// <summary>
        /// The current zero-based page index.
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// The number of items per page.
        /// </summary>
        int? PageSize { get; }
        
        /// <summary>
        /// The total number of items on all pages.
        /// </summary>
        int TotalCount { get; }
        
        /// <summary>
        /// The total number of pages.
        /// </summary>
        int TotalPages { get; }
        
        /// <summary>
        /// Indicates whether there is a previous page.
        /// </summary>
        bool HasPreviousPage { get; }
        
        /// <summary>
        /// Indicates whether there is a next page.
        /// </summary>
        bool HasNextPage { get; }
    }
    
    /// <summary>
    /// A list of items that includes pagination info.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginatedList<T> : List<T>, IPaginatedList<T>
    {
        /// <summary>
        /// The current zero-based page index.
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// The number of items per page.
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// The total number of items on all pages.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Initializes a new <see cref="PaginatedList{T}"/> with the given pagination info and the items from an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="pageIndex">The zero-based page index.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="totalCount">The total number of items on all pages.</param>
        /// <param name="items">The items to add to the list.</param>
        public PaginatedList(
            int pageIndex,
            int? pageSize,
            int totalCount,
            IEnumerable<T> items)
            : base(items)
        {
            Contract.Requires<ArgumentOutOfRangeException>(pageIndex >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(pageSize == null || pageSize > 0);
            Contract.Requires<ArgumentOutOfRangeException>(totalCount >= 0);
            Contract.Requires<ArgumentNullException>(items != null);

            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        /// <summary>
        /// The total number of pages.
        /// </summary>
        public int TotalPages
        {
            get
            {
                if (PageSize == null
                    || TotalCount <= PageSize.Value)
                {
                    return 1;
                }

                return (int)Math.Ceiling(TotalCount / (double)PageSize.Value);
            }
        }

        /// <summary>
        /// Indicates whether there is a previous page.
        /// </summary>
        public bool HasPreviousPage
        {
            get
            {
                return PageIndex > 0;
            }
        }

        /// <summary>
        /// Indicates whether there is a next page.
        /// </summary>
        public bool HasNextPage
        {
            get
            {
                return PageIndex + 1 < TotalPages;
            }
        }
    }

    /// <summary>
    /// Extension methods for <see cref="PaginatedList{T}"/>.
    /// </summary>
    public static class PaginatedListExtensions
    {
        /// <summary>
        /// Returns a new <see cref="PaginatedList{T}"/> with the given pagination info and the items from an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items to add to the list.</param>
        /// <param name="pageIndex">The zero-based page index.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="totalCount">The total number of items on all pages.</param>
        /// <returns></returns>
        public static PaginatedList<T> ToPaginatedList<T>(
            this IEnumerable<T> items,
            int pageIndex,
            int? pageSize,
            int totalCount)
        {
            Contract.Requires<ArgumentNullException>(items != null);
            Contract.Requires<ArgumentOutOfRangeException>(pageIndex >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(pageSize == null || pageSize > 0);
            Contract.Requires<ArgumentOutOfRangeException>(totalCount >= 0);

            return new PaginatedList<T>(
                pageIndex,
                pageSize,
                totalCount,
                items
            );
        }

        /// <summary>
        /// Returns a new <see cref="PaginatedList{T}"/> with the given pagination info and the items from an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items to add to the list.</param>
        /// <param name="parameters">The pagination parameters.</param>
        /// <param name="totalCount">The total number of items on all pages.</param>
        /// <returns></returns>
        public static PaginatedList<T> ToPaginatedList<T>(
            this IEnumerable<T> items,
            IPaginationParameters parameters,
            int totalCount)
        {
            Contract.Requires<ArgumentNullException>(items != null);
            Contract.Requires<ArgumentNullException>(parameters != null);
            Contract.Requires<ArgumentOutOfRangeException>(parameters.PageIndex >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(parameters.PageSize == null || parameters.PageSize > 0);
            Contract.Requires<ArgumentOutOfRangeException>(totalCount >= 0);

            return new PaginatedList<T>(
                parameters.PageIndex,
                parameters.PageSize,
                totalCount,
                items
            );
        }
    }
}
