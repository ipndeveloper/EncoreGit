using System.Web.Mvc;
using NetSteps.Common.Base;

namespace NetSteps.Web.Mvc.ActionResults
{
    /// <summary>
    /// Provides a mapping between <see cref="BasicResponse"/> format and our commonly-used Json result format.
    /// </summary>
    public class JsonBasicResult : ActionResult
    {
        protected JsonResult _innerJsonResult;
        public bool Success { get; set; }
        public string Message { get; set; }

        public JsonBasicResult() : this(true, null) { }
        public JsonBasicResult(bool success) : this(success, null) { }
        public JsonBasicResult(BasicResponse basicResponse) : this(basicResponse.Success, basicResponse.Message) { }
        public JsonBasicResult(bool success, string message)
        {
            _innerJsonResult = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            this.Success = success;
            this.Message = message;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            // Return both "result" and "success" for compatibility
            _innerJsonResult.Data = new { result = this.Success, success = this.Success, message = this.Message };
            _innerJsonResult.ExecuteResult(context);
        }
    }
}
