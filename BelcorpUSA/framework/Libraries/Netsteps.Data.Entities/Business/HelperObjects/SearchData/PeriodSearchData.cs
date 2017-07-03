using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    /// <summary>
    /// Period Business Entity to Search
    /// </summary>
    [Serializable]
    public class PeriodSearchData
    {
        [Display(Name = "ID")]
        public int PeriodID { get; set; }

        [TermName("PlanName")]
        [Display(Name = "Plan name")]
        public string PlanName { get; set; }

        [TermName("Description")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [TermName("StartDate")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        
        [TermName("LockDate")]
        [Display(Name = "Lock Date")]
        public DateTime LockDate { get; set; }

        [TermName("EndDate")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [TermName("ClosedDate")]
        [Display(Name = "Closed Date")]
        public Nullable<DateTime> ClosedDate { get; set; }

        [Display(AutoGenerateField = false)]
        public int PlanID { get; set; }

        [Display(AutoGenerateField = false)]
        public bool? EarningsViewable { get; set; }

        [Display(AutoGenerateField = false)]
        public DateTime? BackOfficeDisplayStartDate { get; set; }

        [Display(AutoGenerateField = false)]
        public bool? DisbursementsProcessed { get; set; }

        [Display(AutoGenerateField = false)]
        public DateTime StartDateUTC { get; set; }

        [Display(AutoGenerateField = false)]
        public DateTime EndDateUTC { get; set; }

        [Display(AutoGenerateField = false)]
        public bool ExistPreviousStartDate { get; set; }

    }

    public class PeriodSelect
    {
        public int PeriodID { get; set; }
        public string Name { get; set; }
    }
}
