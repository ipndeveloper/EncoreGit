using System;
using System.Linq;
using System.Collections.Generic;
using NetSteps.Commissions.Common;
using NetSteps.Commissions.Common.Models;
using NetSteps.Data.Entities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Mvc.Helpers;

namespace DistributorBackOffice.Models.Performance
{
	public class PerformanceOverviewViewModel
	{
		private readonly ICommissionsService _commissionsService = Create.New<ICommissionsService>();

		public bool ShowPerformanceVolumeWidget { get; set; }
		public bool ShowPerformanceTitleProgressionWidget { get; set; }
		public bool UseAdvancedPerformanceTitleProgressionWidget { get; set; }
		public bool ShowPerformanceEarningsWidget { get; set; }
		public bool ShowPerformanceQuickStartWidget { get; set; }
		public ReportResults ReportResults { get; set; }

		//this section is for the EarningsWidget
		public decimal PreviousWeeksEarnings { get; set; }
		public decimal CurrentWeeksEarnings { get; set; }
		public decimal PreviousMonthlyBonus { get; set; }
		public decimal CurrentMonthlyBonus { get; set; }

		//This section is for the QuickStartWidget
		public decimal QS1Volume { get; set; }
		public decimal QS1BonusAmount { get; set; }
		public int QS1DaysLeft { get; set; }

		public decimal QS2Volume { get; set; }
		public decimal QS2BonusAmount { get; set; }
		public int QS2DaysLeft { get; set; }

		public decimal QS3Volume { get; set; }
		public decimal QS3BonusAmount { get; set; }
		public int QS3DaysLeft { get; set; }

		public bool ShowcaseEarned { get; set; }
		public int ShowcaseDaysLeft { get; set; }
		public int ShowcaseVolume { get; set; }
		public string ShowcaseType { get; set; }

		public int SponsoredCashEarned { get; set; }
		public int SponsoredQualified { get; set; }
		public DateTime EnrollmentDate { get; set; }
		public int QuickStartPeriod { get; set; }

		public IEnumerable<IPeriod> CurrentAndPastPeriods
		{
			get
			{
				return this._commissionsService.GetCurrentAndPastPeriods().OrderByDescending(p => p.PeriodId);
			}
		}
	}
}