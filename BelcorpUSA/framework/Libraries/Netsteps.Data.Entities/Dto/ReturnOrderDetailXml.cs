using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
  public  class ReturnOrderDetailXml
    {
        public int OrderID { get; set; }

        public string NumeroPedido { get; set; }

        public long Linea { get; set; }

        public string CategoriaItem { get; set; }

        public string Material { get; set; }

        public int Quantidade { get; set; }

        public int CentroDistribucao { get; set; }

        public decimal PresoPraticado { get; set; }

        public decimal Desconto { get; set; }

        
    }
}
