
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Security;
using System.Threading;
using NetSteps.Encore.Core.Properties;

namespace NetSteps.Encore.Core.Log
{
	/// <summary>
	/// Simple implementation of log event taking string messages.
	/// </summary>
	[Serializable]
	public sealed class SimpleLogEvent : LogEvent
	{
		readonly string _message;

		/// <summary>
		/// Creates a new log event.
		/// </summary>
		/// <param name="source">the source of the event</param>
		/// <param name="eventType">the event type</param>
		/// <param name="appKind">an application specific event kind</param>
		/// <param name="appKindName">an application specific event name</param>
		/// <param name="message">the log message</param>
		/// <param name="stackTrace">a stack trace associated with the event</param>
		public SimpleLogEvent(String source, TraceEventType eventType, int appKind, string appKindName, string message, StackTrace stackTrace)
			: base(source, eventType, appKind, appKindName, stackTrace)
		{
			Contract.Requires<ArgumentNullException>(source != null, Resources.Chk_CannotBeNull);
			_message = message ?? String.Empty;
		}
		/// <summary>
		/// Gets the event's message.
		/// </summary>
		public override string Message
		{
			get { return _message; }
		}
	}	
}
