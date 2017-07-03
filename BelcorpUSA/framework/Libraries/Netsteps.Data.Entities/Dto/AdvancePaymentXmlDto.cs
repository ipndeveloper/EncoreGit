using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Dto
{
    public partial class AdvancePaymentXmlDto
    {
        public decimal ValorFatura { get; set; } 
	    public decimal CreditoPedidoAnterior { get; set; }
	    public decimal DebitoPedidoAnterior { get; set; }
	    public decimal PrimeiraParcelaBoleto { get; set; }
	    public decimal RecebidoBoleto { get; set; }
	    public DateTime? DataRecebimentoBoleto { get; set; }
	    public int BancoRecebedorBoleto { get; set; }
	    public decimal ValorCobradoCartaoCred { get; set; }
	    public int OperadoraCartaoCred { get; set; }
        public decimal ValorAdiantamento { get; set; }
        public string NumeroTitulo { get; set; }
    }
}
