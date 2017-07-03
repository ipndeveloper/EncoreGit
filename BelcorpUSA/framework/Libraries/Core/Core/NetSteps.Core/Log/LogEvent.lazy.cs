
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Security;
using System.Threading;
using NetSteps.Encore.Core.Properties;

namespace NetSteps.Encore.Core.Log
{
	/// <summary>
	/// Implementation that lazily generates the log message. (when/if it is actually written to a log sink)
	/// </summary>
	[Serializable]
	public class LazyLogEvent : LogEvent
	{
		static readonly Func<string> NullMessage = () => { return String.Empty; };
		readonly Lazy<string> _message;

		/// <summary>
		/// Creates a new log event.
		/// </summary>
		/// <param name="source">the source of the event</param>
		/// <param name="eventType">the event type</param>
		/// <param name="appKind">an application specific event kind</param>
		/// <param name="appKindName">an application specific event name</param>
		/// <param name="message">the log message producer</param>
		/// <param name="stackTrace">a stack trace associated with the event</param>
		public LazyLogEvent(String source, TraceEventType eventType, int appKind, string appKindName, Func<string> message, StackTrace stackTrace)
			: base(source, eventType, appKind, appKindName, stackTrace)
		{
			Contract.Requires<ArgumentNullException>(source != null, Resources.Chk_CannotBeNull);
			
			_message = (message != null) ? new Lazy<string>(message) : new Lazy<string>(NullMessage);
		}
		/// <summary>
		/// Gets the event's message.
		/// </summary>
		public override string Message
		{
			get	{ return _message.Value; }
		}
	}
}
