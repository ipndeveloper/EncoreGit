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
    [ContainerRegister(typeof(IProxyLinkBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class ProxyLinkBusinessLogic : BusinessLogicBase<ProxyLink, Int32, IProxyLinkRepository, IProxyLinkBusinessLogic>, IProxyLinkBusinessLogic, IDefaultImplementation
    {
    	public override Func<ProxyLink, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.ProxyLinkID;
    		}
    	}
    	public override Action<ProxyLink, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (ProxyLink i, Int32 id) => i.ProxyLinkID = id;
    		}
    	}
    	public override Func<ProxyLink, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<ProxyLink, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (ProxyLink i, string title) => i.Name = title;
    		}
    	}
    }
}