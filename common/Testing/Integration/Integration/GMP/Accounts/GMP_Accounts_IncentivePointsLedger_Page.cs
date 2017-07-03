using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_IncentivePointsLedger_Page : GMP_Accounts_LedgerBase_Page
    {
         public override bool IsPageRendered()
        {
            return Document.GetElement<Link>(new Param("/Accounts/Ledger/ProductCredit", AttributeName.ID.Href, RegexOptions.None)).Exists && !Document.GetElement<Link>(new Param("/Accounts/Ledger/IncentivePoints", AttributeName.ID.Href, RegexOptions.None)).Exists;
        }
    }
}
