using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_AuditHistory_Page : GMP_Accounts_OverviewBase_Page
    {
         public override bool IsPageRendered()
        {
            return Title.Contains("Audit History");
        }
    }
}
