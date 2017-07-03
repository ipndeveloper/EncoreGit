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
    [ContainerRegister(typeof(IEmailTemplateAttributeConnectionBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class EmailTemplateAttributeConnectionBusinessLogic : BusinessLogicBase<EmailTemplateAttributeConnection, Int32, IEmailTemplateAttributeConnectionRepository, IEmailTemplateAttributeConnectionBusinessLogic>, IEmailTemplateAttributeConnectionBusinessLogic, IDefaultImplementation
    {
    	public override Func<EmailTemplateAttributeConnection, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.EmailTemplateAttributeConnectionID;
    		}
    	}
    	public override Action<EmailTemplateAttributeConnection, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (EmailTemplateAttributeConnection i, Int32 id) => i.EmailTemplateAttributeConnectionID = id;
    		}
    	}
    }
}