using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetSteps.Diagnostics.Utilities
{
	
	public class TraceActivity : IDisposable
	{
		#region Fields

		private readonly Stopwatch Stopwatch;
		private readonly List<TraceEventWait> Events;

		#endregion

		#region Properties

		public Type Type
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			private set;
		}

		public TimeSpan Elapsed
		{
			get
			{
				return Stopwatch.Elapsed;
			}
		}

		#endregion

		#region Construction

		
		internal TraceActivity(Type type, string name)
		{
			Stopwatch = new Stopwatch();
			Events = new List<TraceEventWait>();
			Type = type;
			Name = name;
			Stopwatch.Start();
			Start();
		}

		#endregion

		#region Methods

		public TraceActivity After(TimeSpan duration, TraceEvent value)
		{
			if (!Stopwatch.IsRunning) throw new InvalidOperationException("Activity has already stopped operation is not allowed");
			Events.Add(new TraceEventWait(duration, value));
			return this;
		}

		protected virtual void Start()
		{
			Tracer.Event(Type, new TraceEvent(TraceEventType.Start, String.Format("Started {0}", Name)));
		}

		protected virtual void Stop()
		{
			TraceEventWait wait = Events.FirstOrDefault(p => Elapsed > p.Duration);
			if (wait != null) Tracer.Event(Type, wait.Event);
			Tracer.Event(Type, new TraceEvent(TraceEventType.Stop, String.Format("Stopped {0} at {1}", Name, Elapsed)));
		}

		#endregion

		#region IDisposable Members

		void IDisposable.Dispose()
		{
			try
			{
				Stopwatch.Stop();
				Stop();
			}
			catch
			{
				//Do not re-throw exception during garbage collection
			};
		}

		#endregion

		private class TraceEventWait
		{
			public readonly TimeSpan Duration;
			public readonly TraceEvent Event;
			public TraceEventWait(TimeSpan duration, TraceEvent @event)
			{
				Duration = duration;
				Event = @event;
			}
		}
	}
}
