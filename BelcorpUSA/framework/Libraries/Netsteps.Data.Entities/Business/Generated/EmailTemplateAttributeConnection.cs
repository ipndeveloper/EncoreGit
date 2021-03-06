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
    public partial class EmailTemplateAttributeConnection : EntityBusinessBase<EmailTemplateAttributeConnection, Int32, IEmailTemplateAttributeConnectionRepository, IEmailTemplateAttributeConnectionBusinessLogic>, IDatabaseValidationRules
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
    	public EmailTemplateAttributeConnection() : this(default(IEmailTemplateAttributeConnectionRepository), default(IEmailTemplateAttributeConnectionBusinessLogic))
    	{
    	}
    	public EmailTemplateAttributeConnection(IEmailTemplateAttributeConnectionRepository repo) : this(repo, default(IEmailTemplateAttributeConnectionBusinessLogic))
    	{
    	}
    	public EmailTemplateAttributeConnection(IEmailTemplateAttributeConnectionRepository repo, IEmailTemplateAttributeConnectionBusinessLogic logic)
    		: base(repo, logic)
    	{
    		InitializeEntity();
    	}
    	#endregion
    	
    	#region Methods
    	public virtual void AddDatabaseValidationRules()
    	{ 
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ConnectionName", 50));
    		this.ValidationRules.AddRule(CommonRules.NotNull, new ValidationRuleArgs("ConnectionName"));
    		this.ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ConnectionString", 200));
    		this.ValidationRules.AddRule(CommonRules.NotNull, new ValidationRuleArgs("ConnectionString"));
    	}
    	#endregion
    }
}
