using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects
{
    public class OrderPaymentVirtualDesktop
    {
        public int accountID { get; set; }
        public DateTime? EncerramentoDoCiclo { get; set; }
        public int OrderID { get; set; }
        public float pendingAmount { get; set; }
        public decimal PQV { get; set; }
        public decimal DQV { get; set; }
        public string CareerTitleTerm { get; set; }
        public string PaidAsCurrentMonth { get; set; }
     }
}
