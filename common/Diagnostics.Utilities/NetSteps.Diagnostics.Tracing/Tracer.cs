using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetSteps.Diagnostics.Utilities
{
	public static class Tracer
	{
		#region Fields

		private static readonly ConcurrentDictionary<string, TraceSource[]> Sources;

		#endregion

		#region Properties

		#endregion

		#region Construction

		static Tracer()
		{
			Sources = new ConcurrentDictionary<string, TraceSource[]>();
#if DEBUG			
			Trace.Listeners.Add(new DefaultTraceListener());
#endif
		}

		#endregion

		#region Methods

		public static IEnumerable<TraceSource> Resolve(Type type)
		{
			return Sources.GetOrAdd(type.Assembly.GetName().Name, (key) =>
			{
				string[] path = key.Split('.');
				List<TraceSource> context = new List<TraceSource>();
				for (int index = 0; index < path.Length; index++)
				{
					TraceSource[] value;
					string segment = String.Join(".", path.Take(index + 1));
					if (Sources.TryGetValue(segment, out value)) context.Add(value.Last());
					else
					{
						context.Add(new TraceSource(segment));
						Sources.TryAdd(segment, context.ToArray());
					}
				}
				return context.ToArray();
			});
		}

		public static TraceSource Resolve(Type type, TraceEventType eventType)
		{
			return Resolve(type).Where(p => p.Switch != null && p.Switch.ShouldTrace(eventType)).FirstOrDefault();
		}

		public static void Event(Type type, TraceEvent value)
		{
			if (value == null) throw new ArgumentNullException("value");
			var source = Resolve(type, value.Type);
			if (source != null) source.TraceEvent(value.Type, value.Id, value.ToString());
		}

		public static TraceActivity Activity(Type type, string name)
		{
			if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
			return new TraceActivity(type,name);
		}

		#endregion
	}
}
