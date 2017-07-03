using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Dto
{
    public class ConfirmacionPagos
    {
        #region properties
        public string DocId { get; set; }
        public string CodigoPessoa { get; set; }
        public string CodigoOrdenPagamento { get; set; }
        public string ValorTotalPago { get; set; }

        public string CodigoImposto { get; set; }
        public string PercDesconto { get; set; }
        public string ValorTotalDescontado { get; set; }

        public string CodigoImposto2 { get; set; }
        public string PercDesconto2 { get; set; }
        public string ValorTotalDescontado2 { get; set; }
        #endregion
    }
}
