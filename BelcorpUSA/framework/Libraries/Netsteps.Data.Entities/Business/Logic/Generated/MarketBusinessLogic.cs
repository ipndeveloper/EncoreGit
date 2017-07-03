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
    [ContainerRegister(typeof(IMarketBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class MarketBusinessLogic : BusinessLogicBase<Market, Int32, IMarketRepository, IMarketBusinessLogic>, IMarketBusinessLogic, IDefaultImplementation
    {
    	public override Func<Market, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.MarketID;
    		}
    	}
    	public override Action<Market, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (Market i, Int32 id) => i.MarketID = id;
    		}
    	}
    	public override Func<Market, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<Market, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (Market i, string title) => i.Name = title;
    		}
    	}
    }
}
