using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Commissions.Service.Accounts;
using NetSteps.Commissions.Service.AccountTitles;
using NetSteps.Commissions.Service.BonusKinds;
using NetSteps.Commissions.Service.DisbursementKinds;
using NetSteps.Commissions.Service.DisbursementProfiles;
using NetSteps.Commissions.Service.DistributorPerformance;
using NetSteps.Encore.Core.IoC;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Periods;
using NetSteps.Commissions.Service.CommissionPlans;
using NetSteps.Commissions.Service.Titles;
using NetSteps.Commissions.Service.Interfaces.CommissionPlan;
using NetSteps.Commissions.Common;
using NetSteps.Commissions.Service.TitleKinds;
using NetSteps.Commissions.Service.AccountTitleOverrides;
using NetSteps.Commissions.Service.CalculationOverrides;
using NetSteps.Commissions.Service.DisbursementHolds;
using NetSteps.Commissions.Service.Models;
using NetSteps.Commissions.Service.OverrideKinds;
using NetSteps.Commissions.Service.OverrideReasons;
using NetSteps.Commissions.Service.OverrideReasonSources;
using NetSteps.Commissions.Service.CalculationKinds;
using NetSteps.Commissions.Service.LedgerEntryKinds;

namespace NetSteps.Commissions.Service.Test
{
	[TestClass]
	public class CommissionServiceTest
	{
		private CommissionsService GetCommissionsService()
		{
			return new CommissionsService(
				new PeriodService(new PeriodProvider(new PeriodRepository())),
				new CommissionPlanService(new CommissionPlanProvider(new CommissionPlanRepository())),
				new TitleService(new TitleProvider(new TitleRepository())),
				new TitleKindService(new TitleKindProvider(new TitleKindRepository())),
				new AccountTitleOverrideService(new AccountTitleOverrideProvider(new AccountTitleOverrideRepository(
																						new OverrideReasonProvider(new OverrideReasonRepository(new OverrideReasonSourceProvider(new OverrideReasonSourceRepository()))),
																						new TitleProvider(new TitleRepository()),
																						new TitleKindProvider(new TitleKindRepository()),
																						new PeriodProvider(new PeriodRepository()),
																						new OverrideKindProvider(new OverrideKindRepository()))
																				)),
				new CalculationOverrideService(new CalculationOverrideProvider(new CalculationOverrideRepository(
					new CalculationKindProvider(new CalculationKindRepository()),
 					new OverrideKindProvider(new OverrideKindRepository()),
 					new OverrideReasonProvider(new OverrideReasonRepository(new OverrideReasonSourceProvider(new OverrideReasonSourceRepository()))),
 					new PeriodProvider(new PeriodRepository())
					))),
				new DisbursementHoldService(new DisbursementHoldProvider(new DisbursementHoldRepository(new OverrideReasonProvider(new OverrideReasonRepository(new OverrideReasonSourceProvider(new OverrideReasonSourceRepository())))))),
				new OverrideKindService(new OverrideKindProvider(new OverrideKindRepository())),
				new OverrideReasonService(new OverrideReasonProvider(new OverrideReasonRepository((new OverrideReasonSourceProvider(new OverrideReasonSourceRepository()))))),
				new OverrideReasonSourceService(new OverrideReasonSourceProvider(new OverrideReasonSourceRepository())),
				new LedgerEntryKindService(new LedgerEntryKindProvider(new LedgerEntryKindRepository())),
				new CalculationKindService(new CalculationKindProvider(new CalculationKindRepository())),
				new DisbursementProfileService(new DisbursementProfileProvider(
					new DisbursementProfileRepository()
					, new DisbursementKindService(new DisbursementKindProvider(new DisbursementKindRepository())))),
				new BonusKindService(new BonusKindProvider(new BonusKindRepository())),
				new DistributorPerformanceService(new TitleProvider(new TitleRepository())),
				new AccountService(new AccountProvider(new AccountRepository())),
                new AccountTitleService(new AccountTitleProvider(new AccountTitleRepository()), new PeriodService(new PeriodProvider(new PeriodRepository())))
				);
		}
		
		[TestMethod]
		public void CommissionsService_is_ioc_registered()
		{
			var commissionPlanService = Create.New<ICommissionPlanService>();

			var container = Container.Root;
			var service = Create.New<ICommissionsService>();
			Assert.IsNotNull(service);
		}
		[TestMethod]
		public void CommissionService_should_AddAccountTitleOverride()
		{
			
			var commissionService = GetCommissionsService();
			
			var accountTitleOverride = new AccountTitleOverride();
			accountTitleOverride.AccountId = 1;
			accountTitleOverride.ApplicationSourceId = 1;
			accountTitleOverride.CreatedDateUTC = DateTime.UtcNow;
			accountTitleOverride.Notes = "Test";
			accountTitleOverride.UserId = 1;
			accountTitleOverride.Title = new TitleProvider(new TitleRepository()).First();
			accountTitleOverride.TitleKind = new TitleKindProvider(new TitleKindRepository()).First();
			accountTitleOverride.UpdatedDateUTC = accountTitleOverride.CreatedDateUTC;
			accountTitleOverride.Period = new PeriodService(new PeriodProvider(new PeriodRepository())).GetCurrentPeriod(DisbursementFrequencyKind.Monthly);
			accountTitleOverride.OverrideKind = new OverrideKindProvider(new OverrideKindRepository()).First();
			accountTitleOverride.OverrideReason = new OverrideReasonProvider(new OverrideReasonRepository(new OverrideReasonSourceProvider(new OverrideReasonSourceRepository()))).First();
			accountTitleOverride.OverrideTitle = new TitleProvider(new TitleRepository()).Last();
			accountTitleOverride.Notes = "New Notes";
			
			var savedOverride = commissionService.AddAccountTitleOverride(accountTitleOverride);
			Assert.AreNotEqual(0, savedOverride.AccountTitleOverrideId);

			Assert.IsTrue(commissionService.DeleteAccountTitleOverride(savedOverride.AccountTitleOverrideId));
		}

		[TestMethod]
		public void CommissionService_should_AddCalculationOverride()
		{
            var commissionService = GetCommissionsService();
		    var calculationOverride = new CalculationOverride
		    {
                AccountId = 1,
                ApplicationSourceId = 1,
                CalculationKind = null,
                CalculationOverrideId = 0,
                CreatedDateUTC = DateTime.Now,
                NewValue = new Decimal(1.00),
                Notes = "this is a test override",
                OverrideIfNull = false,
                OverrideKind = null,
                OverrideReason = null,
                Period = null,
                UpdatedDateUTC = DateTime.Now,
                UserId = 1
		    };
		    var saved = commissionService.AddCalculationOverride(calculationOverride);
            Assert.AreNotEqual(0, saved.CalculationOverrideId);
		}

		[TestMethod]
		public void CommissionService_should_AddDisbursementHold()
		{
			var commissionService = GetCommissionsService();

			var disbursementHold = new DisbursementHold
			{
				AccountId = 1, // core base account
				ApplicationSourceId = 1,
				CreatedDate = DateTime.UtcNow,
				HoldUntil = null,
				Notes = "This is a test Disbursement hold.",
				Reason = commissionService.GetOverrideReasons().First(),
				StartDate = DateTime.UtcNow,
				UpdatedDate = DateTime.Now,
				UserId = 1
			};

			var saved = commissionService.AddDisbursementHold(disbursementHold);
			Assert.AreNotEqual(0, saved.DisbursementHoldId);
		}

		[TestMethod]
		public void CommissionService_should_DeleteAccountTitleOverride()
		{
			Assert.Inconclusive();
		}

		[TestMethod]
		public void CommissionService_should_DeleteCalculationOverride()
		{
			Assert.Inconclusive();
		}

		[TestMethod]
		public void CommissionService_should_GetAccountTitleOverride()
		{
			Assert.Inconclusive();
		}

		[TestMethod]
		public void CommissionService_should_GetCalculationKinds()
		{
			var commissionService = GetCommissionsService();
			var calculationKinds = commissionService.GetCalculationKinds();
			Assert.IsNotNull(calculationKinds);
			Assert.AreNotEqual(0, calculationKinds.Count());
		}

		[TestMethod]
		public void CommissionService_should_GetCalculationOverride()
		{
			Assert.Inconclusive();
		}

		[TestMethod]
		public void CommissionService_should_GetDisbursementHold()
		{
			Assert.Inconclusive();
		}

		[TestMethod]
		public void CommissionService_should_GetCommissionPlan_by_DisbursementFrequencyKind()
		{
			var commissionService = GetCommissionsService();
			var commissionPlan = commissionService.GetCommissionPlan(DisbursementFrequencyKind.Monthly);

			Assert.Inconclusive();
		}

		[TestMethod]
		public void CommissionService_should_GetCommissionPlan_by_PlanId()
		{
			var commissionService = GetCommissionsService();
			var commissionPlan = commissionService.GetCommissionPlan((int)DisbursementFrequencyKind.Monthly);

			Assert.Inconclusive();
		}

		[TestMethod]
		public void CommissionService_should_GetCommissionPlans()
		{
			var commissionService = GetCommissionsService();
			var commissionPlans = commissionService.GetCommissionPlans();

			foreach (var commissionPlan in commissionPlans)
			{
				Assert.IsNotNull(commissionPlan);
				Assert.IsTrue(!String.IsNullOrEmpty(commissionPlan.PlanCode));
			}
			Assert.AreEqual(2, commissionPlans.Count());
		}

		[TestMethod]
		public void CommissionService_should_GetCurrentAndPastPeriods()
		{
			var commissionService = GetCommissionsService();
			var periods = commissionService.GetCurrentAndPastPeriods();

			foreach (var period in periods)
			{
				Assert.IsNotNull(period);
				Assert.AreNotEqual(0, period.PeriodId);
				Assert.IsTrue(period.StartDateUTC < DateTime.UtcNow);
			}
			Assert.AreEqual(4, periods.Count());
		}

		[TestMethod]
		public void CommissionService_should_GetCurrentPeriod()
		{
			var commissionService = GetCommissionsService();
			var period = commissionService.GetCurrentPeriod();
			Assert.IsNotNull(period);
			Assert.AreNotEqual(0, period.PeriodId);

			var currentPeriodId = Convert.ToInt32(DateTime.UtcNow.ToString("yyyyMM"));

			Assert.AreEqual(currentPeriodId, period.PeriodId);
		}

		[TestMethod]
		public void CommissionService_should_GetCurrentPeriods()
		{
			var commissionService = GetCommissionsService();
			var periods = commissionService.GetCurrentPeriods();

			var currentPeriodId = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));
			
			foreach (var period in periods)
			{
				Assert.IsNotNull(period);
				Assert.AreEqual(currentPeriodId, period.PeriodId);
			}
			Assert.AreEqual(1, periods.Count());
		}

		[TestMethod]
		public void CommissionService_should_GetDisbursementMethodCode()
		{
			Assert.Inconclusive();
		}

		[TestMethod]
		public void CommissionService_should_GetDisbursementProfileCountByAccountAndDisbursementMethod()
		{
			Assert.Inconclusive();
		}

		[TestMethod]
		public void CommissionService_should_GetDisbursementProfilesByAccountAndDisbursementMethod()
		{
			Assert.Inconclusive();
		}

		[TestMethod]
		public void CommissionService_should_GetDisbursementProfilesByAccountId()
		{
			Assert.Inconclusive();
		}

		[TestMethod]
		public void CommissionService_should_GetDistributorPerformanceData()
		{
		    var commissionsService = GetCommissionsService();
		    var data = commissionsService.GetDistributorPerformanceData(2, 201301);
            Assert.IsNotNull(data);
		}

		[TestMethod]
		public void CommissionService_should_GetDistributorPerformanceOverviewData()
		{
			Assert.Inconclusive();
		}

		[TestMethod]
		public void CommissionService_should_GetDistributorBonusData()
		{
			Assert.Inconclusive();

			//var comSvc = GetCommissionsService();
			//var data = comSvc.GetDistributorBonusData(1001, 201408);
		}

		[TestMethod]
		public void CommissionService_should_GetMaximumDisbursementProfiles()
		{
			Assert.Inconclusive();
		}

		[TestMethod]
		public void CommissionService_should_GetOpenPeriods()
		{
			var commissionService = GetCommissionsService();
			var periods = commissionService.GetOpenPeriods(DisbursementFrequencyKind.Monthly);
			foreach (var period in periods)
			{
				Assert.IsNotNull(period);
				Assert.AreNotEqual(0, period.PeriodId);
			}
			Assert.AreEqual(4, periods.Count());
		}

		[TestMethod]
		public void CommissionService_should_GetOverrideKinds()
		{
			var commissionService = GetCommissionsService();
			var kinds = commissionService.GetOverrideKinds();
			Assert.IsNotNull(kinds);
			Assert.AreEqual(4, kinds.Count());
		}

		[TestMethod]
		public void CommissionService_should_GetOverrideReasonSources()
		{
			var commissionService = GetCommissionsService();
			var sourceSet = commissionService.GetOverrideReasonSources();
			Assert.IsNotNull(sourceSet);
			Assert.AreEqual(3, sourceSet.Count());
		}

		[TestMethod]
		public void CommissionService_should_GetOverrideReasons()
		{
			var overrideReasonSource = new OverrideReasonSource();
			overrideReasonSource.OverrideReasonSourceId = 1;

			var commissionService = GetCommissionsService();
			var reasonSet = commissionService.GetOverrideReasons();
			Assert.IsNotNull(reasonSet);
			Assert.AreEqual(9, reasonSet.Count());
		}

		[TestMethod]
		public void CommissionService_should_GetOverrideReasonsForSourceByObject()
		{
			var overrideReasonSource = new OverrideReasonSource();
			overrideReasonSource.OverrideReasonSourceId = 1;

			var commissionService = GetCommissionsService();
			var reasonSet = commissionService.GetOverrideReasonsForSource(overrideReasonSource);
			Assert.IsNotNull(reasonSet);
			Assert.AreEqual(3, reasonSet.Count());
		}

		[TestMethod]
		public void CommissionService_should_GetOverrideReasonsForSourceById()
		{
			var overrideReasonSourceId = 1;

			var commissionService = GetCommissionsService();
			var reasonSet = commissionService.GetOverrideReasonsForSource(overrideReasonSourceId);
			Assert.IsNotNull(reasonSet);
			Assert.AreEqual(3, reasonSet.Count());
		}

		[TestMethod]
		public void CommissionService_should_GetPeriod()
		{
			var currentPeriodId = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));

			var commissionService = GetCommissionsService();
			var period = commissionService.GetPeriod(currentPeriodId);
			Assert.IsNotNull(period);
			Assert.AreNotEqual(0, period.PeriodId);
		}

		[TestMethod]
		public void CommissionService_should_GetPeriodsForAccount()
		{
			//TODO: After creating demo data in the testing database, verify that this still functions.  Right now we're returning an empty set
			// as there is no data in the database.
			var commissionService = GetCommissionsService();
			var periods = commissionService.GetPeriodsForAccount(1);
			foreach (var period in periods)
			{
				Assert.IsNotNull(period);
				Assert.AreNotEqual(0, period.PeriodId);
			}
			Assert.AreEqual(0, periods.Count());
		}

		[TestMethod]
		public void CommissionService_should_GetTitleKind()
		{
			var commissionService = GetCommissionsService();
			var title = commissionService.GetTitleKind(2);
			Assert.IsNotNull(title);
			Assert.AreEqual("Career Title", title.Name);
			Assert.AreEqual("CareerTitle", title.TermName);
			Assert.AreEqual("CT", title.TitleKindCode);
			Assert.AreEqual(2, title.TitleKindId);
		}

		[TestMethod]
		public void CommissionService_should_GetTitleKinds()
		{
			var commissionService = GetCommissionsService();
			var titleKinds = commissionService.GetTitleKinds();
			foreach (var titleKind in titleKinds)
			{
				Assert.IsNotNull(titleKind);
				Assert.AreNotEqual(0, titleKind.TitleKindId);
			}
			Assert.AreEqual(2, titleKinds.Count());
		}

	    void ValidateDisbursementProfile(IEFTDisbursementProfile profile, IEFTDisbursementProfile savedProfile)
	    {
            Assert.IsNotNull(savedProfile);
            Assert.IsTrue(savedProfile.DisbursementProfileId > 0);
            Assert.AreEqual(profile.AccountId, savedProfile.AccountId);
            Assert.AreEqual(profile.AccountNumber, savedProfile.AccountNumber);
            Assert.AreEqual(profile.AddressId, savedProfile.AddressId);
            Assert.AreEqual(profile.BankAccountTypeId, savedProfile.BankAccountTypeId);
            Assert.AreEqual(profile.BankName, savedProfile.BankName);
            Assert.AreEqual(profile.BankPhone, savedProfile.BankPhone);
            Assert.AreEqual(profile.CurrencyId, savedProfile.CurrencyId);
            Assert.AreEqual(profile.DisbursementMethod, savedProfile.DisbursementMethod);
            Assert.AreEqual(profile.EnrollmentFormReceived, savedProfile.EnrollmentFormReceived);
            Assert.AreEqual(profile.IsEnabled, savedProfile.IsEnabled);
            Assert.AreEqual(profile.NameOnAccount, savedProfile.NameOnAccount);
            Assert.AreEqual(profile.Percentage, savedProfile.Percentage);
            Assert.AreEqual(profile.RoutingNumber, savedProfile.RoutingNumber);        
	    }

		[TestMethod]
		public void CommissionService_should_SaveDisbursementProfile()
		{
		    {
		        var profile = new EFTDisbursementProfile
		        {
		            AccountId = 1,
		            AccountNumber = "29381238",
		            AddressId = 2,
		            BankAccountTypeId = 1,
		            BankName = "Test Bank",
		            BankPhone = "8011234567",
		            CurrencyId = 1,
		            EnrollmentFormReceived = true,
		            IsEnabled = true,
		            NameOnAccount = "Test Account Name",
		            Percentage = 1,
		            RoutingNumber = "123123123"
		        };

		        var commissionService = GetCommissionsService();
		        var savedProfile = commissionService.SaveDisbursementProfile(profile) as IEFTDisbursementProfile;

		        ValidateDisbursementProfile(profile, savedProfile);
		    }
		    {
		        var profile = new EFTDisbursementProfile
		        {
		            AccountId = 1,
		            AddressId = 2,
		            BankAccountTypeId = 1,
		            CurrencyId = 1,
		            EnrollmentFormReceived = true,
		            IsEnabled = true,
		            Percentage = 1,
		        };

		        var commissionService = GetCommissionsService();
		        var savedProfile = commissionService.SaveDisbursementProfile(profile) as IEFTDisbursementProfile;

		        ValidateDisbursementProfile(profile, savedProfile);
		    }
		}

		[TestMethod]
		public void CommissionService_should_SaveTemporaryAccountToCommission()
		{
		    var commissionsService = GetCommissionsService();
		    var accountNum = (new Random()).Next(10000, 1000000).ToString();
		    var success = commissionsService.SavePartialAccountToCommission(0, accountNum);
            Assert.IsTrue(success);
		}

		[TestMethod]
		public void CommissionService_should_SearchAccountTitleOverrides()
		{
			var commissionService = GetCommissionsService();
		    var accountTitleOverrideSearchParameters = new AccountTitleOverrideSearchParameters
		    {
                PageIndex = 3,
                PageSize = 20,
		    };
			var found = commissionService.SearchAccountTitleOverrides(accountTitleOverrideSearchParameters);
			Assert.IsNotNull(found);
			Assert.AreEqual(0, found.TotalCount);
		}

		[TestMethod]
		public void CommissionService_should_SearchCalculationOverrides()
		{
			var commissionService = GetCommissionsService();
			var calculationOverrideSearchParameters = new CalculationOverrideSearchParameters();
			var found = commissionService.SearchCalculationOverrides(calculationOverrideSearchParameters);
			Assert.IsNotNull(found);
			Assert.AreEqual(0, found.TotalCount);
		}

		[TestMethod]
		public void CommissionService_should_SearchDisbursementHolds()
		{
			var commissionService = GetCommissionsService();
			var DisbursementHoldSearchParameters = new DisbursementHoldSearchParameters();
			var found = commissionService.SearchDisbursementHolds(DisbursementHoldSearchParameters);
			Assert.IsNotNull(found);
			Assert.AreEqual(0, found.TotalCount);
		}

		[TestMethod]
		public void CommissionService_should_GetTitle()
		{
			var commissionService = GetCommissionsService();
			var title = commissionService.GetTitle(3);
			Assert.IsNotNull(title);
			Assert.AreEqual(3, title.TitleId);
			Assert.AreEqual("T3", title.TitleCode);
			Assert.AreEqual("Title3", title.TitleName);
			Assert.AreEqual(3, title.SortOrder);
			Assert.AreEqual(true, title.Active);
			Assert.AreEqual("ED", title.ClientCode);
			Assert.AreEqual("Exec Distributor", title.ClientName);
			Assert.AreEqual(true, title.ReportsVisibility);

		}

		[TestMethod]
		public void CommissionService_should_GetTitles()
		{
			var commissionService = GetCommissionsService();
			var titles = commissionService.GetTitles();
			foreach (var title in titles)
			{
				Assert.IsNotNull(title);
				Assert.AreNotEqual(0, title.TitleId);
			}
			Assert.AreEqual(12, titles.Count());
		}

		[TestMethod]
		public void CommissionService_should_GetDisbursementHoldReason()
		{
			Assert.Inconclusive();
		}
	}
}
