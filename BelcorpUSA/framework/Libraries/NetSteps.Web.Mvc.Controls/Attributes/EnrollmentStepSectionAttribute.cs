using System;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common.Provider;

namespace NetSteps.Web.Mvc.Controls.Attributes
{
    public class EnrollmentStepSectionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Do nothing if there is no session
            if (filterContext.HttpContext == null
                || filterContext.HttpContext.Session == null)
            {
                return;
            }

            var enrollmentContext = Create.New<IEnrollmentContextProvider>().GetEnrollmentContext();
            
            // Ensure that sections are in context
            var sections = enrollmentContext.EnrollmentConfig.Steps
                .First(x => x.Controller.Equals(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, StringComparison.OrdinalIgnoreCase))
                .Sections;

            if (sections == null || !sections.Any())
            {
                throw new Exception(string.Format("There are no sections defined for account type ID {0}.", enrollmentContext.AccountTypeID));
            }

            var actionName = filterContext.ActionDescriptor.ActionName;

            // The Index action uses the sections list, but it isn't actually a "section", so it is done.
            if (actionName == "Index")
            {
                return;
            }

            // Check if this action is a valid enrollment step
            var currentSection = sections.FirstOrDefault(x => x.Action.Equals(actionName, StringComparison.OrdinalIgnoreCase));
            if (currentSection == null)
            {
                filterContext.Result = new HttpNotFoundResult();
                return;
            }

            sections.CurrentItem = currentSection;
        }
    }
}
