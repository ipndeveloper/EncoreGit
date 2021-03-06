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
    [ContainerRegister(typeof(ICampaignTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class CampaignTypeBusinessLogic : BusinessLogicBase<CampaignType, Int16, ICampaignTypeRepository, ICampaignTypeBusinessLogic>, ICampaignTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<CampaignType, Int16> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.CampaignTypeID;
    		}
    	}
    	public override Action<CampaignType, Int16> SetIdColumnFunc
    	{
    		get
    		{
    			return (CampaignType i, Int16 id) => i.CampaignTypeID = id;
    		}
    	}
    	public override Func<CampaignType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<CampaignType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (CampaignType i, string title) => i.Name = title;
    		}
    	}
    }
}
