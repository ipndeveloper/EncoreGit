using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Base;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Validation.NetTiers;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities
{
    public interface IOrderPayment : IAddress, IPayment, ITempGuid
    {
        string MaskedAccountNumber { get; }
        string Cvv { get; set; }

        string Message { get; set; }

        Dictionary<string, string> ProcessingAttributes { get; set; }
        string DisplayName { get; }

        int OrderPaymentID { get; set; }

        int OrderID { get; set; }

        int? OrderCustomerID { get; set; }

        string BillingFirstName { get; set; }

        string BillingLastName { get; set; }

        string BillingName { get; set; }

        string BillingAddress1 { get; set; }

        string BillingAddress2 { get; set; }

        string BillingAddress3 { get; set; }

        string BillingCity { get; set; }

        string BillingCounty { get; set; }

        string BillingState { get; set; }

        int? BillingStateProvinceID { get; set; }

        string BillingPostalCode { get; set; }

        int? BillingCountryID { get; set; }

        string BillingPhoneNumber { get; set; }

        string IdentityNumber { get; set; }

        string IdentityState { get; set; }

        decimal Amount { get; set; }

        short OrderPaymentStatusID { get; set; }

        bool IsDeferred { get; set; }

        DateTime? ProcessOnDateUTC { get; set; }

        DateTime? ProcessedDateUTC { get; set; }

        string TransactionID { get; set; }

        decimal? DeferredAmount { get; set; }

        string DeferredTransactionID { get; set; }

        DateTime? ExpirationDateUTC { get; set; }

        byte[] DataVersion { get; set; }

        int CurrencyID { get; set; }

        int? ModifiedByUserID { get; set; }

        short? CreditCardTypeID { get; set; }

        string Request { get; set; }

        string AccountNumberLastFour { get; set; }

        int? SourceAccountPaymentMethodID { get; set; }

        string NachaClassType { get; set; }

        DateTime? NachaSentDate { get; set; }

        Country BillingCountry { get; set; }

        OrderCustomer OrderCustomer { get; set; }

        OrderPaymentStatus OrderPaymentStatus { get; set; }

        StateProvince StateProvince { get; set; }

        Currency Currency { get; set; }

        User User { get; set; }

        Order Order { get; set; }

        TrackableCollection<OrderPaymentResult> OrderPaymentResults { get; set; }

        CreditCardType CreditCardType { get; set; }

        PaymentGateway PaymentGateway { get; set; }

        BankAccountType BankAccountType { get; set; }

        ObjectChangeTracker ChangeTracker { get; set; }

        TimeZoneInfo CurrentTimeZoneInfo { get; set; }
        DateTime? ProcessOnDate { get; set; }
        DateTime? ProcessedDate { get; set; }

        /// <summary>
        /// Property to control whether a custom build lazy-loading should occure. - JHE
        /// This is used in conjunction with lazy loading to prevent regular loading and lazy loading from
        /// loading the data twice for members. - JHE
        /// </summary>
        bool IsLazyLoadingEnabled { get; set; }

        /// <summary>
        /// Determines whether this entity is to suppress events while set to true.
        /// </summary>
        [Bindable(false)]
        [BrowsableAttribute(false), XmlIgnore()]
        bool SuppressEntityEvents { get; set; }

        /// <summary>
        /// Returns the list of <see cref="Validation.ValidationRules"/> associated with this object.
        /// </summary>
        ValidationRules ValidationRules { get; }

        /// <summary>
        /// Returns <see langword="true" /> if the object is valid, 
        /// <see langword="false" /> if the object validation rules that have indicated failure. 
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Returns a list of all the validation rules that failed.
        /// </summary>
        /// <returns><see cref="Validation.BrokenRulesList" /></returns>
        BrokenRulesList BrokenRulesList { get; }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <value></value>
        /// <returns>An error message indicating what is wrong with this object. The default is an empty string ("").</returns>      
        string Error { get; }

        string GetDisplayName();
        BasicResponse IsPaymentValidForAuthorization();

        void AddDatabaseValidationRules();
        IOrderPaymentRepository GetRepository();

        /// <summary>
        /// Method to set all child object to start tracking changes to the Entities. - JHE
        /// </summary>
        void StartEntityTracking();

        /// <summary>
        /// Method to set all child object to start tracking changes to the Entities and enable lazy loading. - DES
        /// </summary>
        void StartEntityTrackingAndEnableLazyLoading();

        void StopEntityTracking();

        /// <summary>
        /// This method is called after saving an Entity to mark the object as unchanged in the Self Tracking Entity. - JHE
        /// </summary>
        void AcceptEntityChanges(List<IObjectWithChangeTracker> allTrackerItems = null);

        string GetValidationErrorMessage();

        /// <summary>
        /// Save Entity to the DataBase. - JHE
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        void Save();

        /// <summary>
        /// Delete Entity from the DataBase. - JHE
        /// </summary>
        /// <param name="obj"></param>
        void Delete();

        /// <summary>
        /// Adds a rule to the list of validated rules.
        /// </summary>
        /// <param name="handler">The method that implements the rule.</param>
        /// <param name="propertyName">
        /// The name of the property on the target object where the rule implementation can retrieve
        /// the value to be validated.
        /// </param>
        void AddValidationRuleHandler(ValidationRuleHandler handler, String propertyName);

        /// <summary>
        /// Adds a rule to the list of validated rules.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="handler">The method that implements the rule.</param>
        /// <param name="args">
        /// A <see cref="Validation.ValidationRuleArgs"/> object specifying the property name and other arguments
        /// passed to the rule method
        /// </param>
        void AddValidationRuleHandler(ValidationRuleHandler handler, ValidationRuleArgs args);

        /// <summary>
        /// Force this object to validate itself using the assigned business rules.
        /// </summary>
        /// <remarks>Validates all properties.</remarks>
        void Validate();

        /// <summary>
        /// Force the object to validate itself using the assigned business rules.
        /// </summary>
        /// <param name="propertyName">Name of the property to validate.</param>
        void Validate(string propertyName);

        /// <summary>
        /// Force the object to validate itself using the assigned business rules.
        /// </summary>
        /// <param name="column">Column enumeration representing the column to validate.</param>
        void Validate(System.Enum column);

        /// <summary>
        /// This is a collection of property names that can be ignored on child objects when validating a parent due to the fact that
        /// Entity Framework will set their values on save. - JHE
        /// Example for Saving and Order: OrderID (gets set on child Entities)
        /// </summary>
        /// <returns></returns>
        List<string> ValidatedChildPropertiesSetByParent();

        /// <summary>
        /// Method to recursively check that an Entity is valid according to Business rules of the Entity
        /// and each child entity. - JHE
        /// TODO: Test this new recursive Validation method - JHE
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        ValidationResult ValidateRecursive();

        void CleanDataBeforeSave();

        /// <summary>
        /// Gets the <see cref="T:String"/> with the specified column name.
        /// </summary>
        /// <value></value>
        string this[string columnName] { get; }

        void Deserialized();

        /// <summary>
        ///  To clone and return an object of same type.
        /// </summary>
        OrderPayment Clone();
    }
}