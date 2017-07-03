using WatiN.Core;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace NetSteps.Testing.Integration.GMP.Admin
{
    public class GMP_Admin_Roles_Page : GMP_Admin_Base_Page
    {
        private Link _addRole;
        private TableCell _coreContent;


        protected override void  InitializeContents()
        {
 	         base.InitializeContents();
            _addRole = Document.GetElement<Link>(new Param("Add new role", AttributeName.ID.InnerText, RegexOptions.None));
            _coreContent = Document.GetElement<TableCell>(new Param("CoreContent", AttributeName.ID.ClassName));
        }

         public override bool IsPageRendered()
        {
            return Document.GetElement<Link>(new Param("/Admin/Roles/Edit", AttributeName.ID.Href, RegexOptions.None)).Exists;
        }

         public GMP_Admin_RoleEdit_Page ClickAddNewRole(int? timeout = null, bool pageRequired = true)
        {
            timeout = Document.GetElement<Link>(new Param("Add new role", AttributeName.ID.InnerText, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Admin_RoleEdit_Page>(timeout, pageRequired);
        }

        public List<string> Roles()
        {
            List<string> roles = new List<string>();
            foreach (Link role in _coreContent.UnorderedList(Find.Any).Links)
            {
                if (!string.IsNullOrEmpty(role.CustomGetText()))
                {
                    roles.Add(role.CustomGetText());
                }
            }
            return roles;
        }

        public GMP_Admin_RoleEdit_Page EditRole(string role = null, int? timeout = null, bool pageRequired = true)
        {
            if (string.IsNullOrEmpty(role))
            {
                List<string> roles = Roles();
                role = roles[Util.GetRandom(0, roles.Count - 1)];
            }

            timeout = _coreContent.GetElement<Link>(new Param(role, AttributeName.ID.InnerText, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Admin_RoleEdit_Page>(timeout, pageRequired);
        }
    }
}
