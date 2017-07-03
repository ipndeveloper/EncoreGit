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
    [ContainerRegister(typeof(IStatisticBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class StatisticBusinessLogic : BusinessLogicBase<Statistic, Int64, IStatisticRepository, IStatisticBusinessLogic>, IStatisticBusinessLogic, IDefaultImplementation
    {
    	public override Func<Statistic, Int64> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.StatisticID;
    		}
    	}
    	public override Action<Statistic, Int64> SetIdColumnFunc
    	{
    		get
    		{
    			return (Statistic i, Int64 id) => i.StatisticID = id;
    		}
    	}
    }
}
