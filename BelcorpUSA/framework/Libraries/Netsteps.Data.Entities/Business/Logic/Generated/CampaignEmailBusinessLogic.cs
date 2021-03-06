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
    [ContainerRegister(typeof(ICampaignEmailBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class CampaignEmailBusinessLogic : BusinessLogicBase<CampaignEmail, Int32, ICampaignEmailRepository, ICampaignEmailBusinessLogic>, ICampaignEmailBusinessLogic, IDefaultImplementation
    {
    	public override Func<CampaignEmail, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.CampaignEmailID;
    		}
    	}
    	public override Action<CampaignEmail, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (CampaignEmail i, Int32 id) => i.CampaignEmailID = id;
    		}
    	}
    }
}
