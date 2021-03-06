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
    public partial class CatalogItem : EntityBusinessBase<CatalogItem, Int32, ICatalogItemRepository, ICatalogItemBusinessLogic>, IDatabaseValidationRules, ISortIndex, IActive, IDateRange
    {
    	#region Primitive Properties
    
    	protected TimeZoneInfo _currentTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Greenwich Standard Time");
    	public virtual TimeZoneInfo CurrentTimeZoneInfo
    	{
    		get { return _currentTimeZoneInfo; }
    		set { _currentTimeZoneInfo = value; }
    	}
    
    	public Nullable<System.DateTime> StartDate
    	{
    		get { return StartDateUTC.UTCToLocal(CurrentTimeZoneInfo); }
    		set { StartDateUTC = value.LocalToUTC(CurrentTimeZoneInfo); }
    	}
    	
    	public Nullable<System.DateTime> EndDate
    	{
    		get { return EndDateUTC.UTCToLocal(CurrentTimeZoneInfo); }
    		set { EndDateUTC = value.LocalToUTC(CurrentTimeZoneInfo); }
    	}
    	
    	#endregion
    
    	#region Constructors
    	public CatalogItem() : this(default(ICatalogItemRepository), default(ICatalogItemBusinessLogic))
    	{
    	}
    	public CatalogItem(ICatalogItemRepository repo) : this(repo, default(ICatalogItemBusinessLogic))
    	{
    	}
    	public CatalogItem(ICatalogItemRepository repo, ICatalogItemBusinessLogic logic)
    		: base(repo, logic)
    	{
    		InitializeEntity();
    	}
    	#endregion
    	
    	#region Methods
    	public virtual void AddDatabaseValidationRules()
    	{ 
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<int>, new CommonRules.CompareValueRuleArgs<int>("CatalogID", 0));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<int>, new CommonRules.CompareValueRuleArgs<int>("ProductID", 0));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<Nullable<System.DateTime>>, new CommonRules.CompareValueRuleArgs<Nullable<System.DateTime>>("StartDateUTC", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<Nullable<System.DateTime>>, new CommonRules.CompareValueRuleArgs<Nullable<System.DateTime>>("EndDateUTC", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ETLNaturalKey", 256));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ETLHash", 50));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ETLPhase", 50));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<Nullable<System.DateTime>>, new CommonRules.CompareValueRuleArgs<Nullable<System.DateTime>>("ETLDate", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    		this.ValidationRules.AddRule(CommonRules.ValidDateRange, new ValidationRuleArgs("StartDateUTC"));
    	}
    	#endregion
    }
}
