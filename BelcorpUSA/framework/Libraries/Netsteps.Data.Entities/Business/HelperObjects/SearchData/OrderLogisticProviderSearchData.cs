using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    public class OrderLogisticProviderSearchData
    {
        //[Display(Name = "Change Logistic Provider")]
        [TermName("ChangeLogisticProvider", "Change Logistic Provider")]
        [Display(Name = "Change Logistic Provider")]
        public string ChangeLogisticProvider { get; set; }

        [Display(AutoGenerateField = false)]
        public int? OrderShipmentID { get; set; }

        [Display(Name = "Order")]
        public int OrderNumber { get; set; }

        [Display(Name = "Logistic Provider")]
        public string LogisticProviderName { get; set; }

        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Order Total")]
        public string OrderTotal { get; set; }

        [Display(AutoGenerateField = false)]
        public string OrderDateReport { get; set; }

    }
}
