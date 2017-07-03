using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_Ledger_Page : GMP_Accounts_LedgerBase_Page
    {
         public override bool IsPageRendered()
        {
            return Document.GetElement<Link>(new Param("addLedger")).Exists;
        }
    }
}
