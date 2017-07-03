using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Admin
{
    public class GMP_Admin_Users_Page : GMP_Admin_Base_Page
    {
         public override bool IsPageRendered()
        {
            return Document.GetElement<Link>(new Param("/Admin/Users/Edit", AttributeName.ID.Href, RegexOptions.None)).Exists;
        }

         public GMP_Admin_UsersEdit_Page ClickAddUser(int? timeout = null, bool pageRequired = true)
        {
            timeout = Document.GetElement<Link>(new Param("/Admin/Users/Edit", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Admin_UsersEdit_Page>(timeout, pageRequired);
        }
    }
}
