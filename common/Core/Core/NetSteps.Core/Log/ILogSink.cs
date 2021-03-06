﻿using System;
using System.Diagnostics;
namespace NetSteps.Encore.Core.Log
{
	/// <summary>
	/// Interface for log sinks.
	/// </summary>
	/// <remarks>
	/// Log sinks take log events generated by the framework and does something
	/// useful with them, such as put them in a database or write them to another
	/// logging service.
	/// </remarks>
	public interface ILogSink
	{
		/// <summary>
		/// The log sink's name.
		/// </summary>
		string Name { get; }
		/// <summary>
		/// The next sink in the sink chain; possibly null.
		/// </summary>
		ILogSink NextSink { get; }
		/// <summary>
		/// The log event writer associated with the sink.
		/// </summary>
		LogEventWriter Writer { get; }
		/// <summary>
		/// The sink's source levels. Determines what actually gets logged.
		/// </summary>
		SourceLevels Levels { get; }
		/// <summary>
		/// The sink's stacktrace threshold. Determines at which level 
		/// a stacktrace is generated.
		/// </summary>
		TraceEventType StackTraceThreshold { get; }

		/// <summary>
		/// Indicates whether the log sink is forwarding messages
		/// at the source level given.
		/// </summary>
		/// <param name="level">the source level to check</param>
		/// <returns>true if forwarding; otherwise false</returns>
		bool IsLogging(SourceLevels level);

		/// <summary>
		/// Notifies the sink that a critical event occurred.
		/// </summary>
		/// <param name="evt">event details</param>
		void Critical(LogEvent evt);
		/// <summary>
		/// Notifies the sink that a error event occurred.
		/// </summary>
		/// <param name="evt">event details</param>
		void Error(LogEvent evt);
		/// <summary>
		/// Notifies the sink that an informational event occurred.
		/// </summary>
		/// <param name="evt">event details</param>
		void Information(LogEvent evt);
		/// <summary>
		/// Notifies the sink that a resume activity occurred.
		/// </summary>
		/// <param name="evt">event details</param>
		void Resume(LogEvent evt);
		/// <summary>
		/// Notifies the sink that a start activity occurred.
		/// </summary>
		/// <param name="evt">event details</param>
		void Start(LogEvent evt);
		/// <summary>
		/// Notifies the sink that a start activity occurred.
		/// </summary>
		/// <param name="evt">event details</param>
		void Stop(LogEvent evt);
		/// <summary>
		/// Notifies the sink that a suspend activity occurred.
		/// </summary>
		/// <param name="evt">event details</param>
		void Suspend(LogEvent evt);
		/// <summary>
		/// Notifies the sink that a transfer activity occurred.
		/// </summary>
		/// <param name="evt">event details</param>
		void Transfer(LogEvent evt);
		/// <summary>
		/// Notifies the sink that a verbose event occurred.
		/// </summary>
		/// <param name="evt">event details</param>
		void Verbose(LogEvent evt);
		/// <summary>
		/// Notifies the sink that a warning event occurred.
		/// </summary>
		/// <param name="evt">event details</param>
		void Warning(LogEvent evt);

	}
}
