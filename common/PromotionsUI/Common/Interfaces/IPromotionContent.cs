using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Promotions.UI.Common.Interfaces
{
    [DTO]
    public interface IPromotionContent
    {
        int PromotionContentID { get; set; }
        int PromotionID { get; set; }
        int LanguageID { get; set; }
        string Title { get; set; }
        string ActionText { get; set; }
        string Description { get; set; }
        string ImagePath { get; set; }
        string AlertTitle { get; set; }
    }
}
