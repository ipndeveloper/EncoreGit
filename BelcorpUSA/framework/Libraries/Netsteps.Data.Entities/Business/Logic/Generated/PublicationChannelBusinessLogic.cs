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
    [ContainerRegister(typeof(IPublicationChannelBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class PublicationChannelBusinessLogic : BusinessLogicBase<PublicationChannel, Int16, IPublicationChannelRepository, IPublicationChannelBusinessLogic>, IPublicationChannelBusinessLogic, IDefaultImplementation
    {
    	public override Func<PublicationChannel, Int16> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.PublicationChannelID;
    		}
    	}
    	public override Action<PublicationChannel, Int16> SetIdColumnFunc
    	{
    		get
    		{
    			return (PublicationChannel i, Int16 id) => i.PublicationChannelID = id;
    		}
    	}
    	public override Func<PublicationChannel, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<PublicationChannel, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (PublicationChannel i, string title) => i.Name = title;
    		}
    	}
    }
}
