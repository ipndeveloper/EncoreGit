
using System.Diagnostics;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.Properties;

namespace NetSteps.Encore.Core.Log
{
	/// <summary>
	/// Echos log events to a System.Diagnostics trace source.
	/// </summary>
	public class TraceLogEventWriter : LogEventWriter
	{
		TraceSource _source;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public TraceLogEventWriter()
		{
		}

		/// <summary>
		/// Writes the log event to a trace source.
		/// </summary>
		/// <param name="evt">the log event</param>
		public override void WriteLogEvent(LogEvent evt)
		{
			Contract.Assert(_source != null, Resources.Err_NotInitialized);
			
			_source.TraceData(evt.EventType, evt.Kind, evt.ToString());
		}

		/// <summary>
		/// Initializes the trace source.
		/// </summary>
		/// <param name="sourceName">the trace source's name</param>
		public override void Initialize(string sourceName)
		{
			Contract.Assert(sourceName != null, Resources.Chk_CannotBeNull);

			_source = new TraceSource(sourceName);
		}
	}
}
