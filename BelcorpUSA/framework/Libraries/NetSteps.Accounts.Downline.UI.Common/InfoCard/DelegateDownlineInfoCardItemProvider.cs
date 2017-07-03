using System;
using System.Diagnostics.Contracts;

namespace NetSteps.Accounts.Downline.UI.Common.InfoCard
{
	public class DelegateDownlineInfoCardItemProvider : IDownlineInfoCardItemProvider
	{
		private readonly Func<IDownlineInfoCardContext, object> _getItemFunc;

		public DelegateDownlineInfoCardItemProvider(Func<IDownlineInfoCardContext, object> getItemFunc)
		{
			Contract.Requires<ArgumentNullException>(getItemFunc != null);

			_getItemFunc = getItemFunc;
		}

		public object GetItem(IDownlineInfoCardContext context)
		{
			return _getItemFunc(context);
		}
	}
}
