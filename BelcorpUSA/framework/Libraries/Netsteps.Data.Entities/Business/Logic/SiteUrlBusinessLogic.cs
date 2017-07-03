using System;
using NetSteps.Common.Globalization;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Validation.NetTiers;
using NetSteps.Data.Entities.Business.Logic.Interfaces;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class SiteUrlBusiness : BusinessLogicBase<SiteUrl, Int32, ISiteUrlRepository, ISiteUrlBusinessLogic>, ISiteUrlBusinessLogic, IDefaultImplementation
    {
        public override void AddValidationRules(SiteUrl Entity)
        {
            base.AddValidationRules(Entity);

            Entity.ValidationRules.AddRule(CommonRules.GreaterThanValue<int>, new CommonRules.CompareValueRuleArgs<int>("SiteID", 0));
            Entity.ValidationRules.AddRule(CommonRules.StringRequired, "Url");
            Entity.ValidationRules.AddRule(CommonRules.RegexIsMatch, new CommonRules.RegexRuleArgs("EmailAddress", @"^(http|https|s?ftps?)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*$", Translation.GetTerm("InvalidEmailErrorMessage", CustomValidationMessages.Email), true));
        }
    }
}
