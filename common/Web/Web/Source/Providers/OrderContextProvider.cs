using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Providers;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Web.Providers
{
    [ContainerRegister(typeof(IOrderContextProvider), RegistrationBehaviors.OverrideDefault, ScopeBehavior = ScopeBehavior.Singleton)]
    public class OrderContextProvider : IOrderContextProvider
    {
        public IOrderContext GetOrderContext()
        {
            return OrderContextSessionProvider.Get(System.Web.HttpContext.Current.Session);
        }
    }
}
