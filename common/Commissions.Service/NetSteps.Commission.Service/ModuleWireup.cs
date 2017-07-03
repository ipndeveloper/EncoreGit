using NetSteps.Commissions.Common;
using NetSteps.Commissions.Service.Accounts;
using NetSteps.Commissions.Service.AccountTitles;
using NetSteps.Commissions.Service.DisbursementHolds;
using NetSteps.Commissions.Service.DisbursementKinds;
using NetSteps.Commissions.Service.DisbursementProfiles;
using NetSteps.Commissions.Service.DistributorPerformance;
using NetSteps.Commissions.Service.Interfaces.Account;
using NetSteps.Commissions.Service.Interfaces.AccountLedger;
using NetSteps.Commissions.Service.Interfaces.AccountTitleOverride;
using NetSteps.Commissions.Service.Interfaces.AccountTitles;
using NetSteps.Commissions.Service.Interfaces.DisbursementProfile;
using NetSteps.Commissions.Service.Interfaces.DistributorPerformance;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryKind;
using NetSteps.Commissions.Service.Interfaces.CalculationKind;
using NetSteps.Commissions.Service.Interfaces.CalculationOverride;
using NetSteps.Commissions.Service.Interfaces.DisbursementHold;
using NetSteps.Commissions.Service.Interfaces.CommissionPlan;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryOrigin;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryReason;
using NetSteps.Commissions.Service.Interfaces.Override;
using NetSteps.Commissions.Service.Interfaces.OverrideKind;
using NetSteps.Commissions.Service.Interfaces.OverrideReason;
using NetSteps.Commissions.Service.Interfaces.OverrideReasonSource;
using NetSteps.Commissions.Service.Interfaces.Period;
using NetSteps.Commissions.Service.Interfaces.ProductCreditLedger;
using NetSteps.Commissions.Service.Interfaces.Title;
using NetSteps.Commissions.Service.Interfaces.TitleKind;
using NetSteps.Commissions.Service.AccountLedgerEntries;
using NetSteps.Commissions.Service.AccountTitleOverrides;
using NetSteps.Commissions.Service.LedgerEntryKinds;
using NetSteps.Commissions.Service.CalculationKinds;
using NetSteps.Commissions.Service.CalculationOverrides;
using NetSteps.Commissions.Service.CommissionPlans;
using NetSteps.Commissions.Service.LedgerEntryOrigins;
using NetSteps.Commissions.Service.LedgerEntryReasons;
using NetSteps.Commissions.Service.OverrideKinds;
using NetSteps.Commissions.Service.OverrideReasons;
using NetSteps.Commissions.Service.OverrideReasonSources;
using NetSteps.Commissions.Service.Periods;
using NetSteps.Commissions.Service.ProductCreditLedgerEntries;
using NetSteps.Commissions.Service.TitleKinds;
using NetSteps.Commissions.Service.Titles;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Commissions.Service.Interfaces.BonusKind;
using NetSteps.Commissions.Service.BonusKinds;
using NetSteps.Commissions.Service.Interfaces.DisbursementKinds;

[assembly: Wireup(typeof(NetSteps.Commissions.Service.ModuleWireup))]

namespace NetSteps.Commissions.Service
{
	public class ModuleWireup : WireupCommand
	{

		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			var root = Container.Root;

			WireupAccountTitleOverrides(root);
			WireupPeriods(root);
			WireupCommissionPlans(root);
			WireupTitles(root);
			WireupTitleKinds(root);
			WireupCalculationOverrides(root);
			WireupDisbursementHolds(root);
			WireupOverrideKinds(root);
			WireupOverrideReasons(root);
			WireupOverrideReasonSources(root);
			WireupLedgerEntryKinds(root);
			WireupCalculationKinds(root);
			WireupAccountLedgerEntries(root);
			WireupProductCreditLedgerEntries(root);
			WireupLedgerEntryOrigins(root);
			WireupLedgerEntryReasons(root);
			WireupLedgerEntryKinds(root);
			WireupDisbursementProfiles(root);
			WireupBonusKinds(root);
			WireupDistributorPerformance(root);
			WireupAccounts(root);
		    WireupAccountTitles(root);

			root.ForType<ICommissionsService>()
				.Register<ICommissionsService>
											((c, p) =>
												{
													return new CommissionsService(
														Create.New<IPeriodService>(),
														Create.New<ICommissionPlanService>(),
														Create.New<ITitleService>(),
														Create.New<ITitleKindService>(),
														Create.New<IAccountTitleOverrideService>(),
														Create.New<ICalculationOverrideService>(),
														Create.New<IDisbursementHoldService>(),
														Create.New<IOverrideKindService>(),
														Create.New<IOverrideReasonService>(),
														Create.New<IOverrideReasonSourceService>(),
														Create.New<ILedgerEntryKindService>(),
														Create.New<ICalculationKindService>(),
														Create.New<IDisbursementProfileService>(),
														Create.New<IBonusKindService>(),
														Create.New<IDistributorPerformanceService>(),
														Create.New<IAccountService>(),
                                                        Create.New<IAccountTitleService>()
														);
												}

											)
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<ICommissionJobService>()
				.Register<CommissionsJobService>()
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IDownlineService>()
				.Register<DownlineService>()
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IProductCreditLedgerService>()
				.Register<IProductCreditLedgerService>
												((c, p) =>
												{
													return new ProductCreditLedgerService(
														Create.New<IProductCreditLedgerEntryService>(),
														Create.New<ILedgerEntryKindService>(),
														Create.New<ILedgerEntryOriginService>(),
														Create.New<ILedgerEntryReasonService>()
														);
												}
											)
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IAccountLedgerService>()
				.Register<IAccountLedgerService>
												((c, p) =>
												{
													return new AccountLedgerService(
														Create.New<IAccountLedgerEntryService>(),
														Create.New<ILedgerEntryKindService>(),
														Create.New<ILedgerEntryOriginService>(),
														Create.New<ILedgerEntryReasonService>()
														);
												}
											)
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IDistributorPerformanceService>()
				.Register<IDistributorPerformanceService>
				((c, p) =>
				{
					return new DistributorPerformanceService(
						Create.New<ITitleProvider>()
						);
				})
				.ResolveAnInstancePerRequest()
				.End();
		}

        private void WireupAccountTitles(IRootContainer root)
        {
            root.ForType<IAccountTitleRepository>()
                .Register<IAccountTitleRepository>((c, p) =>
                {
                    return new AccountTitleRepository();
                })
                .ResolveAnInstancePerRequest()
                .End();

            root.ForType<IAccountTitleProvider>()
                .Register<IAccountTitleProvider>((c, p) =>
                {
                    return new AccountTitleProvider(
                        Create.New<IAccountTitleRepository>()
                        );
                })
                .ResolveAnInstancePerRequest()
                .End();

            root.ForType<IAccountTitleService>()
                .Register<IAccountTitleService>((c, p) =>
                {
                    return new AccountTitleService(
                        Create.New<IAccountTitleProvider>(),
                        Create.New<IPeriodService>());
                })
                .ResolveAnInstancePerRequest()
                .End();
        }

		private void WireupAccounts(IRootContainer root)
		{
			root.ForType<IAccountRepository>()
				.Register<IAccountRepository>((c, p) =>
				{
					return new AccountRepository();
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IAccountProvider>()
				.Register<IAccountProvider>((c, p) =>
				{
					return new AccountProvider(
						Create.New<IAccountRepository>());
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IAccountService>()
				.Register<IAccountService>((c, p) =>
				{
					return new AccountService(
						Create.New<IAccountProvider>());
				})
				.ResolveAnInstancePerRequest()
				.End();
		}

		private void WireupDistributorPerformance(IRootContainer root)
		{
			root.ForType<IDistributorPerformanceService>()
				.Register<IDistributorPerformanceService>((c, p) =>
				{
					return new DistributorPerformanceService(
						Create.New<ITitleProvider>());
				})
				.ResolveAnInstancePerRequest()
				.End();
		}

		private void WireupDisbursementProfiles(IRootContainer root)
		{
			root.ForType<IDisbursementKindRepository>()
				.Register<IDisbursementKindRepository>((c, p) =>
				{
					return new DisbursementKindRepository();
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IDisbursementKindProvider>()
				.Register<IDisbursementKindProvider>((c, p) =>
				{
					return new DisbursementKindProvider(Create.New<IDisbursementKindRepository>());
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IDisbursementKindService>()
				.Register<IDisbursementKindService>((c, p) =>
				{
					return new DisbursementKindService(Create.New<IDisbursementKindProvider>());
				})
				.ResolveAnInstancePerRequest()
				.End();



			root.ForType<IDisbursementProfileRepository>()
				.Register<IDisbursementProfileRepository>((c, p) =>
				{
					return new DisbursementProfileRepository();
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IDisbursementProfileProvider>()
				.Register<IDisbursementProfileProvider>((c, p) =>
				{
					return new DisbursementProfileProvider
									(
										Create.New<IDisbursementProfileRepository>(),
										Create.New<IDisbursementKindService>()
									);
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IDisbursementProfileService>()
				.Register<IDisbursementProfileService>((c, p) =>
				{
					return new DisbursementProfileService
										(
											Create.New<IDisbursementProfileProvider>()
										);
				})
				.ResolveAnInstancePerRequest()
				.End();

		    root.ForType<IDisbursementProfileResolver>()
		        .Register<IDisbursementProfileResolver>((c, p) =>
		        {
		            return new DisbursementProfileResolver(Create.New<IDisbursementProfileProvider>());
		        })
		        .ResolveAnInstancePerRequest()
		        .End();
		}

		private void WireupPeriods(IRootContainer root)
		{
			root.ForType<IPeriodRepository>()
				.Register<IPeriodRepository>((c, p) =>
				{
					return new PeriodRepository();
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IPeriodProvider>()
				.Register<IPeriodProvider>((c, p) =>
				{
					return new PeriodProvider
									(
										Create.New<IPeriodRepository>()
									);
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IPeriodService>()
				.Register<IPeriodService>((c, p) =>
				{
					return new PeriodService
										(
											Create.New<IPeriodProvider>()
										);
				})
				.ResolveAnInstancePerRequest()
				.End();
		}

		private void WireupCommissionPlans(IRootContainer root)
		{
			root.ForType<ICommissionPlanRepository>()
				.Register<ICommissionPlanRepository>((c, p) =>
				{
					return new CommissionPlanRepository();
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<ICommissionPlanProvider>()
				.Register<ICommissionPlanProvider>((c, p) =>
				{
					return new CommissionPlanProvider
									(
										Create.New<ICommissionPlanRepository>()
									);
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<ICommissionPlanService>()
				.Register<ICommissionPlanService>((c, p) =>
				{
					return new CommissionPlanService
										(
											Create.New<ICommissionPlanProvider>()
										);
				})
				.ResolveAnInstancePerRequest()
				.End();
		}

		private void WireupTitles(IRootContainer root)
		{
			root.ForType<ITitleRepository>()
				.Register<ITitleRepository>((c, p) =>
				{
					return new TitleRepository();
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<ITitleProvider>()
				.Register<ITitleProvider>((c, p) =>
				{
					return new TitleProvider
									(
										Create.New<ITitleRepository>()
									);
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<ITitleService>()
				.Register<ITitleService>((c, p) =>
				{
					return new TitleService
										(
											Create.New<ITitleProvider>()
										);
				})
				.ResolveAnInstancePerRequest()
				.End();
		}

		private void WireupTitleKinds(IRootContainer root)
		{
			root.ForType<ITitleKindRepository>()
				.Register<ITitleKindRepository>((c, p) =>
				{
					return new TitleKindRepository();
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<ITitleKindProvider>()
				.Register<ITitleKindProvider>((c, p) =>
				{
					return new TitleKindProvider
									(
										Create.New<ITitleKindRepository>()
									);
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<ITitleKindService>()
				.Register<ITitleKindService>((c, p) =>
				{
					return new TitleKindService
										(
											Create.New<ITitleKindProvider>()
										);
				})
				.ResolveAnInstancePerRequest()
				.End();
		}

		private void WireupAccountTitleOverrides(IRootContainer root)
		{
			root.ForType<IAccountTitleOverrideRepository>()
				.Register<IAccountTitleOverrideRepository>((c, p) =>
				{
					return new AccountTitleOverrideRepository
									(
										Create.New<IOverrideReasonProvider>(),
										Create.New<ITitleProvider>(),
										Create.New<ITitleKindProvider>(),
										Create.New<IPeriodProvider>(),
										Create.New<IOverrideKindProvider>()
									);
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IAccountTitleOverrideProvider>()
				.Register<IAccountTitleOverrideProvider>((c, p) =>
				{
					return new AccountTitleOverrideProvider
									(
										Create.New<IAccountTitleOverrideRepository>()
									);
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IAccountTitleOverrideService>()
				.Register<IAccountTitleOverrideService>((c, p) =>
				{
					return new AccountTitleOverrideService
										(
											Create.New<IAccountTitleOverrideProvider>()
										);
				})
				.ResolveAnInstancePerRequest()
				.End();
		}

		private void WireupCalculationOverrides(IRootContainer root)
		{
			root.ForType<ICalculationOverrideRepository>()
				.Register<ICalculationOverrideRepository>((c, p) =>
				{
					return new CalculationOverrideRepository(
						Create.New<ICalculationKindProvider>()
						, Create.New<IOverrideKindProvider>()
						, Create.New<IOverrideReasonProvider>()
						, Create.New<IPeriodProvider>()
						);
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<ICalculationOverrideProvider>()
				.Register<ICalculationOverrideProvider>((c, p) =>
				{
					return new CalculationOverrideProvider
									(
										Create.New<ICalculationOverrideRepository>()
									);
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<ICalculationOverrideService>()
				.Register<ICalculationOverrideService>((c, p) =>
				{
					return new CalculationOverrideService
										(
											Create.New<ICalculationOverrideProvider>()
										);
				})
				.ResolveAnInstancePerRequest()
				.End();
		}

		private void WireupDisbursementHolds(IRootContainer root)
		{
			root.ForType<IDisbursementHoldRepository>()
				.Register<IDisbursementHoldRepository>((c, p) =>
				{
					return new DisbursementHoldRepository(
						Create.New<IOverrideReasonProvider>());
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IDisbursementHoldProvider>()
				.Register<IDisbursementHoldProvider>((c, p) =>
				{
					return new DisbursementHoldProvider
									(
										Create.New<IDisbursementHoldRepository>()
									);
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IDisbursementHoldService>()
				.Register<IDisbursementHoldService>((c, p) =>
				{
					return new DisbursementHoldService
										(
											Create.New<IDisbursementHoldProvider>()
										);
				})
				.ResolveAnInstancePerRequest()
				.End();
		}

		private void WireupOverrideKinds(IRootContainer root)
		{
			root.ForType<IOverrideKindRepository>()
				.Register<IOverrideKindRepository>((c, p) =>
				{
					return new OverrideKindRepository();
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IOverrideKindProvider>()
				.Register<IOverrideKindProvider>((c, p) =>
				{
					return new OverrideKindProvider
									(
										Create.New<IOverrideKindRepository>()
									);
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IOverrideKindService>()
				.Register<IOverrideKindService>((c, p) =>
				{
					return new OverrideKindService
										(
											Create.New<IOverrideKindProvider>()
										);
				})
				.ResolveAnInstancePerRequest()
				.End();
		}

		private void WireupOverrideReasons(IRootContainer root)
		{
			root.ForType<IOverrideReasonRepository>()
				.Register<IOverrideReasonRepository>((c, p) =>
				{
					return new OverrideReasonRepository
									(
										Create.New<IOverrideReasonSourceProvider>()
									);
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IOverrideReasonProvider>()
				.Register<IOverrideReasonProvider>((c, p) =>
				{
					return new OverrideReasonProvider
									(
										Create.New<IOverrideReasonRepository>()
									);
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IOverrideReasonService>()
				.Register<IOverrideReasonService>((c, p) =>
				{
					return new OverrideReasonService
										(
											Create.New<IOverrideReasonProvider>()
										);
				})
				.ResolveAnInstancePerRequest()
				.End();
		}

		private void WireupOverrideReasonSources(IRootContainer root)
		{
			root.ForType<IOverrideReasonSourceRepository>()
				.Register<IOverrideReasonSourceRepository>((c, p) =>
												{
													return new OverrideReasonSourceRepository();
												})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IOverrideReasonSourceProvider>()
				.Register<IOverrideReasonSourceProvider>((c, p) =>
												{
													return new OverrideReasonSourceProvider
																	(
																		Create.New<IOverrideReasonSourceRepository>()
																	);
												})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IOverrideReasonSourceService>()
				.Register<IOverrideReasonSourceService>((c, p) =>
												{
													return new OverrideReasonSourceService
																		(
																			Create.New<IOverrideReasonSourceProvider>()
																		);
												})
				.ResolveAnInstancePerRequest()
				.End();
		}

		private void WireupLedgerEntryKinds(IRootContainer root)
		{
			root.ForType<ILedgerEntryKindRepository>()
				.Register<ILedgerEntryKindRepository>((c, p) =>
				{
					return new LedgerEntryKindRepository();
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<ILedgerEntryKindProvider>()
				.Register<ILedgerEntryKindProvider>((c, p) =>
				{
					return new LedgerEntryKindProvider
									(
										Create.New<ILedgerEntryKindRepository>()
									);
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<ILedgerEntryKindService>()
				.Register<ILedgerEntryKindService>((c, p) =>
				{
					return new LedgerEntryKindService
										(
											Create.New<ILedgerEntryKindProvider>()
										);
				})
				.ResolveAnInstancePerRequest()
				.End();
		}

		private void WireupCalculationKinds(IRootContainer root)
		{
			root.ForType<ICalculationKindRepository>()
				.Register<ICalculationKindRepository>((c, p) =>
				{
					return new CalculationKindRepository();
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<ICalculationKindProvider>()
				.Register<ICalculationKindProvider>((c, p) =>
				{
					return new CalculationKindProvider
									(
										Create.New<ICalculationKindRepository>()
									);
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<ICalculationKindService>()
				.Register<ICalculationKindService>((c, p) =>
				{
					return new CalculationKindService
										(
											Create.New<ICalculationKindProvider>()
										);
				})
				.ResolveAnInstancePerRequest()
				.End();
		}

		private void WireupAccountLedgerEntries(IRootContainer root)
		{
			root.ForType<IAccountLedgerEntryRepository>()
				.Register<IAccountLedgerEntryRepository>((c, p) =>
				{
					return new AccountLedgerEntryRepository
									(
										Create.New<ILedgerEntryKindProvider>(),
										Create.New<ILedgerEntryOriginProvider>(),
										Create.New<ILedgerEntryReasonProvider>()
									);
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IAccountLedgerEntryProvider>()
				.Register<IAccountLedgerEntryProvider>((c, p) =>
				{
					return new AccountLedgerEntryProvider
									(
										Create.New<IAccountLedgerEntryRepository>()
									);
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IAccountLedgerEntryService>()
				.Register<IAccountLedgerEntryService>((c, p) =>
				{
					return new AccountLedgerEntryService
										(
											Create.New<IAccountLedgerEntryProvider>(),
											Create.New<ILedgerEntryKindProvider>(),
											Create.New<ILedgerEntryOriginProvider>(),
											Create.New<ILedgerEntryReasonProvider>()
										);
				})
				.ResolveAnInstancePerRequest()
				.End();
		}

		private void WireupProductCreditLedgerEntries(IRootContainer root)
		{
			root.ForType<IProductCreditLedgerEntryRepository>()
				.Register<IProductCreditLedgerEntryRepository>((c, p) =>
				{
					return new ProductCreditLedgerEntryRepository
										(
											Create.New<ILedgerEntryKindProvider>(),
											Create.New<ILedgerEntryOriginProvider>(),
											Create.New<ILedgerEntryReasonProvider>()
										);
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IProductCreditLedgerEntryProvider>()
				.Register<IProductCreditLedgerEntryProvider>((c, p) =>
				{
					return new ProductCreditLedgerEntryProvider
									(
										Create.New<IProductCreditLedgerEntryRepository>()
									);
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IProductCreditLedgerEntryService>()
				.Register<IProductCreditLedgerEntryService>((c, p) =>
				{
					return new ProductCreditLedgerEntryService
										(
											Create.New<IProductCreditLedgerEntryProvider>(),
											Create.New<ILedgerEntryKindProvider>(),
											Create.New<ILedgerEntryOriginProvider>(),
											Create.New<ILedgerEntryReasonProvider>()
										);
				})
				.ResolveAnInstancePerRequest()
				.End();
		}

		private void WireupLedgerEntryReasons(IRootContainer root)
		{
			root.ForType<ILedgerEntryReasonRepository>()
				.Register<ILedgerEntryReasonRepository>((c, p) =>
				{
					return new LedgerEntryReasonRepository();
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<ILedgerEntryReasonProvider>()
				.Register<ILedgerEntryReasonProvider>((c, p) =>
				{
					return new LedgerEntryReasonProvider
									(
										Create.New<ILedgerEntryReasonRepository>()
									);
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<ILedgerEntryReasonService>()
				.Register<ILedgerEntryReasonService>((c, p) =>
				{
					return new LedgerEntryReasonService
										(
											Create.New<ILedgerEntryReasonProvider>()
										);
				})
				.ResolveAnInstancePerRequest()
				.End();
		}

		private void WireupLedgerEntryOrigins(IRootContainer root)
		{
			root.ForType<ILedgerEntryOriginRepository>()
				.Register<ILedgerEntryOriginRepository>((c, p) =>
				{
					return new LedgerEntryOriginRepository();
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<ILedgerEntryOriginProvider>()
				.Register<ILedgerEntryOriginProvider>((c, p) =>
				{
					return new LedgerEntryOriginProvider
									(
										Create.New<ILedgerEntryOriginRepository>()
									);
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<ILedgerEntryOriginService>()
				.Register<ILedgerEntryOriginService>((c, p) =>
				{
					return new LedgerEntryOriginService
										(
											Create.New<ILedgerEntryOriginProvider>()
										);
				})
				.ResolveAnInstancePerRequest()
				.End();
		}

		private void WireupBonusKinds(IRootContainer root)
		{
			root.ForType<IBonusKindRepository>()
				.Register<IBonusKindRepository>((c, p) =>
				{
					return new BonusKindRepository();
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IBonusKindProvider>()
				.Register<IBonusKindProvider>((c, p) =>
				{
					return new BonusKindProvider
									(
										Create.New<IBonusKindRepository>()
									);
				})
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<IBonusKindService>()
				.Register<IBonusKindService>((c, p) =>
				{
					return new BonusKindService
										(
											Create.New<IBonusKindProvider>()
										);
				})
				.ResolveAnInstancePerRequest()
				.End();
		}
	}
}
