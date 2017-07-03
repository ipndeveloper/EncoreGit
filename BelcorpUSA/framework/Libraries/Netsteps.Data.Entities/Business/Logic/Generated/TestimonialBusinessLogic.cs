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
    [ContainerRegister(typeof(ITestimonialBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class TestimonialBusinessLogic : BusinessLogicBase<Testimonial, Int32, ITestimonialRepository, ITestimonialBusinessLogic>, ITestimonialBusinessLogic, IDefaultImplementation
    {
    	public override Func<Testimonial, Int32> GetIdColumnFunc
    	{
    		get
    		{
    			return i => i.TestimonialID;
    		}
    	}
    	public override Action<Testimonial, Int32> SetIdColumnFunc
    	{
    		get
    		{
    			return (Testimonial i, Int32 id) => i.TestimonialID = id;
    		}
    	}
    }
}
