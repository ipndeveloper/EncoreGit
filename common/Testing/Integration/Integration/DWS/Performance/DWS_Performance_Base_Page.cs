using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.DWS.Performance
{
    public class DWS_Performance_Base_Page : DWS_Base_Page
    {
        private DWS_Performance_SectionNav_Control _secNav;
        private Div salesIndicator;
        protected Table tableDWSCommonGrid;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _secNav = _sectionNavList.As<DWS_Performance_SectionNav_Control>();
            this.tableDWSCommonGrid = Document.GetElement<Table>(new Param("paginatedGrid"));
            salesIndicator = Document.GetElement<Div>(new Param("SalesIndicator"));
        }

        public override bool IsPageRendered()
        {
            return salesIndicator.Exists;
        }

        public DWS_Performance_SectionNav_Control SectionNav
        {
            get { return _secNav; }
        }
    }
}
