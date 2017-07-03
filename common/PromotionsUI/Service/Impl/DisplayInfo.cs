using System;
using System.Collections.Generic;
using NetSteps.Promotions.UI.Common.Interfaces;

namespace NetSteps.Promotions.UI.Service.Impl
{
    public class DisplayInfo : IDisplayInfo
    {
        /// <summary>
        /// Setting default values if none are provided.
        /// </summary>
        public DisplayInfo()
        {
            ImagePaths = null;
            ExpiredDate = null;
            //Expired = "";
            Title = "";
            Description = "";
            CouponCode = "";
            ActionText = "";
            CouponCode = "";
            PromotionId = "";
        }
        public string PromotionId { get; set; }
        //public string Expired { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string CouponCode { get; set; }
        public string ActionText { get; set; }
        public IEnumerable<string> ImagePaths { get; set; }
        public IFormatProvider FormatProvider { get; set; }
    }
}