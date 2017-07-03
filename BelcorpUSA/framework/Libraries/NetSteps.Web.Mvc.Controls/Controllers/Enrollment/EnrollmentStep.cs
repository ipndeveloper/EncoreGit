using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common.Models.Config;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Web.Mvc.ActionResults;
using NetSteps.Web.Mvc.Controls.Infrastructure;
using NetSteps.Web.Mvc.Extensions;

namespace NetSteps.Web.Mvc.Controls.Controllers.Enrollment
{
    public abstract class EnrollmentStep
    {
        /// <summary>
        /// This is the 'UseControl' attribute in Enrollment.xml.
        /// </summary>
        public string Name { get { return _stepConfig.Controller; } }

        /// <summary>
        /// This is the 'TermName' attribute in Enrollment.xml.
        /// </summary>
        public string TermName { get { return _stepConfig.TermName; } }
        
        /// <summary>
        /// This is the 'Skippable' attribute in Enrollment.xml.
        /// </summary>
        public bool IsSkippable { get { return _stepConfig.Skippable; } }

        /// <summary>
        /// The step info from Enrollment.xml.
        /// </summary>
        protected readonly IEnrollmentStepConfig _stepConfig;

        /// <summary>
        /// The controller that instantiated this enrollment step.
        /// </summary>
        protected readonly Controller _controller;

        /// <summary>
        /// A collection of enrollment data that is stored in session.
        /// </summary>
        protected readonly IEnrollmentContext _enrollmentContext;

        protected virtual string _defaultViewName { get { return _stepConfig.Controller; } }
        protected virtual string _defaultPartialViewName { get { return _stepConfig.Controller; } }

        /// <summary>
        /// Base constructor for all enrollment steps.
        /// </summary>
        public EnrollmentStep(IEnrollmentStepConfig stepConfig, Controller controller, IEnrollmentContext enrollmentContext)
        {
            _stepConfig = stepConfig;
            _controller = controller;
            _enrollmentContext = enrollmentContext;
        }

        /// <summary>
        /// The main entry point for all requests.
        /// </summary>
        public virtual ActionResult ExecuteAction(string actionName)
        {
            // Safety Tip: Executing an action automatically updates the CurrentItem in session.
            // This is the only place where CurrentItem should be set.
            _enrollmentContext.EnrollmentConfig.Steps.CurrentItem = _stepConfig;

            var method = this.GetType().FindActionMethod(_controller.ControllerContext, actionName);
            var result = method.Invoke(this, method.GetValuesFromRequest(_controller)) as ActionResult;

            // This is to avoid multiple calls to OnActionExecuted() when forwarding from
            // one step to another. We should only call OnActionExecuted() on the new step.
            if (_enrollmentContext.EnrollmentConfig.Steps.CurrentItem == _stepConfig)
            {
                OnActionExecuted(result);
            }

            return result;
        }

        /// <summary>
        /// Advances to the next enrollment step and executes the default step action.
        /// This is meant to be called from other step action methods and should not be made public.
        /// </summary>
        protected virtual ActionResult NextStep()
        {
            // Final step should never have a 'next' or 'skip' button, so this should never happen.
            if (!_enrollmentContext.EnrollmentConfig.Steps.HasNextItem)
            {
                return new HttpNotFoundResult();
            }

            var nextStepConfig = _enrollmentContext.EnrollmentConfig.Steps.NextItem;

            if (_controller.Request.IsAjaxRequest())
            {
                // For ajax, we just load the next step and execute it.
                EnrollmentStep step = GetStep(nextStepConfig, _controller, _enrollmentContext);
                return step.ExecuteAction(DefaultStepAction);
            }
            else
            {
                // For non-ajax, we redirect the browser to the next step.
                string stepName = nextStepConfig.Controller;
                return RedirectToStepAction(DefaultStepAction, stepName);
            }
        }

        protected virtual void OnActionExecuted(ActionResult result) { }

        public InventoryBaseRepository Inventory { get { return Create.New<InventoryBaseRepository>(); } }
        public IShippingCalculator ShippingCalculator { get { return Create.New<IShippingCalculator>(); } }

        #region Controller Helper Methods
        #region Controller
        protected HttpContextBase HttpContext { get { return _controller.HttpContext; } }
        protected ModelStateDictionary ModelState { get { return _controller.ModelState; } }
        protected HttpRequestBase Request { get { return _controller.Request; } }
        protected HttpResponseBase Response { get { return _controller.Response; } }
        protected RouteData RouteData { get { return _controller.RouteData; } }
        protected HttpServerUtilityBase Server { get { return _controller.Server; } }
        protected HttpSessionStateBase Session { get { return _controller.Session; } }
        protected TempDataDictionary TempData { get { return _controller.TempData; } }
        protected dynamic ViewBag { get { return _controller.ViewBag; } }
        protected ViewDataDictionary ViewData { get { return _controller.ViewData; } }
        #endregion

        #region Json
        protected JsonResult Json(object data)
        {
            return this.Json(data, null, null, JsonRequestBehavior.AllowGet);
        }

        protected JsonResult Json(object data, string contentType)
        {
            return this.Json(data, contentType, null, JsonRequestBehavior.AllowGet);
        }

        protected JsonResult Json(object data, string contentType, Encoding contentEncoding)
        {
            return this.Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
        }

        protected JsonResult Json(object data, JsonRequestBehavior behavior)
        {
            return this.Json(data, null, null, behavior);
        }

        protected JsonResult Json(object data, string contentType, JsonRequestBehavior behavior)
        {
            return this.Json(data, contentType, null, behavior);
        }

        protected virtual JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = data;
            jsonResult.ContentType = contentType;
            jsonResult.ContentEncoding = contentEncoding;
            jsonResult.JsonRequestBehavior = behavior;
            return jsonResult;
        }

        protected JsonBasicResult JsonSuccess() { return new JsonBasicResult(true); }
        protected JsonBasicResult JsonSuccess(string message) { return new JsonBasicResult(true, message); }
        protected JsonBasicResult JsonError() { return new JsonBasicResult(false); }
        protected JsonBasicResult JsonError(string message) { return new JsonBasicResult(false, message); }
        protected JsonBasicResult JsonBasic(BasicResponse basicResponse) { return new JsonBasicResult(basicResponse); }
        #endregion

        #region PartialView
        protected PartialViewResult PartialView()
        {
            return this.PartialView(_defaultPartialViewName, null);
        }

        protected PartialViewResult PartialView(object model)
        {
            return this.PartialView(_defaultPartialViewName, model);
        }

        protected PartialViewResult PartialView(string viewName)
        {
            return this.PartialView(viewName, null);
        }

        protected virtual PartialViewResult PartialView(string viewName, object model)
        {
            if (model != null)
            {
                _controller.ViewData.Model = model;
            }
            PartialViewResult partialViewResult = new PartialViewResult();
            partialViewResult.ViewName = viewName;
            partialViewResult.ViewData = _controller.ViewData;
            partialViewResult.TempData = _controller.TempData;
            return partialViewResult;
        }
        #endregion

        #region Redirect
        protected virtual RedirectResult Redirect(string url)
        {
            return new RedirectResult(url);
        }

        protected RedirectToRouteResult RedirectToStepAction(string stepActionName)
        {
            return this.RedirectToStepAction(stepActionName, null, null);
        }

        protected RedirectToRouteResult RedirectToStepAction(string stepActionName, object routeValues)
        {
            return this.RedirectToStepAction(stepActionName, null, new RouteValueDictionary(routeValues));
        }

        protected RedirectToRouteResult RedirectToStepAction(string stepActionName, RouteValueDictionary routeValues)
        {
            return this.RedirectToStepAction(stepActionName, null, routeValues);
        }

        protected RedirectToRouteResult RedirectToStepAction(string stepActionName, string stepName)
        {
            return this.RedirectToStepAction(stepActionName, stepName, null);
        }

        protected RedirectToRouteResult RedirectToStepAction(string stepActionName, string stepName, object routeValues)
        {
            return this.RedirectToStepAction(stepActionName, stepName, new RouteValueDictionary(routeValues));
        }

        protected virtual RedirectToRouteResult RedirectToStepAction(string stepActionName, string stepName, RouteValueDictionary routeValues)
        {
            RouteValueDictionary routeValueDictionaries = new RouteValueDictionary();

            if (_controller.RouteData != null && _controller.RouteData.Values != null)
            {
                object obj = null;
                if (_controller.RouteData.Values.TryGetValue("step", out obj))
                {
                    routeValueDictionaries["step"] = obj;
                }
            }

            if (routeValues != null)
            {
                foreach (var routeValue in routeValues)
                {
                    routeValueDictionaries[routeValue.Key] = routeValue.Value;
                }
            }

            if (!string.IsNullOrWhiteSpace(stepActionName))
            {
                routeValueDictionaries["stepAction"] = stepActionName;
            }

            if (!string.IsNullOrWhiteSpace(stepName))
            {
                routeValueDictionaries["step"] = stepName;
            }

            return new RedirectToRouteResult(StepActionRouteName, routeValueDictionaries);
        }
        #endregion

        #region View
        protected ViewResult View()
        {
            return this.View(_defaultViewName, null, null);
        }

        protected ViewResult View(object model)
        {
            return this.View(_defaultViewName, null, model);
        }

        protected ViewResult View(string viewName)
        {
            return this.View(viewName, null, null);
        }

        protected ViewResult View(string viewName, string masterName)
        {
            return this.View(viewName, masterName, null);
        }

        protected ViewResult View(string viewName, object model)
        {
            return this.View(viewName, null, model);
        }

        protected virtual ViewResult View(string viewName, string masterName, object model)
        {
            if (model != null)
            {
                _controller.ViewData.Model = model;
            }
            ViewResult viewResult = new ViewResult();
            viewResult.ViewName = viewName;
            viewResult.MasterName = masterName;
            viewResult.ViewData = _controller.ViewData;
            viewResult.TempData = _controller.TempData;
            return viewResult;
        }
        #endregion
        #endregion Controller Helper Methods

        #region Static
        public const string StepActionRouteName = "Enrollment_StepAction";
        public const string DefaultStepAction = "Index";

        /// <summary>
        /// Instantiates a new <see cref="EnrollmentStep"/>.
        /// </summary>
        public static EnrollmentStep GetStep(IEnrollmentStepConfig stepConfig, Controller controller, IEnrollmentContext enrollmentContext)
        {
            var controlName = stepConfig.Controller;

            // Default constructor parameters for all enrollment steps
            var args = new List<object>
            {
                stepConfig,
                controller,
                enrollmentContext
            };

            //// Get any additional constructor parameters for the step from the config
            //var parametersDynamic = stepConfig.Parameters().Parameter();
            //// Make sure parametersDynamic is an IEnumerable
            //var parametersEnumerable = parametersDynamic as IEnumerable<dynamic>;
            //if (parametersEnumerable == null)
            //{
            //    // If parametersEnumerable is null, then there are either no parameters or just a single parameter.
            //    // Try reading an attribute to see if a parameter exists.
            //    parametersEnumerable = string.IsNullOrWhiteSpace(parametersDynamic.Value)
            //        ? Enumerable.Empty<dynamic>() // no parameters
            //        : new[] { parametersDynamic }; // single parameter
            //}
            //args.AddRange(parametersEnumerable.Select(x => x.Value));

            var stepType = EnrollmentConfigHandler.AllTypes.FirstOrDefault(type => type.Name.Equals(controlName + "Step", StringComparison.OrdinalIgnoreCase));
            if (stepType == default(Type))
                throw new Exception("Enrollment step not found.");

            var step = stepType.New(args.ToArray()) as EnrollmentStep;
            if (step == null)
                throw new Exception("Enrollment step not found.");

            return step;
        }
        #endregion
    }
}