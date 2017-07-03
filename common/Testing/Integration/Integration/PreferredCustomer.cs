using System;

namespace NetSteps.Testing.Integration
{
    public class PreferredCustomer : RetailCustomer
    {
        public PreferredCustomer()
            : base()
        { }

        public PreferredCustomer(string userName, string password)
            : base(userName, password)
        { }

        public PreferredCustomer(string firstName, string lastName, string id)
            : base(firstName, lastName, id)
        { }

        public PreferredCustomer(string firstName, string lastName, string username, string password)
            : base(firstName, lastName, username, password)
        { }

        public PreferredCustomer(string firstName, string lastName, string id, Address mainAddress)
            : base(firstName, lastName, id, mainAddress)
        { }

        public PreferredCustomer(string firstName, string lastName, string email, Language.ID language)
            : base(firstName, lastName, email, language)
        { }

        public PreferredCustomer(string firstName, string lastName, string email, Language.ID language, Address mainAddress)
            : base(firstName, lastName, email, language, mainAddress)
        { }

        public PreferredCustomer(string firstName, string lastName, string email, string username, string password, Language.ID language)
            : base(firstName, lastName, email, username, password, language)
        { }

        public PreferredCustomer(string firstName, string lastName, string email, string username, string password, Language.ID language, Address mainAddress)
            : base(firstName, lastName, email, username, password, language, mainAddress)
        { }
    }
}
