using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;
using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business
{
    /// <summary>
    /// Period Business Entity to Search
    /// </summary>
    [Serializable]
    public class SupportMotiveTaskSearchData
    {
        [Display(Name = "ID")]
        public int SupportMotiveTaskID { get; set; }
        
        [TermName("SupportMotiveID")]      
        public int SupportMotiveID { get; set; }
        
        [TermName("Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }
       
        [Display(Name = "Link")]
        public string Link { get; set; }

        [Display(AutoGenerateField = false)]
        public int SupportMotivePropertyTypeID { get; set; }

        [Display(AutoGenerateField = false)]
        public int SortIndex { get; set; }

        public string NameProperty { get; set; }
    }
}
