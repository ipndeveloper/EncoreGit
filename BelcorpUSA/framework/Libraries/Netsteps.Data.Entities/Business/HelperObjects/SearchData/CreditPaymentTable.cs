using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    public class CreditPaymentTable
    {
        public string TipoCredito { get; set; }
        public string ValorComparacion { get; set; }
        public string CreditoDisponible { get; set; }
        public string EstadoCredito { get; set; }
        public string AfectaCredito { get; set; }

        public int AccountID { get; set; }
        public string AccountCreditAsg { get; set; }
        public string AccountCreditUti { get; set; }
        public string AccountCreditDis { get; set; }
        public string AccountCreditEst { get; set; }
        public string AccountCreditAnt { get; set; }
        public string AccountCreditFec { get; set; }
                
    }
}
