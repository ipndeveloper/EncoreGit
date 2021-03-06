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
    [ContainerRegister(typeof(IAuditMachineNameBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class AuditMachineNameBusinessLogic : BusinessLogicBase<AuditMachineName, Int16, IAuditMachineNameRepository, IAuditMachineNameBusinessLogic>, IAuditMachineNameBusinessLogic, IDefaultImplementation
    {
    	public override Func<AuditMachineName, Int16> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.AuditMachineNameID;
    		}
    	}
    	public override Action<AuditMachineName, Int16> SetIdColumnFunc
    	{
    		get
    		{
    			return (AuditMachineName i, Int16 id) => i.AuditMachineNameID = id;
    		}
    	}
    	public override Func<AuditMachineName, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<AuditMachineName, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (AuditMachineName i, string title) => i.Name = title;
    		}
    	}
    }
}
