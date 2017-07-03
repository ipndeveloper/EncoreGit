using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    public class CTERulesSearchData
    {
        [Display(Name = "Rule ID")]
        public int FineAndInterestRulesID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; } 
    }
}
