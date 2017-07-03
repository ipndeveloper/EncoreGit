using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;
using System.Collections.Generic;


namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class NewPrintOrderSearchData
    {
        [TermName("GeneralTemplateID")]
        [Display(AutoGenerateField = false)]
        public int GeneralTemplateID { get; set; }

        [TermName("Name", "Name")]
        public string Name { get; set; }

        [TermName("_GeneralTemplateSectionID", "_GeneralTemplateSectionID")]
        [Display(AutoGenerateField = false)]
        public short _GeneralTemplateSectionID { get; set; }

        [TermName("Section", "Section")]
        public string Section { get; set; }

        [TermName("Order", "Order")]
        public int Order { get; set; }

        [TermName("Statu", "Status")]
        public bool Statu { get; set; }

        [TermName("DateStar", "Date Star")]
        public DateTime? DateStar { get; set; }

        [TermName("DateEnd", "Date End")]
        public DateTime? DateEnd { get; set; }

        [Display(AutoGenerateField = false)]
        public short GeneralTemplateSectionID { get; set; }

        [TermName("Period", "Period")]
        public int? Period { get; set; }

        [Display(AutoGenerateField = false)]
        public List<GeneralTemplateTranslations> GeneralTemplateTranslations  { get; set; }

        [Display(AutoGenerateField = false)]
        public NewPrintOrderGeneralTemplateSection GeneralTemplateSection  { get; set; }

        [Display(AutoGenerateField = false)]
        public GeneralTemplateTranslations objGeneralTemplateTranslations { get; set; }
    }

    public class NewPrintOrderPeriodo 
    {
        public int PeriodID { get; set; }
    }

    public class NewPrintOrderGeneralTemplateSection
    {
        public Int16 GeneralTemplateSectionID { get; set; }
        public string Name { get; set; }
    }

    public class GeneralTemplateTranslations
    {
         public int GeneralTemplateTranslationID { get; set; }
        public int GeneralTemplateID { get; set; }
        public int LanguageID { get; set; }
        public string Body { get; set; }
        public bool Active { get; set; }
    }
}
