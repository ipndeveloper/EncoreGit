using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Dto;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;

namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    public interface IConsultSupportTicketRepository
    {
        List<ConsultSupportTicketDto> GetSupportTicket(SupportTicketSearchDetailsParameter parameters);  
    }
}
