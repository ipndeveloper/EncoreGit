using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Data.Entities.Business
{
    public class CTENegotiationSearchData
    {
        [Display(Name = "NegotiationLevelID")]
        public int NegotiationLevelID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; } 
    }
}
