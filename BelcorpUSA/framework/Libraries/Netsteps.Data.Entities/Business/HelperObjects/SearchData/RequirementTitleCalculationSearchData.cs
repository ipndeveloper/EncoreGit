using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NetSteps.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    [Serializable]
    public class RequirementTitleCalculationSearchData
    {
        [TermName("TitleID")]
        [Display(Name = "ID")]
        public int TitleID { get; set; }

        [TermName("CalculationTypeID")]
        [Display(Name = "CalculationTypeID")]
        public int CalculationTypeID { get; set; }

        [TermName("PlanID")]
        [Display(Name = "PlanID")]
        public int PlanID { get; set; }

        [TermName("MinValue")]
        [Display(Name = "MinValue")]
        public decimal  MinValue { get; set; }

        [TermName("MaxValue")]
        [Display(Name = "MaxValue")]
        public decimal MaxValue { get; set; }

        [TermName("DateModified")]
        [Display(Name = "DateModified")]
        public DateTime DateModified { get; set; }

        //R2841 - HUNDRED(JICM) - CORRECCIÓN INTEGRACIÓN BR 
        [Display(AutoGenerateField = false)]
        public string CalculationTypeDescription { get; set; }
        [Display(AutoGenerateField = false)]
        public bool isDelete { get; set; }
    }
}
