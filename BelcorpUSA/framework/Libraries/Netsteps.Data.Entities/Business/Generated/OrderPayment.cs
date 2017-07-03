//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using NetSteps.Common;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Validation.NetTiers;
using NetSteps.Data.Entities.Base;
using NetSteps.Data.Entities.Business.Logic.Interfaces;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Cache;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities
{
    public partial class OrderPayment : EntityBusinessBase<OrderPayment, Int32, IOrderPaymentRepository, IOrderPaymentBusinessLogic>, IDatabaseValidationRules, IModifiedByUserID, IDateCreated, IDateLastModified, IDataVersion
    {
    	#region Primitive Properties
    
    	protected TimeZoneInfo _currentTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Greenwich Standard Time");
    	public virtual TimeZoneInfo CurrentTimeZoneInfo
    	{
    		get { return _currentTimeZoneInfo; }
    		set { _currentTimeZoneInfo = value; }
    	}
    
    	public Nullable<System.DateTime> ProcessOnDate
    	{
    		get { return ProcessOnDateUTC.UTCToLocal(CurrentTimeZoneInfo); }
    		set { ProcessOnDateUTC = value.LocalToUTC(CurrentTimeZoneInfo); }
    	}
    	
    	public Nullable<System.DateTime> ProcessedDate
    	{
    		get { return ProcessedDateUTC.UTCToLocal(CurrentTimeZoneInfo); }
    		set { ProcessedDateUTC = value.LocalToUTC(CurrentTimeZoneInfo); }
    	}
    	
    	public Nullable<System.DateTime> ExpirationDate
    	{
    		get { return ExpirationDateUTC.UTCToLocal(CurrentTimeZoneInfo); }
    		set { ExpirationDateUTC = value.LocalToUTC(CurrentTimeZoneInfo); }
    	}
    	
    	public System.DateTime DateCreated
    	{
    		get { return DateCreatedUTC.UTCToLocal(CurrentTimeZoneInfo); }
    		set { DateCreatedUTC = value.LocalToUTC(CurrentTimeZoneInfo); }
    	}
    	
    	public System.DateTime DateLastModified
    	{
    		get { return DateLastModifiedUTC.UTCToLocal(CurrentTimeZoneInfo); }
    		set { DateLastModifiedUTC = value.LocalToUTC(CurrentTimeZoneInfo); }
    	}
    	
    	public Nullable<System.DateTime> CurrentExpirationDate
    	{
    		get { return CurrentExpirationDateUTC.UTCToLocal(CurrentTimeZoneInfo); }
    		set { CurrentExpirationDateUTC = value.LocalToUTC(CurrentTimeZoneInfo); }
    	}
    	
    	public Nullable<System.DateTime> DateLastTotalAmount
    	{
    		get { return DateLastTotalAmountUTC.UTCToLocal(CurrentTimeZoneInfo); }
    		set { DateLastTotalAmountUTC = value.LocalToUTC(CurrentTimeZoneInfo); }
    	}
    	
    	#endregion
    
    	#region Constructors
    	public OrderPayment() : this(default(IOrderPaymentRepository), default(IOrderPaymentBusinessLogic))
    	{
    	}
    	public OrderPayment(IOrderPaymentRepository repo) : this(repo, default(IOrderPaymentBusinessLogic))
    	{
    	}
    	public OrderPayment(IOrderPaymentRepository repo, IOrderPaymentBusinessLogic logic)
    		: base(repo, logic)
    	{
    		InitializeEntity();
    	}
    	#endregion
    	
    	#region Methods
    	public virtual void AddDatabaseValidationRules()
    	{ 
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<int>, new CommonRules.CompareValueRuleArgs<int>("OrderID", 0));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<int>, new CommonRules.CompareValueRuleArgs<int>("PaymentTypeID", 0));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("AccountNumber", 200));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BillingFirstName", 100));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BillingLastName", 100));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BillingName", 200));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BillingAddress1", 200));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BillingAddress2", 200));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BillingAddress3", 200));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BillingCity", 200));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BillingCounty", 100));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BillingState", 100));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BillingPostalCode", 20));
    		this.ValidationRules.AddRule(CommonRules.NotNull, new ValidationRuleArgs("BillingPostalCode"));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BillingPhoneNumber", 50));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("IdentityNumber", 100));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("IdentityState", 100));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("RoutingNumber", 50));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<short>, new CommonRules.CompareValueRuleArgs<short>("OrderPaymentStatusID", 0));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<Nullable<System.DateTime>>, new CommonRules.CompareValueRuleArgs<Nullable<System.DateTime>>("ProcessOnDateUTC", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<Nullable<System.DateTime>>, new CommonRules.CompareValueRuleArgs<Nullable<System.DateTime>>("ProcessedDateUTC", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TransactionID", 50));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DeferredTransactionID", 100));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<Nullable<System.DateTime>>, new CommonRules.CompareValueRuleArgs<Nullable<System.DateTime>>("ExpirationDateUTC", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<int>, new CommonRules.CompareValueRuleArgs<int>("CurrencyID", 0));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("NameOnCard", 100));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("AccountNumberLastFour", 10));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BankName", 100));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("NachaClassType", 10));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<Nullable<System.DateTime>>, new CommonRules.CompareValueRuleArgs<Nullable<System.DateTime>>("NachaSentDate", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<System.DateTime>, new CommonRules.CompareValueRuleArgs<System.DateTime>("DateCreatedUTC", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<System.DateTime>, new CommonRules.CompareValueRuleArgs<System.DateTime>("DateLastModifiedUTC", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ETLNaturalKey", 256));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ETLHash", 50));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ETLPhase", 50));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<Nullable<System.DateTime>>, new CommonRules.CompareValueRuleArgs<Nullable<System.DateTime>>("ETLDate", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("BillingStreet", 100));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<Nullable<System.DateTime>>, new CommonRules.CompareValueRuleArgs<Nullable<System.DateTime>>("OriginalExpirationDate", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<Nullable<System.DateTime>>, new CommonRules.CompareValueRuleArgs<Nullable<System.DateTime>>("CurrentExpirationDateUTC", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<Nullable<System.DateTime>>, new CommonRules.CompareValueRuleArgs<Nullable<System.DateTime>>("DateLastTotalAmountUTC", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<Nullable<System.DateTime>>, new CommonRules.CompareValueRuleArgs<Nullable<System.DateTime>>("DateValidity", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    	}
    	#endregion
    }
}
