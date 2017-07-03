using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Enrollment.Common.Models.Config;
using NetSteps.Enrollment.XmlConfiguration;

namespace XmlConfigurationTests
{
	[TestClass]
	public class XmlEnrollmentConfigurationTests
	{
		[TestMethod]
		public void CanReadXmlFile()
		{
			var provider = new XmlEnrollmentConfigurationProvider(getTestEnrollmentFilePath());
		}

		[TestMethod]
		public void AccountTypeEnabled_ConfigurationFile()
		{
			var provider = new XmlEnrollmentConfigurationProvider(getTestEnrollmentFilePath());

			bool isOneEnabled = provider.AccountTypeEnabled(1);
			bool isTwoEnabled = provider.AccountTypeEnabled(2);
			bool isThreeEnabled = provider.AccountTypeEnabled(3);

			Assert.IsTrue(isOneEnabled);
			Assert.IsFalse(isTwoEnabled);
			Assert.IsFalse(isThreeEnabled);
		}

		[TestMethod]
		public void GetEnrollmentConfig_SponsorConfig()
		{
			var config = getDefaultEnrollmentConfig();

			Assert.IsTrue(config.Sponsor.DenySponsorChange);
		}

		[TestMethod]
		public void GetEnrollmentConfig_Steps()
		{
			var config = getDefaultEnrollmentConfig();

			bool hasSponsor = config.Steps.Any(s => s.Name == "Choose Your Sponsor");
			bool hasAccountInfo = config.Steps.Any(s => s.Name == "Personal Information");
			bool hasProducts = config.Steps.Any(s => s.Name == "Enrollment Products");
			bool hasAgreements = config.Steps.Any(s => s.Name == "Agreements");
			bool hasReview = config.Steps.Any(s => s.Name == "Review");
			bool hasReceipt = config.Steps.Any(s => s.Name == "Receipt");

			Assert.IsTrue(hasSponsor);
			Assert.IsTrue(hasAccountInfo);
			Assert.IsTrue(hasProducts);
			Assert.IsTrue(hasAgreements);
			Assert.IsTrue(hasReview);
			Assert.IsTrue(hasReceipt);
		}

		[TestMethod]
		public void GetEnrollmentConfig_BasicInfo_BlankXmlDefaultsFalse()
		{
			var config = getDefaultEnrollmentConfig();

			Assert.IsFalse(config.BasicInfo.SetBillingAddressFromMain);
			Assert.IsFalse(config.BasicInfo.SetShippingAddressFromMain);
		}

		[TestMethod]
		public void GetEnrollmentConfig_DisbursementProfiles()
		{
			var config = getDefaultEnrollmentConfig();

			Assert.IsTrue(config.DisbursementProfiles.CheckEnabled);
			Assert.IsTrue(config.DisbursementProfiles.EftEnabled);
			Assert.IsFalse(config.DisbursementProfiles.Hidden);
		}

		private string getTestEnrollmentFilePath()
		{
			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Enrollment.xml");
			return filePath;
		}

		private IEnrollmentConfig getDefaultEnrollmentConfig()
		{
			return new XmlEnrollmentConfigurationProvider(getTestEnrollmentFilePath()).GetEnrollmentConfig(1, 1);
		}
	}
}
