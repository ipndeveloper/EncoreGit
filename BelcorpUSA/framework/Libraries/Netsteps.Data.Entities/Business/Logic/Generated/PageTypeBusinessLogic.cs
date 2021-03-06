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
    [ContainerRegister(typeof(IPageTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class PageTypeBusinessLogic : BusinessLogicBase<PageType, Int16, IPageTypeRepository, IPageTypeBusinessLogic>, IPageTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<PageType, Int16> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.PageTypeID;
    		}
    	}
    	public override Action<PageType, Int16> SetIdColumnFunc
    	{
    		get
    		{
    			return (PageType i, Int16 id) => i.PageTypeID = id;
    		}
    	}
    	public override Func<PageType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<PageType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (PageType i, string title) => i.Name = title;
    		}
    	}
    }
}
