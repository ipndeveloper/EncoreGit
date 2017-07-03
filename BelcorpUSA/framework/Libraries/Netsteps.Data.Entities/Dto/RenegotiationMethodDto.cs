using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Dto
{
    public class RenegotiationMethodDto
    {
        public int FineAndInterestRulesPerNegotiationLevelID { get; set; }
        public int FineAndInterestRulesID { get; set; }
        public int RenegotiationConfigurationID { get; set; }
        public string Plano { get; set; }
        public int Cuotas { get; set; }
        public decimal Juros_Dia { get; set; }
        public string Taxa { get; set; }
        public string DiscountDesc { get; set; }//Descuento
        //--------------------------------------
        public int OrderPaymentID { get; set; }
        public string Site { get; set; }
    }
}
