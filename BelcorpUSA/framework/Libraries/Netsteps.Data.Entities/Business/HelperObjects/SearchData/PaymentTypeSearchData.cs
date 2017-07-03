using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    public class PaymentTypeSearchData
    {
        
        [Display(Name = "PaymentTypeID")]
        public int PaymentTypeID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }            

    }
}
