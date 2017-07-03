using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class ClientXml 
    {
      public string NumeroPedido { get; set; }
      public int ClienteID { get; set; }
      public int ENovo { get; set; }
      public string Nome { get; set; }
      public string Rua { get; set; }
      public string NumeroRua { get; set; }
       
      public string CEP { get; set; }
      public string Cidade { get; set; }
      public string Regiao { get; set; }
      public string Email { get; set; }
      public string CPF { get; set; }
      public int SetorIndustrial { get; set; }
      public string Recebedor { get; set; }
      public string RuaRecebedor { get; set; }
      public string NumeroRuaRecebedor { get; set; }
      public string CEPRecebedor { get; set; }
      public string CidadeRecebedor { get; set; }
      public string RegiaoRecebedor { get; set; }
      public string EmailRecebedor { get; set; }
      public int OrderCustomerID { get; set; }

      #region Cliente Sap
      //public string Bairro { get; set; }
      //public string NumeroRua2 { get; set; }
      //public string NumeroRuaRecebedor2 { get; set; }
      //public string BairroRecebedor { get; set; }
      //public string CEPRecebedor { get; set; }
      //public string TelefoneFixo { get; set; }
      //public string TelefoneCelular { get; set; }
      //public string SituacaoCliente { get; set; }
      #endregion 
        public string OrderNumber { get; set; }
        public string Sexo { get; set; }
        public string Barrio { get; set; }
        public string BairroRecebedor { get; set; }

        public string REFERENCIALOCL { get; set; }
    }
}
