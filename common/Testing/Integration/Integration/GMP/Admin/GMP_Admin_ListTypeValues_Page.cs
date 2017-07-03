using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Admin
{
    public class GMP_Admin_ListTypeValues_Page : GMP_Admin_Base_Page
    {
         public override bool IsPageRendered()
        {
            return Document.GetElement<Link>(new Param("/Admin/ListTypes", AttributeName.ID.Href, RegexOptions.None)).Exists && Document.GetElement<Link>(new Param("btnAdd")).Exists;
        }

         public GMP_Admin_ListTypes_Page Cancel(int? timeout = null, bool pageRequired = true)
        {
            timeout = Document.GetElement<Link>(new Param("Button", AttributeName.ID.ClassName)).CustomClick(timeout);
            return Util.GetPage<GMP_Admin_ListTypes_Page>(timeout, pageRequired);
        }
    }
}
