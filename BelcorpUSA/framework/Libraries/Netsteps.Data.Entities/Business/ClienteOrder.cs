using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 

namespace NetSteps.Data.Entities.Business
{
   public  class ClienteOrder
    {
      public ClientXml objClientXml { get; set; }
      public PedidoXml objPedidoXml { get; set; }
      public AdiantamentoXml objAdiantamentoXml { get; set; }
      public IEnumerable<OrderItemsXml> lstOrderItemsXml { get; set; }
    }
}
