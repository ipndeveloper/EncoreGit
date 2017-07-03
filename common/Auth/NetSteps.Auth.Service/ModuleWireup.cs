using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Auth.Service;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.IoC;
using NetSteps.Auth.Common;
using NetSteps.Auth.Common.CoreImplementations;
using NetSteps.Auth.Service.Providers;
using System.Configuration;
using System.Reflection;
using NetSteps.Auth.Common.Configuration;

[assembly: Wireup(typeof(NetSteps.Auth.Service.ModuleWireup))]

namespace NetSteps.Auth.Service
{
	[WireupDependency(typeof(NetSteps.Auth.Common.ModuleWireup))]
	public class ModuleWireup : WireupCommand
	{
		public static void PerformWireup()
		{
			Container.Current.ForType<IAuthenticationProvider>()
				.RegisterWithName<AccountIDAuthenticationProvider>(EncoreAuthenticationProviderNames.AccountIDProvider, (c, p) => { return new AccountIDAuthenticationProvider(() => { return Create.New<IAuthenticationStore>(); }); })
				.ResolveAsSingleton()
				.End();

			Container.Current.ForType<IAuthenticationProvider>()
				.RegisterWithName<CorporateUsernameAuthenticationProvider>(EncoreAuthenticationProviderNames.CorporateUsernameProvider, (c, p) => { return new CorporateUsernameAuthenticationProvider(() => { return Create.New<IAuthenticationStore>(); }); })
				.ResolveAsSingleton()
				.End();

			Container.Current.ForType<IAuthenticationProvider>()
				.RegisterWithName<UsernameAuthenticationProvider>(EncoreAuthenticationProviderNames.UsernameProvider, (c, p) => { return new UsernameAuthenticationProvider(() => { return Create.New<IAuthenticationStore>(); }); })
				.ResolveAsSingleton()
				.End();

			Container.Current.ForType<IAuthenticationProvider>()
				.RegisterWithName<EmailAddressAuthenticationProvider>(EncoreAuthenticationProviderNames.EmailAddressProvider, (c, p) => { return new EmailAddressAuthenticationProvider(() => { return Create.New<IAuthenticationStore>(); }); })
				.ResolveAsSingleton()
				.End();

			
			AuthenticationConfiguration config = null;
			var section = ConfigurationManager.GetSection("netStepsAuthentication");
			if (section != null)
				config = section as AuthenticationConfiguration;
			Container.Current.ForType<IAuthenticationProviderManager>()
				.Register<AuthenticationProviderManager>((c,p) => { return new AuthenticationProviderManager(config); })
				.ResolveAnInstancePerRequest()
				.End();

			Container.Current.ForType<IAuthenticationService>()
				.Register<AuthenticationService>(Param.Resolve<IAuthenticationProviderManager>())
				.ResolveAsSingleton()
				.End();

		}

		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			PerformWireup();
		}
	}
}
