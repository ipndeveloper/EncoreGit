using System.Collections.Generic;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Cache;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.TokenValueProviders
{
	[ContainerRegister(typeof(PartyGuestTokenValueProvider), RegistrationBehaviors.IfNoneOther, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class PartyGuestTokenValueProvider : ITokenValueProvider
    {
        #region Tokens
        protected const string GUEST_ID = "GuestID";
        protected const string GUEST_NAME = "GuestName";
        protected const string GUEST_EMAIL = "GuestEmail";
        #endregion

        private PartyGuest _guest;

        public PartyGuestTokenValueProvider(PartyGuest guest)
        {
            _guest = guest;
        }

        public IEnumerable<string> GetKnownTokens()
        {
            return new List<string>()
            {
                GUEST_ID,
                GUEST_NAME,
                GUEST_EMAIL
            };
        }

        public virtual string GetTokenValue(string token)
        {
            if (_guest == null)
                return null;
            switch (token)
            {
                case GUEST_ID:
                    return _guest.PartyGuestID.ToString();
                case GUEST_NAME:
                    return Account.ToFullName(_guest.FirstName, string.Empty, _guest.LastName, SmallCollectionCache.Instance.Languages.GetById(ApplicationContext.Instance.CurrentLanguageID).CultureInfo);
                case GUEST_EMAIL:
                    return _guest.EmailAddress;
                default:
                    return null;
            }
        }
    }

    public class FakePartyGuestTokenValueProvider : PartyGuestTokenValueProvider
    {
        public FakePartyGuestTokenValueProvider() : base(null) { }

        public override string GetTokenValue(string token)
        {
            switch (token)
            {
                case GUEST_ID:
                    return "0";
                case GUEST_NAME:
                    return "John Guest";
                case GUEST_EMAIL:
                    return "email@email.com";
                default:
                    return null;
            }
        }
    }
}
