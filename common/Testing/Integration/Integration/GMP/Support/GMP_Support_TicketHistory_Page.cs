using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Support
{
    public class GMP_Support_TicketHistory_Page : GMP_Support_TicketEditBase_Page
    {
         public override bool IsPageRendered()
        {
            return Document.GetElement<TextField>(new Param("pkInputFilter")).Exists;
        }
    }
}
