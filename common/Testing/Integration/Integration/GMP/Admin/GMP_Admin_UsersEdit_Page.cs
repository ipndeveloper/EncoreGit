using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Admin
{
    public class GMP_Admin_UsersEdit_Page : GMP_Admin_Base_Page
    {
        protected override void  InitializeContents()
        {
 	         base.InitializeContents();
        }

         public override bool IsPageRendered()
        {
            return Document.GetElement<TableCell>(new Param("roles")).Exists;
        }

         public GMP_Admin_Roles_Page Cancel(int? timeout = null, bool pageRequired = true)
        {
            timeout = Document.GetElement<Link>(new Param("Button", AttributeName.ID.ClassName)).CustomClick(timeout);
            return Util.GetPage<GMP_Admin_Roles_Page>(timeout, pageRequired);
        }
    }
}
