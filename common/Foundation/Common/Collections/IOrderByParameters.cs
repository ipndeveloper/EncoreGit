using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Foundation.Common
{
    /// <summary>
    /// Parameters to specify how a result should be sorted.
    /// </summary>
    [DTO]
    public interface IOrderByParameters
    {
        /// <summary>
        /// A string indicating the property to order by.
        /// </summary>
        string OrderBy { get; set; }
        
        /// <summary>
        /// Indicates whether the sort order is descending.
        /// </summary>
        bool OrderByDescending { get; set; }
    }

    /// <summary>
    /// Extension methods for sorting.
    /// </summary>
    public static class IOrderByParametersExtensions
    {
        /// <summary>
        /// Returns an "OrderBy" string that includes sort direction to allow the use of DynamicQueryable.OrderBy().
        /// </summary>
        public static string DynamicOrderByString(this IOrderByParameters parameters)
        {
            Contract.Requires<ArgumentNullException>(parameters != null);

            if (string.IsNullOrWhiteSpace(parameters.OrderBy))
            {
                return string.Empty;
            }

            return parameters.OrderByDescending
                ? parameters.OrderBy + " DESC"
                : parameters.OrderBy;
        }
    }
}
