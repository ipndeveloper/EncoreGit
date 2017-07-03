using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{

    public class PreOrder
    {
        public int PreOrderId { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public int AccountID { get; set; }
        public int SiteID { get; set; }
        public int OrderID { get; set; }
        public DateTime LastUpdateDateUTC { get; set; }
        public int WareHouseID { get; set; }
        public string OrderType { get; set; }
        public DateTime getShippingDate { get; set; }
    }

    public class ProductRelations
    {
        public int ProductIDRelation { get; set; }
        public int ProductID { get; set; }
        public int MaterialId { get; set; }
        public int Quantity { get; set; }
        public int isKit { get; set; }
        public int MaterialRelarionID { get; set; }
        public int? FinalAvailable { get; set; }
        public int? IsRecuperable { get; set; }
    }


    public class ProductReplacement
    {
        public int ProductID { get; set; }
        public int MaterialID { get; set; }
        public int Quantity { get; set; }
    }

    public class InventoryCheck
    {
        public int MaterialID { get; set; }
        public int Available { get; set; }
    }

    public class Replacement
    {
        public int MaterialID { get; set; }
        public int Available { get; set; }
        public int ProductID { get; set; }
    } 

    public class IncludeInventoryCheck
    {
        public int MaterialID { get; set; }
        public int Available { get; set; }
    }
    public class CheckKitReplacement
    {
        public int MaterialID { get; set; }
        public int Available { get; set; }
    }
    public class CheckKitInventory
    {
        public int MaterialID { get; set; }
        public int Available { get; set; }
    }

    public class ProductMaterial
    {
        public int MaterialID { get; set; }
    }
    public class InventoryCheckDetail
    {
        public int MaterialID { get; set; }
        public int Available { get; set; }
    }
    public class ConfigKit
    {
        public int MaterialID { get; set; }
        public int Available { get; set; }
    }
    public class OrderItemUpdate
    {
        public int Product { get; set; }
        public int Quantity { get; set; }
        public bool status { get; set; }
        public bool  IsPromotion { get; set; }
    }

    public class OrdersStatus
    {
        public int Status { get; set; }
        public string OrderNumber { get; set; }
    }
    public class ProductPromotion
    {
        public int PromotionProductID { get; set; }
        public int Quantity { get; set; }
    }


    public class DispatchOrderItems
    {
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
    }

    public class AddLineValidStock
    {
        public int ProductID { get; set; }
        public int MaterialOriginal { get; set; }
        public string SKUOriginal { get; set; }
        public int materialID { get; set; }
        public string SKUReplacement { get; set; }
        public int Quantity { get; set; }
    }

    public class MaterialDTO
    {
        public string SKU { get; set; }
        public string Name { get; set; }
        public int? Quantity { get; set; }
        public int? MaterialID { get; set; }
    }

	 public class MaterialIDs
    {
        public int MaterialId { get; set; } 
    }  

    public class MaterialValionDTO
    {        
        public int MaterialID { get; set; }
        public int Quantity { get; set; }
        public bool HasChanged { get; set; }        
    }

}
