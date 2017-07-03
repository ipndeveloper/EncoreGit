using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Accounts.Downline.UI.Common.Configuration;
using NetSteps.Accounts.Downline.UI.Common.InfoCard;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Accounts.Downline.UI.Service.Configuration
{
	[ContainerRegister(typeof(IDownlineUIConfiguration), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class DownlineUIConfiguration : IDownlineUIConfiguration
	{
		public virtual ICollection<IDownlineInfoCardExtension> InfoCardExtensions { get; protected set; }
		protected virtual ICollection<string> InfoCardListItemKeys { get; private set; }

		public DownlineUIConfiguration()
		{
			InfoCardExtensions = GetInfoCardExtensionsFromConfig().ToList();
			InfoCardListItemKeys = GetInfoCardListItemKeysFromConfig().ToList();
		}

		public virtual IEnumerable<string> GetInfoCardListItemKeysToDisplay(IDownlineInfoCardContext context, IEnumerable<string> registeredItemKeys)
		{
			return InfoCardListItemKeys.Any()
				? InfoCardListItemKeys
				: registeredItemKeys;
		}

		#region Helpers
		public virtual DownlineUIConfigurationSection GetConfig()
		{
			return ConfigurationManager.GetSection("netsteps.accounts.downline.ui") as DownlineUIConfigurationSection;
		}

		public virtual IEnumerable<IDownlineInfoCardExtension> GetInfoCardExtensionsFromConfig()
		{
			Contract.Ensures(Contract.Result<IEnumerable<IDownlineInfoCardExtension>>() != null);

			var config = GetConfig();
			if (config == null
				|| config.InfoCardExtensions == null)
			{
				yield break;
			}

			foreach (InfoCardExtensionElement infoCardExtensionElement in config.InfoCardExtensions)
			{
				yield return (IDownlineInfoCardExtension)Activator.CreateInstance(Type.GetType(infoCardExtensionElement.Type, true));
			}
		}
		
		public virtual IEnumerable<string> GetInfoCardListItemKeysFromConfig()
		{
			Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);

			var config = GetConfig();
			if (config == null
				|| config.InfoCardListItems == null)
			{
				yield break;
			}

			foreach (InfoCardListItemElement infoCardListItemElement in config.InfoCardListItems)
			{
				yield return infoCardListItemElement.Key;
			}
		}
		#endregion
	}
}
