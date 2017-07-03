using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Foundation.Common
{
    /// <summary>
    /// Parameters to specify how a result should be paginated.
    /// </summary>
    [DTO]
    public interface IPaginationParameters
    {
        /// <summary>
        /// The zero-based page index requested.
        /// </summary>
        int PageIndex { get; set; }
        
        /// <summary>
        /// The number of items per page requested.
        /// </summary>
        int? PageSize { get; set; }
    }

    /// <summary>
    /// Extension methods for pagination.
    /// </summary>
    public static class IPaginationParametersExtensions
    {
        /// <summary>
        /// Applies pagination to an <see cref="IQueryable{T}"/> using the specified <see cref="IPaginationParameters"/>.
        /// </summary>
        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, IPaginationParameters parameters)
        {
            Contract.Requires<ArgumentNullException>(query != null);
            Contract.Requires<ArgumentNullException>(parameters != null);
            Contract.Requires<ArgumentOutOfRangeException>(parameters.PageIndex >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(parameters.PageSize == null || parameters.PageSize > 0);

            if (parameters.PageSize == null)
            {
                return query;
            }

            return query
                .Skip(parameters.PageIndex * parameters.PageSize.Value)
                .Take(parameters.PageSize.Value);
        }
    }
}
