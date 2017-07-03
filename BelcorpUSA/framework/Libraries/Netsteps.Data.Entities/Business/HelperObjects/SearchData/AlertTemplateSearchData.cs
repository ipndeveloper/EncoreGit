using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public struct AlertTemplateSearchData
    {
        [TermName("ID")]
        [Display(AutoGenerateField = false)]
        public int AlertTemplateID { get; set; }

        [TermName("Name", "Name")]
        public string Name { get; set; }

        [TermName("StoredProcedureName", "Stored Procedure Name")]
        [Display(AutoGenerateField = false)]
        public string StoredProcedureName { get; set; }

        [TermName("AlertTemplatePriority", "Alert Template Priority")]
        [Display(AutoGenerateField = false)]
        public short AlertPriorityID { get; set; }

        [TermName("Active", "Active")]
        public bool Active { get; set; }
    }
}
