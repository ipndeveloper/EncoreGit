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
    [ContainerRegister(typeof(ICountryBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class CountryBusinessLogic : BusinessLogicBase<Country, Int32, ICountryRepository, ICountryBusinessLogic>, ICountryBusinessLogic, IDefaultImplementation
    {
    	public override Func<Country, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.CountryID;
    		}
    	}
    	public override Action<Country, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (Country i, Int32 id) => i.CountryID = id;
    		}
    	}
    	public override Func<Country, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<Country, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (Country i, string title) => i.Name = title;
    		}
    	}
    }
}