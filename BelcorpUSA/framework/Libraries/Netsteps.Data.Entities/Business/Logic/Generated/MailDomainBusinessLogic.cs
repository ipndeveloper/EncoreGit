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
    [ContainerRegister(typeof(IMailDomainBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class MailDomainBusinessLogic : BusinessLogicBase<MailDomain, Int32, IMailDomainRepository, IMailDomainBusinessLogic>, IMailDomainBusinessLogic, IDefaultImplementation
    {
    	public override Func<MailDomain, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.MailDomainID;
    		}
    	}
    	public override Action<MailDomain, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (MailDomain i, Int32 id) => i.MailDomainID = id;
    		}
    	}
    }
}