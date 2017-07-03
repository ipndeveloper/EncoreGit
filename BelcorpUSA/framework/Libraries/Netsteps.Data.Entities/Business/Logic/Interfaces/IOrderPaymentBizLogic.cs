using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Business.HelperObjects;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public interface IOrderPaymentBizLogic
    {

        List<DebtsPerAgeSearchData> GetTableDebtsPerAge(DebtsPerAgeSearchParameters parameters);
        List<DebtsPerAgeSearchData> TableDebtsPerAgeExport(DebtsPerAgeSearchParameters parameters);
        List<TicketPaymentPerMonthSearchData> GetTableTicketPaymentsPerMonth(TicketPaymentPerMonthSearchParameters parameters);
        List<TicketPaymentPerMonthSearchData> TableTicketPaymentsPerMonthExport(TicketPaymentPerMonthSearchParameters parameters);

        Dictionary<int, string> GetDropDownStatuses();
        Dictionary<int, string> GetTicketNumberLookUp(string ticketNumberPrefix);

    }
}
