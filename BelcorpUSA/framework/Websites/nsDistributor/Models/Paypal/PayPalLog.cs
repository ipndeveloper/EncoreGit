using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nsDistributor.Models.Paypal
{
    public class PayPalLog
    {
        public int PayPal_Trans_ID { get; set; }
        public string Site { get; set; }
        public string PayPalTransDate { get; set; }
        public int OrderID { get; set; }
        public int AccountId { get; set; }
        public string PayPal_Response { get; set; }
        public string PayPal_Error { get; set; }
        public string PayPal_Process { get; set; }
        public string PayPal_Status { get; set; }
        public int PayPal_STS_Term { get; set; }
        public string PayPal_AmountPay { get; set; }
    }
}