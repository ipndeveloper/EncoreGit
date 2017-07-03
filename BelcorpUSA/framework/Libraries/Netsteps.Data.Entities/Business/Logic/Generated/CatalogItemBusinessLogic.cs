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
    [ContainerRegister(typeof(ICatalogItemBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class CatalogItemBusinessLogic : BusinessLogicBase<CatalogItem, Int32, ICatalogItemRepository, ICatalogItemBusinessLogic>, ICatalogItemBusinessLogic, IDefaultImplementation
    {
    	public override Func<CatalogItem, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.CatalogItemID;
    		}
    	}
    	public override Action<CatalogItem, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (CatalogItem i, Int32 id) => i.CatalogItemID = id;
    		}
    	}
    }
}