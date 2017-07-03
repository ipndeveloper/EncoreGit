using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class PedidoXml
    {
        #region Sap
        public string TipoOrdem { get; set; }
        public int EmisordaOrdem { get; set; }
        public string RecebedorMercaderia { get; set; }
        public string Trasportador { get; set; }
        public string NumeroPedido { get; set; }
        public DateTime DataPedido { get; set; }
        public string Incoterm { get; set; }
        public decimal Frete { get; set; }
        public int OrderCustomerID { get; set; }
        public string FormaPgto { get; set; }
        #endregion
        #region  rapidao
        //public string NomeRecebedor { get; set; }
        //public string CEP { get; set; }
        //public string Rua { get; set; }
        //public string NumeroRua { get; set; }
        //public string NumeroRua2 { get; set; }

        #endregion

        public int LoteTransporte { get; set; }
    }
}
