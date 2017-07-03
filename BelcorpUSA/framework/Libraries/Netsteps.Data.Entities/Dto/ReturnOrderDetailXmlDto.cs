using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Dto
{
   public  class ReturnOrderDetailXmlDto
    {
    public int   OrderID	{get;set;}
    public string NumeroPedido	{get;set;}
    public long   Linea	{get;set;}
    public string CategoriaItem	{get;set;}
    public string Material	{get;set;}
    public int  Quantidade	{get;set;}
    public string CentroDistribucao	{get;set;}
    public decimal PresoPraticado	{get;set;}
    public decimal Desconto	{get;set;}
    public bool TieneProductosLibre { get; set; }


    public static explicit operator ReturnOrderDetailXml(ReturnOrderDetailXmlDto objReturnOrderDetailXmlDto)
    {
        return
            new  ReturnOrderDetailXml()
            {
                OrderID = objReturnOrderDetailXmlDto.OrderID,
                NumeroPedido = objReturnOrderDetailXmlDto.NumeroPedido,
                Linea = objReturnOrderDetailXmlDto.Linea,
                CategoriaItem = objReturnOrderDetailXmlDto.CategoriaItem,
                Material = objReturnOrderDetailXmlDto.Material,
                Quantidade = objReturnOrderDetailXmlDto.Quantidade,
                CentroDistribucao = objReturnOrderDetailXmlDto.CentroDistribucao,
                PresoPraticado = objReturnOrderDetailXmlDto.PresoPraticado,
                Desconto = objReturnOrderDetailXmlDto.Desconto,
                TieneProductosLibre = objReturnOrderDetailXmlDto.TieneProductosLibre,
            };
    }
    }
}
