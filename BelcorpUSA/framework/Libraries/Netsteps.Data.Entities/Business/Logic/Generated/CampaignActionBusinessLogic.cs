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
    [ContainerRegister(typeof(ICampaignActionBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class CampaignActionBusinessLogic : BusinessLogicBase<CampaignAction, Int32, ICampaignActionRepository, ICampaignActionBusinessLogic>, ICampaignActionBusinessLogic, IDefaultImplementation
    {
    	public override Func<CampaignAction, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.CampaignActionID;
    		}
    	}
    	public override Action<CampaignAction, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (CampaignAction i, Int32 id) => i.CampaignActionID = id;
    		}
    	}
    	public override Func<CampaignAction, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<CampaignAction, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (CampaignAction i, string title) => i.Name = title;
    		}
    	}
    }
}
