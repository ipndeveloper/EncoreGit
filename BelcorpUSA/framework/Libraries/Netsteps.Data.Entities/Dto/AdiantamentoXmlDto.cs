using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Dto
{
    public class AdiantamentoXmlDto
    {
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
       public string  NumeroParcelas { get; set; }
       public string NumeroTitulo{ get; set; }

       public static explicit operator AdiantamentoXml(AdiantamentoXmlDto objAdiantamentoXmlDto)
       {
           return new AdiantamentoXml
           {
               NumeroParcelas = objAdiantamentoXmlDto.NumeroParcelas,
               NumeroTitulo = objAdiantamentoXmlDto.NumeroTitulo,

               BancoRecebedorBoleto = objAdiantamentoXmlDto.BancoRecebedorBoleto,
               CreditoPedidoAnterior = objAdiantamentoXmlDto.CreditoPedidoAnterior,
               DataRecebimentoBoleto = objAdiantamentoXmlDto.DataRecebimentoBoleto,
               DebitoPedidoAnterior = objAdiantamentoXmlDto.DebitoPedidoAnterior,
               OperadoraCartaoCred = objAdiantamentoXmlDto.OperadoraCartaoCred,
               OrderCustomerID = objAdiantamentoXmlDto.OrderCustomerID,
               PrimeiraParcelaBoleto = objAdiantamentoXmlDto.PrimeiraParcelaBoleto,
               RecebidoBoleto = objAdiantamentoXmlDto.RecebidoBoleto,
               ValorAdiantamento = objAdiantamentoXmlDto.ValorAdiantamento,
               ValorCobradoCartaoCred = objAdiantamentoXmlDto.ValorCobradoCartaoCred,
               ValorFatura = objAdiantamentoXmlDto.ValorFatura,
               NumeroPedido=objAdiantamentoXmlDto.NumeroPedido
           };

       }
    }
}
