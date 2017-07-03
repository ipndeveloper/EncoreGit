using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NetSteps.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    [Serializable]
    public class RuleTypeSearchData
    {


        [TermName("RuleTypeID")]
        [Display(AutoGenerateField = false)]
        public int RuleTypeID { get; set; }

        [TermName("Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [TermName("TermName")]
        [Display(Name = "Term Name")]
        public string TermName { get; set; }

        [TermName("Description")]
        [Display(Name = "Description")]
        public string Description { get; set; }


        [TermName("ValueType1")]
        [Display(Name = "Condition Type 1")]
        public string ValueType1 { get; set; }

        [TermName("ValueType2")]
        [Display(Name = "Condition Type 2")]
        public string ValueType2 { get; set; }

        [TermName("ValueType3")]
        [Display(Name = "Condition Type 3")]
        public string ValueType3 { get; set; }

        [TermName("ValueType4")]
        [Display(Name = "Condition Type 4")]
        public string ValueType4 { get; set; }
    }
}
