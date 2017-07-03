using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_Page : GMP_Reports_Base_Page
    {
        protected override void InitializeContents()
        {
            base.InitializeContents();
        }

         public override bool IsPageRendered()
        {
            return _content.Exists;
        }
    }
}
