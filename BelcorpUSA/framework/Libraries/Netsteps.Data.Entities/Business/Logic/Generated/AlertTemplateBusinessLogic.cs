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
    [ContainerRegister(typeof(IAlertTemplateBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class AlertTemplateBusinessLogic : BusinessLogicBase<AlertTemplate, Int32, IAlertTemplateRepository, IAlertTemplateBusinessLogic>, IAlertTemplateBusinessLogic, IDefaultImplementation
    {
    	public override Func<AlertTemplate, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.AlertTemplateID;
    		}
    	}
    	public override Action<AlertTemplate, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (AlertTemplate i, Int32 id) => i.AlertTemplateID = id;
    		}
    	}
    	public override Func<AlertTemplate, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<AlertTemplate, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (AlertTemplate i, string title) => i.Name = title;
    		}
    	}
    }
}
