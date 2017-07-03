using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class ApplyPaymentSearchData
    {
        public int? PaymentConfigurationID { get; set; }
        public decimal? Amount { get; set; }
        public string CreditCardNumber { get; set; }
        public string NameOnCard { get; set; }
        public DateTime? CardExpirationDate { get; set; }
        public int? NumberCuota { get; set; }
        public string AutorizationNumber { get; set; }
        public int OrderPaymentId { get; set; }
        public int PreOrderID { get; set; }
        public class paymentYpe
        {
            public bool IsCreditCard { get; set; }
            public bool IsTicket { get; set; }
        }

        public class paymentSelect
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class OrderShipment
        {
            public string Name { get; set; }
            public DateTime DateStimate{ get; set; }
        }


    }

}
