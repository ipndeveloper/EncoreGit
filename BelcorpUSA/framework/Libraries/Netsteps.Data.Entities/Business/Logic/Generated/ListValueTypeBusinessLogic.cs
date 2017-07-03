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
    [ContainerRegister(typeof(IListValueTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class ListValueTypeBusinessLogic : BusinessLogicBase<ListValueType, Int16, IListValueTypeRepository, IListValueTypeBusinessLogic>, IListValueTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<ListValueType, Int16> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.ListValueTypeID;
    		}
    	}
    	public override Action<ListValueType, Int16> SetIdColumnFunc
    	{
    		get
    		{
    			return (ListValueType i, Int16 id) => i.ListValueTypeID = id;
    		}
    	}
    	public override Func<ListValueType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<ListValueType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (ListValueType i, string title) => i.Name = title;
    		}
    	}
    }
}
