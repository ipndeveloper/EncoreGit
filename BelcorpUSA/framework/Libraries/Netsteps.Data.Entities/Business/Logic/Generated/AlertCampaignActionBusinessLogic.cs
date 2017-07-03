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
    [ContainerRegister(typeof(IAlertCampaignActionBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class AlertCampaignActionBusinessLogic : BusinessLogicBase<AlertCampaignAction, Int32, IAlertCampaignActionRepository, IAlertCampaignActionBusinessLogic>, IAlertCampaignActionBusinessLogic, IDefaultImplementation
    {
    	public override Func<AlertCampaignAction, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.AlertCampaignActionID;
    		}
    	}
    	public override Action<AlertCampaignAction, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (AlertCampaignAction i, Int32 id) => i.AlertCampaignActionID = id;
    		}
    	}
    }
}
