using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Admin
{
    public class GMP_Admin_RoleEdit_Page : GMP_Admin_Base_Page
    {
        TextField _name;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _name = Document.GetElement<TextField>(new Param("name"));
        }
         public override bool IsPageRendered()
        {
            return Document.GetElement<TextField>(new Param("name")).Exists;
        }

        public string Role
        {
            get { return _name.CustomGetText(); }
            set { _name.CustomSetTextHelper(value); }
        }

        public bool IsRoleNameLocked
        {
            get { return Document.Image(Find.ByAlt("locked")).Exists; }
        }

        public GMP_Admin_Roles_Page Cancel(int? timeout = null, bool pageRequired = true)
        {
            timeout = Document.GetElement<Link>(new Param("Button", AttributeName.ID.ClassName)).CustomClick(timeout);
            return Util.GetPage<GMP_Admin_Roles_Page>(timeout, pageRequired);
        }
    }
}
