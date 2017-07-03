using System.Collections.Generic;

namespace NetSteps.Testing.Integration
{
    public class Distributor : PreferredCustomer
    {
        public Distributor()
            : base()
        {}

        public Distributor(string userName, string password)
            : base(userName, password)
        {}

        public Distributor(string firstName, string lastName, string id)
            : base(firstName, lastName, id)
        {}

        public Distributor(string firstName, string lastName, string username, string password)
            : base(firstName, lastName, username, password)
        {}

        public Distributor(string firstName, string lastName, string id, Address mainAddress)
            : base(firstName, lastName, id, mainAddress)
        {}

        public Distributor(string firstName, string lastName, string email, Language.ID language)
            : base(firstName, lastName, email, language)
        {
        }

        public Distributor(string firstName, string lastName, string email, string username, string password, Language.ID language)
            : base(firstName, lastName, email, username, password, language)
        {}

        public Distributor(string firstName, string lastName, string email, string username, string password, Language.ID language, Address mainAddress)
            : base(firstName, lastName, email, username, password, language, mainAddress)
        {}

        public string TaxExemptReason
        { get; set; }

        public Address CheckDisbursementAddress
        { get; set; }

        public string Website
        { get; set; }

        public EFT EFT
        { get; set; }

        public Address CheckAddress
        { get; set; }


    }
}
