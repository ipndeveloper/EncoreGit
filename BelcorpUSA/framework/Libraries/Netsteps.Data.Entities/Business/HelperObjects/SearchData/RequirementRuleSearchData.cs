using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NetSteps.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    [Serializable]
    public class RequirementRuleSearchData
    {
        [TermName("RuleRequirementID")]
        [Display(AutoGenerateField = false)]
        public int RuleRequirementID { get; set; }

        [TermName("RuleTypeID")]
        [Display(AutoGenerateField = false)]
        public int RuleTypeID { get; set; }

        [TermName("PlanID")]
        [Display(AutoGenerateField = false)]
        public int PlanID { get; set; }

        [TermName("RuleName")]
        [Display(Name = "Rule Name")]
        public string RuleName { get; set; }

        [TermName("PlanName")]
        [Display(Name = "Plan Name")]
        public string PlanName { get; set; }

        [TermName("Description")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [TermName("Value1")]
        [Display(Name = "Value Condition 1")]
        public string  Value1 { get; set; }

        [TermName("ValueType1")]
        [Display(Name = "Condition Type 1")]
        public string ValueType1 { get; set; }

        [TermName("Value2")]
        [Display(Name = "Value Condition 2")]
        public string  Value2 { get; set; }

        [TermName("ValueType2")]
        [Display(Name = "Condition Type 2")]
        public string ValueType2 { get; set; }

        [TermName("Value3")]
        [Display(Name = "Value Condition 3")]
        public string Value3 { get; set; }

        [TermName("ValueType3")]
        [Display(Name = "Condition Type 3")]
        public string ValueType3 { get; set; }

        [TermName("Value4")]
        [Display(Name = "Value Condition 4")]
        public string Value4 { get; set; }

        [TermName("ValueType4")]
        [Display(Name = "Condition Type 4")]
        public string ValueType4 { get; set; }

    }
}
