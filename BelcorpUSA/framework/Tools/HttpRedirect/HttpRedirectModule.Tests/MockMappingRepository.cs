// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockMappingRepository.cs" company="NetSteps">
//   Copyright 2012, NetSteps, LLC
// </copyright>
// <summary>
//   A Mock Mapping Repository
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using NetSteps.Encore.Core.Wireup.Meta;

namespace HttpRedirectModule.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using HttpRedirectModule.Entities;
    using HttpRedirectModule.Models;

    using NetSteps.Encore.Core.IoC;

    /// <summary>
    /// A Mock Mapping Repository
    /// </summary>
    public class MockMappingRepository : IMappingRepository
    {
        /// <summary>
        /// Gets the new site based on mapping results
        /// </summary>
        /// <param name="oldSiteName">
        /// The old site name.
        /// </param>
        /// <returns>
        /// returns a string
        /// </returns>
        public string GetNewSiteURL(string oldSiteName)
        {
            var mockListSite = this.GetMockSitesList();
            var retVal = mockListSite.Where(x => x.OldSiteName == oldSiteName).Select(x => x.NewSiteURL).FirstOrDefault();
            return retVal;
        }

        /// <summary>
        /// get mock list of sites
        /// </summary>
        /// <returns>
        /// returns a mock list of sites
        /// </returns>
        private IEnumerable<MappingModel> GetMockSitesList()
        {
            return new List<MappingModel>
                {
                    new MappingModel
                        {
                            AccountID = 1,
                            NewSiteURL = "http://hulk.micheunittest.com/",
                            OldSiteName = "BruceBanner",
                            SiteID = 1001
                        },
                    new MappingModel
                        {
                            AccountID = 2,
                            NewSiteURL = "http://ironman.micheunittest.com/",
                            OldSiteName = "TonyStark",
                            SiteID = 1002
                        },
                    new MappingModel
                        {
                            AccountID = 3,
                            NewSiteURL = "http://spiderman.micheunittest.com/",
                            OldSiteName = "PeterParker",
                            SiteID = 1003
                        }
                };
        }
    }
}