using NetSteps.Diagnostics.Utilities.Configuration;
using System;
using System.Diagnostics;
using System.Linq;

namespace NetSteps.Diagnostics.Utilities.Listeners
{
	public class CustomEventLogTraceListener : TraceListener
	{
		#region Fields

		private readonly bool Installed;
		private static readonly object Lock = new object();
		public static readonly int MaximumMessageSize = 31839;
		public static readonly int MaximumParts = 256;

		#endregion

		#region Properties

		private EventLog _log;
		protected EventLog Log
		{
			get
			{
				if (_log == null)
				{
					lock (Lock)
					{
						if (_log == null)
						{
							_log = new EventLog(CustomEventLogTraceListenerInstaller.Name);
							_log.Source = CustomEventLogTraceListenerInstaller.Source;
						}
					}
				}
				return _log;
			}
		}

		#endregion

		#region Construction

		public CustomEventLogTraceListener()
		{
			Installed = EventLog.GetEventLogs().Any(p => p.Log == CustomEventLogTraceListenerInstaller.Name);
			if (!Installed) Trace.TraceWarning(String.Format("Unable to log messages using '{0}' the writer is missing it's associated event log. All unfiltered trace events if any will be discarded.", GetType().FullName));
		}

		#endregion

		#region Methods

		private EventLogEntryType Map(TraceEventType eventType)
		{
			EventLogEntryType result = EventLogEntryType.Information;
			switch (eventType)
			{
				case TraceEventType.Warning:
					result = EventLogEntryType.Warning;
					break;
				case TraceEventType.Critical:
				case TraceEventType.Error:
					result = EventLogEntryType.Error;
					break;
			}
			return result;
		}

		protected virtual void WriteEntry(string message, TraceEventType eventType, int id)
		{
			if (Installed)
			{
				try
				{
					if (message.Length > MaximumMessageSize)
					{
						string notice = "This consists of multiple parts see prior or subsequent messages for additional detail\r\n";
						int maximum = MaximumMessageSize - (notice.Length + 128);
						Debug.Assert(maximum > 0, "MaximumMessageSize must be greater than zero and larger than notice message with 128 padding for part header");
						int parts = message.Length / maximum;
						int part = 1;
						if (parts < MaximumParts)
							while (message.Length > 0)
							{
								if (message.Length < maximum) maximum = message.Length;
								string chunk = message.Substring(0, maximum);
								int length = chunk.Length;
								chunk = String.Concat(notice, String.Format("Message {0} of {1}\r\n", part, parts), chunk);
								Log.WriteEntry(chunk, Map(eventType), id);
								message = message.Substring(length);
								part++;
								if (part > MaximumParts)
								{
									Log.WriteEntry(String.Format("{0} message contains too many parts remaining parts will be discarded.", notice), EventLogEntryType.Warning);
									Debug.Assert(false, "Really?");
									break;
								}
							}
					}
					else Log.WriteEntry(message, Map(eventType), id, 0);
				}
				//Don't kill the application if there is an error logging data
				catch
				{
#if DEBUG
					throw;
#endif
				}
			}
		}

		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
			if (Filter == null || Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null)) WriteEntry(Convert.ToString(data), eventType, id);
		}

		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
		{
			if (Filter == null || Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, data)) WriteEntry(String.Join(String.Empty, data.Select(s => Convert.ToString(s))), eventType, id);
		}

		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
		{
			if (Filter == null || Filter.ShouldTrace(eventCache, source, eventType, id, format, args, null, null)) WriteEntry(String.Format(format, args), eventType, id);
		}

		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
		{
			if (Filter == null || Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null)) WriteEntry(message, eventType, id);
		}

		public override void Write(string message)
		{

			WriteEntry(message, TraceEventType.Information, 0);
		}

		public override void WriteLine(string message)
		{
			Write(String.Concat(message, Environment.NewLine));
		}


		public override void Close()
		{
			if (_log != null)
			{
				lock (Lock)
				{
					if (_log != null)
					{
						_log.Dispose();
						_log = null;
					}
				}

			}
		}
		#endregion
	}
}
