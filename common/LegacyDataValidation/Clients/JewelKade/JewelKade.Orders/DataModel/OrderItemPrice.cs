//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JewelKade.Orders.DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderItemPrice
    {
        public int OrderItemPriceID { get; set; }
        public int OrderItemID { get; set; }
        public Nullable<decimal> OriginalUnitPrice { get; set; }
        public int ProductPriceTypeID { get; set; }
        public decimal UnitPrice { get; set; }
    
        public virtual OrderItem OrderItem { get; set; }
    }
}
