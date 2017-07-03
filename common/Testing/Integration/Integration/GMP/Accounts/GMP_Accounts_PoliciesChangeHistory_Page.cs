using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_PoliciesChangeHistory_Page : GMP_Accounts_OverviewBase_Page
    {
         public override bool IsPageRendered()
        {
            return Document.GetElement<Link>(new Param("/Accounts/Overview/AuditHistory", AttributeName.ID.Href, RegexOptions.None)).Exists && !Document.GetElement<Link>(new Param("/Accounts/Overview/PoliciesChangeHistory", AttributeName.ID.Href, RegexOptions.None)).Exists;
        }
    }
}
