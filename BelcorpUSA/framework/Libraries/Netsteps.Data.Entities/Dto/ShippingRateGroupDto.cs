using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Dto
{
    public class ShippingRateGroupDto 
    {
        public int ShippingRateGroupID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GroupCode { get; set; }
        public bool Active { get; set; }
        public string RowTotal { get; set; }
    }
}
