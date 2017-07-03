using System.Collections.Generic;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.TokenValueProviders
{
    public class SupportTicketTokenValueProvider : ITokenValueProvider
    {

        private const string RECIPIENT_FIRST_NAME = "RecipientFirstName";
        private const string RECIPIENT_LAST_NAME = "RecipientLastName";
        private const string RECIPIENT_FULL_NAME = "RecipientFullName";
        private const string SUPPORT_TICKET_URL = "SupportTicketURL";

        private SupportTicket _ticket;
        private string _distributorWorkstationUrl;

        public SupportTicketTokenValueProvider(SupportTicket supportTicket, string distributorWorkstationUrl)
        {
            _ticket = supportTicket;
            _distributorWorkstationUrl = distributorWorkstationUrl;
        }

        public IEnumerable<string> GetKnownTokens()
        {
            return new List<string>()
            {
                RECIPIENT_FIRST_NAME,
                RECIPIENT_LAST_NAME,
                RECIPIENT_FULL_NAME, 
                SUPPORT_TICKET_URL,
            };
        }

        public string GetTokenValue(string token)
        {
            switch (token)
            {
                case RECIPIENT_FIRST_NAME:
                    return _ticket.Account.FirstName;
                case RECIPIENT_LAST_NAME:
                    return _ticket.Account.LastName;
                case RECIPIENT_FULL_NAME:
                    return _ticket.Account.FullName;
                case SUPPORT_TICKET_URL:
                    return string.Format("{0}/Support/ViewTicket/{1}", _distributorWorkstationUrl, _ticket.SupportTicketID.ToString());
                default:
                    return null;
            }
        }
    }
}
