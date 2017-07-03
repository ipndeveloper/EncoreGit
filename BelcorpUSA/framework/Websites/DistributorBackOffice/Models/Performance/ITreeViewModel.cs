using NetSteps.Accounts.Downline.UI.Common.InfoCard;
using NetSteps.Accounts.Downline.UI.Common.TreeView;
using NetSteps.Encore.Core.Dto;

namespace DistributorBackOffice.Models.Performance
{
	[DTO]
	public interface ITreeViewModel
	{
		ITreeNodeModel RootNode { get; set; }
		IDownlineInfoCardModel DownlineInfoCard { get; set; }
	}
}
