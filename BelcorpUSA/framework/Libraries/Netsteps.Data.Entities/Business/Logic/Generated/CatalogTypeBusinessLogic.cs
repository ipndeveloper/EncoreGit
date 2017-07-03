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
    [ContainerRegister(typeof(ICatalogTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class CatalogTypeBusinessLogic : BusinessLogicBase<CatalogType, Int16, ICatalogTypeRepository, ICatalogTypeBusinessLogic>, ICatalogTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<CatalogType, Int16> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.CatalogTypeID;
    		}
    	}
    	public override Action<CatalogType, Int16> SetIdColumnFunc
    	{
    		get
    		{
    			return (CatalogType i, Int16 id) => i.CatalogTypeID = id;
    		}
    	}
    	public override Func<CatalogType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<CatalogType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (CatalogType i, string title) => i.Name = title;
    		}
    	}
    }
}