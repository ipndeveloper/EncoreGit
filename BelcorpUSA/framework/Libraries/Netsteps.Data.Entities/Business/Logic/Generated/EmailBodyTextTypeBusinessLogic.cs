//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Business.Logic.Interfaces;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Business.Logic
{
    [ContainerRegister(typeof(IEmailBodyTextTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class EmailBodyTextTypeBusinessLogic : BusinessLogicBase<EmailBodyTextType, Int16, IEmailBodyTextTypeRepository, IEmailBodyTextTypeBusinessLogic>, IEmailBodyTextTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<EmailBodyTextType, Int16> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.EmailBodyTextTypeID;
    		}
    	}
    	public override Action<EmailBodyTextType, Int16> SetIdColumnFunc
    	{
    		get
    		{
    			return (EmailBodyTextType i, Int16 id) => i.EmailBodyTextTypeID = id;
    		}
    	}
    	public override Func<EmailBodyTextType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<EmailBodyTextType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (EmailBodyTextType i, string title) => i.Name = title;
    		}
    	}
    }
}
