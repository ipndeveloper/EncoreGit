using System.Collections.Generic;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;

namespace nsCore.Areas.Communication.Models
{
    public class GeneralTemplateModel 
    {

        public NewPrintOrderSearchData NewPrintOrderSearchData { get; set; }
           
        public Dictionary<int, string> Languages { get; set; }

        public bool IsNewsLetterType { get; set; }

        public bool IsNewEmailTemplate { get; set; }

        public int LanguageCount
        {
            get
            {
                return Languages == null ? 0 : Languages.Count;
            }
        }
    }
}