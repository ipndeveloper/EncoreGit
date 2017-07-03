using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;
using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class ShippingRulesLogisticsSearchData
    {
        [TermName("ShippingRuleCode","Shipping Rule Code")]
        [Display(Name = "Shipping Rule Code")]
        public int ShippingOrderTypeID { get; set; }

        [TermName("Name","Name")]
        [Display(Name = "Name")]
        public string ShippingRuleName { get; set; }

        [Display(AutoGenerateField = false)]
        public int WarehouseID { get; set; }

        [TermName("Warehouse","Warehouse")]
        [Display(Name = "Warehouse")]
        public string Warehouse { get; set; }

        [Display(AutoGenerateField = false)]
        public int ShippingMethodID { get; set; }

        [TermName("ShippingMethod", "Shipping Method")]
        [Display(Name = "Shipping Method")]
        public string ShippingMethod { get; set; }

        [Display(AutoGenerateField = false)]
        public int LogisticProviderID { get; set; }

        [TermName("LogisticProvider", "Logistic Provider")]
        [Display(Name = "Logistic Provider")]
        public string LogisticProvider { get; set; }

        [TermName("Status", "Status")]
        [Display(Name = "Status")]
        public bool Status { get; set; }

        [Display(AutoGenerateField = false)]
        public int CountryID { get; set; }

        [Display(AutoGenerateField = false)]
        public int ShippingRateGroupID { get; set; }

        [Display(AutoGenerateField = false)]
        public int OrderTypeID { get; set; }

        [Display(AutoGenerateField = false)]
        public bool IsDefaultShippingMethod { get; set; }
        
        [Display(AutoGenerateField = false)]
        public int DaysForDelivey { get; set; }

        [Display(AutoGenerateField = false)]
        public bool AllowDirectShipments { get; set; }
    }

    [Serializable]
    public class ClassX
    {
        [TermName("Prop1")]
        [Display(Name = "Prop1")]
        public int Prop1 { get; set; }

        [TermName("Prop2")]
        [Display(Name = "Prop2")]
        public int Prop2 { get; set; }
    }
}
