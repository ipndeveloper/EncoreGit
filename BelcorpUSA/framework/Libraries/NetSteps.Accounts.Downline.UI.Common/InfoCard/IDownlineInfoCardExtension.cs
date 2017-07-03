using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace NetSteps.Accounts.Downline.UI.Common.InfoCard
{
	[ContractClass(typeof(Contracts.DownlineInfoCardExtensionContracts))]
	public interface IDownlineInfoCardExtension
	{
		void LoadData(IDownlineInfoCardContext context);
		void InitializeItemProviders(IDictionary<string, IDownlineInfoCardItemProvider> providers);
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(IDownlineInfoCardExtension))]
		internal abstract class DownlineInfoCardExtensionContracts : IDownlineInfoCardExtension
		{
			void IDownlineInfoCardExtension.LoadData(IDownlineInfoCardContext context)
			{
				Contract.Requires<ArgumentNullException>(context != null);
				throw new NotImplementedException();
			}

			void IDownlineInfoCardExtension.InitializeItemProviders(IDictionary<string, IDownlineInfoCardItemProvider> providers)
			{
				Contract.Requires<ArgumentNullException>(providers != null);
				throw new NotImplementedException();
			}
		}
	}
}
