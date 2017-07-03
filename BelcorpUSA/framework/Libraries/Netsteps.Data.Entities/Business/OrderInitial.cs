using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class OrderInitial
    {
        public string CreditAvailable { get; set; }
        public string CommisionableTotal { get; set; }
        public string QualificationTotal { get; set; }
        public decimal CreditAvailableTotal { get; set; }
    }
}
