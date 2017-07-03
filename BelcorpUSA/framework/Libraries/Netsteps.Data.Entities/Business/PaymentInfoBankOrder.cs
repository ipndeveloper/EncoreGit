using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{


    public enum ColumnPaymentInfoBancoOrden
    {
        OrderPaymentID,
        BankID,
        BankName,
        PaymentTypeID,
        IsCreditCard,
        BankCode
    }
    public  class PaymentInfoBancoOrden
    {
        public int OrderPaymentID { get; set; }
        public int BankID { get; set; }
        public string  BankName { get; set; }
        public int PaymentTypeID { get; set; }
        public bool IsCreditCard { get; set; }
        public int BankCode { get; set; }
    }
}
