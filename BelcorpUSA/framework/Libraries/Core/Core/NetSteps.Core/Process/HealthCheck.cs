using System;
using System.Dynamic;
using System.Reflection;
using NetSteps.Encore.Core.Log;
using NetSteps.Encore.Core.Wireup;
using Newtonsoft.Json;

namespace NetSteps.Encore.Core.Process
{
	/// <summary>
	/// Base class for health checks.
	/// </summary>
	public class HealthCheck
	{
		static readonly ILogSink __log = typeof(HealthCheck).GetLogSink();
		static readonly string Succeeded = "Succeeded";
		static readonly string Failed = "Failed";

		/// <summary>
		/// Indicates whether an status represents success.
		/// </summary>
		/// <param name="status">status object</param>
		/// <returns><em>true</em> if successful; otherwise <em>false</em></returns>
		public static bool IsSuccess(dynamic status)
		{
			return status != null
				&& !String.IsNullOrEmpty(status.HealthCheck)
				&& String.Equals(Succeeded, status.HealthCheck, StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Generates a status object.
		/// </summary>
		/// <returns>the status</returns>
		public dynamic GetStatus()
		{
#if DEBUG
			return GetStatus(true);
#else
			return GetStatus(false);
#endif
		}
		/// <summary>
		/// Generates a status object.
		/// </summary>
		/// <param name="showStackTrace">indicates whether a stack trace should be returned on errors</param>
		/// <returns>the status</returns>
		public dynamic GetStatus(bool showStackTrace)
		{
			dynamic result = new ExpandoObject();
			try
			{
				result.ProcessIdentity = ProcessIdentity.MakeProcessIdentity();
				result.WiredAssemblies = WireupCoordinator.Instance.ExposeDependenciesFor(Assembly.GetCallingAssembly());
				result.SpecializedHealthCheckKind = this.GetType().FullName;
				result.HealthCheck = PerformGetStatus(result) ? Succeeded : Failed;
			}
			catch (Exception e)
			{
				result.HealthCheck = Failed;
				result.SpecializedHealthCheck = Failed;
				result.ErrorInfo = e.FormatForLogging(showStackTrace);
				__log.Error(new { UnexpectedException = e.FormatForLogging() });
			}
			return result;
		}

		/// <summary>
		/// Performs specialized status checks.
		/// </summary>
		/// <param name="status">a dynamic object for collecting specialized status</param>
		/// <returns>true if the specialized status was successful; otherwise false</returns>
		protected virtual bool PerformGetStatus(dynamic status)
		{
			return true;
		}

		/// <summary>
		/// Converst the status object to JSON.
		/// </summary>
		/// <param name="status"></param>
		/// <returns></returns>
		public static string FromStatusToJson(dynamic status)
		{
			return JsonConvert.SerializeObject(status, Formatting.Indented);
		}
	}
}
