using System.IO;
using System.Reflection;
using System.Web.Mvc;

namespace NetSteps.Web.Mvc.Extensions
{
    public static class ControllerExtensions
    {
        public static string RenderPartialViewToString(this ControllerBase controller, string partialPath, object model)
        {
            if (string.IsNullOrEmpty(partialPath))
                partialPath = controller.ControllerContext.RouteData.GetRequiredString("action");

            controller.ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, partialPath);
                ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                // copy model state items to the html helper 
                foreach (var item in viewContext.Controller.ViewData.ModelState)
                    if (!viewContext.ViewData.ModelState.Keys.Contains(item.Key))
                    {
                        viewContext.ViewData.ModelState.Add(item);
                    }


                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Finds an action method, respecting ActionMethodSelectorAttributes (default MVC behavior).
        /// </summary>
        public static MethodInfo FindActionMethod(this ControllerBase controller, string actionName)
        {
            return controller.GetType().FindActionMethod(controller.ControllerContext, actionName);
        }
    }
}
