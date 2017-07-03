using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_CommissionCalculationOverrides_Page : GMP_Accounts_Section_Page
    {
        Link _addOverride;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _addOverride = Document.GetElement<Link>(new Param("/Accounts/CalculationOverrides/Edit/", AttributeName.ID.Href, RegexOptions.None));
        }

         public override bool IsPageRendered()
        {
            return _addOverride.Exists;
        }

         public GMP_Accounts_CommissionCalculationOverridesEdit_Page ClickAddOverride(int? timeout = null, bool pageRequired = true)
        {
            timeout = _addOverride.CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_CommissionCalculationOverridesEdit_Page>(timeout, pageRequired);
        }
    }
}
