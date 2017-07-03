using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
namespace NetSteps.Web.Mvc.Filters
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class CacheFilterAttribute : ActionFilterAttribute
	{
		#region Fields


		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets cache scope
		/// </summary>
		/// <remarks>
		/// Public - resource can be cached on proxy 
		/// Private - resource should only be cached in web browser
		/// </remarks>
		public HttpCacheability Cacheability { get; set; }

		/// <summary>
		/// Timespan in string format when the requested resource will absolutely expire
		/// </summary>
		/// <remarks>
		/// 1.00:00:00 Cache for 1 day
		/// </remarks>
		public string ExpiresAfter { get; set; }

		private TimeSpan ExpiresAfterAsTimeSpan
		{
			get
			{
				TimeSpan result = TimeSpan.Zero;
				if (!String.IsNullOrWhiteSpace(ExpiresAfter) && !TimeSpan.TryParse(ExpiresAfter, out result)) throw new HttpException(String.Format("The cache control ExpiresAfter value of {0} declared on the current action could not be parsed.", ExpiresAfter));
				return result;
			}
		}

		/// <summary>
		/// Timespan in string format how long the requested resource will remain in cache
		/// </summary>
		/// <remarks>
		/// 1.00:00:00 Cache for 1 day
		/// </remarks>
		public string MaximumAge { get; set; }

		private TimeSpan MaximumAgeAsTimeSpan
		{
			get
			{
				TimeSpan result = TimeSpan.Zero;
				if (!String.IsNullOrWhiteSpace(MaximumAge) && !TimeSpan.TryParse(MaximumAge, out result)) throw new HttpException(String.Format("The cache control MaximumAge value of {0} declared on the current action could not be parsed.", MaximumAge));
				return result;
			}
		}

		public bool ValidUntilExpires { get; set; }

		public bool SlidingExpiration { get; set; }

		public bool NoStore { get; set; }

		#endregion

		#region Constructors

		public CacheFilterAttribute()
		{
			Cacheability = HttpCacheability.Private;
			ExpiresAfter = TimeSpan.Zero.ToString();
			ValidUntilExpires = true;
			NoStore = false;
			SlidingExpiration = false;
		}

		#endregion

		#region Methods

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext != null 
				&& filterContext.HttpContext != null 
				&& filterContext.HttpContext.Response != null 
				&& filterContext.HttpContext.Response.StatusCode == (int)HttpStatusCode.OK)
			{
				HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
				DateTime date = DateTime.UtcNow;
				cache.SetCacheability(Cacheability);
				if (ExpiresAfterAsTimeSpan != TimeSpan.Zero) cache.SetExpires(date.Add(ExpiresAfterAsTimeSpan));
				if (MaximumAgeAsTimeSpan > TimeSpan.Zero)
				{
					cache.SetMaxAge(MaximumAgeAsTimeSpan);
					cache.SetSlidingExpiration(SlidingExpiration);
				}
				cache.SetValidUntilExpires(ValidUntilExpires);
				if (NoStore) cache.SetNoStore();
			}
			base.OnActionExecuted(filterContext);
		}

		#endregion
	}
}
