using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Attributes;
using System.ComponentModel.DataAnnotations;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business;

  namespace NetSteps.Data.Entities.Dto
{
      public enum CloumnOrderToBatchSearch
      {
          Period,
          CompleteDate,
          Quantity,
          Amount,
          Consultant,
          Transporter,
          Route,
          State,
          ShipmentMethod,
          OrderCustomerID,
          OrderID,
          BatchGenerated
      }
    [Serializable]
    public class OrderToBatchDto
    {
        
            //[TermName("ID")]
            //[Display(AutoGenerateField = false)]
            public int OrderID { get; set; }

            //[TermName("Period", "Order")]
            public int  Period { get; set; }

            //[TermName("CompleteDate", "Complete Date")]
            public DateTime  CompleteDate { get; set; }

            //[TermName("ItemQuantity", "Item Quantity")]
            public decimal Quantity { get; set; }

            //[TermName("Amount", "Amount")]
            public decimal Amount { get; set; }

            //[TermName("Consultant", "Consultant")]
            public string Consultant { get; set; }

            //[TermName("Transporter", "Transporter")]
            public string Transporter { get; set; }

            //[TermName("Route", "Route")]
            public string Route { get; set; }

            //[TermName("CityState", "City/State")]
            public string CityState { get; set; }

            //[TermName("ShipmentMethod", "ShipmentMethod")]
            public string ShipmentMethod  { get; set; }
            public int OrderCustomerID { get; set; }

            public bool BatchGenerated { get; set; }

            public static explicit operator OrderToBatch(OrderToBatchDto objOrderToBatchDto)
            {
                OrderToBatch objOrderToBatch = new OrderToBatch()
                {
                    Amount = objOrderToBatchDto.Amount,
                    CityState = objOrderToBatchDto.CityState,
                    CompleteDate = objOrderToBatchDto.CompleteDate,
                    Consultant = objOrderToBatchDto.Consultant,
                    OrderCustomerID = objOrderToBatchDto.OrderCustomerID,
                    OrderID = objOrderToBatchDto.OrderID,
                    Period = objOrderToBatchDto.Period,
                    Quantity = objOrderToBatchDto.Quantity,
                    Route = objOrderToBatchDto.Route,
                    ShipmentMethod = objOrderToBatchDto.ShipmentMethod,
                    Transporter = objOrderToBatchDto.Transporter,
                    BatchGenerated = objOrderToBatchDto.BatchGenerated
                };
                return objOrderToBatch;
            }
}

}
