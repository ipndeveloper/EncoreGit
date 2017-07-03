using System.Collections.Generic;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.TokenValueProviders
{
    public class ContactMeTokenValueProvider : ITokenValueProvider
    {
        private const string CONTACTME_NAME = "ContactMeName";
        private const string CONTACTME_EMAIL = "ContactMeEmail";
        private const string CONTACTME_PHONE = "ContactMePhone";
        private const string CONTACTME_MESSAGE = "ContactMeMessage";
        private const string CONTACTME_STATE = "ContactMeState";

        private readonly string _name;
        private readonly string _phone;
        private readonly string _email;
        private readonly string _message;
        private readonly string _state;

        public ContactMeTokenValueProvider(string name, string email, string phone, string message, string state)
        {
            _name = name;
            _phone = phone;
            _email = email;
            _message = message;
            _state = state;
        }

        public virtual IEnumerable<string> GetKnownTokens()
        {
            return new List<string>()
            {
                CONTACTME_NAME,
                CONTACTME_EMAIL,
                CONTACTME_PHONE,
                CONTACTME_MESSAGE,
                CONTACTME_STATE
            };  
        }       
                
        public virtual string GetTokenValue(string token)
        {
            switch (token)
            {
                case CONTACTME_NAME:
                    return _name;
                case CONTACTME_EMAIL:
                    return _email;
                case CONTACTME_PHONE:
                    return _phone;
                case CONTACTME_MESSAGE:
                    return _message;
                case CONTACTME_STATE:
                    return _state;
                default:
                    return null;
            }
        }
    }
}
