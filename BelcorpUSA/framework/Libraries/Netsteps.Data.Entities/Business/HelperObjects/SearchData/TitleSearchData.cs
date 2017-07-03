using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NetSteps.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    [Serializable]
    public class TitleSearchData
    {
        [TermName("TitleID")]
        [Display(Name = "ID")]
        public int TitleID { get; set; }

        [TermName("TitleCode")]
        [Display(Name = "Code")]
        public string TitleCode { get; set; }

        [TermName("ClientName")]
        [Display(Name = "Client Name")]
        public string ClientName { get; set; }

        [TermName("ClientCode")]
        [Display(Name = "Client Code")]
        public string ClientCode { get; set; }

        [TermName("Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [TermName("SortOrder")]
        [Display(Name = "Sort Order")]
        public int SortOrder { get; set; }

        [TermName("RequirementTitleCalculations")]
        [Display(AutoGenerateField = false)]
        public List<RequirementTitleCalculationSearchData> RequirementTitleCalculations { get; set; }

        [TermName("RequirementLegs")]
        [Display(AutoGenerateField = false)]
        public List<RequirementLegSearchData> RequirementLegs { get; set; }


        [TermName("TitlePhase")]
        [Display(AutoGenerateField = false)]
        public int TitlePhaseID { get; set; }

        [Display(AutoGenerateField = false)]
        public string TermName { get; set; }
    }
  
}
