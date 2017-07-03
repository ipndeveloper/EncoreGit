using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Dto
{
    public class OrderItemsXmlDto
    {
       public int Linea { get; set; }
       public int CategoriaItem { get; set; }
       public string Material { get; set; }
       public int Quantidade { get; set; }
       public string CentroDistribucao { get; set; }
       public decimal PresoPraticado { get; set; }
       public decimal Desconto { get; set; }
       public int OrderCustomerID { get; set; }

       public static explicit operator OrderItemsXml(OrderItemsXmlDto objOrderItemsXmlDto)
       {
           return new OrderItemsXml()
           {
               CategoriaItem = objOrderItemsXmlDto.CategoriaItem,
               CentroDistribucao = objOrderItemsXmlDto.CentroDistribucao,
               Desconto = objOrderItemsXmlDto.Desconto,
               Linea = objOrderItemsXmlDto.Linea,
               Material = objOrderItemsXmlDto.Material,
               OrderCustomerID = objOrderItemsXmlDto.OrderCustomerID,
               PresoPraticado = objOrderItemsXmlDto.PresoPraticado,
               Quantidade = objOrderItemsXmlDto.Quantidade
           };
       }
       
    }
}
