
using System;
using System.Configuration;
using System.Diagnostics;
using NetSteps.Encore.Core.Configuration;

namespace NetSteps.Encore.Core.Log
{
	/// <summary>
	/// Configuration element collection for specializing logging for a namespaces.
	/// </summary>
	public class NamespaceElementCollection : AbstractConfigurationElementCollection<NamespaceElement, string>
	{
		/// <summary>
		/// Gets the element's key
		/// </summary>
		/// <param name="element">the element</param>
		/// <returns>the key</returns>
		protected override string PerformGetElementKey(NamespaceElement element)
		{
			return element.Namespace;
		}
	}

	/// <summary>
	/// Configuration element for specializing logging for a namespace.
	/// </summary>
	public class NamespaceElement : ConfigurationElement
	{
		const string PropertyName_namespace = "namespace";
		const string PropertyName_specialized = "specialized";
		const string PropertyName_sourceLevel = "sourceLevel";
		const string PropertyName_stackTraceThreshold = "stackTraceThreshold";
		const string PropertyName_writerName = "writerName";
		const string PropertyName_writerType = "writer";

		/// <summary>
		/// The namespace to which the configuration element applies.
		/// </summary>
		[ConfigurationProperty(PropertyName_namespace
			, IsKey = true
			, IsRequired = true)]
		public string Namespace
		{
			get { return (string)this[PropertyName_namespace]; }
			set { this[PropertyName_namespace] = value; }
		}

		/// <summary>
		/// Whether the namespace has specialized source levels, etc.
		/// </summary>
		[ConfigurationProperty(PropertyName_specialized, DefaultValue = false)]
		public bool IsSpecialized
		{
			get { return (bool)this[PropertyName_specialized]; }
			set { this[PropertyName_specialized] = value; }
		}

		/// <summary>
		/// The source levels.
		/// </summary>
		/// <seealso cref="System.Diagnostics.SourceLevels"/>
		[ConfigurationProperty(PropertyName_sourceLevel, DefaultValue = LogConfigurationSection.CDefaultSourceLevel)]
		public SourceLevels SourceLevel
		{
			get { return (SourceLevels)this[PropertyName_sourceLevel]; }
			set { this[PropertyName_sourceLevel] = value; }
		}

		/// <summary>
		/// The stack trace threshold
		/// </summary>
		/// <seealso cref="System.Diagnostics.TraceEventType"/>
		[ConfigurationProperty(PropertyName_stackTraceThreshold, DefaultValue = LogConfigurationSection.CDefaultStackTraceThreshold)]
		public TraceEventType StackTraceThreshold
		{
			get { return (TraceEventType)this[PropertyName_stackTraceThreshold]; }
			set { this[PropertyName_stackTraceThreshold] = value; }
		}

		/// <summary>
		/// The type of sink to construct.
		/// </summary>
		[ConfigurationProperty(PropertyName_writerType)]
		public string WriterTypeName
		{
			get { return (string)this[PropertyName_writerType]; }
			set { this[PropertyName_writerType] = value; }
		}

		/// <summary>
		/// The name of the sink (in the parent object's 'sinks' collection)
		/// </summary>
		[ConfigurationProperty(PropertyName_writerName)]
		public string WriterName
		{
			get { return (string)this[PropertyName_writerName]; }
			set { this[PropertyName_writerName] = value; }
		}

		Type _writerType;
		internal Type ResolvedWriterType
		{
			get
			{
				return Util.NonBlockingLazyInitializeVolatile(ref _writerType, () =>
				{
					var name = WriterTypeName;
					return (!String.IsNullOrEmpty(name)) ? Type.GetType(name) : null;
				});
			}
		}
	}
}
