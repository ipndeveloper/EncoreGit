using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public abstract class GMP_Accounts_LedgerBase_Page : GMP_Accounts_Section_Page
    {
        public GMP_Accounts_Ledger_Page ClickAccountLedger(int? timeout = null, bool pageRequired = true)
        {
            timeout = Document.GetElement<Link>(new Param("/Accounts/Ledger", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_Ledger_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_ProductCreditLedger_Page ClickProductCreditLedger(int? timeout = null, bool pageRequired = true)
        {
            timeout = Document.GetElement<Link>(new Param("/Accounts/Ledger/ProductCredit", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_ProductCreditLedger_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_IncentivePointsLedger_Page ClickIncentivePointsLedger(int? timeout = null, bool pageRequired = true)
        {
            timeout = Document.GetElement<Link>(new Param("/Accounts/Ledger/IncentivePoints", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_IncentivePointsLedger_Page>(timeout, pageRequired);
        }
    }
}
