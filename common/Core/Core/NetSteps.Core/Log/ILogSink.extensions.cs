using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using NetSteps.Encore.Core.Properties;
using System.Diagnostics.Contracts;

namespace NetSteps.Encore.Core.Log
{
	/// <summary>
	/// LogSink extensions
	/// </summary>
	public static class ILogSinkExtensions
	{
		private static StackTrace GetStackTrace(TraceEventType eventType, TraceEventType threshold)
		{
			return (eventType <= threshold)
				? new System.Diagnostics.StackTrace(2)
				: null;
		}

		/// <summary>
		/// Notifies the sink that a critical event occurred.
		/// </summary>
		/// <typeparam name="TData">data type TData</typeparam>
		/// <param name="sink">the target sink</param>
		/// <param name="data">an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Critical<TData>(this ILogSink sink, TData data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Critical(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, data);
		}

		/// <summary>
		/// Notifies the sink that a critical event occurred.
		/// </summary>
		/// <typeparam name="TData">data type TData</typeparam>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="data">an object providing data about the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Critical<TData>(this ILogSink sink, int appEventKind, string appEventName, TData data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.Critical)) return null;

			var evt = new DataLogEvent<TData>(sink.Name, TraceEventType.Critical, appEventKind, appEventName, data, GetStackTrace(TraceEventType.Critical, sink.StackTraceThreshold));
			sink.Critical(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a critical event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="data">function that resolves an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Critical<TData>(this ILogSink sink, Func<TData> data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return Critical(sink, LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, data);
		}

		/// <summary>
		/// Notifies the sink that a error event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="data">function that resolves an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Critical<TData>(this ILogSink sink, int appEventKind, string appEventName, Func<TData> data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.Critical)) return null;

			var evt = new LazyDataLogEvent<TData>(sink.Name, TraceEventType.Error, appEventKind, appEventName, data, GetStackTrace(TraceEventType.Error, sink.StackTraceThreshold));
			sink.Error(evt);
			return evt;
		}
		/// <summary>
		/// Notifies the sink that a critical event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Critical(this ILogSink sink, string message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Critical(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, message);
		}

		/// <summary>
		/// Notifies the sink that a critical event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="formattedMessage">a formatted message describing the event</param>
		/// <param name="messageArgs">arguments used when formatting the log message</param>
		/// <returns>the log event</returns>
		public static LogEvent Critical(this ILogSink sink, string formattedMessage, params object[] messageArgs)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Critical(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, () => String.Format(formattedMessage, messageArgs));
		}

		/// <summary>
		/// Notifies the sink that a critical event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="formattedMessage">a formatted message describing the event</param>
		/// <param name="messageArgs">arguments used when formatting the log message</param>
		/// <returns>the log event</returns>
		public static LogEvent Critical(this ILogSink sink, int appEventKind, string appEventName, string formattedMessage, params object[] messageArgs)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Critical(appEventKind, appEventName, () => String.Format(formattedMessage, messageArgs));
		}

		/// <summary>
		/// Notifies the sink that a critical event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Critical(this ILogSink sink, int appEventKind, string appEventName, string message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.Critical)) return null;

			var evt = new SimpleLogEvent(sink.Name, TraceEventType.Critical, appEventKind, appEventName, message, GetStackTrace(TraceEventType.Critical, sink.StackTraceThreshold));
			sink.Critical(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a critical event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="message">a function that produces a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Critical(this ILogSink sink, Func<string> message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Critical(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, message);
		}

		/// <summary>
		/// Notifies the sink that a critical event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="message">a function that produces a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Critical(this ILogSink sink, int appEventKind, string appEventName, Func<string> message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.Critical)) return null;

			var evt = new LazyLogEvent(sink.Name, TraceEventType.Critical, appEventKind, appEventName, message, GetStackTrace(TraceEventType.Critical, sink.StackTraceThreshold));
			sink.Critical(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a error event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="data">an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Error<TData>(this ILogSink sink, TData data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Error(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, data);
		}

		/// <summary>
		/// Notifies the sink that a error event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="data">an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Error<TData>(this ILogSink sink, int appEventKind, string appEventName, TData data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.Error)) return null;

			var evt = new DataLogEvent<TData>(sink.Name, TraceEventType.Error, appEventKind, appEventName, data, GetStackTrace(TraceEventType.Error, sink.StackTraceThreshold));
			sink.Error(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a error event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="data">function that resolves an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Error<TData>(this ILogSink sink, Func<TData> data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return Error(sink, LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, data);
		}
		/// <summary>
		/// Notifies the sink that a error event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="data">function that resolves an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Error<TData>(this ILogSink sink, int appEventKind, string appEventName, Func<TData> data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.Error)) return null;

			var evt = new LazyDataLogEvent<TData>(sink.Name, TraceEventType.Error, appEventKind, appEventName, data, GetStackTrace(TraceEventType.Error, sink.StackTraceThreshold));
			sink.Error(evt);
			return evt;
		}


		/// <summary>
		/// Notifies the sink that a error event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Error(this ILogSink sink, string message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Error(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, message);
		}

		/// <summary>
		/// Notifies the sink that a error event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="formattedMessage">a formatted message describing the event</param>
		/// <param name="messageArgs">arguments used when formatting the log message</param>
		/// <returns>the log event</returns>
		public static LogEvent Error(this ILogSink sink, string formattedMessage, params object[] messageArgs)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Error(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, () => String.Format(formattedMessage, messageArgs));
		}

		/// <summary>
		/// Notifies the sink that a error event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="formattedMessage">a formatted message describing the event</param>
		/// <param name="messageArgs">arguments used when formatting the log message</param>
		/// <returns>the log event</returns>
		public static LogEvent Error(this ILogSink sink, int appEventKind, string appEventName, string formattedMessage, params object[] messageArgs)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Error(appEventKind, appEventName, () => String.Format(formattedMessage, messageArgs));
		}

		/// <summary>
		/// Notifies the sink that a error event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Error(this ILogSink sink, int appEventKind, string appEventName, string message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.Error)) return null;

			var evt = new SimpleLogEvent(sink.Name, TraceEventType.Error, appEventKind, appEventName, message, GetStackTrace(TraceEventType.Error, sink.StackTraceThreshold));
			sink.Error(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a error event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Error(this ILogSink sink, Func<string> message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Error(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, message);
		}

		/// <summary>
		/// Notifies the sink that a error event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="message">a function that produces a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Error(this ILogSink sink, int appEventKind, string appEventName, Func<string> message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.Error)) return null;

			var evt = new LazyLogEvent(sink.Name, TraceEventType.Error, appEventKind, appEventName, message, GetStackTrace(TraceEventType.Error, sink.StackTraceThreshold));
			sink.Error(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a informational event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="data">an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Information<TData>(this ILogSink sink, TData data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Information(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, data);
		}

		/// <summary>
		/// Notifies the sink that a informational event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="data">an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Information<TData>(this ILogSink sink, int appEventKind, string appEventName, TData data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.Information)) return null;

			var evt = new DataLogEvent<TData>(sink.Name, TraceEventType.Information, appEventKind, appEventName, data, GetStackTrace(TraceEventType.Information, sink.StackTraceThreshold));
			sink.Information(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a informational event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="data">function that resolves an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Information<TData>(this ILogSink sink, Func<TData> data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return Information(sink, LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, data);
		}

		/// <summary>
		/// Notifies the sink that a informational event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="data">function that resolves an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Information<TData>(this ILogSink sink, int appEventKind, string appEventName, Func<TData> data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.Information)) return null;

			var evt = new LazyDataLogEvent<TData>(sink.Name, TraceEventType.Error, appEventKind, appEventName, data, GetStackTrace(TraceEventType.Error, sink.StackTraceThreshold));
			sink.Error(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a informational event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Information(this ILogSink sink, string message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Information(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, message);
		}

		/// <summary>
		/// Notifies the sink that a informational event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="formattedMessage">a formatted message describing the event</param>
		/// <param name="messageArgs">arguments used when formatting the log message</param>
		/// <returns>the log event</returns>
		public static LogEvent Information(this ILogSink sink, string formattedMessage, params object[] messageArgs)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Information(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, () => String.Format(formattedMessage, messageArgs));
		}

		/// <summary>
		/// Notifies the sink that a informational event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="formattedMessage">a formatted message describing the event</param>
		/// <param name="messageArgs">arguments used when formatting the log message</param>
		/// <returns>the log event</returns>
		public static LogEvent Information(this ILogSink sink, int appEventKind, string appEventName, string formattedMessage, params object[] messageArgs)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Information(appEventKind, appEventName, () => String.Format(formattedMessage, messageArgs));
		}

		/// <summary>
		/// Notifies the sink that a informational event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Information(this ILogSink sink, int appEventKind, string appEventName, string message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.Information)) return null;

			var evt = new SimpleLogEvent(sink.Name, TraceEventType.Information, appEventKind, appEventName, message, GetStackTrace(TraceEventType.Information, sink.StackTraceThreshold));
			sink.Information(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a informational event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Information(this ILogSink sink, Func<string> message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Information(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, message);
		}

		/// <summary>
		/// Notifies the sink that a informational event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="message">a function that produces a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Information(this ILogSink sink, int appEventKind, string appEventName, Func<string> message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.Information)) return null;

			var evt = new LazyLogEvent(sink.Name, TraceEventType.Information, appEventKind, appEventName, message, GetStackTrace(TraceEventType.Information, sink.StackTraceThreshold));
			sink.Information(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="data">an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Resume<TData>(this ILogSink sink, TData data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Resume(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, data);
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="data">an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Resume<TData>(this ILogSink sink, int appEventKind, string appEventName, TData data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.ActivityTracing)) return null;

			var evt = new DataLogEvent<TData>(sink.Name, TraceEventType.Resume, appEventKind, appEventName, data, GetStackTrace(TraceEventType.Resume, sink.StackTraceThreshold));
			sink.Resume(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="data">function that resolves an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Resume<TData>(this ILogSink sink, Func<TData> data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return Resume(sink, LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, data);
		}
		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="data">function that resolves an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Resume<TData>(this ILogSink sink, int appEventKind, string appEventName, Func<TData> data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.ActivityTracing)) return null;

			var evt = new LazyDataLogEvent<TData>(sink.Name, TraceEventType.Resume, appEventKind, appEventName, data, GetStackTrace(TraceEventType.Resume, sink.StackTraceThreshold));
			sink.Error(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Resume(this ILogSink sink, string message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Resume(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, message);
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="formattedMessage">a formatted message describing the event</param>
		/// <param name="messageArgs">arguments used when formatting the log message</param>
		/// <returns>the log event</returns>
		public static LogEvent Resume(this ILogSink sink, string formattedMessage, params object[] messageArgs)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Resume(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, () => String.Format(formattedMessage, messageArgs));
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="formattedMessage">a formatted message describing the event</param>
		/// <param name="messageArgs">arguments used when formatting the log message</param>
		/// <returns>the log event</returns>
		public static LogEvent Resume(this ILogSink sink, int appEventKind, string appEventName, string formattedMessage, params object[] messageArgs)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Resume(appEventKind, appEventName, () => String.Format(formattedMessage, messageArgs));
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Resume(this ILogSink sink, int appEventKind, string appEventName, string message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.ActivityTracing)) return null;

			var evt = new SimpleLogEvent(sink.Name, TraceEventType.Resume, appEventKind, appEventName, message, GetStackTrace(TraceEventType.Resume, sink.StackTraceThreshold));
			sink.Resume(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Resume(this ILogSink sink, Func<string> message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Resume(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, message);
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Resume(this ILogSink sink, int appEventKind, string appEventName, Func<string> message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.ActivityTracing)) return null;

			var evt = new LazyLogEvent(sink.Name, TraceEventType.Resume, appEventKind, appEventName, message, GetStackTrace(TraceEventType.Resume, sink.StackTraceThreshold));
			sink.Resume(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="data">an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Start<TData>(this ILogSink sink, TData data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Start(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, data);
		}

		/// <summary>
		/// Notifies the sink that a start activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="data">an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Start<TData>(this ILogSink sink, int appEventKind, string appEventName, TData data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.ActivityTracing)) return null;

			var evt = new DataLogEvent<TData>(sink.Name, TraceEventType.Start, appEventKind, appEventName, data, GetStackTrace(TraceEventType.Start, sink.StackTraceThreshold));
			sink.Start(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a start activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="data">function that resolves an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Start<TData>(this ILogSink sink, Func<TData> data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return Start(sink, LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, data);
		}
		/// <summary>
		/// Notifies the sink that a start activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="data">function that resolves an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Start<TData>(this ILogSink sink, int appEventKind, string appEventName, Func<TData> data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.ActivityTracing)) return null;

			var evt = new LazyDataLogEvent<TData>(sink.Name, TraceEventType.Start, appEventKind, appEventName, data, GetStackTrace(TraceEventType.Start, sink.StackTraceThreshold));
			sink.Error(evt);
			return evt;
		}
		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Start(this ILogSink sink, string message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Start(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, message);
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="formattedMessage">a formatted message describing the event</param>
		/// <param name="messageArgs">arguments used when formatting the log message</param>
		/// <returns>the log event</returns>
		public static LogEvent Start(this ILogSink sink, string formattedMessage, params object[] messageArgs)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Start(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, () => String.Format(formattedMessage, messageArgs));
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="formattedMessage">a formatted message describing the event</param>
		/// <param name="messageArgs">arguments used when formatting the log message</param>
		/// <returns>the log event</returns>
		public static LogEvent Start(this ILogSink sink, int appEventKind, string appEventName, string formattedMessage, params object[] messageArgs)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Start(appEventKind, appEventName, () => String.Format(formattedMessage, messageArgs));
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Start(this ILogSink sink, int appEventKind, string appEventName, string message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.ActivityTracing)) return null;

			var evt = new SimpleLogEvent(sink.Name, TraceEventType.Start, appEventKind, appEventName, message, GetStackTrace(TraceEventType.Start, sink.StackTraceThreshold));
			sink.Start(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="message">a function that produces a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Start(this ILogSink sink, Func<string> message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Start(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, message);
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="message">a function that produces a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Start(this ILogSink sink, int appEventKind, string appEventName, Func<string> message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.ActivityTracing)) return null;

			var evt = new LazyLogEvent(sink.Name, TraceEventType.Start, appEventKind, appEventName, message, GetStackTrace(TraceEventType.Start, sink.StackTraceThreshold));
			sink.Start(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="data">an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Stop<TData>(this ILogSink sink, TData data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Stop(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, data);
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="data">an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Stop<TData>(this ILogSink sink, int appEventKind, string appEventName, TData data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.ActivityTracing)) return null;

			var evt = new DataLogEvent<TData>(sink.Name, TraceEventType.Stop, appEventKind, appEventName, data, GetStackTrace(TraceEventType.Stop, sink.StackTraceThreshold));
			sink.Stop(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Stop(this ILogSink sink, string message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Stop(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, message);
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="formattedMessage">a formatted message describing the event</param>
		/// <param name="messageArgs">arguments used when formatting the log message</param>
		/// <returns>the log event</returns>
		public static LogEvent Stop(this ILogSink sink, string formattedMessage, params object[] messageArgs)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Stop(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, () => String.Format(formattedMessage, messageArgs));
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="formattedMessage">a formatted message describing the event</param>
		/// <param name="messageArgs">arguments used when formatting the log message</param>
		/// <returns>the log event</returns>
		public static LogEvent Stop(this ILogSink sink, int appEventKind, string appEventName, string formattedMessage, params object[] messageArgs)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Stop(appEventKind, appEventName, () => String.Format(formattedMessage, messageArgs));
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Stop(this ILogSink sink, int appEventKind, string appEventName, string message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.ActivityTracing)) return null;

			var evt = new SimpleLogEvent(sink.Name, TraceEventType.Stop, appEventKind, appEventName, message, GetStackTrace(TraceEventType.Stop, sink.StackTraceThreshold));
			sink.Stop(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="message">a function that produces a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Stop(this ILogSink sink, Func<string> message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Stop(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, message);
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="message">a function that produces a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Stop(this ILogSink sink, int appEventKind, string appEventName, Func<string> message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.ActivityTracing)) return null;

			var evt = new LazyLogEvent(sink.Name, TraceEventType.Stop, appEventKind, appEventName, message, GetStackTrace(TraceEventType.Stop, sink.StackTraceThreshold));
			sink.Stop(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="data">an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Suspend<TData>(this ILogSink sink, TData data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Suspend(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, data);
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="data">an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Suspend<TData>(this ILogSink sink, int appEventKind, string appEventName, TData data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.ActivityTracing)) return null;

			var evt = new DataLogEvent<TData>(sink.Name, TraceEventType.Suspend, appEventKind, appEventName, data, GetStackTrace(TraceEventType.Suspend, sink.StackTraceThreshold));
			sink.Suspend(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Suspend(this ILogSink sink, string message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Suspend(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, message);
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="formattedMessage">a formatted message describing the event</param>
		/// <param name="messageArgs">arguments used when formatting the log message</param>
		/// <returns>the log event</returns>
		public static LogEvent Suspend(this ILogSink sink, string formattedMessage, params object[] messageArgs)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Suspend(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, () => String.Format(formattedMessage, messageArgs));
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="formattedMessage">a formatted message describing the event</param>
		/// <param name="messageArgs">arguments used when formatting the log message</param>
		/// <returns>the log event</returns>
		public static LogEvent Suspend(this ILogSink sink, int appEventKind, string appEventName, string formattedMessage, params object[] messageArgs)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Suspend(appEventKind, appEventName, () => String.Format(formattedMessage, messageArgs));
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Suspend(this ILogSink sink, int appEventKind, string appEventName, string message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.ActivityTracing)) return null;

			var evt = new SimpleLogEvent(sink.Name, TraceEventType.Suspend, appEventKind, appEventName, message, GetStackTrace(TraceEventType.Suspend, sink.StackTraceThreshold));
			sink.Suspend(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="message">a function that produces a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Suspend(this ILogSink sink, Func<string> message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Suspend(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, message);
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="message">a function that produces a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Suspend(this ILogSink sink, int appEventKind, string appEventName, Func<string> message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.ActivityTracing)) return null;

			var evt = new LazyLogEvent(sink.Name, TraceEventType.Suspend, appEventKind, appEventName, message, GetStackTrace(TraceEventType.Suspend, sink.StackTraceThreshold));
			sink.Suspend(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="data">an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Transfer<TData>(this ILogSink sink, TData data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Transfer(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, data);
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="data">an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Transfer<TData>(this ILogSink sink, int appEventKind, string appEventName, TData data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.ActivityTracing)) return null;

			var evt = new DataLogEvent<TData>(sink.Name, TraceEventType.Transfer, appEventKind, appEventName, data, GetStackTrace(TraceEventType.Transfer, sink.StackTraceThreshold));
			sink.Transfer(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Transfer(this ILogSink sink, string message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Transfer(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, message);
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="formattedMessage">a formatted message describing the event</param>
		/// <param name="messageArgs">arguments used when formatting the log message</param>
		/// <returns>the log event</returns>
		public static LogEvent Transfer(this ILogSink sink, string formattedMessage, params object[] messageArgs)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Transfer(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, () => String.Format(formattedMessage, messageArgs));
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="formattedMessage">a formatted message describing the event</param>
		/// <param name="messageArgs">arguments used when formatting the log message</param>
		/// <returns>the log event</returns>
		public static LogEvent Transfer(this ILogSink sink, int appEventKind, string appEventName, string formattedMessage, params object[] messageArgs)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Transfer(appEventKind, appEventName, () => String.Format(formattedMessage, messageArgs));
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Transfer(this ILogSink sink, int appEventKind, string appEventName, string message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.ActivityTracing)) return null;

			var evt = new SimpleLogEvent(sink.Name, TraceEventType.Transfer, appEventKind, appEventName, message, GetStackTrace(TraceEventType.Transfer, sink.StackTraceThreshold));
			sink.Transfer(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="message">a function that produces a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Transfer(this ILogSink sink, Func<string> message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Transfer(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, message);
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="message">a function that produces a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Transfer(this ILogSink sink, int appEventKind, string appEventName, Func<string> message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.ActivityTracing)) return null;

			var evt = new LazyLogEvent(sink.Name, TraceEventType.Transfer, appEventKind, appEventName, message, GetStackTrace(TraceEventType.Transfer, sink.StackTraceThreshold));
			sink.Transfer(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a warning event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Warning(this ILogSink sink, string message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Warning(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, message);
		}

		/// <summary>
		/// Notifies the sink that a warning event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="data">an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Warning<TData>(this ILogSink sink, TData data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Warning(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, data);
		}

		/// <summary>
		/// Notifies the sink that a warning event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="data">an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Warning<TData>(this ILogSink sink, int appEventKind, string appEventName, TData data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.Warning)) return null;

			var evt = new DataLogEvent<TData>(sink.Name, TraceEventType.Warning, appEventKind, appEventName, data, GetStackTrace(TraceEventType.Warning, sink.StackTraceThreshold));
			sink.Warning(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a warning event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="data">function that resolves an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Warning<TData>(this ILogSink sink, Func<TData> data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return Warning(sink, LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, data);
		}

		/// <summary>
		/// Notifies the sink that a warning event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="data">function that resolves an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Warning<TData>(this ILogSink sink, int appEventKind, string appEventName, Func<TData> data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.Warning)) return null;

			var evt = new LazyDataLogEvent<TData>(sink.Name, TraceEventType.Warning, appEventKind, appEventName, data, GetStackTrace(TraceEventType.Warning, sink.StackTraceThreshold));
			sink.Error(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a warning event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="formattedMessage">a formatted message describing the event</param>
		/// <param name="messageArgs">arguments used when formatting the log message</param>
		/// <returns>the log event</returns>
		public static LogEvent Warning(this ILogSink sink, string formattedMessage, params object[] messageArgs)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Warning(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, () => String.Format(formattedMessage, messageArgs));
		}

		/// <summary>
		/// Notifies the sink that a warning event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="formattedMessage">a formatted message describing the event</param>
		/// <param name="messageArgs">arguments used when formatting the log message</param>
		/// <returns>the log event</returns>
		public static LogEvent Warning(this ILogSink sink, int appEventKind, string appEventName, string formattedMessage, params object[] messageArgs)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Warning(appEventKind, appEventName, () => String.Format(formattedMessage, messageArgs));
		}

		/// <summary>
		/// Notifies the sink that a warning event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Warning(this ILogSink sink, int appEventKind, string appEventName, string message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.Warning)) return null;

			var evt = new SimpleLogEvent(sink.Name, TraceEventType.Warning, appEventKind, appEventName, message, GetStackTrace(TraceEventType.Warning, sink.StackTraceThreshold));
			sink.Warning(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a warning event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="message">a function that produces a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Warning(this ILogSink sink, Func<string> message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Warning(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, message);
		}

		/// <summary>
		/// Notifies the sink that a warning event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="message">a function that produces a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Warning(this ILogSink sink, int appEventKind, string appEventName, Func<string> message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.Warning)) return null;

			var evt = new LazyLogEvent(sink.Name, TraceEventType.Warning, appEventKind, appEventName, message, GetStackTrace(TraceEventType.Warning, sink.StackTraceThreshold));
			sink.Warning(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a warning event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="data">an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Verbose<TData>(this ILogSink sink, TData data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Verbose(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, data);
		}

		/// <summary>
		/// Notifies the sink that a warning event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="data">an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Verbose<TData>(this ILogSink sink, int appEventKind, string appEventName, TData data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);
			
			if (!sink.Levels.HasFlag(SourceLevels.Verbose)) return null;

			var evt = new DataLogEvent<TData>(sink.Name, TraceEventType.Verbose, appEventKind, appEventName, data, GetStackTrace(TraceEventType.Verbose, sink.StackTraceThreshold));
			sink.Verbose(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a verbose event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="data">function that resolves an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Verbose<TData>(this ILogSink sink, Func<TData> data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return Verbose(sink, LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, data);
		}
		/// <summary>
		/// Notifies the sink that a Verbose event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="data">function that resolves an object describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Verbose<TData>(this ILogSink sink, int appEventKind, string appEventName, Func<TData> data)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.Verbose)) return null;

			var evt = new LazyDataLogEvent<TData>(sink.Name, TraceEventType.Verbose, appEventKind, appEventName, data, GetStackTrace(TraceEventType.Verbose, sink.StackTraceThreshold));
			sink.Verbose(evt);
			return evt;
		}
		/// <summary>
		/// Notifies the sink that a warning event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Verbose(this ILogSink sink, string message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Verbose(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, message);
		}

		/// <summary>
		/// Notifies the sink that a warning event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="formattedMessage">a formatted message describing the event</param>
		/// <param name="messageArgs">arguments used when formatting the log message</param>
		/// <returns>the log event</returns>
		public static LogEvent Verbose(this ILogSink sink, string formattedMessage, params object[] messageArgs)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Verbose(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, () => String.Format(formattedMessage, messageArgs));
		}

		/// <summary>
		/// Notifies the sink that a warning event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="formattedMessage">a formatted message describing the event</param>
		/// <param name="messageArgs">arguments used when formatting the log message</param>
		/// <returns>the log event</returns>
		public static LogEvent Verbose(this ILogSink sink, int appEventKind, string appEventName, string formattedMessage, params object[] messageArgs)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Verbose(appEventKind, appEventName, () => String.Format(formattedMessage, messageArgs));
		}

		/// <summary>
		/// Notifies the sink that a warning event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="message">a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Verbose(this ILogSink sink, int appEventKind, string appEventName, string message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.Verbose)) return null;

			var evt = new SimpleLogEvent(sink.Name, TraceEventType.Verbose, appEventKind, appEventName, message, GetStackTrace(TraceEventType.Verbose, sink.StackTraceThreshold));
			sink.Verbose(evt);
			return evt;
		}

		/// <summary>
		/// Notifies the sink that a warning event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="message">a function that produces a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Verbose(this ILogSink sink, Func<string> message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			return sink.Verbose(LogSink.DefaultApplicationSpecificLogEventKind, LogSink.DefaultApplicationSpecificLogEventName, message);
		}

		/// <summary>
		/// Notifies the sink that a warning event occurred.
		/// </summary>
		/// <param name="sink">the target sink</param>
		/// <param name="appEventKind">application specific event kind</param>
		/// <param name="appEventName">application specific event name</param>
		/// <param name="message">a function that produces a message describing the event</param>
		/// <returns>the log event</returns>
		public static LogEvent Verbose(this ILogSink sink, int appEventKind, string appEventName, Func<string> message)
		{
			Contract.Requires<ArgumentNullException>(sink != null, Resources.Chk_CannotBeNull);

			if (!sink.Levels.HasFlag(SourceLevels.Verbose)) return null;

			var evt = new LazyLogEvent(sink.Name, TraceEventType.Verbose, appEventKind, appEventName, message, GetStackTrace(TraceEventType.Verbose, sink.StackTraceThreshold));
			sink.Verbose(evt);
			return evt;
		}
	}

}
