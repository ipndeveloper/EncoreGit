using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//@1 20151607 BR-CC-012 GYS MD: se creo la clase OrderPaymentsLog para correspondiente a la tabla OrderPaymentsLog
namespace NetSteps.Data.Entities.Business
{
  public  class OrderPaymentsLog
    {
        public int LogID { get; set; }
        public int OrderPaymentID { get; set; }
        public int ReasonID { get; set; }
        public decimal InitialAmount { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal FineAmount { get; set; }
        public decimal DisccountedAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime ExpirationDateUTC { get; set; }
        public DateTime DateModifiedUTC { get; set; }
        public int ModifiedByUserID { get; set; }

    }
}
