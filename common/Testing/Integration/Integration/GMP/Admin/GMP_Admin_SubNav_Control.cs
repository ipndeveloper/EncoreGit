using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Admin
{
    public class GMP_Admin_SubNav_Control : Control<UnorderedList>
    {
        public GMP_Admin_Users_Page ClickUsers(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Admin/Users", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Admin_Users_Page>(timeout, pageRequired);
        }

        public GMP_Admin_Roles_Page ClickRoles(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Admin/Roles", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Admin_Roles_Page>(timeout, pageRequired);
        }

        public GMP_Admin_ListTypes_Page ClickListTypes(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Admin/ListTypes", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Admin_ListTypes_Page>(timeout, pageRequired);
        }

        public GMP_Admin_AutoshipSchedules_Page ClickAutoshipSchedules(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Admin/AutoshipSchedules", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Admin_AutoshipSchedules_Page>(timeout, pageRequired);
        }

        public GMP_Admin_ValidateConfig_Page ClickValidateConfig(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Admin/ShowConfig", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Admin_ValidateConfig_Page>(timeout, pageRequired);
        }
    }
}
