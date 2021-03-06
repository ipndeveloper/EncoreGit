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
    public partial class AccountReport : EntityBusinessBase<AccountReport, Int32, IAccountReportRepository, IAccountReportBusinessLogic>, IDatabaseValidationRules, IModifiedByUserID, ICreatedByUserID, IDateCreated
    {
    	#region Primitive Properties
    
    	protected TimeZoneInfo _currentTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Greenwich Standard Time");
    	public virtual TimeZoneInfo CurrentTimeZoneInfo
    	{
    		get { return _currentTimeZoneInfo; }
    		set { _currentTimeZoneInfo = value; }
    	}
    
    	public System.DateTime DateCreated
    	{
    		get { return DateCreatedUTC.UTCToLocal(CurrentTimeZoneInfo); }
    		set { DateCreatedUTC = value.LocalToUTC(CurrentTimeZoneInfo); }
    	}
    	
    	#endregion
    
    	#region Constructors
    	public AccountReport() : this(default(IAccountReportRepository), default(IAccountReportBusinessLogic))
    	{
    	}
    	public AccountReport(IAccountReportRepository repo) : this(repo, default(IAccountReportBusinessLogic))
    	{
    	}
    	public AccountReport(IAccountReportRepository repo, IAccountReportBusinessLogic logic)
    		: base(repo, logic)
    	{
    		InitializeEntity();
    	}
    	#endregion
    	
    	#region Methods
    	public virtual void AddDatabaseValidationRules()
    	{ 
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<short>, new CommonRules.CompareValueRuleArgs<short>("AccountReportTypeID", 0));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Name", 255));
    		this.ValidationRules.AddRule(CommonRules.NotNull, new ValidationRuleArgs("Name"));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<System.DateTime>, new CommonRules.CompareValueRuleArgs<System.DateTime>("DateCreatedUTC", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    	}
    	#endregion
    }
}
