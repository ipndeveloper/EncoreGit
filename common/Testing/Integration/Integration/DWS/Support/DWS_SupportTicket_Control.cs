using System;
using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Support
{
    public class DWS_SupportTicket_Control : Control<TableRow>
    {
        private Link _ticket;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _ticket = Element.GetElement<TableCell>(new Param(0)).GetElement<Link>();
        }

        public string Title
        {
            get { return Element.GetElement<TableCell>(new Param(1)).CustomGetText(); }
        }
    }
}
