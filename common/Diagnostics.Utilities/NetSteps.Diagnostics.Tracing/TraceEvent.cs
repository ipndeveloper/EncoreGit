using System.Diagnostics;

namespace NetSteps.Diagnostics.Utilities
{
	public class TraceEvent
	{
		#region Fields

		private readonly string Message;

		#endregion

		#region Properties

		public TraceEventType Type
		{
			get;
			protected internal set;
		}

		public int Id { get; private set; }

		#endregion

		#region Construction

		private TraceEvent(string message)
		{
			Message = message;
			Type = TraceEventType.Verbose;
			Id = 0;
		}

		protected TraceEvent(TraceEventType type, int id)
		{
			Type = type;
			Id = id;
		}

		protected TraceEvent(TraceEventType type)
			: this(type, 0)
		{ }

		protected TraceEvent()
			: this(TraceEventType.Verbose, 0)
		{ }

		public TraceEvent(TraceEventType type, int id, string message)
			: this(type, id)
		{
			Message = message;
		}

		public TraceEvent(TraceEventType type, string message)
			: this(type, 0, message)
		{ }

		#endregion

		#region Methods

		public static implicit operator TraceEvent(string instance)
		{
			return new TraceEvent(instance);
		}

		public override string ToString()
		{
			return Message;
		}

		#endregion
	}
}
