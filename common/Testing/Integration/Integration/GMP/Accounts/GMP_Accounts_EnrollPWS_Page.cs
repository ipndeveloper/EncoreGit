using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_EnrollPWS_Page : GMP_Accounts_Base_Page
    {
        private TextField _txtWebsiteURL;
        private Link _next, _skip;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _txtWebsiteURL = Document.GetElement<TextField>(new Param("subdomain required", AttributeName.ID.ClassName));
            _next = Document.GetElement<Link>(new Param("btnNext"));
            _skip = Document.GetElement<Link>(new Param("btnSkip"));
        }

         public override bool IsPageRendered()
        {
            return _txtWebsiteURL.Exists;
        }

        public GMP_Accounts_EnrollPWS_Page EnterPWSUrl(string urlPWS)
        {
            _txtWebsiteURL.CustomSetTextQuicklyHelper(urlPWS);
            return this;
        }

        public GMP_Accounts_EnrollCompletion_Page ClickNext(int? timeout = null, bool pageRequired = true)
        {
            timeout = _next.CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_EnrollCompletion_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_EnrollCompletion_Page ClickSkip(int? timeout = null, bool pageRequired = true)
        {
            timeout = _skip.CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_EnrollCompletion_Page>(timeout, pageRequired);
        }
    }
}
