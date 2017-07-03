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
    [ContainerRegister(typeof(IHtmlContentWorkflowBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class HtmlContentWorkflowBusinessLogic : BusinessLogicBase<HtmlContentWorkflow, Int32, IHtmlContentWorkflowRepository, IHtmlContentWorkflowBusinessLogic>, IHtmlContentWorkflowBusinessLogic, IDefaultImplementation
    {
    	public override Func<HtmlContentWorkflow, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.HtmlContentWorkflowID;
    		}
    	}
    	public override Action<HtmlContentWorkflow, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (HtmlContentWorkflow i, Int32 id) => i.HtmlContentWorkflowID = id;
    		}
    	}
    	public override Func<HtmlContentWorkflow, string> GetTitleColumnFunc
    	{
    		get
    		{
    			return i => i.Title;
    		}
    	}
    	public override Action<HtmlContentWorkflow, string> SetTitleColumnFunc
    	{
    		get
    		{
    			return (HtmlContentWorkflow i, string title) => i.Title = title;
    		}
    	}
    }
}
