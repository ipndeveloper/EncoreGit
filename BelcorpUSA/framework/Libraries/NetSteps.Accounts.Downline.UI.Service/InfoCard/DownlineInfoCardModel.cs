using NetSteps.Accounts.Downline.UI.Common.InfoCard;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Mvc.Models;

namespace NetSteps.Accounts.Downline.UI.Service.InfoCard
{
	[ContainerRegister(typeof(IDownlineInfoCardModel), RegistrationBehaviors.Default)]
	public class AccountInfoCardModel : DynamicViewModel, IDownlineInfoCardModel { }
}
