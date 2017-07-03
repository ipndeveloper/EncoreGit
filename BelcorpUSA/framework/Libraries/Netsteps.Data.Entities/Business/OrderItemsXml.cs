using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class OrderItemsXml
    {
       public int Linea { get; set; }
       public int CategoriaItem { get; set; }
       public string Material { get; set; }
       public int Quantidade { get; set; }
       public string  CentroDistribucao { get; set; }
       public decimal PresoPraticado { get; set; }
       public decimal Desconto { get; set; }
       public int OrderCustomerID { get; set; }
    }
}
