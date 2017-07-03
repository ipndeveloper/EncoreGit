using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class OrderHeaderXml
    {
        public int TipoOrdem { get; set; }
        public int EmisordaOrdem { get; set; }
        public int SourceAddressID { get; set; }
        public short Trasportador { get; set; }
        public string NumeroPedido { get; set; }
        public string DataPedido { get; set; }
        public int FormaPagamento { get; set; }
        public string Incoterm { get; set; }
        public decimal Frete { get; set; }

        public string RecebedorMercaderia { get; set; }

        public string LoteTransporte { get; set; }
    }
}
