using System;
using System.Diagnostics;
using System.Text;

namespace NetSteps.Diagnostics.Utilities
{
	public class ExceptionTraceEvent
		: TraceEvent
	{

		#region Fields

		private readonly Exception Exception;
		private StringBuilder Message;

		#endregion

		#region Properties



		#endregion

		#region Construction

		#endregion

		#region Methods

		public ExceptionTraceEvent(TraceEventType type, int id, Exception exception)
			: base(type,id)
		{
			Exception = exception;
		}

		public ExceptionTraceEvent(TraceEventType type, Exception exception)
			: base(type)
		{
			Exception = exception;
		}

		public ExceptionTraceEvent(int id, Exception exception)
			: this(TraceEventType.Error, id, exception)
		{ }

		public ExceptionTraceEvent(Exception exception)
			: this(TraceEventType.Error, exception)
		{ }

		#endregion

		#region Methods

		public static implicit operator ExceptionTraceEvent(Exception instance)
		{
			return new ExceptionTraceEvent(instance);
		}

		public override string ToString()
		{
			if (Message == null)
			{
				Message = new StringBuilder();
				Exception exception = Exception;
				int depth = 0;
				do
				{
					Message.AppendFormat("[{0}] ", depth).Append(exception).AppendLine();
					exception = exception.InnerException;
				}
				while (exception != null);
			}
			return Message.ToString();
		}

		#endregion
	}
}
