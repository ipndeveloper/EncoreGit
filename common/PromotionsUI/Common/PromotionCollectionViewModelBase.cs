using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.UI.Common.Interfaces;

namespace NetSteps.Promotions.UI.Common
{
    /// <summary>
    /// This is a base implementation of what a promotions collection should contain at minimum.
    /// </summary>
    public abstract class PromotionCollectionViewModelBase : IPromotionCollectionViewModel
    {
        protected IPromotionCollectionInfo _context;
        /// <summary>
        /// A base constructor that takes in our collection of client baked information.
        /// </summary>
        /// <param name="context"></param>
        protected PromotionCollectionViewModelBase(IPromotionCollectionInfo context)
        {
            _context = context;
        }

        abstract public IEnumerable<IDisplayInfo> ViewablePromotions { get; }
    }
}
