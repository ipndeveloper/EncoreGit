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
    [ContainerRegister(typeof(ISiteStatusChangeReasonBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class SiteStatusChangeReasonBusinessLogic : BusinessLogicBase<SiteStatusChangeReason, Int16, ISiteStatusChangeReasonRepository, ISiteStatusChangeReasonBusinessLogic>, ISiteStatusChangeReasonBusinessLogic, IDefaultImplementation
    {
    	public override Func<SiteStatusChangeReason, Int16> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.SiteStatusChangeReasonID;
    		}
    	}
    	public override Action<SiteStatusChangeReason, Int16> SetIdColumnFunc
    	{
    		get
    		{
    			return (SiteStatusChangeReason i, Int16 id) => i.SiteStatusChangeReasonID = id;
    		}
    	}
    	public override Func<SiteStatusChangeReason, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<SiteStatusChangeReason, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (SiteStatusChangeReason i, string title) => i.Name = title;
    		}
    	}
    }
}