using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_CommissionCalculationOverridesEdit_Page : GMP_Accounts_Section_Page
    {
        Form _form;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _form = Document.Form("calculationOverrideForm");
        }

         public override bool IsPageRendered()
        {
            return _form.Exists;
        }
    }
}
