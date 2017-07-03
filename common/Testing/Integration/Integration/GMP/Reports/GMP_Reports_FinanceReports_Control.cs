using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_FinanceReports_Control : GMP_Reports_Reports_Control
    {
        public GMP_Reports_1099Listing_Page ClickTenNintyNineListing(int? timeout = null, int? delay = 2)
        {
            GMP_Reports_1099Listing_Page report = OpenReport<GMP_Reports_1099Listing_Page>("1099\\+Listing", timeout);
            report.WaitForReport(timeout);
            return report;
        }

        public GMP_Reports_AccountLedgerAdjustments_Page ClickAccountLedgerAdjustments(int? timeout = null)
        {
            return OpenReport<GMP_Reports_AccountLedgerAdjustments_Page>("Account\\+Ledger\\+Adjustments", timeout);
        }

        public GMP_Reports_AccountLedgerBalance_Page ClickAccountLedgerBalance(int? timeout = null)
        {
            return OpenReport<GMP_Reports_AccountLedgerBalance_Page>("Account\\+Ledger\\+Balance", timeout);
        }

        public GMP_Reports_AccountCalculationsAudit_Page ClickAccountCalculationsAudit(int? timeout = null)
        {
            return OpenReport<GMP_Reports_AccountCalculationsAudit_Page>("Account\\+Calculations\\+Audit", timeout);
        }

        public GMP_Reports_AccountTitlesAudit_Page ClickAccountTitlesAudit(int? timeout = null)
        {
            return OpenReport<GMP_Reports_AccountTitlesAudit_Page>("Account\\+Titles\\+Audit", timeout);
        }

        public GMP_Reports_BonusPayout_Page ClickBonusPayout(int? timeout = null)
        {
            return OpenReport<GMP_Reports_BonusPayout_Page>("Bonus\\+Payout", timeout);
        }

        public GMP_Reports_BonusQualificationMatrix_Page ClickBonusQulaificationsMatrix(int? timeout = null)
        {
            return OpenReport<GMP_Reports_BonusQualificationMatrix_Page>("Bonus\\+Qualifications\\+Matrix", timeout);
        }

        public GMP_Reports_CalculationOverrides_Page ClickCalculationOverrides(int? timeout = null)
        {
            return OpenReport<GMP_Reports_CalculationOverrides_Page>("Calculation\\+Overrides", timeout);
        }

        public GMP_Reports_CalculatedSalesByRep_Page ClickCalculatedSalesByRep(int? timeout = null)
        {
            return OpenReport<GMP_Reports_CalculatedSalesByRep_Page>("Commission\\+Calculated\\+Sales\\+by\\+Rep", timeout);
        }

        public GMP_Reports_CalculatedSalesByRepDrillthrough_Page ClickCalculatedSalesByRepDrillthrough(int? timeout = null)
        {
            return OpenReport<GMP_Reports_CalculatedSalesByRepDrillthrough_Page>("Commission\\+Calculated\\+Sales\\+by\\+Rep\\+Drillthrough", timeout);
        }

        public GMP_Reports_CommissionableOrders_Page ClickCommissionableOrders(int? timeout = null)
        {
            return OpenReport<GMP_Reports_CommissionableOrders_Page>("Commissionable\\+Orders", timeout);
        }

        public GMP_Reports_CommissionsEarnings_Page ClickCommissionsEarnings(int? timeout = null)
        {
            return OpenReport<GMP_Reports_CommissionsEarnings_Page>("Commissions\\+Earnings", timeout);
        }

        public GMP_Reports_Demotions_Page ClickDemotions(int? timeout = null)
        {
            return OpenReport<GMP_Reports_Demotions_Page>("Demotions", timeout);
        }

        public GMP_Reports_DisbursementHolds_Page ClickDisbursementHolds(int? timeout = null)
        {
            return OpenReport<GMP_Reports_DisbursementHolds_Page>("Disbursement\\+Holds", timeout);
        }

        public GMP_Reports_EnhancedBonusDetails_Page ClickEnhancedBonusDetails(int? timeout = null)
        {
            return OpenReport<GMP_Reports_EnhancedBonusDetails_Page>("Enhanced\\+Bonus\\+Details", timeout);
        }

        public GMP_Reports_ExpiringCreditCardNumbers_Page ClickExpiringCreditCardNumbers(int? timeout = null)
        {
            return OpenReport<GMP_Reports_ExpiringCreditCardNumbers_Page>("Expiring\\+Credit\\+Card\\+Numbers", timeout);
        }

        public GMP_Reports_InvalidRoutingNumbers_Page ClickInvalidRoutingNumbers(int? timeout = null)
        {
            return OpenReport<GMP_Reports_InvalidRoutingNumbers_Page>("Invalid\\+Routing\\+Numbers", timeout);
        }

        public GMP_Reports_MonthlyBonusesByTitle_Page ClickMonthlyBonusByTitle(int? timeout = null)
        {
            return OpenReport<GMP_Reports_MonthlyBonusesByTitle_Page>("Monthly\\+Bonuses\\+By\\+Title", timeout);
        }

        public GMP_Reports_OrdersByCreditCardNumber_Page ClickOrdersByCreditCardNumber(int? timeout = null)
        {
            return OpenReport<GMP_Reports_OrdersByCreditCardNumber_Page>("Orders\\+by\\+Credit\\+Card\\+Number", timeout);
        }

        public GMP_Reports_ProductCreditBalance_Page ClickProductCreditBalance(int? timeout = null)
        {
            return OpenReport<GMP_Reports_ProductCreditBalance_Page>("Product\\+Credit\\+Balance", timeout);
        }

        public GMP_Reports_Promotions_Page ClickPromotions(int? timeout = null)
        {
            return OpenReport<GMP_Reports_Promotions_Page>("Promotions", timeout);
        }

        public GMP_Reports_SalesTax_Page ClickSalesTax(int? timeout = null)
        {
            return OpenReport<GMP_Reports_SalesTax_Page>("Sales\\+Tax", timeout);
        }

        public GMP_Reports_Sales_Tax_Details_Page ClickSalesTaxDetails(int? timeout = null)
        {
            return OpenReport<GMP_Reports_Sales_Tax_Details_Page>("Sales\\+Tax\\+DetailedF", timeout);
        }

        public GMP_Reports_Terminations_Page ClickTerminations(int? timeout = null)
        {
            return OpenReport<GMP_Reports_Terminations_Page>("Terminations", timeout);
        }

        public GMP_Reports_TitleOverrides_Page ClickTitleOverrides(int? timeout = null)
        {
            return OpenReport<GMP_Reports_TitleOverrides_Page>("Title\\+Overrides", timeout);
        }

        public GMP_Reports_TitlePopulations_Page ClickTitlePopulations(int? timeout = null)
        {
            return OpenReport<GMP_Reports_TitlePopulations_Page>("Title\\+Populations", timeout);
        }

        public GMP_Reports_TitleQualificationMatrix_Page ClickTitleQualificationsMatrix(int? timeout = null)
        {
            return OpenReport<GMP_Reports_TitleQualificationMatrix_Page>("Title\\+Qualifications\\+Matrix", timeout);
        }

        public GMP_Reports_TopEarnersByVolume_Page ClickTopEarnersByVolume(int? timeout = null)
        {
            return OpenReport<GMP_Reports_TopEarnersByVolume_Page>("Top\\+Earners\\+by\\+Volume", timeout);
        }

        public GMP_Reports_TerminationsAndCancellations_Page ClickTerminationsAndCancellations(int? timeout = null)
        {
            return OpenReport<GMP_Reports_TerminationsAndCancellations_Page>("Terminations\\+and\\+Cancellations", timeout);
        }
    }
}
