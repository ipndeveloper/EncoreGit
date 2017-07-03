using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nsDistributor.Models.Shared
{
    public class MiniShopUpdate
    {
        public virtual int ProductID { get; set; }
        public virtual int Quantity { get; set; }
    }
}