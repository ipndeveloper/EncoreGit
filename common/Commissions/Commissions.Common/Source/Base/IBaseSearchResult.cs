using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Base
{
    /// <summary>
    /// base interface for search results
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseSearchResult<T>
    {
        /// <summary>
        /// list of results
        /// </summary>
        IEnumerable<T> Results { get; set; }

        /// <summary>
        /// total count of results before paging
        /// </summary>
        int TotalCount { get; set; }
    }
}
