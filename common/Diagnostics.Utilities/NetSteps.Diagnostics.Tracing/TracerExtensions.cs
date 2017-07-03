using System.Diagnostics;

namespace NetSteps.Diagnostics.Utilities
{
	public static class TracerExtensions
	{
		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Construction

		#endregion

		#region Methods

		public static void TraceEvent<T>(this T instance, TraceEvent value)
		{
			Tracer.Event(typeof(T), value);
		}

		public static void TraceVerbose<T>(this T instance, TraceEvent value)
		{
			value.Type = TraceEventType.Verbose;
			Tracer.Event(typeof(T), value);
		}

		public static void TraceInformation<T>(this T instance, TraceEvent value)
		{
			value.Type = TraceEventType.Information;
			Tracer.Event(typeof(T), value);
		}

		public static void TraceWarning<T>(this T instance, TraceEvent value)
		{
			value.Type = TraceEventType.Warning;
			Tracer.Event(typeof(T), value);
		}

		public static void TraceError<T>(this T instance, TraceEvent value)
		{
			value.Type = TraceEventType.Error;
			Tracer.Event(typeof(T), value);
		}

		public static void TraceException<T>(this T instance, ExceptionTraceEvent value)
		{
			Tracer.Event(typeof(T),value);
		}

		public static TraceActivity TraceActivity<T>(this T instance, string name)
		{
			return Tracer.Activity(typeof(T),name);
		}

		#endregion
	}
}
