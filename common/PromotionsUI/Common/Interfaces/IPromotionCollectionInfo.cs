using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Common.Context;

namespace NetSteps.Promotions.UI.Common.Interfaces
{
    /// <summary>
    /// Information that may vary client to client.
    /// Use this interface with your client implementation.
    /// </summary>
    public interface IPromotionCollectionInfo
    {
        /// <summary>
        /// This should be your original set of data unfiltered
        /// </summary>
        IEnumerable<IDisplayInfo> AllPromotions { get; }
        /// <summary>
        /// provides you the order context 
        /// </summary>
        IOrderContext OrderContext { get; }
    }
}
