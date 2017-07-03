using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class CTEUpdateBalance
    {
        public decimal MultaCalulada { get; set; }
        public decimal InteresCalculado { get; set; }
        public int NumDocument { get; set; }
        public decimal FinancialAmount { get; set; }
        public decimal TotalAmount { get; set; }        
    }
}
