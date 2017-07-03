﻿using System.Configuration;

namespace NetSteps.Encore.Core.Reflection.Emit
{
	/// <summary>
	/// Configuration section for cache settings.
	/// </summary>
	public class RuntimeAssembliesConfigSection : ConfigurationSection
	{
		/// <summary>
		/// Configuration section name for cache settings
		/// </summary>
		public static readonly string SectionName = "netsteps.emit";

		const string CDefaultDynamicAssemblyPrefix = "NetSteps.Dynamic";
		const string PropertyName_dynamicAssemblyPrefix = "dynamicAssemblyPrefix";

		const bool CDefaultWriteAssembliesOnExit = false;
		const string PropertyName_writeAssembliesOnExit = "writeAssembliesOnExit";

		/// <summary>
		/// Prefix for the emitted assembly.
		/// </summary>
		[ConfigurationProperty(PropertyName_dynamicAssemblyPrefix, DefaultValue = CDefaultDynamicAssemblyPrefix)]
		public string DynamicAssemblyPrefix
		{
			get { return (string)this[PropertyName_dynamicAssemblyPrefix]; }
			set { this[PropertyName_dynamicAssemblyPrefix] = value; }
		}

		/// <summary>
		/// Indicates whether emitted assemblies should be written to disk on exit.
		/// </summary>
		[ConfigurationProperty(PropertyName_writeAssembliesOnExit, DefaultValue = CDefaultWriteAssembliesOnExit)]
		public bool WriteAssembliesOnExit
		{
			get { return (bool)this[PropertyName_writeAssembliesOnExit]; }
			set { this[PropertyName_writeAssembliesOnExit] = value; }
		}

		/// <summary>
		/// Gets the current configuration section.
		/// </summary>
		public static RuntimeAssembliesConfigSection Current
		{
			get
			{
				RuntimeAssembliesConfigSection config = ConfigurationManager.GetSection(
					RuntimeAssembliesConfigSection.SectionName) as RuntimeAssembliesConfigSection;
				return config ?? new RuntimeAssembliesConfigSection();
			}
		}		
	}

}
