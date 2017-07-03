using System;
using System.Diagnostics;

namespace NetSteps.Diagnostics.Utilities
{
	public static class TypeExtensions
	{
		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Construction

		#endregion

		#region Methods

		public static void TraceEvent(this Type instance, TraceEvent value)
		{
			Tracer.Event(instance, value);
		}

		public static void TraceVerbose(this Type instance, TraceEvent value)
		{
			value.Type = TraceEventType.Verbose;
			Tracer.Event(instance, value);
		}

		public static void TraceInformation(this Type instance, TraceEvent value)
		{
			value.Type = TraceEventType.Information;
			Tracer.Event(instance, value);
		}

		public static void TraceWarning(this Type instance, TraceEvent value)
		{
			value.Type = TraceEventType.Warning;
			Tracer.Event(instance, value);
		}

		public static void TraceError(this Type instance, TraceEvent value)
		{
			value.Type = TraceEventType.Error;
			Tracer.Event(instance, value);
		}

		public static void TraceException<T>(this Type instance, ExceptionTraceEvent value)
		{
			Tracer.Event(instance, value);
		}

		public static TraceActivity TraceActivity<T>(this Type instance, string name)
		{
			return Tracer.Activity(instance, name);
		}

		#endregion
	}
}
