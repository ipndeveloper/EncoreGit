using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    /// <summary>
    /// Plan Business Entity to Search
    /// </summary>
    [Serializable]
    public class PlanSearchData
    {
        //[Display(Name = "ID")]
        [Display(AutoGenerateField = false)]
        public int PlanID { get; set; }

        [TermName("Code")]
        [Display(Name = "Code")]
        public string PlanCode { get; set; }

        [TermName("Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [TermName("Enabled")]
        [Display(Name = "Enabled")]
        public bool Enabled { get; set; }

        [TermName("Default")]
        [Display(Name = "Default")]
        public bool DefaultPlan { get; set; }

        [Display(AutoGenerateField = false)]
        public string TermName { get; set; }
    }
}
