using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;
using NetSteps.Encore.Core.Properties;
using NetSteps.Encore.Core.Parallel;

namespace NetSteps.Encore.Core.Log
{
	/// <summary>
	/// Default implementaton of the LogSink interface.
	/// </summary>
	public sealed class LogSink : ILogSinkManagement
	{
		/// <summary>
		/// Default value used for ApplicationSpecificLogEventKind when none is given.
		/// </summary>
		public static readonly int DefaultApplicationSpecificLogEventKind = 0;

		/// <summary>
		/// Default value used for ApplicationSpecificLogEventName when none is given.
		/// </summary>
		public static readonly string DefaultApplicationSpecificLogEventName = String.Empty;

		readonly ILogSinkGhostWriter _ghostWriter;
		readonly string _name;
		SourceLevels _levels;
		TraceEventType _stackTraceThreshold;
		TraceEventType _traceThreshold;
		ILogSink _next;
		LogEventWriter _writer;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="name">the log sink's name</param>
		/// <param name="levels">a source level</param>
		/// <param name="stackTraceThreshold">the stack threshold</param>
		/// <param name="writer">an event writer</param>
		/// <param name="next">the next sink in the chain or null</param>
		/// <param name="ghostWriter"></param>
		public LogSink(ILogSinkGhostWriter ghostWriter, string name, SourceLevels levels, TraceEventType stackTraceThreshold, LogEventWriter writer, ILogSink next)
		{
			Contract.Requires<ArgumentNullException>(ghostWriter != null, Resources.Chk_CannotBeNull);
			Contract.Requires<ArgumentNullException>(name != null, Resources.Chk_CannotBeNull);
			Contract.Requires<ArgumentException>(name.Length > 0, Resources.Chk_CannotBeEmpty);
			Contract.Requires<ArgumentNullException>(writer != null, Resources.Chk_CannotBeEmpty);

			_ghostWriter = ghostWriter;
			_name = name;
			_levels = levels;
			_traceThreshold = TranslateSourceLevelToTraceThreshold(levels);
			_stackTraceThreshold = stackTraceThreshold;
			_next = next;
			_writer = writer;
		}

		private static TraceEventType TranslateSourceLevelToTraceThreshold(SourceLevels levels)
		{
			switch (levels)
			{
				case SourceLevels.ActivityTracing:
				case SourceLevels.All:
					return TraceEventType.Start;
				case SourceLevels.Critical:
					return TraceEventType.Critical;
				case SourceLevels.Error:
					return TraceEventType.Error;
				case SourceLevels.Information:
					return TraceEventType.Information;
				case SourceLevels.Off:
					return (TraceEventType)0;
				case SourceLevels.Verbose:
					return TraceEventType.Verbose;
				case SourceLevels.Warning:
					return TraceEventType.Warning;
				default:
					return default(TraceEventType);
			}
		}

		/// <summary>
		/// The sink's name.
		/// </summary>
		public string Name { get { return _name; } }
		/// <summary>
		/// The sink's source level.
		/// </summary>
		public SourceLevels Levels
		{
			get
			{
				Thread.MemoryBarrier();
				var value = _levels;
				Thread.MemoryBarrier();
				return value;
			}
		}
		/// <summary>
		/// The next sink in the chain.
		/// </summary>
		public ILogSink NextSink
		{
			get
			{
				Thread.MemoryBarrier();
				var value = _next;
				Thread.MemoryBarrier();
				return value;
			}
		}

		/// <summary>
		/// The sink's stack trace threshold.
		/// </summary>
		public TraceEventType StackTraceThreshold
		{
			get
			{
				Thread.MemoryBarrier();
				var value = _stackTraceThreshold;
				Thread.MemoryBarrier();
				return value;
			}
		}

		private TraceEventType TraceThreshold
		{
			get
			{
				Thread.MemoryBarrier();
				var value = _traceThreshold;
				Thread.MemoryBarrier();
				return value;
			}
		}

		/// <summary>
		/// The sink's event writer.
		/// </summary>
		public LogEventWriter Writer
		{
			get
			{
				Thread.MemoryBarrier();
				var value = _writer;
				Thread.MemoryBarrier();
				return value;
			}
		}


		/// <summary>
		/// Indicates whether the log sink is forwarding messages
		/// at the source level given.
		/// </summary>
		/// <param name="level">the source level to check</param>
		/// <returns>true if forwarding; otherwise false</returns>
		public bool IsLogging(SourceLevels level)
		{
			return Levels.HasFlag(level);
		}

		void ILogSinkManagement.Reconfigure(SourceLevels level, TraceEventType stackTraceThreshold, LogEventWriter writer, ILogSink next)
		{
			Contract.Assert(writer != null, Resources.Chk_CannotBeNull);

			_levels = level;
			_traceThreshold = TranslateSourceLevelToTraceThreshold(level);
			_stackTraceThreshold = stackTraceThreshold;
			_next = next;
			_writer = writer;
		}

		/// <summary>
		/// Notifies the sink that a critical event occurred.
		/// </summary>
		/// <param name="evt">event details</param>
		public void Critical(LogEvent evt)
		{
			Contract.Assert(evt != null, Resources.Chk_CannotBeNull);

			if (evt.EventType >= TraceThreshold) { _ghostWriter.GhostWrite(Writer, evt); }
			var next = NextSink;
			if (next != null)
			{
				next.Critical(evt);
			}
		}

		/// <summary>
		/// Notifies the sink that a error event occurred.
		/// </summary>
		/// <param name="evt">event details</param>
		public void Error(LogEvent evt)
		{
			Contract.Assert(evt != null, Resources.Chk_CannotBeNull);

			if (evt.EventType >= TraceThreshold) { _ghostWriter.GhostWrite(Writer, evt); }
			var next = NextSink;
			if (next != null)
			{
				next.Error(evt);
			}
		}

		/// <summary>
		/// Notifies the sink that a informational event occurred.
		/// </summary>
		/// <param name="evt">event details</param>
		public void Information(LogEvent evt)
		{
			Contract.Assert(evt != null, Resources.Chk_CannotBeNull);

			if (evt.EventType >= TraceThreshold) { _ghostWriter.GhostWrite(Writer, evt); }
			var next = NextSink;
			if (next != null)
			{
				next.Information(evt);
			}
		}

		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="evt">event details</param>
		public void Resume(LogEvent evt)
		{
			Contract.Assert(evt != null, Resources.Chk_CannotBeNull);

			if (evt.EventType >= TraceThreshold) { _ghostWriter.GhostWrite(Writer, evt); }
			var next = NextSink;
			if (next != null)
			{
				next.Resume(evt);
			}
		}

		/// <summary>
		/// Notifies the sink that a start activity occurred.
		/// </summary>
		/// <param name="evt">event details</param>
		public void Start(LogEvent evt)
		{
			Contract.Assert(evt != null, Resources.Chk_CannotBeNull);

			if (evt.EventType >= TraceThreshold) { _ghostWriter.GhostWrite(Writer, evt); }
			var next = NextSink;
			if (next != null)
			{
				next.Start(evt);
			}
		}

		/// <summary>
		/// Notifies the sink that a stop activity occurred.
		/// </summary>
		/// <param name="evt">event details</param>
		public void Stop(LogEvent evt)
		{
			Contract.Assert(evt != null, Resources.Chk_CannotBeNull);

			if (evt.EventType >= TraceThreshold) { _ghostWriter.GhostWrite(Writer, evt); }
			var next = NextSink;
			if (next != null)
			{
				next.Stop(evt);
			}
		}

		/// <summary>
		/// Notifies the sink that a suspend activity occurred.
		/// </summary>
		/// <param name="evt">event details</param>
		public void Suspend(LogEvent evt)
		{
			Contract.Assert(evt != null, Resources.Chk_CannotBeNull);

			if (evt.EventType >= TraceThreshold) { _ghostWriter.GhostWrite(Writer, evt); }
			var next = NextSink;
			if (next != null)
			{
				next.Suspend(evt);
			}
		}

		/// <summary>
		/// Notifies the sink that a transfer activity occurred.
		/// </summary>
		/// <param name="evt">event details</param>
		public void Transfer(LogEvent evt)
		{
			Contract.Assert(evt != null, Resources.Chk_CannotBeNull);

			if (evt.EventType >= TraceThreshold) { _ghostWriter.GhostWrite(Writer, evt); }
			var next = NextSink;
			if (next != null)
			{
				next.Transfer(evt);
			}
		}

		/// <summary>
		/// Notifies the sink that a warning event occurred.
		/// </summary>
		/// <param name="evt">event details</param>
		public void Warning(LogEvent evt)
		{
			Contract.Assert(evt != null, Resources.Chk_CannotBeNull);

			if (evt.EventType >= TraceThreshold) { _ghostWriter.GhostWrite(Writer, evt); }
			var next = NextSink;
			if (next != null)
			{
				next.Warning(evt);
			}
		}

		/// <summary>
		/// Notifies the sink that a verbose event occurred.
		/// </summary>
		/// <param name="evt">event details</param>
		public void Verbose(LogEvent evt)
		{
			Contract.Assert(evt != null, Resources.Chk_CannotBeNull);

			if (evt.EventType >= TraceThreshold) { _ghostWriter.GhostWrite(Writer, evt); }
			var next = NextSink;
			if (next != null)
			{
				next.Verbose(evt);
			}
		}

		[ContractInvariantMethod]
		void InvariantContracts()
		{
			Contract.Invariant(_writer != null, Resources.Chk_CannotBeNull);
		}
	}

}
