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
    public partial class DynamicKitGroupRule : EntityBusinessBase<DynamicKitGroupRule, Int32, IDynamicKitGroupRuleRepository, IDynamicKitGroupRuleBusinessLogic>, IDatabaseValidationRules
    {
    	#region Primitive Properties
    
    	protected TimeZoneInfo _currentTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Greenwich Standard Time");
    	public virtual TimeZoneInfo CurrentTimeZoneInfo
    	{
    		get { return _currentTimeZoneInfo; }
    		set { _currentTimeZoneInfo = value; }
    	}
    
    	#endregion
    
    	#region Constructors
    	public DynamicKitGroupRule() : this(default(IDynamicKitGroupRuleRepository), default(IDynamicKitGroupRuleBusinessLogic))
    	{
    	}
    	public DynamicKitGroupRule(IDynamicKitGroupRuleRepository repo) : this(repo, default(IDynamicKitGroupRuleBusinessLogic))
    	{
    	}
    	public DynamicKitGroupRule(IDynamicKitGroupRuleRepository repo, IDynamicKitGroupRuleBusinessLogic logic)
    		: base(repo, logic)
    	{
    		InitializeEntity();
    	}
    	#endregion
    	
    	#region Methods
    	public virtual void AddDatabaseValidationRules()
    	{ 
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<int>, new CommonRules.CompareValueRuleArgs<int>("DynamicKitGroupID", 0));
    	}
    	#endregion
    }
}
