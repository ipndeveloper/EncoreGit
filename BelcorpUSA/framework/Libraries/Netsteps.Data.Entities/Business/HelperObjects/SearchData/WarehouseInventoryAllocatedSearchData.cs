﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

//@01 20150831 BR-IN-004 CSTI JMO: Removed autogenerated columns

namespace NetSteps.Data.Entities.Business
{
    public class WarehouseInventoryAllocatedSearchData
    {
        //KLC - CSTI - (BR-IN-004)
        [Display(AutoGenerateField = false)]
        public int MaterialID { get; set; }

        [Display(AutoGenerateField = false)]
        public int ProductID { get; set; }

        [Display(AutoGenerateField = false)]
        public int WarehouseID { get; set; }

        [TermName("Warehouse")]
        [Display(Name = "Warehouse")]
        public string WarehouseName { get; set; }

        /* 01 D1
        [TermName("CUV")]
        [Display(Name = "CUV")]
        */
        /*01 A1*/
        [Display(AutoGenerateField = false)]
        public string ProductSKU { get; set; }

        /* 01 D2
        [TermName("Product")]
        [Display(Name = "Product")]
        */
        /*01 A2*/
        [Display(AutoGenerateField = false)] 
        public string ProductName { get; set; }

        [TermName("MaterialCode")]
        [Display(Name = "Material Code")]
        public string MaterialSKU { get; set; }

        [TermName("MaterialName")]
        [Display(Name = "Material Name")]
        public string MaterialName { get; set; }

        [TermName("Quantity")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [TermName("Order")]
        [Display(Name = "Order")]
        public int PreOrderID { get; set; }

        [TermName("Date")]
        [Display(Name = "Date")]
        public DateTime AllocationDateUTC { get; set; }

        


    }
}
