using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;
using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class SupportMotivePropertyValuesSearchData
    {

        [TermName("SupportMotivePropertyValueID")]
        [Display(AutoGenerateField = false)]
        public int SupportMotivePropertyValueID { get; set; }


        [TermName("SupportMotivePropertyTypeID")]
        [Display(AutoGenerateField = false)]
        public int SupportMotivePropertyTypeID { get; set; }

        [TermName("Value")]
        [Display(Name = "Value")]
        public string Value { get; set; }

        [TermName("SortIndex")]
        [Display(Name = "Sort Index")]
        public int SortIndex { get; set; }
    }
}
