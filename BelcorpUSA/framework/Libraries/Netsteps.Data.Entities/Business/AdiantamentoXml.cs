using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities 
{
    public class AdiantamentoXml 
    {
        public AdiantamentoXml()
        { 
            ValorFatura=0;
            CreditoPedidoAnterior =0;
            DebitoPedidoAnterior =0;
            PrimeiraParcelaBoleto =0;
            RecebidoBoleto =0;
            DataRecebimentoBoleto =DateTime.Now;
            BancoRecebedorBoleto ="";
            ValorCobradoCartaoCred =0;
            OperadoraCartaoCred =0;
            ValorAdiantamento =0;
            OrderCustomerID =0;
            NumeroPedido = "";
        }
       public decimal ValorFatura { get; set; }
       public decimal CreditoPedidoAnterior { get; set; }
       public decimal DebitoPedidoAnterior { get; set; }
       public decimal PrimeiraParcelaBoleto { get; set; }
       public decimal RecebidoBoleto { get; set; }
       public DateTime DataRecebimentoBoleto { get; set; }
       public string BancoRecebedorBoleto { get; set; }
       public decimal ValorCobradoCartaoCred { get; set; }
       public int OperadoraCartaoCred { get; set; }
       public decimal ValorAdiantamento { get; set; }
       public int OrderCustomerID { get; set; }
       public string NumeroPedido { get; set; }

       public string NumeroTitulo { get; set; }

       public string NumeroParcelas { get; set; }
    }
}
