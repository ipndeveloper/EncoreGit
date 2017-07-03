using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    /// <summary>
    /// SCTI FHP
    /// </summary>
    [Serializable]
    public class WareHouseMaterialSearchData
    {
        //csti-mescobar-EB-478-23/02/2016-inicio
        [Sortable(false)]
        [TermName("Warehouse")]
        [Display(Name = "Warehouse")]
        public string Warehouse { get; set; }

        [Sortable(false)]
        [TermName("ProductCode")]
        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }


        //csti-mescobar-EB-478-23/02/2016-fin
        [Sortable(false)]
        [TermName("MaterialCode")]
        [Display(Name = "Material Code")]
        public string SKU { get; set; }

        [Sortable(false)]
        [TermName("MaterialName")]
        [Display(Name = "Material Name")]
        public string MaterialName { get; set; }


        [Sortable(false)]
        [Display(AutoGenerateField = false)]
        public int MaterialID { get; set; }

        [Sortable(false)]
        [TermName("AvgCost")]
        [Display(Name = "Avg. Cost")]
        public decimal CostAvarage { get; set; }

        [Sortable(false)]
        [TermName("QuantityOnHand")]
        [Display(Name = "On Hand")]
        public int QuantityOnHand { get; set; }

        [Sortable(false)]
        [TermName("QuantityBuffer")]
        [Display(Name = "Buffer")]
        public int QuantityBuffer { get; set; }

        [Sortable(false)]
        [TermName("ReorderLevel")]
        [Display(Name = "Reorder Level")]
        public int ReorderLevel { get; set; }

        [Sortable(false)]
        [TermName("Allocated")]
        [Display(Name = "Allocated")]
        public int QuantityAllocated { get; set; }

        [Sortable(false)]
        [TermName("AddManualMovement")]
        [Display(Name = "Add Manual Movement")]
        public int WarehouseMaterialID { get; set; }

        [Sortable(false)]
        [TermName("ViewReemplacements")]
        [Display(Name = "View Reemplacements")]
        public string ProductWarehouseMaterialID { get; set; } 
    }

    public class WareHouseMaterialDetails
    {
        public int WareHouseMaterialID { get; set; }
        public string CodeMaterial { get; set; }
        public string MaterialName { get; set; }
        public string WareHouseName { get; set; }
        public int WareHouseId { get; set; }
        public int MaterialId { get; set; } 
    }

    public class WareHouseMaterialControls
    {
        public int WareHouseMaterialID { get; set; }
        public string CodeMaterial { get; set; }
        public string MaterialName { get; set; }
        public string WareHouseName { get; set; }
    }
}
