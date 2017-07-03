// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MappingRepository.cs" company="NetSteps">
//   Copyright 2012, NetSteps, LLC
// </copyright>
// <summary>
//   The Mapping Repository
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace HttpRedirectModule
{
    using System.Linq;

    using HttpRedirectModule.Entities;

    using NetSteps.Encore.Core.IoC;

    /// <summary>
    /// The Mapping Repository
    /// </summary>
    [ContainerRegister(typeof(IMappingRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public class MappingRepository : IMappingRepository
    {
        /// <summary>
        /// Gets the new site url based on the old website name
        /// </summary>
        /// <param name="oldSiteName">
        /// The old site name.
        /// </param>
        /// <returns>
        /// returns a string containing the new Site URL
        /// </returns>
        public string GetNewSiteURL(string oldSiteName)
        {
            using (var context = new MicheCoreEntities())
            {
                var matchingSites = context.RedirectMappingDatas.FirstOrDefault(x => x.OldWebsiteName == oldSiteName);

                if (matchingSites == null)
                {
                    return null;
                }
                    
                var newSiteURL = context.SiteUrls.FirstOrDefault(x => x.SiteID == matchingSites.SiteID);

                return newSiteURL == null ? null : newSiteURL.Url;
            }
        }
    }
}