using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace NetSteps.Diagnostics.Utilities
{
	public class DeferredTraceEvent : TraceEvent
	{
		#region Fields

		private readonly Action<StringWriter> Writer;

		#endregion

		#region Properties

		#endregion

		#region Construction

		public DeferredTraceEvent(TraceEventType type, int id, Action<StringWriter> writer)
			: base(type, id)
		{
			if (writer == null) throw new ArgumentNullException("writer");
			Writer = writer;
		}

		public DeferredTraceEvent(TraceEventType type, Action<StringWriter> writer)
			: base(type)
		{
			if (writer == null) throw new ArgumentNullException("writer");
			Writer = writer;
		}

		#endregion

		#region Methods

		public override string ToString()
		{
			string result = base.ToString();
			if (Writer != null)
			{
				StringBuilder body = new StringBuilder();
				using (StringWriter writer = new StringWriter(body)) Writer(writer);
				result = body.ToString();
			}
			return result;
		}

		#endregion
	}
}
