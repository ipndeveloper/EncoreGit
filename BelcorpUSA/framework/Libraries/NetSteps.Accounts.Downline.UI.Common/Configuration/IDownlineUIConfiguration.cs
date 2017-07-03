using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Accounts.Downline.UI.Common.InfoCard;

namespace NetSteps.Accounts.Downline.UI.Common.Configuration
{
	[ContractClass(typeof(Contracts.DownlineUIConfigurationContracts))]
	public interface IDownlineUIConfiguration
	{
		ICollection<IDownlineInfoCardExtension> InfoCardExtensions { get; }
		IEnumerable<string> GetInfoCardListItemKeysToDisplay(IDownlineInfoCardContext context, IEnumerable<string> registeredItemKeys);
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(IDownlineUIConfiguration))]
		internal abstract class DownlineUIConfigurationContracts : IDownlineUIConfiguration
		{
			ICollection<IDownlineInfoCardExtension> IDownlineUIConfiguration.InfoCardExtensions
			{
				get { throw new NotImplementedException(); }
			}

			IEnumerable<string> IDownlineUIConfiguration.GetInfoCardListItemKeysToDisplay(IDownlineInfoCardContext context, IEnumerable<string> registeredItemKeys)
			{
				Contract.Requires<ArgumentNullException>(context != null);
				Contract.Requires<ArgumentNullException>(registeredItemKeys != null);
				Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);
				throw new NotImplementedException();
			}
		}
	}
}
