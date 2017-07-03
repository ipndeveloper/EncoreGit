using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    /// <summary>
    /// Warehouse Inventory Movements Business Entity to search
    /// </summary>
    [Serializable]
    public class WarehouseInventoryMovementsSearchData
    {
        [Display(AutoGenerateField = false)]
        public int MaterialID { get; set; }

        [TermName("MaterialCode")]
        [Display(Name = "Material Code")]
        public string MaterialSKU { get; set; }

        [TermName("MaterialName")]
        [Display(Name = "Material Name")]
        public string MaterialName { get; set; }

        [TermName("MovementDate")]
        [Display(Name = "Movement Date")]
        public DateTime MovementDateUTC { get; set; }

        [Display(AutoGenerateField = false)]
        public int WarehouseID { get; set; }

        [TermName("Warehouse")]
        [Display(Name = "Warehouse")]
        public string WarehouseName { get; set; }

        [Display(AutoGenerateField = false)]
        public int InventoryMovementTypeID { get; set; }

        [TermName("MovementType")]
        [Display(Name = "Movement Type")]
        public string InventoryMovementTypeName { get; set; }

        [TermName("InitialOnHand")]
        [Display(Name = "Initial OnHand")]
        public int QuantityOnHandBefore { get; set; }

        [TermName("QuantityMoved")]
        [Display(Name = "Quantity Moved")]
        public int QuantityMov { get; set; }

        [TermName("FinalOnHand")]
        [Display(Name = "Final OnHand")]
        public int QuantityOnHandAfter { get; set; }

        [TermName("AvgCost")]
        [Display(Name = "Avg. Cost")]
        public Double AverageCost { get; set; }

        [Display(AutoGenerateField = false)]
        public int OrderID { get; set; }

        [TermName("OrderNumber")]
        [Display(Name = "Order Number")]
        public string OrderNumber { get; set; }

        [TermName("Description")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(AutoGenerateField = false)]
        public int UserID { get; set; }

        [TermName("User")]
        [Display(Name = "User")]
        public string UserName { get; set; }
    } 
}
