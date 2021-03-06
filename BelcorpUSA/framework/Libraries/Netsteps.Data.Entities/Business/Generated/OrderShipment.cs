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
    public partial class OrderShipment : EntityBusinessBase<OrderShipment, Int32, IOrderShipmentRepository, IOrderShipmentBusinessLogic>, IDatabaseValidationRules, IModifiedByUserID, IDateCreated, IDateLastModified, IDataVersion
    {
    	#region Primitive Properties
    
    	protected TimeZoneInfo _currentTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Greenwich Standard Time");
    	public virtual TimeZoneInfo CurrentTimeZoneInfo
    	{
    		get { return _currentTimeZoneInfo; }
    		set { _currentTimeZoneInfo = value; }
    	}
    
    	public Nullable<System.DateTime> DateShipped
    	{
    		get { return DateShippedUTC.UTCToLocal(CurrentTimeZoneInfo); }
    		set { DateShippedUTC = value.LocalToUTC(CurrentTimeZoneInfo); }
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
    	
    	public Nullable<System.DateTime> EstimatedDate
    	{
    		get { return EstimatedDateUTC.UTCToLocal(CurrentTimeZoneInfo); }
    		set { EstimatedDateUTC = value.LocalToUTC(CurrentTimeZoneInfo); }
    	}
    	
    	public Nullable<System.DateTime> DeliveryDate
    	{
    		get { return DeliveryDateUTC.UTCToLocal(CurrentTimeZoneInfo); }
    		set { DeliveryDateUTC = value.LocalToUTC(CurrentTimeZoneInfo); }
    	}
    	
    	#endregion
    
    	#region Constructors
    	public OrderShipment() : this(default(IOrderShipmentRepository), default(IOrderShipmentBusinessLogic))
    	{
    	}
    	public OrderShipment(IOrderShipmentRepository repo) : this(repo, default(IOrderShipmentBusinessLogic))
    	{
    	}
    	public OrderShipment(IOrderShipmentRepository repo, IOrderShipmentBusinessLogic logic)
    		: base(repo, logic)
    	{
    		InitializeEntity();
    	}
    	#endregion
    	
    	#region Methods
    	public virtual void AddDatabaseValidationRules()
    	{ 
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<int>, new CommonRules.CompareValueRuleArgs<int>("OrderID", 0));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<short>, new CommonRules.CompareValueRuleArgs<short>("OrderShipmentStatusID", 0));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("FirstName", 100));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LastName", 100));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Attention", 100));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Name", 200));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Address1", 200));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Address2", 200));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Address3", 200));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("City", 200));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("County", 100));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("State", 100));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PostalCode", 20));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<int>, new CommonRules.CompareValueRuleArgs<int>("CountryID", 0));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Email", 250));
    		this.ValidationRules.AddRule(CommonRules.RegexIsMatch, new CommonRules.RegexRuleArgs("Email", RegularExpressions.EmailOrEmpty, Translation.GetTerm("InvalidEmailErrorMessage", CustomValidationMessages.Email), true));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DayPhone", 50));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EveningPhone", 50));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TrackingNumber", 1000));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TrackingURL", 1000));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<Nullable<System.DateTime>>, new CommonRules.CompareValueRuleArgs<Nullable<System.DateTime>>("DateShippedUTC", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("GovernmentReceiptNumber", 200));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PickupPointCode", 100));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<System.DateTime>, new CommonRules.CompareValueRuleArgs<System.DateTime>("DateCreatedUTC", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<System.DateTime>, new CommonRules.CompareValueRuleArgs<System.DateTime>("DateLastModifiedUTC", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ETLNaturalKey", 256));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ETLHash", 50));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ETLPhase", 50));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<Nullable<System.DateTime>>, new CommonRules.CompareValueRuleArgs<Nullable<System.DateTime>>("ETLDate", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Street", 100));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<Nullable<System.DateTime>>, new CommonRules.CompareValueRuleArgs<Nullable<System.DateTime>>("EstimatedDateUTC", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<Nullable<System.DateTime>>, new CommonRules.CompareValueRuleArgs<Nullable<System.DateTime>>("DeliveryDateUTC", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    	}
    	#endregion
    }
}
