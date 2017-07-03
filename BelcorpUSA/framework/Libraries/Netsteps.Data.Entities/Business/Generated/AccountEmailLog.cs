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
    public partial class AccountEmailLog : EntityBusinessBase<AccountEmailLog, Int32, IAccountEmailLogRepository, IAccountEmailLogBusinessLogic>, IDatabaseValidationRules
    {
    	#region Primitive Properties
    
    	protected TimeZoneInfo _currentTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Greenwich Standard Time");
    	public virtual TimeZoneInfo CurrentTimeZoneInfo
    	{
    		get { return _currentTimeZoneInfo; }
    		set { _currentTimeZoneInfo = value; }
    	}
    
    	public Nullable<System.DateTime> DateSent
    	{
    		get { return DateSentUTC.UTCToLocal(CurrentTimeZoneInfo); }
    		set { DateSentUTC = value.LocalToUTC(CurrentTimeZoneInfo); }
    	}
    	
    	#endregion
    
    	#region Constructors
    	public AccountEmailLog() : this(default(IAccountEmailLogRepository), default(IAccountEmailLogBusinessLogic))
    	{
    	}
    	public AccountEmailLog(IAccountEmailLogRepository repo) : this(repo, default(IAccountEmailLogBusinessLogic))
    	{
    	}
    	public AccountEmailLog(IAccountEmailLogRepository repo, IAccountEmailLogBusinessLogic logic)
    		: base(repo, logic)
    	{
    		InitializeEntity();
    	}
    	#endregion
    	
    	#region Methods
    	public virtual void AddDatabaseValidationRules()
    	{ 
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EmailAddress", 255));
    		this.ValidationRules.AddRule(CommonRules.RegexIsMatch, new CommonRules.RegexRuleArgs("EmailAddress", RegularExpressions.EmailOrEmpty, Translation.GetTerm("InvalidEmailErrorMessage", CustomValidationMessages.Email), true));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<Nullable<System.DateTime>>, new CommonRules.CompareValueRuleArgs<Nullable<System.DateTime>>("DateSentUTC", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    	}
    	#endregion
    }
}
