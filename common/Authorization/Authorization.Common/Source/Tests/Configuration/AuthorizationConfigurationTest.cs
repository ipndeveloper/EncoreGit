using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using NetSteps.Authorization.Common.Configuration;
using NetSteps.Authorization.Common;

namespace NetSteps.Authorization.Common.Test.Configuration
{
	[TestClass]
	public class AuthorizationConfigurationTest
	{
		[TestMethod]
		public void AuthorizationConfiguration_should_retrieve_configuration_settings()
		{
			// This integration test reads from configuration settings in the Test Project's App.Config.  
			Assert.IsNotNull(ConfigurationManager.GetSection("netStepsAuthorization"));
			var config = ConfigurationManager.GetSection("netStepsAuthorization") as AuthorizationConfiguration;
			Assert.IsNotNull(config);
			Assert.AreEqual("Commissions", config.Functions[0].Name);
			Assert.AreEqual("Commissions-Run Commissions", config.Functions[1].Name);
			Assert.AreEqual("Commissions-Publish Commissions", config.Functions[2].Name);
		}
	}
}
