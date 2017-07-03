using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetSteps.Common;
using NetSteps.Common.EldResolver;
using NetSteps.Sites.Common;
using NetSteps.Sites.Common.Configuration;
using NetSteps.Sites.Common.Models;
using NetSteps.Sites.Common.Repositories;

namespace NetSteps.Sites.Service.Tests
{
    /// <summary>
    /// The site service tests.
    /// </summary>
    [TestClass]
    public class SiteServiceTests
    {
        /// <summary>
        /// The expect all values on settings object.
        /// </summary>
        [TestMethod]
        public void ExpectAllValuesOnSettingsObject()
        {
            // Arrange
			var siteSettingRepository = new Mock<ISiteSettingRepository>();
			siteSettingRepository.Setup(r => r.GetSiteSettings(It.IsAny<int>())).Returns(new Dictionary<string, string>
			{
				{ "BaseGoogleAnalyticsTrackerID", "UA-3" },
				{ "GoogleAnalyticsTrackerID", "UA-4" },
			});

            var analyticsConfiguration = new Mock<IAnalyticsConfiguration>();
            analyticsConfiguration.SetupGet(c => c.IsDebug).Returns(true);
            analyticsConfiguration.SetupGet(c => c.PropertyIds).Returns(new List<string> { "UA-1", "UA-2" });

            var eldResolver = new Mock<IEldResolver>();
            eldResolver.Setup(r => r.EldDecode(It.IsAny<UriBuilder>())).Returns((UriBuilder u) => u);

            var siteService = GetSiteService(
				siteSettingRepository: siteSettingRepository.Object,
				analyticsConfiguration: analyticsConfiguration.Object,
				eldResolver: eldResolver.Object
			);

            // Act
			var result = siteService.GetGoogleAnalyticsSettings(1);

            // Assert
			Assert.IsNotNull(result);
            Assert.AreEqual(true, result.IsDebug);
            Assert.AreEqual(4, result.PropertyIds.Count);
            Assert.IsTrue(result.PropertyIds.Contains("UA-1"));
            Assert.IsTrue(result.PropertyIds.Contains("UA-2"));
            Assert.IsTrue(result.PropertyIds.Contains("UA-3"));
            Assert.IsTrue(result.PropertyIds.Contains("UA-4"));
        }

		private SiteService GetSiteService(
			ISiteRepository siteRepository = null,
			ISiteSettingRepository siteSettingRepository = null,
			ISitesConfiguration sitesConfiguration = null,
			IAnalyticsConfiguration analyticsConfiguration = null,
			IEldResolver eldResolver = null)
		{
			return new SiteService(
				siteRepository: siteRepository ?? new Mock<ISiteRepository>().Object,
				siteSettingRepository: siteSettingRepository ?? new Mock<ISiteSettingRepository>().Object,
				sitesConfiguration: sitesConfiguration ?? new Mock<ISitesConfiguration>().Object,
				analyticsConfiguration: analyticsConfiguration ?? new Mock<IAnalyticsConfiguration>().Object,
				eldResolver : eldResolver ?? new Mock<IEldResolver>().Object
			);
		}
    }
}
