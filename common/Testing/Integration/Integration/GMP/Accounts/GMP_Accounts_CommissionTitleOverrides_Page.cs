using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_CommissionTitleOverrides_Page : GMP_Accounts_Section_Page
    {
         public override bool IsPageRendered()
        {
            return Document.GetElement<Link>(new Param("/Accounts/AccountTitleOverrides/Edit", AttributeName.ID.Href, RegexOptions.None)).Exists;  //Title.Contains("Commission Title Overrides");
        }
    }
}
