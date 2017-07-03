using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nsDistributor.Models.Shared
{
    public class TotalsOrderItemModel
    {

        public virtual decimal TotalQV { get; set; }
        public virtual decimal TotalCV { get; set; }
        public virtual decimal TotalSubTotal { get; set; }
        public virtual decimal TotalPrice { get; set; }

        #region Infrastructure
        public virtual TotalsOrderItemModel LoadResources(
            decimal TotalQV,
            decimal TotalCV,
            decimal TotalSubTotal,
            decimal TotalPrice)
        {
            this.TotalQV = TotalQV;
            this.TotalCV = TotalCV;
            this.TotalSubTotal = TotalSubTotal;
            this.TotalPrice = TotalPrice;
            return this;
        }
        #endregion
    }
}
