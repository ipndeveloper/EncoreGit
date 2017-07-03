using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    public class BackOrderedProductData
    {
        public int? ProductID { get; set; }
        public int? Quantity { get; set; }
        public int? OrderItemClaimID { get; set; } 
    }

    public class WareHouseMaterialAllocations
    {
        public int Quantity { get; set; }
        public int ProductID { get; set; }
        public int WarehouseMaterialID { get; set; }
        public int MaterialID { get; set; }
        public int WareHouseId { get; set; }
    }
    public class MaterialWareHouseMaterial
    { 
        public int MaterialID { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public int WareHouseID { get; set; }
        public int ProductID { get; set; }
    }

    public class MaterialOrderItem
    {
        public int OrderCustomerID { get; set; }
        public int MaterialID { get; set; }
    }

    public class OrderItemsStock
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }

    /*
 * wv:20160606 Estructuras para manejar los procesos de los Dispatch
 */
    [Serializable]
    public class DispatchNameProducts
    {
        public string Name { get; set; }
        public string SKU { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int? ParentOrderItemID { get; set; }
        public int OrderItemID { get; set; }
        public int OrderDispatchID { get; set; }
        public string NameType { get; set; }
    }

    [Serializable]
    public class DispatchProducts
    {
        public string Name { get; set; }
        public string SKU { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int? ParentOrderItemID { get; set; }
        public int OrderItemID { get; set; }
        public int OrderDispatchID { get; set; }
        public string NameType { get; set; }
    }

    [Serializable]
    public class DisplayDispatch
    {
        public string SKU { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Descripcion { get; set; }
    }

    [Serializable]
    public class getDispatchByOrder
    {
        public int OrderItemID { get; set; }
        public int DispatchID { get; set; }
        public int DispatchTypeID { get; set; }
        public string DispatchType { get; set; }
        public string Ddescripcion { get; set; }
    }

    [Serializable]
    public class getListInfoOrderItems
    {
        public int OrderItemID { get; set; }
        public int ProductID { get; set; }
        public String ProductName { get; set; }
        public int DispatchQty { get; set; }
    }

    [Serializable]
    public class GrupoDispatchByOrder
    {
        public int DispatchID { get; set; }
        public string Ddescripcion { get; set; }
    }
    // --------------------------------------------------



    public class OrderItemsClaims
    {
        public int ProductID { get; set; }
        public int OrderItemClaimID { get; set; }
    }

    
    public class MaterialName
    {
        public string Name { get; set; }
        public string SKU { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal Importe { get; set; }
        //public decimal TotalOrder { get; set; }
    }

    public class ProductNameClains
    {
        public string Name { get; set; }
        public string SKU { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int? ParentOrderItemID { get; set; }
        public int OrderItemID { get; set; }
        public int OrderClaimID { get; set; } 
    }
     

    public class ProductClains
    {
        public string Name { get; set; }
        public string SKU { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int? ParentOrderItemID { get; set; }
        public int OrderItemID { get; set; }
        public int OrderClaimID { get; set; } 
        
    }

    public class PreOrderCondition
    {
        public bool Exist { get; set; }
        public string Descriptions { get; set; }
    }
    
}
