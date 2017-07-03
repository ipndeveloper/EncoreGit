using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Performance
{
    public class DWS_Perforformance_Report_Page : DWS_Performance_Base_Page
    {
        private Div _footerInfo;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _footerInfo = Document.GetElement<Div>(new Param("gridFooterInfo"));
        }

         public override bool IsPageRendered()
        {
            return _footerInfo.Exists;
        }
    }
}
