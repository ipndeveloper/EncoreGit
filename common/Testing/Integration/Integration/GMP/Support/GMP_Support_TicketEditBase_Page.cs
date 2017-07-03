using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Support
{
    public abstract class GMP_Support_TicketEditBase_Page : GMP_Support_Base_Page
    {
        public GMP_Support_TicketEditSectionNav_Control SectionNav
        {
            get { return _secNav.As<GMP_Support_TicketEditSectionNav_Control>(); }
        }
    }
}
