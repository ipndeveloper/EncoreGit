using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NetSteps.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    [Serializable]
    public class RequirementLegSearchData
    {
        [TermName("TitleID")]
        [Display(Name = "ID")]
        public int TitleID { get; set; }
        
        [TermName("PlanID")]
        [Display(Name = "PlanID")]
        public int PlanID { get; set; }

        [TermName("TitleRequired")]
        [Display(Name = "TitleRequired")]
        public int TitleRequired { get; set; }

        [TermName("Generation")]
        [Display(Name = "Generation")]
        public int Generation { get; set; }

        [TermName("Level")]
        [Display(Name = "Level")]
        public int Level { get; set; }

        [TermName("TitleQTY")]
        [Display(Name = "TitleQTY")]
        public decimal TitleQTY { get; set; }

        //R2841 - HUNDRED(JICM) - CORRECCIÓN INTEGRACIÓN BR 
        [Display(AutoGenerateField = false)]
        public bool isDelete { get; set; }
    }
}
