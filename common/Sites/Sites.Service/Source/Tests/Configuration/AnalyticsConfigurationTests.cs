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
	public class AnalyticsConfigurationTests
	{
		[TestMethod]
		public void AnalyticsConfiguration_NullSection_Initializes()
		{
			var test = new
			{
				AnalyticsConfigurationSection = default(AnalyticsConfigurationSection),
				ExpectedPropertyIds = new string[] { },
				ExpectedIsDebug = false
			};

			var result = new AnalyticsConfiguration(test.AnalyticsConfigurationSection);

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.PropertyIds);
			Assert.AreEqual(test.ExpectedPropertyIds.Length, result.PropertyIds.Count);
			Assert.AreEqual(test.ExpectedIsDebug, result.IsDebug);
		}

		[TestMethod]
		public void AnalyticsConfiguration_FromConfig_Initializes()
		{
			var test = new
			{
				AnalyticsConfigurationSection = ConfigurationManager.GetSection("analytics") as AnalyticsConfigurationSection
			};

			// Ensure we got stuff from config.
			if (test.AnalyticsConfigurationSection == null || test.AnalyticsConfigurationSection.PropertyIds == null)
			{
				Assert.Inconclusive("Missing 'analytics' section in App.Config.");
			}

			var result = new AnalyticsConfiguration(test.AnalyticsConfigurationSection);

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.PropertyIds);
			Assert.AreEqual(test.AnalyticsConfigurationSection.PropertyIds.Count, result.PropertyIds.Count);
			int i = 0;
			foreach (PropertyIdElement propertyIdElement in test.AnalyticsConfigurationSection.PropertyIds)
			{
				Assert.AreEqual(propertyIdElement.PropertyId, result.PropertyIds.ElementAt(i));
				i++;
			}
			Assert.AreEqual(test.AnalyticsConfigurationSection.Debug, result.IsDebug);
		}
	}
}
