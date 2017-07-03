using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.TokenValueProviders;
using NetSteps.QueueProcessing.Modules.ModuleBase;

namespace NetSteps.QueueProcessing.Modules.DomainEvent.Class
{
    public class PartyGuestReminderCampaignEmailSender : CampaignEmailSender
    {
        //List of parties for them mail should be sent
        public List<PartyGuest> PartyGuests { get; set; }

        protected override IEnumerable<PartyGuest> GetRecipientEmailNameAndAddress()
        {
            return PartyGuests;
        }

        protected override string GetRecipientEmailAddress()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// no implemention is required for this class
        /// </summary>
        /// <returns></returns>
        protected override string GetRecipientFullName()
        {
            throw new NotImplementedException();
        }

        protected override int GetRecipientLanguageID()
        {
            return (int)Constants.Language.English;
        }

        /// <summary>
        /// no implemention is required for this class
        /// </summary>
        /// <returns></returns>
        protected override ITokenValueProvider GetTokenValueProvider()
        {
            throw new NotImplementedException();
        }

        protected override ITokenValueProvider GetTokenValueProvider(PartyGuest guest)
        {
            return new PartyGuestTokenValueProvider(guest);
        }

        public PartyGuestReminderCampaignEmailSender(Data.Entities.DomainEventQueueItem domainEventQueueItem, Data.Entities.EmailCampaignAction emailCampaignAction)
            : base(domainEventQueueItem, emailCampaignAction)
        {
            InitializeMembers(domainEventQueueItem);
        }

        private void InitializeMembers(DomainEventQueueItem domainEventQueueItem)
        {
            PartyGuests = new List<PartyGuest>();
            if (domainEventQueueItem.EventContext != null && domainEventQueueItem.EventContext.PartyID.HasValue)
            {
                var partyGuests = GetPartyGuests(domainEventQueueItem);
                var account = new Account();
                foreach (var guest in partyGuests)
                {
                    //If the account id is null or 0 than the guest has not become as Retail Customer/or done any order
                    //consider this guests for senidng the order
                    if (guest.AccountID == null || guest.AccountID == 0)
                    {
                        PartyGuests.Add(guest);
                    }
                }
            }
        }

        /// <summary>
        /// Get party guests 
        /// </summary>
        /// <param name="domainEventQueueItem"></param>
        /// <returns></returns>
        private List<PartyGuest> GetPartyGuests(DomainEventQueueItem domainEventQueueItem)
        {
            //get the event context
            var eventContext = EventContext.Load(domainEventQueueItem.EventContextID);

            var partyId = (int)eventContext.PartyID;
            var party = Party.LoadWithGuests(partyId);
            return party.PartyGuests.ToList();
        }
    }
}
