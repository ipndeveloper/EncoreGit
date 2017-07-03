
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Security;
using System.Threading;
using NetSteps.Encore.Core.Properties;

namespace NetSteps.Encore.Core.Log
{
	/// <summary>
	/// Log message for data.
	/// </summary>
	/// <typeparam name="TData">data type TData</typeparam>
	[Serializable]
	public sealed class DataLogEvent<TData> : LazyLogEvent
	{
		/// <summary>
		/// Creates a new log event.
		/// </summary>
		/// <param name="source">the source of the event</param>
		/// <param name="eventType">the event type</param>
		/// <param name="appKind">an application specific event kind</param>
		/// <param name="appKindName">an application specific event name</param>
		/// <param name="data">data to be transformed for data</param>
		/// <param name="stackTrace">a stack trace associated with the event</param>
		public DataLogEvent(String source, TraceEventType eventType, int appKind, string appKindName, TData data, StackTrace stackTrace)
			: base(source, eventType, appKind, appKindName, () => LogDataTransform<TData>.Transform(data), stackTrace)
		{
			Contract.Requires<ArgumentNullException>(source != null, Resources.Chk_CannotBeNull);
		}
	}

	/// <summary>
	/// Log message for data.
	/// </summary>
	/// <typeparam name="TData">data type TData</typeparam>
	[Serializable]
	public sealed class LazyDataLogEvent<TData> : LazyLogEvent
	{
		/// <summary>
		/// Creates a new log event.
		/// </summary>
		/// <param name="source">the source of the event</param>
		/// <param name="eventType">the event type</param>
		/// <param name="appKind">an application specific event kind</param>
		/// <param name="appKindName">an application specific event name</param>
		/// <param name="data">data to be transformed for data</param>
		/// <param name="stackTrace">a stack trace associated with the event</param>
		public LazyDataLogEvent(String source, TraceEventType eventType, int appKind, string appKindName, Func<TData> data, StackTrace stackTrace)
			: base(source, eventType, appKind, appKindName, () => LogDataTransform<TData>.Transform(data()), stackTrace)
		{
			Contract.Requires<ArgumentNullException>(source != null, Resources.Chk_CannotBeNull);
		}
	}
}
