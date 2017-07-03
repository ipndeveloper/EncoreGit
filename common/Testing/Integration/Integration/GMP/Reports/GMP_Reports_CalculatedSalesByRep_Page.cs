using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_CalculatedSalesByRep_Page : GMP_Reports_Viewer_Page
    {
        private TableRow _parameters;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
            Util.LogFail("page not available");
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }
    }
}
