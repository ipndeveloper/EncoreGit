using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class OrderPaymentTemplates : DynamicModel
    {
        public OrderPaymentTemplates() : base("Core", "OrderPaymentTemplates", "OrderPaymentTemplateId") { }
    }
}
