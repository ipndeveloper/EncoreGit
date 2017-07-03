using System.Web;
using NetSteps.Encore.Core.IoC;
using NetSteps.Foundation.Common;

namespace NetSteps.Web.Context
{
	[ContainerRegister(typeof(IExecutionContext), RegistrationBehaviors.Default)]
	public class ExecutionContext : IExecutionContext
	{
		public object CurrentUser
		{
			get
			{
				if (HttpContext.Current != null && HttpContext.Current.Session != null)
				{
					return HttpContext.Current.Session["CurrentUser"];
				}

				return null;
			}
			set
			{
				if (HttpContext.Current != null && HttpContext.Current.Session != null)
				{
					HttpContext.Current.Session["CurrentUser"] = value;
				}
			}
		}

		public string   CurrentCultureInfo
		{
			get
			{
				if (HttpContext.Current != null && HttpContext.Current.Session != null)
				{
					return (string)HttpContext.Current.Session["CurrentCultureInfo"];
				}

				return string.Empty;
			}
			set
			{
				if (HttpContext.Current != null && HttpContext.Current.Session != null)
				{
					HttpContext.Current.Session["CurrentCultureInfo"] = value;
				}
			}
		}
	}
}
