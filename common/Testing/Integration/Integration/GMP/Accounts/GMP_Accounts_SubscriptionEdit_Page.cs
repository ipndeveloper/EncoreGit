using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_SubscriptionEdit_Page : GMP_Accounts_Base_Page
    {
        private TextField _website;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _website = _content.GetElement<TextField>(new Param("subdomain", AttributeName.ID.ClassName, RegexOptions.None));
        }

        public string Website
        {
            get { return _website.CustomGetText(); }
        }

        public override bool IsPageRendered()
        {
            return _website.Exists;
        }
    }
}
