using NetSteps.Commissions.Common;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;

namespace DistributorBackOffice.Models.Performance
{
	public class PerformanceLandingViewModel
	{
		public PerformanceLandingViewModel(IDistributorPeriodPerformanceData performanceData)
		{
			_performanceData = performanceData ?? Create.New<IDistributorPeriodPerformanceData>();
		}

		private IDistributorPeriodPerformanceData _performanceData;

		public decimal Volume
		{
			get { return _performanceData.Volume; }
		}

		public decimal RequiredVolume
		{
			get
			{
				var currentValue = _performanceData.RequiredVolume;
                return (currentValue > decimal.Zero) ? currentValue : 150m;
			}
		}

		public string VolumeDescription
		{
			get { return Translation.GetTerm("RequiredVolumeAsterik", "Your total sales volume must be a minimum of {0} in order to maintain your title", this.RequiredVolume.GetRoundedNumber()); }
		}

		public int? CurrentLevel
		{
			get { return (_performanceData.CurrentTitle != null) ? _performanceData.CurrentTitle.TitleId : (int?)null; }
		}

		public int? PaidAsLevel
		{
			get { return (_performanceData.PaidAsTitle != null) ? _performanceData.PaidAsTitle.TitleId : (int?)null; }
		}

		public string CurrentLevelName
		{
			get { return (_performanceData.CurrentTitle != null) ? Translation.GetTerm(_performanceData.CurrentTitle.TermName) : null; }
		}

		public string PaidAsLevelName
		{
			get { return (_performanceData.PaidAsTitle != null) ? Translation.GetTerm(_performanceData.PaidAsTitle.TermName) : null; }
		}

		public string SalesIndicatorLevel
		{
			get { return _performanceData.SalesIndicatorLevel; }
		}

		public string PV { get; set; }

	}
}