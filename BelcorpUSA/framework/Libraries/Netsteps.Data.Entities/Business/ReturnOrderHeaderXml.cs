using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class ReturnOrderHeaderXml
    {
        public string NumeroPedido { get; set; }

        public int TipoOrdem { get; set; }

        public string EmisordaOrdem { get; set; }

        public string RecebedorMercaderia { get; set; }

        public string Trasportador { get; set; }

        public string NumeroPedidoAnterior { get; set; }

        public string DataOrder { get; set; }

        public string FormaPgto { get; set; }

        public string Incoterm { get; set; }

        public decimal Frete { get; set; }

        public int TipoDevol { get; set; }
        public string numeroTitulo { get; set; }
        public string loteTransporte { get; set; }

    }
}
