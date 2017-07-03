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
    [ContainerRegister(typeof(ICreditCardTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class CreditCardTypeBusinessLogic : BusinessLogicBase<CreditCardType, Int16, ICreditCardTypeRepository, ICreditCardTypeBusinessLogic>, ICreditCardTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<CreditCardType, Int16> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.CreditCardTypeID;
    		}
    	}
    	public override Action<CreditCardType, Int16> SetIdColumnFunc
    	{
    		get
    		{
    			return (CreditCardType i, Int16 id) => i.CreditCardTypeID = id;
    		}
    	}
    	public override Func<CreditCardType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<CreditCardType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (CreditCardType i, string title) => i.Name = title;
    		}
    	}
    }
}
