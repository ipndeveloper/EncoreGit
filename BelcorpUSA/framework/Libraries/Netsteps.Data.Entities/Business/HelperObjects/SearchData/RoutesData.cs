using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    public class RoutesData
    {
        [TermName("RouteCodes","Route Code")]
        [Display(Name = "Route Code")]
        public int RouteID { get; set; }

        [TermName("Name","Name")]
        [Display(Name = "Name")]
        public string Name { get; set; } 
    }
}
