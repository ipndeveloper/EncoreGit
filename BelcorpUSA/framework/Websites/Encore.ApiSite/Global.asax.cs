namespace Encore.ApiSite
{
    using System;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Routing;

    using NetSteps.Common.Extensions;
    using NetSteps.Data.Entities.Exceptions;
    using NetSteps.Encore.Core.Wireup;

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }

        protected void Application_Start()
        {
            try
            {
                AreaRegistration.RegisterAllAreas();

                RegisterGlobalFilters(GlobalFilters.Filters);
                RegisterRoutes(RouteTable.Routes);
                WireupCoordinator.SelfConfigure();
            }
            catch (ReflectionTypeLoadException reflectionTypeLoadException)
            {
                if (reflectionTypeLoadException.LoaderExceptions.CountSafe() > 0)
                {
                    throw reflectionTypeLoadException.LoaderExceptions[0];
                }
                throw;
            }
            catch (Exception ex)
            {
                EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw;
            }
        }
    }
}