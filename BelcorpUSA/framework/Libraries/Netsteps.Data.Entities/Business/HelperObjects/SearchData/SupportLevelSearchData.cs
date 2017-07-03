using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    /// <summary>
    /// Period Business Entity to Search
    /// </summary>
    [Serializable]
    public class SupportLevelSearchData
    {
        [Display(Name = "ID")]
        public int SupportLevelID { get; set; }

        [TermName("Name")]
        [Display(Name = "ParentSupportLevelID")]
        public int ParentSupportLevelID { get; set; }

        [TermName("Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [TermName("Description")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [TermName("TermName")]
        [Display(Name = "TermName")]
        public string TermName { get; set; }

        public int SortIndex { get; set; }

        public bool IsVisibleToWorkStation { get; set; }

        public bool Active { get; set; }

        
        [Display(AutoGenerateField = false)]
        public bool HasChild { get; set; }
    }
}
