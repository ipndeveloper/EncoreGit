using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Promotions.UI.Common.Interfaces
{
    public interface IPromotionCollectionViewModel
    {
        /// <summary>
        /// This is your filtered set of data that should be displayed in your ui.
        /// </summary>
        IEnumerable<IDisplayInfo> ViewablePromotions { get; }
    }
}
