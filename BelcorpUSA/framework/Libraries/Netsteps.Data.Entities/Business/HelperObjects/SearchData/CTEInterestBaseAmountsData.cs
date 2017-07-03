using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Data.Entities.Business
{
    public class CTEInterestBaseAmountsData
    {
        [Display(Name = "InterestBaseAmountID")]
        public int InterestBaseAmountID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; } 
    }
}
