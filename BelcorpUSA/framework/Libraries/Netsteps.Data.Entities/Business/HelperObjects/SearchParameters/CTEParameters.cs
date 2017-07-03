using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;
using NetSteps.Encore.Core;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    //Modificaciones:
    //@1 20151607 BR-CC-012 GYS MD: Se implemento la clase CTEParameters
    public class CTEParameters
    {
        public int OrderPaymentID { get; set; }
        public int BankPaymentID { get; set; }
        public string BankName { get; set; }
        public DateTime? ProcessOnDateUTC { get; set; }
        public DateTime? ProcessedDateUTC { get; set; }
        public bool? Accepted { get; set; }
        public bool? Applied { get; set; }
    }
}
