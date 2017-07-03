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
    public class SupportMotivePropertyTypeSearchData
    {
        [Display(Name = "ID Motive Property")]
        public int SupportMotivePropertyTypeID { get; set; }
        
        [TermName("SupportMotiveID")]      
        public int SupportMotiveID { get; set; }
        
        [TermName("Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [TermName("DataType")]
        [Display(Name = "DataType")]
        public string DataType { get; set; }

        [TermName("Required")]
        [Display(Name = "Required")]
        public bool Required { get; set; }

        [TermName("SortIndex")]
        [Display(Name = "SortIndex")]
        public int SortIndex { get; set; }

        [TermName("Visible in WS")]
        [Display(Name = "Visible in WS")]
        public bool IsVisibleToWorkStation { get; set; }

        [TermName("Editable")]
        [Display(Name = "Editable")]
        public bool Editable { get; set; }

        [TermName("FiledSolution")]
        [Display(Name = "Solution")]
        public bool FieldSolution { get; set; }

        [TermName("SupportMotivePropertyDinamicID")]
        [Display(Name = "SupportMotivePropertyDinamicID")]
        public int SupportMotivePropertyDinamicID { get; set; }


    }
}
