using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Common.Globalization;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;

namespace NetSteps.Web.Mvc.Attributes
{
	public class FunctionFilterAttribute : ActionFilterAttribute
	{
		private string Function { get; set; }
		public string RedirectUrl { get; set; }
		public Constants.SiteType SiteTypeID { get; set; }

		protected IUser User
		{
			get
			{
				return ApplicationContext.Instance.CurrentUser ?? ApplicationContext.Instance.CurrentAccount;
			}
		}

		protected Role Anonymous
		{
			get
			{
				return ApplicationContext.Instance.AnonymousRole;
			}
		}

		protected FunctionFilterAttribute(string redirectUrl, Constants.SiteType siteTypeID = Data.Entities.Generated.ConstantsGenerated.SiteType.NotSet)
		{
			RedirectUrl = redirectUrl.StartsWith("~") ? VirtualPathUtility.ToAbsolute(redirectUrl) : redirectUrl;
			SiteTypeID = siteTypeID;
		}

		public FunctionFilterAttribute(string function, string redirectUrl, Constants.SiteType siteTypeID = Data.Entities.Generated.ConstantsGenerated.SiteType.NotSet)
			: this(redirectUrl,siteTypeID)
		{
			Function = function;
		}

		protected virtual bool IsMatch()
		{ 
			if(User == null && Anonymous.HasFunction(Function))
			{
				return true;
			}

			bool checkWorkstationUserRole = false;
			SiteType backOfficeSiteType = SmallCollectionCache.Instance.SiteTypes.FirstOrDefault(t => t.Name == "BackOffice");
			if (SiteTypeID > 0 && backOfficeSiteType != null && SiteTypeID == (Constants.SiteType)backOfficeSiteType.SiteTypeID)
			{
				checkWorkstationUserRole = true;
			}
			
			if(User != null && User.HasFunction(Function, true, checkWorkstationUserRole))
			{
				return true;
			}

			return false;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (User == null) filterContext.Controller.TempData["AttemptedPage"] = filterContext.RequestContext.HttpContext.Request.Url.PathAndQuery;
			if (!IsMatch())
			{
				if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
				{
					JsonResult jsonResult = new JsonResult();
					jsonResult.Data = new { result = false, message = String.Format(Translation.GetTerm("YouDoNotHaveTheNecessaryFunction", "You do not have the necessary function: {0}."), Function) };
					jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
					filterContext.Result = jsonResult;
				}
				else
				{
					filterContext.Result = new RedirectResult(RedirectUrl);
				}
				return;
			}
			base.OnActionExecuting(filterContext);
		}
	}
}
