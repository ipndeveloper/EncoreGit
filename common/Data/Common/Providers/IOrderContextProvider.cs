using System;
using System.Linq;
using NetSteps.Data.Common.Context;

namespace NetSteps.Data.Common.Providers
{
    /// <summary>
    /// Provides the information for your current order context.
    /// </summary>
    public interface IOrderContextProvider
    {
        /// <summary>
        /// Gets your IOrderContext.
        /// </summary>
        /// <returns></returns>
        IOrderContext GetOrderContext();


    }
}
