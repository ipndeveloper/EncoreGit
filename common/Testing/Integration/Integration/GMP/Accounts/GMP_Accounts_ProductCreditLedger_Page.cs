using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_ProductCreditLedger_Page : GMP_Accounts_LedgerBase_Page
    {
         protected override void InitializeContents()
        {
            base.InitializeContents();
        }

         public override bool IsPageRendered()
        {
            return Document.GetElement<Link>(new Param("addLedger")).Exists && !Document.GetElement<Link>(new Param("/Accounts/Ledger/ProductCredit", AttributeName.ID.Href, RegexOptions.None)).Exists;
        }
    }
}
