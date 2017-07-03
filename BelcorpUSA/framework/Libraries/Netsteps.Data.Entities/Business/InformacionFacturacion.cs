using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{

    public enum ColumnasInformacionFacturacion
    {
 
        OrderId ,
        AccountID ,
        AccountName,
        Rua,
        Numero,
        Referencia,
        Barrio,
        Ciudad,
        Estado,
        Cep,
        CurrentExpirationDateUTC ,
        PaisId,
        Pais,
        TransationId,
        ExpirationId,
        PaymentStatus,
        OrderPaymentID
    
    }
   public  class InformacionFacturacion
    {
        public int OrderId{get;set;}
		public  int AccountID{get;set;}
		public string AccountName{get;set;}
		public string  Rua {get;set;}
		public string  Numero{get;set;}
		public string  Referencia{get;set;}
		public string Barrio{get;set;}
		public string  Ciudad{get;set;}
		public string  Estado {get;set;}
		public string  Cep{get;set;}
		public DateTime  CurrentExpirationDateUTC {get;set;}
		public int PaisId{get;set;}
		public string  Pais{get;set;}
		public string  TransationId{get;set;}
		public int  ExpirationId{get;set;}
		public string   PaymentStatus{get;set;}
		public int  OrderPaymentID {get;set;}
    }
}
