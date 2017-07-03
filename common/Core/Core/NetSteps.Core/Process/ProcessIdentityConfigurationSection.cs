using System;
using System.Configuration;
using System.Diagnostics;

namespace NetSteps.Encore.Core.Process
{
	/// <summary>
	/// Configuration section for log settings.
	/// </summary>
	public sealed class ProcessIdentifyConfigurationSection : ConfigurationSection
	{
		/// <summary>
		/// Configuration section name for trace settings.
		/// </summary>
		public static readonly string SectionName = "netsteps.processid";

		const string CUnknownValue = "unknown";
		/// <summary>
		/// Default environment string
		/// </summary>
		public const string CDefaultEnvironment = "dev";
		/// <summary>
		/// Default tenant string
		/// </summary>
		public const string CDefaultTenant = "Encore";

		/// <summary>
		/// Property name for environment.
		/// </summary>
		public const string PropertyName_environment = "environment";		

		/// <summary>
		/// Property name for component.
		/// </summary>
		public const string PropertyName_component = "component";

		/// <summary>
		/// Property name for tenant.
		/// </summary>
		public const string PropertyName_tenant = "tenant";
		
		/// <summary>
		/// Indicates the name of the component that the current application represents.
		/// The meaning of "component" is up to the user but in general indicates a
		/// role that an application performs within a system.
		/// </summary>
		[ConfigurationProperty(PropertyName_component, DefaultValue = CUnknownValue)]		
		public string Component
		{
			get { return (string)this[PropertyName_component]; }
			set { this[PropertyName_component] = value; }
		}
				
		/// <summary>
		/// Indicates the name of the environment in which the application is executing.
		/// The meaning of "environment" is up to the user but in general indicates an
		/// environment such as: { dev | test | stage | prod }. In cases where
		/// events in one environment can be heard by journalers in another environment
		/// the presence of this value in an event helps with filtering.
		/// </summary>
		[ConfigurationProperty(PropertyName_environment, DefaultValue = CDefaultEnvironment)]
		public string Environment
		{
			get { return (string)this[PropertyName_environment]; }
			set { this[PropertyName_environment] = value; }
		}

		/// <summary>
		/// Indicates the name of the tenant or customer.
		/// </summary>
		[ConfigurationProperty(PropertyName_tenant, DefaultValue = CDefaultTenant)]
		public string Tenant
		{
			get { return (string)this[PropertyName_tenant]; }
			set { this[PropertyName_tenant] = value; }
		}
		
		/// <summary>
		/// Gets the current configuration section.
		/// </summary>
		public static ProcessIdentifyConfigurationSection Current
		{
			get
			{
				ProcessIdentifyConfigurationSection config = ConfigurationManager.GetSection(
					ProcessIdentifyConfigurationSection.SectionName) as ProcessIdentifyConfigurationSection;
				return config ?? new ProcessIdentifyConfigurationSection();
			}
		}		
	}
}