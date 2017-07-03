using System.Collections.Generic;
using NetSteps.Data.Entities;

namespace nsCore.Areas.Communication.Models
{
    public class AlertTemplateViewModel
    {
        public AlertTemplate AlertTemplate { get; set; }

        public Dictionary<int, string> Languages { get; set; }

        public AlertTemplateTranslation CurrentTemplateTranslation { get; set; }

        public int LanguageCount
        {
            get
            {
                return Languages == null ? 0 : Languages.Count;
            }
        }


    }
}