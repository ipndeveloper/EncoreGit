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
    [ContainerRegister(typeof(ISiteWidgetBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class SiteWidgetBusinessLogic : BusinessLogicBase<SiteWidget, Int32, ISiteWidgetRepository, ISiteWidgetBusinessLogic>, ISiteWidgetBusinessLogic, IDefaultImplementation
    {
    	public override Func<SiteWidget, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.SiteWidgetID;
    		}
    	}
    	public override Action<SiteWidget, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (SiteWidget i, Int32 id) => i.SiteWidgetID = id;
    		}
    	}
    }
}
