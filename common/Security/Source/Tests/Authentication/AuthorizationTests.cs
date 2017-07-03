using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Security.Authentication;

namespace NetSteps.Security.Tests.Authentication
{
	/// <summary>
	/// Summary description for AuthorizationTests
	/// </summary>
	[TestClass]
	public class AuthorizationTests
	{
		[TestMethod]
		public void DefaultAuthorizationProviders_Is_Default_IAuthorizationProviders_For_IoC()
		{
			WireupCoordinator.Instance.WireupDependencies(typeof(IAuthorizationProviders).Assembly);
			IAuthorizationProviders providers = Create.New<IAuthorizationProviders>();

			Assert.IsInstanceOfType(providers, typeof(DefaultAuthorizationProviders));
		}
	}
}
