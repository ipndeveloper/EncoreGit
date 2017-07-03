using System.Collections.Generic;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;

namespace DistributorBackOffice.Models.Contacts
{
    public class ViewContactModel
    {
        public Account Account { get; set; }
        public List<DistributionList> DistributionLists { get; set; }
        public OrderSearchData MostRecentOrder { get; set; }
        public bool isDistributor { get; set; }
        public bool isProspect { get; set; }
        public Address MainAddress { get; set; }
        public string AccountEmailAddress { get; set; }
    }
}