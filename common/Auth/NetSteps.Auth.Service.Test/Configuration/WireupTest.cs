using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.IoC;
using NetSteps.Auth.Common;

namespace NetSteps.Auth.Service.Test.Configuration
{
	[TestClass]
	public class WireupTest
	{
		[TestMethod]
		public void AuthenticationService_should_wire_up_successfully()
		{
			// This integration test reads from configuration settings in the Test Project's App.Config.  
			WireupCoordinator.SelfConfigure();
			var manager = Create.New<IAuthenticationProviderManager>();
			Assert.AreEqual(4, manager.GetRegisteredProviderNames().Count());
			var names = manager.GetRegisteredProviderNames();
			Assert.AreEqual(EncoreAuthenticationProviderNames.AccountIDProvider, names.ElementAt(0));
			Assert.AreEqual(EncoreAuthenticationProviderNames.CorporateUsernameProvider, names.ElementAt(1));
			Assert.AreEqual(EncoreAuthenticationProviderNames.EmailAddressProvider, names.ElementAt(2));
			Assert.AreEqual(EncoreAuthenticationProviderNames.UsernameProvider, names.ElementAt(3));
		}
	}
}
