using System;
using NetSteps.Addresses.Common.Models;

namespace NetSteps.Data.Entities.Business.Interfaces
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Interface to make working with payments on all different objects easier. - JHE
    /// Created: 4/13/2010
    /// </summary>
    public interface IPayment
    {
        string NameOnCard { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string AccountName { get; set; }
        string BankName { get; set; }
        string AccountNumber { get; }
        string RoutingNumber { get; set; }
        short? BankAccountTypeID { get; set; }
        string DecryptedAccountNumber { get; set; }
        DateTime? ExpirationDate { get; set; }
        string CVV { get; set; }
        int PaymentTypeID { get; set; }
        IAddress BillingAddress { get; set; }
        bool IsDefault { get; set; }
        short? PaymentGatewayID { get; set; }
        bool CanPayForTax { get; }
        bool CanPayForShippingAndHandling { get; }
        bool IsCommissionable { get; }
    }
}