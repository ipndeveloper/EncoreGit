using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    public class PaymentsTable
    {
        public int? PaymentConfigurationID { get; set; }
        public decimal? AppliedAmount { get; set; }
        public int PaymentStatusID { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int? NumberCuota { get; set; }
        public string AutorizationNumber { get; set; }
        public int OrderPaymentId { get; set; }
        public int PaymentType { get; set; }
        public int PreOrderID { get; set; }
        public int ubic { get; set; }
    }
}
