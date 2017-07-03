using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetSteps.Sites.Service.Configuration;

namespace NetSteps.Sites.Service.Tests.Configuration
{
	[TestClass]
	public class SitesConfigurationSectionTests
	{
		[TestMethod]
		public void SiteTypeID_DefaultsToZero()
		{
			var test = new
			{
				ExpectedSiteTypeID = 0
			};
			
			var target = new SitesConfigurationSection();

			Assert.AreEqual(test.ExpectedSiteTypeID, target.SiteTypeID);
		}
	}
}
