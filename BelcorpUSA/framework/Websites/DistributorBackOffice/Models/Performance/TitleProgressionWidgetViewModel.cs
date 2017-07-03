using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Commissions.Common.Models;

namespace DistributorBackOffice.Models.Performance
{
	public class TitleProgressionWidgetViewModel
	{
		private ICommissionsService commissionsService = Create.New<ICommissionsService>();
		public TitleProgressionWidgetViewModel() { }

		public int CurrentLevel { get; set; }
		public int PaidAsLevel { get; set; }
		public string CurrentLevelName { get; set; }
		public string PaidAsLevelName { get; set; }
	}
}