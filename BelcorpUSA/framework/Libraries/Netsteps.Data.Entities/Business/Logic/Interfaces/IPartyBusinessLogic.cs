using System;
using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IPartyBusinessLogic
    {
        IEnumerable<HostessRewardRule> GetApplicableHostessRewardRules(Party party, int? marketID = null);
        OrderCustomer GetOrderCustomerForHostessRewardRule(Party party, HostessRewardRule hostessRewardRule);
        DateTime GetMaximumFuturePartyDate(Party party);
        OrderCustomer GetParentPartyHostess(Party party);
        void SendReminderToPartyGuests(IEnumerable<Party> parties);
        bool HasMetTotalAboveMinimumAmount(Party party);
        void IsOrderTotalAboveMinimumAmount(Party entity);
    }
}
