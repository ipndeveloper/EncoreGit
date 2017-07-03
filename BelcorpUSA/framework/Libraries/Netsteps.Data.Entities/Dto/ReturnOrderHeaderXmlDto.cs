using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Dto
{
   public  class ReturnOrderHeaderXmlDto
    {
        public string    NumeroPedido	{get;set;}
        public int       TipoOrdem	{get;set;}
        public string    EmisordaOrdem	{get;set;}
        public string    RecebedorMercaderia	{get;set;}
        public string    Trasportador	{get;set;}

        public string    NumeroPedidoAnterior {get;set;}	
        public string    DataOrder	{get;set;}
        public string    FormaPgto	{get;set;}
        public string    Incoterm	{get;set;}
        public  decimal  Frete	{get;set;}
        public int       TipoDevol{get;set;}
        public string numeroTitulo { get; set; }
        public string loteTransporte { get; set; }

        public static explicit operator ReturnOrderHeaderXml(ReturnOrderHeaderXmlDto objReturnOrderHeaderXmlDto)
   {
   
   return
       new  ReturnOrderHeaderXml()
           {
                NumeroPedido=objReturnOrderHeaderXmlDto.NumeroPedido,
                TipoOrdem=objReturnOrderHeaderXmlDto.TipoOrdem	,
                EmisordaOrdem=objReturnOrderHeaderXmlDto.EmisordaOrdem	,
                RecebedorMercaderia=objReturnOrderHeaderXmlDto.RecebedorMercaderia,
                Trasportador=objReturnOrderHeaderXmlDto.Trasportador,
                NumeroPedidoAnterior =objReturnOrderHeaderXmlDto.NumeroPedidoAnterior,
                DataOrder=objReturnOrderHeaderXmlDto.DataOrder,
                FormaPgto=objReturnOrderHeaderXmlDto.FormaPgto,
                Incoterm=objReturnOrderHeaderXmlDto.Incoterm,
                Frete=objReturnOrderHeaderXmlDto.Frete,
                TipoDevol=objReturnOrderHeaderXmlDto.TipoDevol,
                numeroTitulo=objReturnOrderHeaderXmlDto.numeroTitulo,
                loteTransporte = objReturnOrderHeaderXmlDto.loteTransporte
           };
   }
   }
}
