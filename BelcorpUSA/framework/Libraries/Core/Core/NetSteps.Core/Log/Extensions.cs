
using System;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.Reflection;
using NetSteps.Encore.Core.Properties;
using System.Text;

namespace NetSteps.Encore.Core.Log
{
	/// <summary>
	/// Utility class containing extensions for logging.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Gets the log source name for a type.
		/// </summary>
		/// <param name="type">the type</param>
		/// <returns>the log source name</returns>
		public static string GetLogSourceName(this Type type)
		{
			Contract.Requires<ArgumentNullException>(type != null, Resources.Chk_CannotBeNull);

			return type.GetReadableFullName();
		}

		/// <summary>
		/// Gets the type's log sink.
		/// </summary>
		/// <param name="type">the target type.</param>
		/// <returns>A log sink.</returns>
		public static ILogSink GetLogSink(this Type type)
		{
			Contract.Requires<ArgumentNullException>(type != null, Resources.Chk_CannotBeNull);
						
			return LogSinkManager.Singleton.GetLogSinkForType(type);		
		}

		/// <summary>
		/// Gets the instance's log sink.
		/// </summary>
		/// <param name="item">the target instance.</param>
		/// <returns>A log sink.</returns>
		public static ILogSink GetLogSink<T>(this T item)
		{
			Contract.Requires<ArgumentNullException>(item != null, Resources.Chk_CannotBeNull);

			return item.GetType().GetLogSink();
		}

		/// <summary>
		/// Formats an exception for output into the log.
		/// </summary>
		/// <param name="ex">the exception</param>
		/// <returns>a string representation of the exception</returns>
		public static string FormatForLogging(this Exception ex)
		{
#if DEBUG
			return FormatForLogging(ex, true);
#else
			return FormatForLogging(ex, false);
#endif
		}

		/// <summary>
		/// Formats an exception for output into the log.
		/// </summary>
		/// <param name="ex">the exception</param>
		/// <param name="exposeStackTrace">indicates whether stack trace should be exposed in the output</param>
		/// <returns>a string representation of the exception</returns>
		public static string FormatForLogging(this Exception ex, bool exposeStackTrace)
		{
			Contract.Requires<ArgumentNullException>(ex != null);
			var builder = new StringBuilder(400)
					.Append(ex.GetType().FullName).Append(": ").Append(ex.Message);
			var e = ex.InnerException;
			while (e != null)
			{
				builder.Append(Environment.NewLine).Append("\t InnerException >")
						.Append(e.GetType().FullName).Append(": ").Append(e.Message);

				e = e.InnerException;
			}
			if (exposeStackTrace)
			{
				builder.Append(Environment.NewLine).Append("\t StackTrace >>")
					.Append(ex.StackTrace);
			}
			return builder.ToString();
		}
	}
}
