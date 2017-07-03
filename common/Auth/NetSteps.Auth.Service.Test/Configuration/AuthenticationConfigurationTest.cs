using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using NetSteps.Auth.Common.Configuration;
using NetSteps.Auth.Common;

namespace NetSteps.Auth.Service.Test.Configuration
{
	[TestClass]
	public class AuthenticationConfigurationTest
	{
        [TestMethod]
        public void AuthenticationConfiguration_should_retrieve_configuration_settings1()
        {
            // This integration test reads from configuration settings in the Test Project's App.Config.  
            Assert.IsNotNull(ConfigurationManager.GetSection("netStepsAuthentication1"));
            var config = ConfigurationManager.GetSection("netStepsAuthentication1") as AuthenticationConfiguration;
            Assert.IsNotNull(config);
            Assert.AreEqual(4, config.Providers.Count);
            Assert.AreEqual(EncoreAuthenticationProviderNames.AccountIDProvider, config.Providers[0].Name);
            Assert.AreEqual(EncoreAuthenticationProviderNames.CorporateUsernameProvider, config.Providers[1].Name);
            Assert.AreEqual(EncoreAuthenticationProviderNames.EmailAddressProvider, config.Providers[2].Name);
            Assert.AreEqual(EncoreAuthenticationProviderNames.UsernameProvider, config.Providers[3].Name);
            Assert.AreEqual(true, config.AdminSettings.GetValue(DefaultAdminSettingKinds.EnableForgotPassword));
            Assert.AreEqual(false, config.AdminSettings.GetValue(DefaultAdminSettingKinds.EnableFormUsernameField));
        }

        [TestMethod]
        public void AuthenticationConfiguration_should_retrieve_configuration_settings2()
        {
            // This integration test reads from configuration settings in the Test Project's App.Config.  
            Assert.IsNotNull(ConfigurationManager.GetSection("netStepsAuthentication2"));
            var config = ConfigurationManager.GetSection("netStepsAuthentication2") as AuthenticationConfiguration;
            Assert.IsNotNull(config);
            Assert.AreEqual(3, config.Providers.Count);
            Assert.AreEqual(EncoreAuthenticationProviderNames.EmailAddressProvider, config.Providers[0].Name);
            Assert.AreEqual(EncoreAuthenticationProviderNames.AccountIDProvider, config.Providers[1].Name);
            Assert.AreEqual(EncoreAuthenticationProviderNames.CorporateUsernameProvider, config.Providers[2].Name);
            Assert.AreEqual(false, config.AdminSettings.GetValue(DefaultAdminSettingKinds.EnableForgotPassword));
            Assert.AreEqual(true, config.AdminSettings.GetValue(DefaultAdminSettingKinds.EnableFormUsernameField));
        }

        [TestMethod]
        public void AuthenticationConfiguration_should_retrieve_configuration_settings3()
        {
            // This integration test reads from configuration settings in the Test Project's App.Config.  
            Assert.IsNotNull(ConfigurationManager.GetSection("netStepsAuthentication3"));
            var config = ConfigurationManager.GetSection("netStepsAuthentication3") as AuthenticationConfiguration;
            Assert.IsNotNull(config);
            Assert.AreEqual(2, config.Providers.Count);
            Assert.AreEqual(EncoreAuthenticationProviderNames.AccountIDProvider, config.Providers[0].Name);
            Assert.AreEqual(EncoreAuthenticationProviderNames.EmailAddressProvider, config.Providers[1].Name);
            Assert.AreEqual(false, config.AdminSettings.GetValue(DefaultAdminSettingKinds.EnableForgotPassword));
            Assert.AreEqual(false, config.AdminSettings.GetValue(DefaultAdminSettingKinds.EnableFormUsernameField));
        }

    }
}
