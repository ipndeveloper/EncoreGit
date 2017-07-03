using System;

namespace NetSteps.Testing.Integration
{
    public class BillingProfile
    {
        public BillingProfile(CreditCard.ID cardType, string name, string account, string cvv, DateTime expiration)
        {
            CardType = cardType;
            Name = name;
            Account = account;
            CVV = cvv;
            Expiration = expiration;
        }
        public BillingProfile(CreditCard.ID cardType, string name, string account, string cvv, DateTime expiration, Address address)
            : this(cardType, name, account, cvv, expiration)
        {
            Address = address;
        }

        public string Name
        { get; set; }

        public CreditCard.ID CardType
        { get; set; }

        public string Account
        { get; set; }

        public DateTime Expiration
        { get; set; }

        public string CVV
        { get; set; }

        public Address Address
        { get; set; }
    }
}
