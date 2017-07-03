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
    [ContainerRegister(typeof(ICurrencyBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class CurrencyBusinessLogic : BusinessLogicBase<Currency, Int32, ICurrencyRepository, ICurrencyBusinessLogic>, ICurrencyBusinessLogic, IDefaultImplementation
    {
    	public override Func<Currency, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.CurrencyID;
    		}
    	}
    	public override Action<Currency, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (Currency i, Int32 id) => i.CurrencyID = id;
    		}
    	}
    	public override Func<Currency, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<Currency, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (Currency i, string title) => i.Name = title;
    		}
    	}
    }
}