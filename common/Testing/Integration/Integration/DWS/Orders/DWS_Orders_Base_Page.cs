using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Orders
{
    public abstract class DWS_Orders_Base_Page : DWS_Base_Page
    {
        private DWS_Orders_SectionNav_Control _sectionNav;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _sectionNav = _sectionNavList.As<DWS_Orders_SectionNav_Control>();
        }

        public DWS_Orders_SectionNav_Control SectionNav
        {
            get { return _sectionNav; }
        }
    }
}
