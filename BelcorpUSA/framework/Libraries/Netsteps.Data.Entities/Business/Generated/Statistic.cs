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
    public partial class Statistic : EntityBusinessBase<Statistic, Int64, IStatisticRepository, IStatisticBusinessLogic>, IDatabaseValidationRules
    {
    	#region Primitive Properties
    
    	protected TimeZoneInfo _currentTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Greenwich Standard Time");
    	public virtual TimeZoneInfo CurrentTimeZoneInfo
    	{
    		get { return _currentTimeZoneInfo; }
    		set { _currentTimeZoneInfo = value; }
    	}
    
    	public System.DateTime OccuredDateTime
    	{
    		get { return OccuredDateTimeUTC.UTCToLocal(CurrentTimeZoneInfo); }
    		set { OccuredDateTimeUTC = value.LocalToUTC(CurrentTimeZoneInfo); }
    	}
    	
    	#endregion
    
    	#region Constructors
    	public Statistic() : this(default(IStatisticRepository), default(IStatisticBusinessLogic))
    	{
    	}
    	public Statistic(IStatisticRepository repo) : this(repo, default(IStatisticBusinessLogic))
    	{
    	}
    	public Statistic(IStatisticRepository repo, IStatisticBusinessLogic logic)
    		: base(repo, logic)
    	{
    		InitializeEntity();
    	}
    	#endregion
    	
    	#region Methods
    	public virtual void AddDatabaseValidationRules()
    	{ 
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<System.DateTime>, new CommonRules.CompareValueRuleArgs<System.DateTime>("OccuredDateTimeUTC", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<short>, new CommonRules.CompareValueRuleArgs<short>("StatisticTypeID", 0));
    	}
    	#endregion
    }
}
