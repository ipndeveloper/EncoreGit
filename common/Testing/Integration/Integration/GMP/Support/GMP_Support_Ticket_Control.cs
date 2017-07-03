using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Support
{
    public class GMP_Support_Ticket_Control : Control<TableRow>
    {
        private Link _number;

        protected override void  InitializeContents()
        {
 	        base.InitializeContents();
            _number = Element.GetElement<TableCell>(new Param(0)).GetElement<Link>();
        }

        public string Number
        {
            get { return _number.CustomGetText(); }
        }

        public GMP_Support_TicketEditDetails_Page SelectTicket(int? timeout = null, bool pageRequired = true)
        {
            timeout = _number.CustomClick(timeout);
            return Util.GetPage<GMP_Support_TicketEditDetails_Page>(timeout, pageRequired);
        }
    }
}
