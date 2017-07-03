using NetSteps.Promotions.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Promotions.Plugins.EntityModel
{
    public partial class PromotionQualificationCustomerIsHost : IPromotionQualificationSimpleExtension
    {
        public int PromotionQualificationID { get; set; }
    }
}
