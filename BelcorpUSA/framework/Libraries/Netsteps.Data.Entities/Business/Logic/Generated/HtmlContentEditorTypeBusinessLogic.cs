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
    [ContainerRegister(typeof(IHtmlContentEditorTypeBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class HtmlContentEditorTypeBusinessLogic : BusinessLogicBase<HtmlContentEditorType, Int16, IHtmlContentEditorTypeRepository, IHtmlContentEditorTypeBusinessLogic>, IHtmlContentEditorTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<HtmlContentEditorType, Int16> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.HtmlContentEditorTypeID;
    		}
    	}
    	public override Action<HtmlContentEditorType, Int16> SetIdColumnFunc
    	{
    		get
    		{
    			return (HtmlContentEditorType i, Int16 id) => i.HtmlContentEditorTypeID = id;
    		}
    	}
    	public override Func<HtmlContentEditorType, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Name;
    		}
    	}
    	public override Action<HtmlContentEditorType, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (HtmlContentEditorType i, string title) => i.Name = title;
    		}
    	}
    }
}
