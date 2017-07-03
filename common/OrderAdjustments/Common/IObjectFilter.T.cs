using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.OrderAdjustments.Common
{
    /// <summary>
    /// Very simple pattern for filtration.  
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectFilter<T>
    {
        IQueryable<T> BuildQueryFrom(IQueryable<T> QueryBase);
    }
}
