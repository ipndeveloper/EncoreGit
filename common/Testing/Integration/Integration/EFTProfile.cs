using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Testing.Integration
{
    public class EFTProfile
    {

        public EFTProfile()
        {
        }

        public EFTProfile(string accountName, string routing, string accountNumber, string bank, Phone bankPhone, Address bankAddress, string accountType, int depositPercentage)
        {
            AccountName = accountName;
            Routing = routing;
            AccountNumber = accountNumber;
            Bank = bank;
            BankPhone = bankPhone;
            BankAddress = bankAddress;
            AccountType = accountType;
            DepositPercentage = depositPercentage;
        }

        public string AccountName
        { get; set; }

        public string Routing
        { get; set; }

        public string AccountNumber
        { get; set; }

        public string Bank
        { get; set; }

        public Phone BankPhone
        { get; set; }

        public Address BankAddress
        { get; set; }

        public string AccountType
        { get; set; }

        public int DepositPercentage
        { get; set; }
        
    }
}
