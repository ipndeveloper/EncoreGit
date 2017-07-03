using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class SupportTickets : DynamicModel
    {
        public SupportTickets() : base("Core", "SupportTickets", "SupportTicketID") { }
    }
}
