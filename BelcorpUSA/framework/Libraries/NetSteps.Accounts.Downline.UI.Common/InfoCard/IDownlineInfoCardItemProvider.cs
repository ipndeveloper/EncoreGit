using System;
using System.Diagnostics.Contracts;

namespace NetSteps.Accounts.Downline.UI.Common.InfoCard
{
	[ContractClass(typeof(Contracts.DownlineInfoCardItemProviderContracts))]
	public interface IDownlineInfoCardItemProvider
	{
		object GetItem(IDownlineInfoCardContext context);
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(IDownlineInfoCardItemProvider))]
		internal abstract class DownlineInfoCardItemProviderContracts : IDownlineInfoCardItemProvider
		{
			object IDownlineInfoCardItemProvider.GetItem(IDownlineInfoCardContext context)
			{
				Contract.Requires<ArgumentNullException>(context != null);
				throw new NotImplementedException();
			}
		}
	}
}
