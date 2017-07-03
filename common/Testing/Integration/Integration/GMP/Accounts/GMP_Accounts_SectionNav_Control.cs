using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_SectionNav_Control : Control<UnorderedList>
    {

        public GMP_Accounts_Overview_Page ClickOverview(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Overview", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_Overview_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_Edit_Page ClickEditAccount(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Edit", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_Edit_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_BillingAndShipping_Page ClickBillingAndShippingProfiles(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/BillingShippingProfiles", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_BillingAndShipping_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_Ledger_Page ClickLedger(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Ledger", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_Ledger_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_DisbursementProfiles_Page ClickDisbursementProfiles(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/DisbursementProfiles", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_DisbursementProfiles_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_OrderHistory_Page ClickOrderHistory(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/OrderHistory", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_OrderHistory_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_CommissionCalculationOverrides_Page ClickCalculationOverrides(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/CalculationOverrides", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_CommissionCalculationOverrides_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_CommissionTitleOverrides_Page ClickTitleOverrides(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/AccountTitleOverrides", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_CommissionTitleOverrides_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_PaymentDisbursementHolds_Page ClickPaymentDisbursementHolds(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/DisbursementHolds", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_PaymentDisbursementHolds_Page>(timeout, pageRequired);
        }
    }
}
