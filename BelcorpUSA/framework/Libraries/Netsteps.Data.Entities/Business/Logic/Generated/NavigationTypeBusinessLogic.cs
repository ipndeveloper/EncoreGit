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
    [ContainerRegister(typeof(INavigationTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class NavigationTypeBusinessLogic : BusinessLogicBase<NavigationType, Int32, INavigationTypeRepository, INavigationTypeBusinessLogic>, INavigationTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<NavigationType, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.NavigationTypeID;
    		}
    	}
    	public override Action<NavigationType, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (NavigationType i, Int32 id) => i.NavigationTypeID = id;
    		}
    	}
    	public override Func<NavigationType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<NavigationType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (NavigationType i, string title) => i.Name = title;
    		}
    	}
    }
}
