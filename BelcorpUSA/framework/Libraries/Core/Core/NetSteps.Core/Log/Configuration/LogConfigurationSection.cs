
using System;
using System.Configuration;
using System.Diagnostics;

namespace NetSteps.Encore.Core.Log
{
	/// <summary>
	/// Configuration section for log settings.
	/// </summary>
	public sealed class LogConfigurationSection : ConfigurationSection
	{
		/// <summary>
		/// Configuration section name for trace settings.
		/// </summary>
		public static readonly string SectionName = "netsteps.log";

		const string CUnknownValue = "unknown";

		/// <summary>
		/// Default source level name.
		/// </summary>
		public const SourceLevels CDefaultSourceLevel = SourceLevels.Warning;

		/// <summary>
		/// Default stack trace threshold
		/// </summary>
		public const TraceEventType CDefaultStackTraceThreshold = TraceEventType.Warning;

		/// <summary>
		/// Default parallel disptach threshold
		/// </summary>
		public const int CDefaultParallelDispatchThreshold = 10000;

		/// <summary>
		/// Property name for namespaces
		/// </summary>
		public const string PropertyName_namespaces = "namespaces";

		/// <summary>
		/// Property name for defaultWriter.
		/// </summary>
		public const string PropertyName_defaultWriterType = "defaultWriterType";

		/// <summary>
		/// Property name for defaultSourceLevel.
		/// </summary>
		public const string PropertyName_defaultSourceLevel = "defaultSourceLevel";

		/// <summary>
		/// Property name for defaultStackTraceThreshold.
		/// </summary>
		public const string PropertyName_defaultStackTraceThreshold = "defaultStackTraceThreshold";

		/// <summary>
		/// Property name for parallelDispatchThreshold.
		/// </summary>
		public const string PropertyName_parallelDispatchThreshold = "parallelDispatchThreshold";
				
		/// <summary>
		/// The default source levels.
		/// </summary>
		/// <seealso cref="System.Diagnostics.SourceLevels"/>
		[ConfigurationProperty(PropertyName_defaultSourceLevel, DefaultValue = CDefaultSourceLevel)]
		public SourceLevels DefaultSourceLevel
		{
			get { return (SourceLevels)this[PropertyName_defaultSourceLevel]; }
			set { this[PropertyName_defaultSourceLevel] = value; }
		}

		/// <summary>
		/// The default stack trace threshold
		/// </summary>
		/// <seealso cref="System.Diagnostics.TraceEventType"/>
		[ConfigurationProperty(PropertyName_defaultStackTraceThreshold, DefaultValue = CDefaultStackTraceThreshold)]
		public TraceEventType DefaultStackTraceThreshold
		{
			get { return (TraceEventType)this[PropertyName_defaultStackTraceThreshold]; }
			set { this[PropertyName_defaultStackTraceThreshold] = value; }
		}

		/// <summary>
		/// The log sink manager's parallel dispatch threshold.
		/// </summary>
		[ConfigurationProperty(PropertyName_parallelDispatchThreshold, DefaultValue = CDefaultParallelDispatchThreshold)]
		public int ParallelDispatchThreshold
		{
			get { return (int)this[PropertyName_parallelDispatchThreshold]; }
			set { this[PropertyName_parallelDispatchThreshold] = value; }
		}
				
		/// <summary>
		/// The default LogEventWriter
		/// </summary>
		[ConfigurationProperty(PropertyName_defaultWriterType)]
		public string DefaultWriterType
		{
			get { return (string)this[PropertyName_defaultWriterType]; }
			set { this[PropertyName_defaultWriterType] = value; }
		}

		/// <summary>
		/// Gets the confgured namespace elements.
		/// </summary>
		[ConfigurationProperty(PropertyName_namespaces, IsDefaultCollection = true)]
		public NamespaceElementCollection Namespaces
		{
			get { return (NamespaceElementCollection)this[PropertyName_namespaces]; }
		}
				
		/// <summary>
		/// Gets the current configuration section.
		/// </summary>
		public static LogConfigurationSection Current
		{
			get
			{
				LogConfigurationSection config = ConfigurationManager.GetSection(
					LogConfigurationSection.SectionName) as LogConfigurationSection;
				return config ?? new LogConfigurationSection();
			}
		}

        static LogEventWriter __defaultSink;
        internal LogEventWriter ResolvedDefaultLogWriter
		{
			get
			{
				return Util.NonBlockingLazyInitializeVolatile(ref __defaultSink, () =>
				{
					var writer = (LogEventWriter)Activator.CreateInstance(ResolvedDefaultWriterType);
					writer.Initialize("default");
                    return writer;
				});
			}
		}

		Type _writerType;
		internal Type ResolvedDefaultWriterType
		{
			get
			{
				return Util.NonBlockingLazyInitializeVolatile(ref _writerType, () =>
				{
					var name = DefaultWriterType;
					return (!String.IsNullOrEmpty(name)) ? Type.GetType(name) : typeof(TraceLogEventWriter);
				});
			}
		}				

	}
}