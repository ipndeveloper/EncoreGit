using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Dto
{
    public class PedidoXmlDto
    {
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
        #region  rapidao
        //public string NomeRecebedor { get; set; }
        //public string CEP { get; set; }
        //public string Rua { get; set; }
        //public string NumeroRua { get; set; }
        //public string NumeroRua2 { get; set; }

        #endregion

        public static explicit operator PedidoXml(PedidoXmlDto objPedidoXmlDto)
        {
            return new PedidoXml
            {
                    DataPedido = objPedidoXmlDto.DataPedido,
                    EmisordaOrdem = objPedidoXmlDto.EmisordaOrdem,
                    Frete = objPedidoXmlDto.Frete,
                    Incoterm = objPedidoXmlDto.Incoterm,
                    NumeroPedido = objPedidoXmlDto.NumeroPedido,
                    OrderCustomerID = objPedidoXmlDto.OrderCustomerID,
                    RecebedorMercaderia = objPedidoXmlDto.RecebedorMercaderia,
                    TipoOrdem = objPedidoXmlDto.TipoOrdem,
                    Trasportador = objPedidoXmlDto.Trasportador,
                    FormaPgto = objPedidoXmlDto.FormaPgto,
                    LoteTransporte = objPedidoXmlDto.LoteTransporte
                    //NomeRecebedor = objPedidoXmlDto.NomeRecebedor,
                    //CEP = objPedidoXmlDto.CEP,
                    //Rua = objPedidoXmlDto.Rua,
                    //NumeroRua = objPedidoXmlDto.NumeroRua,
                    //NumeroRua2 = objPedidoXmlDto.NumeroRua2
            };
        }

        public int LoteTransporte { get; set; }
    }
}
