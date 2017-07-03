using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IPartyRepository : ISearchRepository<PartySearchParameters, PaginatedList<PartySearchData>>
    {
		List<PartySearchData> GetOpenParties(int accountID, List<int> statuses);
        bool HasHostedParties(int accountID);
        List<Party> GetHostedParties(int accountID);
        List<Party> GetHostedPartiesByDate(DateTime partyEventDate);
        Party LoadWithGuests(int partyID);
        Party LoadFullByOrderID(int orderID);
        Party LoadFullWithChildParties(int partyID);
        bool IsParty(int orderID);
        int GetTotalBookingCreditsRedeemed(int parentPartyOrderID);
        int GetTotalBookedCustomerCountForParty(int partyID);
        Party LoadByOrderID(int orderID);
    }
}
