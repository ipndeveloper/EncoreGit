using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Commissions
{
    public class GMP_Commissions_Runner_Page : GMP_Commissions_Base_Page
    {
         public override bool IsPageRendered()
        {
            return SubNav.Element.GetElement<ListItem>(new Param("selected", AttributeName.ID.ClassName)).GetElement<Link>(new Param("/Commissions/Runner", AttributeName.ID.Href, RegexOptions.None)).Exists;
        }
    }
}
