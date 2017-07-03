using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
    public class OrderPaymentTemplatesSearchData
	{

        [Display(Name = "ID")]
        public int OrderPaymentTemplateId { get; set; }

        [TermName("Type", "Type")]
        [Display(Name = "Type")]
		public string Type { get; set; }

        [TermName("Description", "Description")]
        [Display(Name = "Description")]
        public string Description { get; set; }

		[TermName("Days", "Days")]
        [Display(Name = "Days")]
		public int Days { get; set; }

        [TermName("MinimalAmount", "Minimal Amount")]
        [Display(Name = "Minimal Amount")]
        public int MinimalAmount { get; set; }

        [TermName("EmailTemplateId", "EmailTemplateId")]
        [Display(AutoGenerateField = false)]
        public int EmailTemplateId { get; set; }

        [TermName("Template", "Template")]
        [Display(Name = "Email Template Name")]
        public string EmailTemplateName { get; set; }

      
	}
}
