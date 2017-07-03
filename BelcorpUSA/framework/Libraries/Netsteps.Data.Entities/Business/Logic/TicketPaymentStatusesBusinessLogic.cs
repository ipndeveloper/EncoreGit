using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class TicketPaymentStatusesBusinessLogic
    {

        public IEnumerable<dynamic> GetAllTicketPaymentStatuses()
        {
            var table = new TicketPaymentStatuses();
            return table.All();
        }

        public IEnumerable<dynamic> GetTicketPaymentStatusesByArrayId(string ticketPaymentStatusIdArray)
        {
            var table = new TicketPaymentStatuses();
            var ticketPaymentStatusList = table.Query("EXEC UPS_GET_TICKETPAYMENTSTATUSED_BY_ARRAY_ID @0", ticketPaymentStatusIdArray);
            return ticketPaymentStatusList;
        }

    }
}