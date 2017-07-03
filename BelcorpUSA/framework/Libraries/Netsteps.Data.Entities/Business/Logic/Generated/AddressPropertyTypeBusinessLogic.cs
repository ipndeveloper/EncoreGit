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
    [ContainerRegister(typeof(IAddressPropertyTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class AddressPropertyTypeBusinessLogic : BusinessLogicBase<AddressPropertyType, Int32, IAddressPropertyTypeRepository, IAddressPropertyTypeBusinessLogic>, IAddressPropertyTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<AddressPropertyType, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.AddressPropertyTypeID;
    		}
    	}
    	public override Action<AddressPropertyType, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (AddressPropertyType i, Int32 id) => i.AddressPropertyTypeID = id;
    		}
    	}
    	public override Func<AddressPropertyType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<AddressPropertyType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (AddressPropertyType i, string title) => i.Name = title;
    		}
    	}
    }
}
