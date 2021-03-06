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
    [ContainerRegister(typeof(IHtmlSectionContentBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class HtmlSectionContentBusinessLogic : BusinessLogicBase<HtmlSectionContent, Int32, IHtmlSectionContentRepository, IHtmlSectionContentBusinessLogic>, IHtmlSectionContentBusinessLogic, IDefaultImplementation
    {
    	public override Func<HtmlSectionContent, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.HtmlSectionContentID;
    		}
    	}
    	public override Action<HtmlSectionContent, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (HtmlSectionContent i, Int32 id) => i.HtmlSectionContentID = id;
    		}
    	}
    }
}
