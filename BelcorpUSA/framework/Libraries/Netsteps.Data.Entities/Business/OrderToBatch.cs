using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Data.Entities.Business
{
    
    [Serializable]
    public class OrderToBatch
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

            /*CS.19AG2016.Inicio.Nueva columna de retorno*/
            public bool BatchGenerated { get; set; }
            /*CS.19AG2016.Fin.Nueva columna de retorno*/
}
}
