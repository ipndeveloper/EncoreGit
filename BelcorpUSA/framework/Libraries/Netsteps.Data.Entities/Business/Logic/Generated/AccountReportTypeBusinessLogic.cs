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
    [ContainerRegister(typeof(IAccountReportTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class AccountReportTypeBusinessLogic : BusinessLogicBase<AccountReportType, Int16, IAccountReportTypeRepository, IAccountReportTypeBusinessLogic>, IAccountReportTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<AccountReportType, Int16> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.AccountReportTypeID;
    		}
    	}
    	public override Action<AccountReportType, Int16> SetIdColumnFunc
    	{
    		get
    		{
    			return (AccountReportType i, Int16 id) => i.AccountReportTypeID = id;
    		}
    	}
    	public override Func<AccountReportType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<AccountReportType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (AccountReportType i, string title) => i.Name = title;
    		}
    	}
    }
}
