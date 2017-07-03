using System;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Base;
using NetSteps.Common.Globalization;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common.Provider;
using NetSteps.Web.Mvc.ActionResults;
using NetSteps.Web.Mvc.Controls.Models.Enrollment;
using NetSteps.Web.Mvc.Extensions;

namespace NetSteps.Web.Mvc.Controls.Attributes
{
    /// <summary>
    /// Ensures that the <see cref="EnrollmentContext"/> has been initialized and that the requested controller is a valid enrollment step.
    /// </summary>
    public class EnrollmentStepAttribute : ActionFilterAttribute
    {
        public string OnFailedUrl { get; private set; }

        public EnrollmentStepAttribute(string onFailedUrl)
        {
            this.OnFailedUrl = onFailedUrl;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Do nothing if there is no session
            if (filterContext.HttpContext == null
                || filterContext.HttpContext.Session == null)
            {
                return;
            }

            // Check for required properties
			var enrollmentContext = Create.New<IEnrollmentContextProvider>().GetEnrollmentContext();
			if (enrollmentContext.AccountTypeID == 0
                || enrollmentContext.EnrollmentConfig == null
                || enrollmentContext.EnrollmentConfig.Steps == null)
            {
                var response = new BasicResponse
                {
                    Success = false,
                    Message = _errorEnrollmentSessionTimedOut
                };

                if(filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonBasicResult(response);
                }
                else
                {
                    filterContext.Controller.AddMessageToTempData(response);
                    filterContext.Result = new RedirectResult(this.OnFailedUrl);
                }
                return;
            }

            // Check if this controller is a valid enrollment step
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var stepConfig = enrollmentContext.EnrollmentConfig.Steps
                .FirstOrDefault(x => x.Controller.Equals(controllerName, StringComparison.OrdinalIgnoreCase));
            if (stepConfig == null)
            {
                filterContext.Result = new HttpNotFoundResult();
                return;
            }

            // Validation was successful, set the current enrollment step to this controller
            enrollmentContext.EnrollmentConfig.Steps.CurrentItem = stepConfig;
        }

        protected virtual string _errorEnrollmentSessionTimedOut { get { return Translation.GetTerm("ErrorEnrollmentSessionTimedOut", "We're sorry, but your session timed out. Please try again."); } }
    }
}
