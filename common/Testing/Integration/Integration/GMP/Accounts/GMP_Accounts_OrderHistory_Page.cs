using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_OrderHistory_Page : GMP_Accounts_Section_Page
    {
         public override bool IsPageRendered()
        {
            return Document.GetElement<Link>(new Param("/Orders/OrderEntry", AttributeName.ID.Href, RegexOptions.None)).Exists;
        }
    }
}
