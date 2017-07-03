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
    [ContainerRegister(typeof(IAutoresponderMessageBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class AutoresponderMessageBusinessLogic : BusinessLogicBase<AutoresponderMessage, Int32, IAutoresponderMessageRepository, IAutoresponderMessageBusinessLogic>, IAutoresponderMessageBusinessLogic, IDefaultImplementation
    {
    	public override Func<AutoresponderMessage, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.AutoresponderMessageID;
    		}
    	}
    	public override Action<AutoresponderMessage, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (AutoresponderMessage i, Int32 id) => i.AutoresponderMessageID = id;
    		}
    	}
    }
}
