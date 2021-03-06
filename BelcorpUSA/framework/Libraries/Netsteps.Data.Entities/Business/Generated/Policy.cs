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
    public partial class Policy : EntityBusinessBase<Policy, Int32, IPolicyRepository, IPolicyBusinessLogic>, IDatabaseValidationRules, ITermName, ILanguageID, IActive
    {
    	#region Primitive Properties
    
    	protected TimeZoneInfo _currentTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Greenwich Standard Time");
    	public virtual TimeZoneInfo CurrentTimeZoneInfo
    	{
    		get { return _currentTimeZoneInfo; }
    		set { _currentTimeZoneInfo = value; }
    	}
    
    	public Nullable<System.DateTime> DateReleased
    	{
    		get { return DateReleasedUTC.UTCToLocal(CurrentTimeZoneInfo); }
    		set { DateReleasedUTC = value.LocalToUTC(CurrentTimeZoneInfo); }
    	}
    	
    	#endregion
    
    	#region Constructors
    	public Policy() : this(default(IPolicyRepository), default(IPolicyBusinessLogic))
    	{
    	}
    	public Policy(IPolicyRepository repo) : this(repo, default(IPolicyBusinessLogic))
    	{
    	}
    	public Policy(IPolicyRepository repo, IPolicyBusinessLogic logic)
    		: base(repo, logic)
    	{
    		InitializeEntity();
    	}
    	#endregion
    	
    	#region Methods
    	public virtual void AddDatabaseValidationRules()
    	{ 
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("VersionNumber", 50));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<Nullable<System.DateTime>>, new CommonRules.CompareValueRuleArgs<Nullable<System.DateTime>>("DateReleasedUTC", new DateTime(1753, 1, 1), Translation.GetTerm("InvalidDateTimeMinErrorMessageWithValue", CustomValidationMessages.DateTimeMinWithValue)));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("FilePath", 255));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Name", 50));
    		this.ValidationRules.AddRule(CommonRules.GreaterThanValue<int>, new CommonRules.CompareValueRuleArgs<int>("LanguageID", 0));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TermName", 255));
    	}
    	#endregion
    }
}
