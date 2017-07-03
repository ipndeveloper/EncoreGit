using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_EnrollShipping_Page : GMP_Accounts_Base_Page
    {
        private Link _next, _skip;
        private RadioButton _shipMethod0, _shipMethod1, _shipMethod2, _shipMethod3;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _next = Document.GetElement<Link>(new Param("btnNext"));
            _skip = Document.GetElement<Link>(new Param("btnSkip"));
            Param shippingMethod = new Param("shippingMethod", AttributeName.ID.Id, RegexOptions.None);
            _shipMethod0 = Document.GetElement<RadioButton>(new Param(0).And(shippingMethod));
            _shipMethod1 = Document.GetElement<RadioButton>(new Param(1).And(shippingMethod));
            _shipMethod2 = Document.GetElement<RadioButton>(new Param(2).And(shippingMethod));
            _shipMethod3 = Document.GetElement<RadioButton>(new Param(3).And(shippingMethod));
        }

         public override bool IsPageRendered()
        {
            return _shipMethod0.Exists;
        }


         public GMP_Accounts_EnrollDisbursementProfiles_Page ClickSkip(int? timeout = null, bool pageRequired = true)
        {
            timeout = _skip.CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_EnrollDisbursementProfiles_Page>(timeout, pageRequired);
        }


         public GMP_Accounts_EnrollDisbursementProfiles_Page ClickNext(int? timeout = null, bool pageRequired = true)
        {
            timeout = _next.CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_EnrollDisbursementProfiles_Page>(timeout, pageRequired);
        }
    }
}
