using System;
using System.Threading;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Web.Mvc.Business.Helpers
{
    public class WebHelpers
    {
        // IBaseControllerActionFilter allows a client to inject some logic that runs before the code in the BaseController - JHE
        static Lazy<IBaseControllerActionFilter> _filter = new Lazy<IBaseControllerActionFilter>(ResolveBaseControllerOverrideActionFilter, LazyThreadSafetyMode.ExecutionAndPublication);

        public static IBaseControllerActionFilter BaseControllerOverrideActionFilter
        {
            get { return _filter.Value; }
        }

        static IBaseControllerActionFilter ResolveBaseControllerOverrideActionFilter()
        {
            try
            {
                if (Container.Current.Registry.IsTypeRegistered<IBaseControllerActionFilter>())
                {
                    return Create.New<IBaseControllerActionFilter>();
                }
                else
                {
                    return null;
                }
            }
            catch (ContainerException)
            {
                // there isn't one registered, return null.
                return null;
            }
        }
    }
}
