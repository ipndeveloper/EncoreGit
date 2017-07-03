using System.Reflection;
using System.Web.Mvc;

namespace NetSteps.Web.Mvc.Attributes
{
    /// <summary>
    /// Prevents an action method from being executed.
    /// </summary>
    public class HiddenActionAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return false;
        }
    }
}
