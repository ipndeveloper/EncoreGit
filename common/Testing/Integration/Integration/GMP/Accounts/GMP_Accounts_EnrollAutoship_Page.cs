using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_EnrollAutoship_Page : GMP_Accounts_Base_Page
    {
        private Link _next, _skip;
        private UnorderedList _autoships;
        private Para _enroll;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _next = Document.GetElement<Link>(new Param("btnNext"));
            _skip = Document.GetElement<Link>(new Param("btnSkip"));
            _autoships = Document.GetElement<UnorderedList>(new Param("Autoships"));
            _autoships.CustomWaitForVisibility();
            _enroll = Document.GetElement<Para>(new Param("Enrollment SubmitPage", AttributeName.ID.ClassName));
        }

         public override bool IsPageRendered()
        {
            return _autoships.Exists;
        }

         public TPage ClickNext<TPage>(int? timeout = null, bool pageRequired = true) where TPage : GMP_Accounts_Base_Page, new()
        {
            timeout = _next.CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

         public TPage ClickSkip<TPage>(int? timeout = null, bool pageRequired = true) where TPage : GMP_Accounts_Base_Page, new()
        {
            timeout = _skip.CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }
    }
}
