using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace TestMasterHelpProvider.Configuration
{
	public class UrlBuilderConfigurationSection : ConfigurationSection
	{
		#region Fields

		public const string ProductionNsCore = "Prod-nsCore";
		public const string ProductionNsCorporate = "Prod-nsCorporate";
		public const string ProductionNsDistributor = "Prod-nsDistributor";
		public const string ProductionNsBackOffice = "Prod-nsBackOffice";
		public const string StagingNsCore = "Stag-nsCore";
		public const string StagingNsCorporate = "Stag-nsCorporate";
		public const string StagingNsDistributor = "Stag-nsDistributor";
		public const string StagingNsBackOffice = "Stag-nsBackOffice";
		public const string QaNsCore = "Qa-nsCore";
		public const string QaNsCorporate = "Qa-nsCorporate";
		public const string QaNsDistributor = "Qa-nsDistributor";
		public const string QaNsBackOffice = "Qa-nsBackOffice";
		public const string DevNsCore = "Dev-nsCore";
		public const string DevNsCorporate = "Dev-nsCorporate";
		public const string DevNsDistributor = "Dev-nsDistributor";
		public const string DevNsBackOffice = "Dev-nsBackOffice";
		public const string TestNsCore = "Test-nsCore";
		public const string TestNsCorporate = "Test-nsCorporate";
		public const string TestNsDistributor = "Test-nsDistributor";
		public const string TestNsBackOffice = "Test-nsBackOffice";

		private static readonly ConfigurationProperty UrlBuilderComponentsElement = new ConfigurationProperty(UrlBuilderConfigurationSection.UrlBuilderComponentsElementName, typeof(UrlBuilderConfigurationElementCollection), null, ConfigurationPropertyOptions.IsRequired);

		private const string UrlBuilderComponentsElementName = "UrlBuilderComponents";

		#endregion

		#region Properties

		/// <summary>
		/// Gets the UrlBuilderComponents element collection.
		/// </summary>
		public UrlBuilderConfigurationElementCollection UrlBuilderComponents
		{
			get { return (UrlBuilderConfigurationElementCollection)this[UrlBuilderConfigurationSection.UrlBuilderComponentsElement]; }
		}

		#endregion

		#region Constructors

		public UrlBuilderConfigurationSection()
		{
			// Left intentionally blank.
		}

		#endregion
	}
}
