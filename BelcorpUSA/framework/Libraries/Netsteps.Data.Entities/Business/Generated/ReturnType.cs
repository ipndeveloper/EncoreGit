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
    public partial class ReturnType : EntityBusinessBase<ReturnType, Int32, IReturnTypeRepository, IReturnTypeBusinessLogic>, IDatabaseValidationRules, IModifiedByUserID, ITermName, IEditable, IActive
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
    	public ReturnType() : this(default(IReturnTypeRepository), default(IReturnTypeBusinessLogic))
    	{
    	}
    	public ReturnType(IReturnTypeRepository repo) : this(repo, default(IReturnTypeBusinessLogic))
    	{
    	}
    	public ReturnType(IReturnTypeRepository repo, IReturnTypeBusinessLogic logic)
    		: base(repo, logic)
    	{
    		InitializeEntity();
    	}
    	#endregion
    	
    	#region Methods
    	public virtual void AddDatabaseValidationRules()
    	{ 
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Name", 50));
    		this.ValidationRules.AddRule(CommonRules.NotNull, new ValidationRuleArgs("Name"));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TermName", 255));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Description", 255));
    	}
    	// Generated for the "small collection" tables. - JHE
    	public static List<ReturnType> LoadAll()
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
    	public static List<ReturnType> LoadAllFull()
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
    
    	public static SqlUpdatableList<ReturnType> LoadAllFullWithSqlDependency()
    	{
    		try
    		{
    			var returnTypes = Repository.LoadAllFullWithSqlDependency();
    			foreach (var returnType in returnTypes)
    			{
    				returnType.StartTracking();
    				returnType.IsLazyLoadingEnabled = true;
    			}
    			return returnTypes;
    		}
    		catch (Exception ex)
    		{
    			throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
    		}
    	}
    	
    	public static PaginatedList<AuditLogRow> GetAuditLog(int returnTypeID, AuditLogSearchParameters param)
    	{
    		try
    		{
    			return BusinessLogic.GetAuditLog(Repository, returnTypeID, param);
    		}
    		catch (Exception ex)
    		{
    			throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, null, null);
    		}
    	}
    	#endregion
    }
}
