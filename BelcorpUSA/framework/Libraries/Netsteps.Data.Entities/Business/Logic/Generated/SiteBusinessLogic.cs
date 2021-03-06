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
    [ContainerRegister(typeof(ISiteBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class SiteBusinessLogic : BusinessLogicBase<Site, Int32, ISiteRepository, ISiteBusinessLogic>, ISiteBusinessLogic, IDefaultImplementation
    {
    	public override Func<Site, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.SiteID;
    		}
    	}
    	public override Action<Site, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (Site i, Int32 id) => i.SiteID = id;
    		}
    	}
    	public override Func<Site, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<Site, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (Site i, string title) => i.Name = title;
    		}
    	}
    }
}
