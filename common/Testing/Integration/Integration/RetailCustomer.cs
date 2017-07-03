using System;
using System.Collections.Generic;
using NetSteps.Testing.Integration.GMP.Accounts;
using NetSteps.Testing.Integration.DWS.Orders.Party;

namespace NetSteps.Testing.Integration
{
    public class RetailCustomer
    {
        List<Phone> _phones = new List<Phone>();

        #region Constructors

        public RetailCustomer()
        {
        }

        public RetailCustomer(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public RetailCustomer(string firstName, string lastName, string id)
        {
            FirstName = firstName;
            LastName = lastName;
            ID = id;
        }

        public RetailCustomer(string firstName, string lastName, string username, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = username;
            Password = password;
        }

        public RetailCustomer(string firstName, string lastName, string id, Address mainAddress)
            : this(firstName, lastName, id)
        {
            MainAddress = mainAddress;
        }

        public RetailCustomer(string firstName, string lastName, string email, Language.ID language)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Language = language;
        }

        public RetailCustomer(string firstName, string lastName, string email, Language.ID language, Address mainAddress)
            : this(firstName, lastName, email, language)
        {
            MainAddress = mainAddress;
        }

        public RetailCustomer(string firstName, string lastName, string email, string username, string password, Language.ID language)
            : this(firstName, lastName, email, language)
        {
            UserName = username;
            Password = password;
        }

        public RetailCustomer(string firstName, string lastName, string email, string username, string password, Language.ID language, Address mainAddress)
            : this(firstName, lastName, email, username, password, language)
        {
            MainAddress = mainAddress;
        }

        #endregion Constructors

        #region Properties

        public string ID
        { get; set; }

        public AccountStatus.ID AccountStatus
        { get; set; }

        public bool ApplicationOnFile
        { get; set; }

        public string BusinessName
        { get; set; }

        public string Email
        { get; set; }

        public string FirstName
        { get; set; }

        public string MiddleName
        { get; set; }

        public string LastName
        { get; set; }

        public DateTime BirthDate
        { get; set; }

        public Gender.ID Gender
        { get; set; }

        public Language.ID Language
        { get; set; }

        public Distributor Sponsor
        { get; set; }

        public Distributor Placement
        { get; set; }

        public bool TaxExempt
        { get; set; }

        public string TaxID
        { get; set; }

        public string UserName
        { get; set; }

        public string Password
        { get; set; }

        public Address MainAddress
        { get; set; }

        public Billing Billing
        { get; set; }

        public Shipping Shipping
        { get; set; }

        public ShoppingBag ShoppingBag
        { get; set; }

        public Phones Phones
        { get; set; }

        public decimal ProductCredit
        { get; set; }

        #endregion Properties
    }
}
