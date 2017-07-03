using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.XML;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common.Configuration;
using NetSteps.Enrollment.Common.Models;
using NetSteps.Enrollment.Common.Models.Config;

namespace NetSteps.Enrollment.XmlConfiguration
{
	[ContainerRegister(typeof(IEnrollmentConfigurationProvider), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class XmlEnrollmentConfigurationProvider : IEnrollmentConfigurationProvider
	{
		private readonly dynamic _config;
		private readonly Lazy<IEnumerable<short>> _enrollableAccountTypeIDsFactory;

		public XmlEnrollmentConfigurationProvider()
			: this(System.Web.Hosting.HostingEnvironment.MapPath("~/Enrollment.xml"))
		{
		}

		public XmlEnrollmentConfigurationProvider(string filePath)
		{
			if (!File.Exists(filePath))
			{
				return;
			}

			var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
			_config = new DynamicXElement(XElement.Parse(new StreamReader(stream).ReadToEnd()));
			stream.Close();

			_enrollableAccountTypeIDsFactory = new Lazy<IEnumerable<short>>(getEnrollableAccountTypeIDs);
		}

		public IEnumerable<short> getEnrollableAccountTypeIDs()
		{
			// WARNING: Must use implicit casting to ensure we get an IEnumerable (do not use 'var' here!)
			IEnumerable<dynamic> accountTypesEnumerable = _config
				.AccountTypes()
				.AccountType();

			return accountTypesEnumerable
				.Where(x => bool.Parse((string)x.Enabled))
				.Select(x => short.Parse((string)x.AccountTypeID))
				.ToList();
		}

		public bool AccountTypeEnabled(int accountTypeID)
		{
			return bool.Parse(GetDynamicAccountTypeConfig(accountTypeID).Enabled);
		}

		public IEnumerable<dynamic> GetProperties(IEnrollmentStep currentStep)
		{
			string controlName = Regex.Replace(currentStep.GetType().Name, "Step$", "");
			return GetProperties(controlName);
		}

		public IEnumerable<dynamic> GetProperties(string controlName)
		{
			dynamic control = _config.Controls().XPath(string.Format(".//Control[@Name='{0}']", controlName));
			if (control is DynamicXElement)
			{
				return control.Properties().Property();
			}

			return Enumerable.Empty<dynamic>();
		}

		public dynamic GetProperty(string controlName, string propertyName)
		{
			var properties = GetProperties(controlName).ToList();
			return properties
				.FirstOrDefault(x => x.Name == propertyName);
		}

		public List<short> GetEnabledAccountTypeIDs()
		{
			// WARNING: Must use implicit casting to ensure we get an IEnumerable (do not use 'var' here!)
			IEnumerable<dynamic> accountTypesEnumerable = _config
				.AccountTypes()
				.AccountType();

			return accountTypesEnumerable
				.Where(x => bool.Parse((string)x.Enabled))
				.Select(x => short.Parse((string)x.AccountTypeID))
				.ToList();
		}

		public IEnrollmentStepSectionConfig createEnrollmentStepSectionConfigFromDynamicStepConfig(dynamic dynamicStepConfig)
		{
			var returnValue = Create.New<IEnrollmentStepSectionConfig>();

			returnValue.Name = dynamicStepConfig.Name;
			returnValue.TermName = dynamicStepConfig.TermName;
			returnValue.Action = dynamicStepConfig.Action;
			string skippable = dynamicStepConfig.Skippable;
			returnValue.Skippable = !string.IsNullOrEmpty(skippable) && Convert.ToBoolean(skippable);

			return returnValue;
		}

		public IEnrollmentStepConfig createEnrollmentStepConfigFromDynamicStepConfig(dynamic dynamicStepConfig, int accountTypeID)
		{
			var returnValue = Create.New<IEnrollmentStepConfig>();

			returnValue.Name = dynamicStepConfig.Name;
			returnValue.TermName = dynamicStepConfig.TermName;
			returnValue.Controller = dynamicStepConfig.UseControl;

			string skippable = dynamicStepConfig.Skippable;
			returnValue.Skippable = !string.IsNullOrEmpty(skippable) && Convert.ToBoolean(skippable);
			returnValue.Sections =
				new OrderedList<IEnrollmentStepSectionConfig>(
					GetDynamicStepSectionConfigs(dynamicStepConfig as object, accountTypeID).Select(
						createEnrollmentStepSectionConfigFromDynamicStepConfig));

			return returnValue;
		}

		public IEnrollmentConfig GetEnrollmentConfig(int accountTypeID, int siteTypeID)
		{
			var accountTypeConfig = GetDynamicAccountTypeConfig(accountTypeID);
			var config = Create.New<IEnrollmentConfig>();

			config.SiteTypeID = siteTypeID;
			config.AccountTypeID = (short)accountTypeID;
			config.Enabled = bool.Parse((string)accountTypeConfig.Enabled);

			IEnumerable<dynamic> dynamicStepConfigs = GetDynamicStepConfigs(accountTypeID);
			IEnumerable<IEnrollmentStepConfig> enrollmentStepConfigs = dynamicStepConfigs.Select(x => (IEnrollmentStepConfig)createEnrollmentStepConfigFromDynamicStepConfig(x, accountTypeID));


			config.Steps = new OrderedList<IEnrollmentStepConfig>(enrollmentStepConfigs);

			config.Sponsor = GetSponsorConfig(accountTypeID);
			config.BasicInfo = GetBasicInfoConfig(accountTypeID);
			config.Billing = GetBillingConfig(accountTypeID);
			config.Website = GetWebsiteConfig();
			config.EnrollmentOrder = GetEnrollmentOrderConfig(accountTypeID);
			config.Autoship = GetAutoshipConfig(accountTypeID);
			config.Subscription = GetSubscriptionAutoshipConfig(accountTypeID);
			config.DisbursementProfiles = GetDisbursementConfig(accountTypeID);
			
			return config;
		}

		public string GetOverrideAssemblyName()
		{
			return _config.Assemblies().Assembly().Name;
		}

		public IEnumerable<short> EnrollableAccountTypeIDs
		{
			get { return _enrollableAccountTypeIDsFactory.Value; }
		}

		public dynamic GetDynamicAccountTypeConfig(int accountTypeID)
		{
			var accountTypeDynamic = _config
				.AccountTypes()
				.XPath(string.Format(".//AccountType[@AccountTypeID='{0}']", accountTypeID));

			try
			{
				if (accountTypeDynamic.AccountTypeID == "")
					throw new Exception();
			}
			catch
			{
				throw new Exception(string.Format("No enrollment config found for account type ID '{0}'", accountTypeID));
			}

			return accountTypeDynamic;
		}

		public IEnumerable<dynamic> GetDynamicStepConfigs(int accountTypeID)
		{
			// WARNING: Must use implicit casting to ensure we get an IEnumerable (do not use 'var' here!)
			IEnumerable<dynamic> stepConfigsEnumerable = GetDynamicAccountTypeConfig(accountTypeID)
				.Steps()
				.Step();

			if (!stepConfigsEnumerable.Any())
			{
				throw new Exception(string.Format("No enrollment steps found for account type ID '{0}'", accountTypeID));
			}

			return stepConfigsEnumerable;
		}

		public IEnumerable<dynamic> GetDynamicStepSectionConfigs(dynamic stepConfig, int accountTypeID)
		{
			var sectionsProperty = GetProperty(stepConfig.UseControl, "Sections");
			if (sectionsProperty == null)
			{
				return Enumerable.Empty<dynamic>();
			}

			// WARNING: Must use implicit casting to ensure we get an IEnumerable (do not use 'var' here!)
			IEnumerable<dynamic> sectionConfigsEnumerable = sectionsProperty
				.XPath(string.Format(".//Sections[@AccountTypeID='{0}']", accountTypeID))
				.Section();

			return sectionConfigsEnumerable;
		}

		public IEnrollmentSponsorConfig GetSponsorConfig(int accountTypeID)
		{
			var config = Create.New<IEnrollmentSponsorConfig>();

			dynamic property = GetProperty("Sponsor", "Sponsor");
			if (property != null)
			{
				dynamic dynamicConfig = property.XPath(string.Format(".//Sponsor[@AccountTypeID='{0}']", accountTypeID));
				if (dynamicConfig is DynamicXElement)
				{
					if (dynamicConfig.DenySponsorChange != "")
						config.DenySponsorChange = Convert.ToBoolean(dynamicConfig.DenySponsorChange);
				}
			}

			return config;
		}

		public IEnrollmentBasicInfoConfig GetBasicInfoConfig(int accountTypeID)
		{
			var config = Create.New<IEnrollmentBasicInfoConfig>();

			dynamic property = GetProperty("AccountInfo", "BasicInfo");
			if (property != null)
			{
				dynamic dynamicConfig = property.XPath(string.Format(".//BasicInfo[@AccountTypeID='{0}']", accountTypeID));
				if (dynamicConfig is DynamicXElement)
				{
					if (dynamicConfig.SetShippingAddressFromMain != "")
						config.SetShippingAddressFromMain = Convert.ToBoolean(dynamicConfig.SetShippingAddressFromMain);
					if (dynamicConfig.SetBillingAddressFromMain != "")
						config.SetBillingAddressFromMain = Convert.ToBoolean(dynamicConfig.SetBillingAddressFromMain);
				}
			}

			return config;
		}

		public IEnrollmentBillingConfig GetBillingConfig(int accountTypeID)
		{
			var config = Create.New<IEnrollmentBillingConfig>();

			dynamic property = GetProperty("AccountInfo", "Billing");
			if (property != null)
			{
				dynamic dynamicConfig = property.XPath(string.Format(".//Billing[@AccountTypeID='{0}']", accountTypeID));
				if (dynamicConfig is DynamicXElement)
				{
					if (dynamicConfig.HideBillingAddress != "")
						config.HideBillingAddress = Convert.ToBoolean(dynamicConfig.HideBillingAddress);
				}
			}

			return config;
		}

		public IEnrollmentWebsiteConfig GetWebsiteConfig()
		{
			var config = Create.New<IEnrollmentWebsiteConfig>();

			dynamic property = GetProperty("AccountInfo", "Website");
			if (property != null)
			{
				if (property.AutoshipScheduleID != "")
					config.AutoshipScheduleID = Convert.ToInt32(property.AutoshipScheduleID);
			}

			return config;
		}

		public IEnrollmentOrderConfig GetEnrollmentOrderConfig(int accountTypeID)
		{
			var config = Create.New<IEnrollmentOrderConfig>();

			dynamic property = GetProperty("Products", "InitialOrder");
			if (property != null)
			{
				dynamic dynamicConfig = property.XPath(string.Format(".//InitialOrder[@AccountTypeID='{0}']", accountTypeID));
				if (dynamicConfig is DynamicXElement)
				{
					if (dynamicConfig.ImportShoppingOrder != "")
						config.ImportShoppingOrder = Convert.ToBoolean(dynamicConfig.ImportShoppingOrder);
					if (dynamicConfig.SaveAsAutoshipOrder != "")
						config.SaveAsAutoshipOrder = Convert.ToBoolean(dynamicConfig.SaveAsAutoshipOrder);
					if (dynamicConfig.MinimumCommissionableTotal != "")
						config.MinimumCommissionableTotal = Convert.ToDecimal(dynamicConfig.MinimumCommissionableTotal);
				}
			}

			return config;
		}

		public IEnrollmentAutoshipConfig GetAutoshipConfig(int accountTypeID)
		{
			var config = Create.New<IEnrollmentAutoshipConfig>();

			dynamic property = GetProperty("Products", "Autoship");
			if (property != null)
			{
				dynamic dynamicConfig = property.XPath(string.Format(".//Autoship[@AccountTypeID='{0}']", accountTypeID));
				if (dynamicConfig is DynamicXElement)
				{
					if (dynamicConfig.AutoshipScheduleID != "")
						config.AutoshipScheduleID = Convert.ToInt32(dynamicConfig.AutoshipScheduleID);
					if (dynamicConfig.ImportShoppingOrder != "")
						config.ImportShoppingOrder = Convert.ToBoolean(dynamicConfig.ImportShoppingOrder);
					if (dynamicConfig.Hidden != "")
						config.Hidden = Convert.ToBoolean(dynamicConfig.Hidden);
					if (dynamicConfig.MinimumCommissionableTotal != "")
						config.MinimumCommissionableTotal = Convert.ToDecimal(dynamicConfig.MinimumCommissionableTotal);
					if (dynamicConfig.Skippable != "")
						config.Skippable = Convert.ToBoolean(dynamicConfig.Skippable);
				}
			}

			return config;
		}

		public IEnrollmentSubscriptionAutoshipConfig GetSubscriptionAutoshipConfig(int accountTypeID)
		{
			var config = Create.New<IEnrollmentSubscriptionAutoshipConfig>();

			dynamic property = GetProperty("Products", "Subscription");
			if (property != null)
			{
				dynamic dynamicConfig = property.XPath(string.Format(".//Subscription[@AccountTypeID='{0}']", accountTypeID));
				if (dynamicConfig is DynamicXElement)
				{
					if (dynamicConfig.AutoshipScheduleID != "")
						config.AutoshipScheduleID = Convert.ToInt32(dynamicConfig.AutoshipScheduleID);
				}
			}

			return config;
		}

		public IDisbursementProfilesConfig GetDisbursementConfig(int accountTypeID)
		{
			var config = Create.New<IDisbursementProfilesConfig>();

			IEnumerable<dynamic> disbProfilesProperties = GetProperties("DisbursementProfiles");
			if (disbProfilesProperties != null)
			{
				var hiddenValue = (string)disbProfilesProperties.FirstOrDefault(p => p.Name == "Hidden");
				config.Hidden = string.IsNullOrEmpty(hiddenValue) || bool.Parse(hiddenValue);
				var eftEnabledValue = (string)disbProfilesProperties.FirstOrDefault(p => p.Name == "EnableEFTProfile");
				config.EftEnabled = string.IsNullOrEmpty(eftEnabledValue) || bool.Parse(eftEnabledValue);
				var checkEnabledValue = (string)disbProfilesProperties.FirstOrDefault(p => p.Name == "EnableCheckProfile");
				config.CheckEnabled = string.IsNullOrEmpty(checkEnabledValue) || bool.Parse(checkEnabledValue);

			}
			else
			{
				config.Hidden = true;
				config.EftEnabled = false;
				config.CheckEnabled = false;
			}

			return config;
		}
	}
}
