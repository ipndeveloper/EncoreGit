using System;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace NetSteps.Encore.Core.Wireup
{
	/// <summary>
	/// Configuration element for wiring up an assembly.
	/// </summary>
	public class WireupConfigurationElement : ConfigurationElement
	{
		const string PropertyName_assembly = "assembly";
		const string PropertyName_ordinal = "ordinal";
		Assembly _asm;

		/// <summary>
		/// The name of the assembly upon which wireup is performed.
		/// </summary>
		[ConfigurationProperty(PropertyName_assembly
			, IsKey = true
			, IsRequired = true)]
		public string AssemblyName
		{
			get { return (string)this[PropertyName_assembly]; }
			set { this[PropertyName_assembly] = value; }
		}

		/// <summary>
		/// The ordinal; indicates the order in which assemblies are registered.
		/// </summary>
		[ConfigurationProperty(PropertyName_ordinal, DefaultValue = 0)]
		public int Ordinal
		{
			get { return (int)this[PropertyName_ordinal]; }
			set { this[PropertyName_ordinal] = value; }
		}

		internal Assembly ResolveAssembly
		{
			get
			{
				if (_asm == null && !String.IsNullOrEmpty(this.AssemblyName))
				{
					_asm = Assembly.Load(this.AssemblyName);
				}
				return _asm;
			}
		}

		internal void PerformWireup(IWireupCoordinator coordinator)
		{
			Contract.Requires<ArgumentNullException>(coordinator != null);
			coordinator.WireupDependencies(this.ResolveAssembly);
		}
	}
}
