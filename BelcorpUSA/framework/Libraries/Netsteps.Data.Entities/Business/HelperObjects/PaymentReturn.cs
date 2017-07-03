using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects
{
    public class PaymentReturn
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string NameCard { get; set; }
    }
}
