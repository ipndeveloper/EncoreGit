using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Commissions
{
    public class GMP_Commissions_Disbursements_Page : GMP_Commissions_Base_Page
    {
        private SelectList _period;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _period = Document.SelectList("periodID");
        }

         public override bool IsPageRendered()
        {
            return Document.GetElement<TextField>(new Param("periodID")).Exists;
        }
    }
}
