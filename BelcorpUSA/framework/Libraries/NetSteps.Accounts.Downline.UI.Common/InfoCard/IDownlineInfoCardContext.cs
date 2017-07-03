using System.Collections.Generic;
using NetSteps.Accounts.Downline.Common.Models;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Accounts.Downline.UI.Common.InfoCard
{
	[DTO]
	public interface IDownlineInfoCardContext
	{
		int RootAccountId { get; set; }
		IDownlineAccountInfo AccountInfo { get; set; }
		dynamic CustomData { get; set; }
		IDictionary<string, object> CustomDataDictionary { get; set; }
	}
}
