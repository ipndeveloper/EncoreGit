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
    [ContainerRegister(typeof(IAddressTaxOverrideBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class AddressTaxOverrideBusinessLogic : BusinessLogicBase<AddressTaxOverride, Int32, IAddressTaxOverrideRepository, IAddressTaxOverrideBusinessLogic>, IAddressTaxOverrideBusinessLogic, IDefaultImplementation
    {
    	public override Func<AddressTaxOverride, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.AddressTaxOverrideId;
    		}
    	}
    	public override Action<AddressTaxOverride, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (AddressTaxOverride i, Int32 id) => i.AddressTaxOverrideId = id;
    		}
    	}
    }
}