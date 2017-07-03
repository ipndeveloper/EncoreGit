using NetSteps.Common.Interfaces;
using NetSteps.Common.Models;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Mvc.Helpers;

namespace DistributorBackOffice.Infrastructure
{
    [ContainerRegister(typeof(ILocalizationInfoProvider), RegistrationBehaviors.OverrideDefault)]
    public class LocalizationInfoProvider : ILocalizationInfoProvider
    {
        public ILocalizationInfo GetLocalizationInfo()
        {
            var localizationInfo = Create.New<ILocalizationInfo>();
            localizationInfo.CultureName = CoreContext.CurrentCultureInfo.Name;
            localizationInfo.LanguageId = CoreContext.CurrentLanguageID;
            return localizationInfo;
        }
    }
}