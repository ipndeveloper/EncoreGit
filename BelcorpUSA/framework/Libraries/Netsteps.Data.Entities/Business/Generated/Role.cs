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
    public partial class Role : EntityBusinessBase<Role, Int32, IRoleRepository, IRoleBusinessLogic>, IDatabaseValidationRules, ITermName, IEditable, IActive
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
    	public Role() : this(default(IRoleRepository), default(IRoleBusinessLogic))
    	{
    	}
    	public Role(IRoleRepository repo) : this(repo, default(IRoleBusinessLogic))
    	{
    	}
    	public Role(IRoleRepository repo, IRoleBusinessLogic logic)
    		: base(repo, logic)
    	{
    		InitializeEntity();
    	}
    	#endregion
    	
    	#region Methods
    	public virtual void AddDatabaseValidationRules()
    	{ 
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<short>, new CommonRules.CompareValueRuleArgs<short>("RoleTypeID", 0));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Name", 50));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TermName", 255));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Description", 255));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("StartPage", 255));
    	}
    	// Generated for the "small collection" tables. - JHE
    	public static List<Role> LoadAll()
    	{
    		try
    		{
    			return BusinessLogic.LoadAll(Repository);
    		}
    		catch (Exception ex)
    		{
    			throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
    		}
    	}
    	
    	// Generated for the "small collection" tables. - JHE
    	public static List<Role> LoadAllFull()
    	{
    		try
    		{
    			return BusinessLogic.LoadAllFull(Repository);
    		}
    		catch (Exception ex)
    		{
    			throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
    		}
    	}	
    
    	public static SqlUpdatableList<Role> LoadAllFullWithSqlDependency()
    	{
    		try
    		{
    			var roles = Repository.LoadAllFullWithSqlDependency();
    			foreach (var role in roles)
    			{
    				role.StartTracking();
    				role.IsLazyLoadingEnabled = true;
    			}
    			return roles;
    		}
    		catch (Exception ex)
    		{
    			throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
    		}
    	}
    	#endregion
    }
}
