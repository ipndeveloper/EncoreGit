using System;
using System.Collections.Generic;

namespace NetSteps.Promotions.UI.Common.Interfaces
{
    /// <summary>
    /// Represents the basic information for a promotion
    /// </summary>
    public interface IDisplayInfo
    {
        string PromotionId { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        DateTime? ExpiredDate { get; set; }
        /// <summary>
        /// Should be null if there are no optional values.
        /// </summary>
        string CouponCode { get; set; }
        string ActionText { get; set; }
        IEnumerable<string> ImagePaths { get; set; }
        IFormatProvider FormatProvider { get; set; }
    }
}
