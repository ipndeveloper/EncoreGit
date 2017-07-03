using System;

namespace NetSteps.Encore.Core.Log
{
	/// <summary>
	/// Manages log sinks.
	/// </summary>
	public interface ILogSinkManager
	{
		/// <summary>
		/// Gets the default log sink.
		/// </summary>
		ILogSink DefaultLogSink { get; }

		/// <summary>
		/// Gets the currently configured log sink for the given type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		ILogSink GetLogSinkForType(Type type);		
	}
}
