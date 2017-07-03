using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_CommissionPrepReports_Control : GMP_Reports_Reports_Control
    {

        public GMP_Reports_AccountTitlesAudit_Page ClickAccountTitlesAudit(int? timeout = null)
        {
            return OpenReport<GMP_Reports_AccountTitlesAudit_Page>("Account\\+Titles\\+Audit", timeout);
        }

        public GMP_Reports_ActiveLegs_Page ClickActiveLegs(int? timeout = null)
        {
            return OpenReport<GMP_Reports_ActiveLegs_Page>("Active\\+Legs", timeout);
        }

        public GMP_Reports_BonusPayout_Page ClickBonusPayout(int? timeout = null)
        {
            return OpenReport<GMP_Reports_BonusPayout_Page>("Bonus\\+Payout", timeout);
        }

        public GMP_Reports_BonusQualificationMatrix_Page ClickBonusQulaificationsMatrix(int? timeout = null)
        {
            return OpenReport<GMP_Reports_BonusQualificationMatrix_Page>("Bonus\\+Qualifications\\+Matrix", timeout);
        }

        public GMP_Reports_TitleQualificationMatrix_Page ClickTitleQualificationsMatrix(int? timeout = null)
        {
            return OpenReport<GMP_Reports_TitleQualificationMatrix_Page>("Title\\+Qualifications\\+Matrix", timeout);
        }

        public GMP_Reports_EnhancedBonusDetails_Page ClickEnhancedBonusDetails(int? timeout = null)
        {
            return OpenReport<GMP_Reports_EnhancedBonusDetails_Page>("Enhanced\\+Bonus\\+Details", timeout);
        }

        public GMP_Reports_TopEarnersByVolume_Page ClickTopEarnersByVolume(int? timeout = null)
        {
            return OpenReport<GMP_Reports_TopEarnersByVolume_Page>("Top\\+Earners\\+by\\+Volume", timeout);
        }

        public GMP_Reports_Promotions_Page ClickPromotions(int? timeout = null)
        {
            return OpenReport<GMP_Reports_Promotions_Page>("Promotions", timeout);
        }

        public GMP_Reports_AccountCalculationsAudit_Page ClickAccountCalculationsAudit(int? timeout = null)
        {
            return OpenReport<GMP_Reports_AccountCalculationsAudit_Page>("Account\\+Calculations\\+Audit", timeout);
        }

        public GMP_Reports_CommissionableOrders_Page ClickCommissionableOrders(int? timeout = null)
        {
            return OpenReport<GMP_Reports_CommissionableOrders_Page>("Commissionable\\+Orders", timeout);
        }

        public GMP_Reports_Demotions_Page ClickDemotions(int? timeout = null)
        {
            return OpenReport<GMP_Reports_Demotions_Page>("Demotions", timeout);
        }

        public GMP_Reports_MonthlyBonusesByTitle_Page ClickMonthlyBonusesByTitle(int? timeout = null)
        {
            return OpenReport<GMP_Reports_MonthlyBonusesByTitle_Page>("Monthly\\+Bonuses\\+By\\+Title", timeout);
        }

        public GMP_Reports_Terminations_Page ClickTerminations(int? timeout = null)
        {
            return OpenReport<GMP_Reports_Terminations_Page>("Terminations", timeout);
        }

        public GMP_Reports_TerminationsAndCancellations_Page ClickTerminationsAndCancellations(int? timeout = null)
        {
            return OpenReport<GMP_Reports_TerminationsAndCancellations_Page>("Terminations\\+and\\+Cancellations", timeout);
        }
    }
}
