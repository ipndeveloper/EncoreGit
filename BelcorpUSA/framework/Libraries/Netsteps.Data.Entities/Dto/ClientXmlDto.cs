using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Dto
{
    public class ClientXmlDto
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
      public string OrderNumber { get; set; }
     
        public string Sexo { get; set; }
      public string Barrio{ get; set; }
      public string BairroRecebedor  { get; set; }
      public string REFERENCIALOCL { get; set; }

        #region Cliente Rapidao
      //public string Bairro { get; set; }
      //public string NumeroRua2 { get; set; }
      //public string NumeroRuaRecebedor2 { get; set; }
      //public string BairroRecebedor { get; set; }
      
      //public string TelefoneFixo { get; set; }
      //public string TelefoneCelular { get; set; }
      //public string SituacaoCliente { get; set; }
      #endregion
      public static explicit operator ClientXml(ClientXmlDto objClientXmlDto)
      {
          return new
          ClientXml
          {
            Sexo=objClientXmlDto.Sexo,
            Barrio = objClientXmlDto.Barrio,
            BairroRecebedor = objClientXmlDto.BairroRecebedor,
            CEP = objClientXmlDto.CEP,
              
            Cidade = objClientXmlDto.Cidade,
            CidadeRecebedor = objClientXmlDto.CidadeRecebedor,

            ClienteID = objClientXmlDto.ClienteID,
            CPF = objClientXmlDto.CPF,
            Email = objClientXmlDto.Email,
            EmailRecebedor = objClientXmlDto.EmailRecebedor,

            ENovo = objClientXmlDto.ENovo,
            Nome = objClientXmlDto.Nome,
            NumeroRua = objClientXmlDto.NumeroRua,
            NumeroRuaRecebedor = objClientXmlDto.NumeroRuaRecebedor,

            OrderCustomerID = objClientXmlDto.OrderCustomerID,
            Recebedor = objClientXmlDto.Recebedor,
            Regiao = objClientXmlDto.Regiao,
            RegiaoRecebedor = objClientXmlDto.RegiaoRecebedor,

            Rua = objClientXmlDto.Rua,
            RuaRecebedor = objClientXmlDto.RuaRecebedor,
            SetorIndustrial = objClientXmlDto.SetorIndustrial,
            NumeroPedido = objClientXmlDto.SetorIndustrial.ToString() ,
            OrderNumber = objClientXmlDto.OrderNumber,
            REFERENCIALOCL= objClientXmlDto.REFERENCIALOCL,
            CEPRecebedor= objClientXmlDto.CEPRecebedor,
            #region Cliente Rapidao
            //Bairro = objClientXmlDto.Bairro,
            //NumeroRua2 = objClientXmlDto.NumeroRua2,
            //NumeroRuaRecebedor2 = objClientXmlDto.NumeroRuaRecebedor2,
            //BairroRecebedor = objClientXmlDto.BairroRecebedor,
            //TelefoneFixo = objClientXmlDto.TelefoneFixo,
            //TelefoneCelular = objClientXmlDto.TelefoneCelular,
            //SituacaoCliente = objClientXmlDto.SituacaoCliente 
            #endregion

          };

      }

      

    }
}
