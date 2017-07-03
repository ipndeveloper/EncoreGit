using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using TestMasterHelpProvider.Configuration;

namespace TestMasterHelpProvider
{
	public class UrlBuilder
	{
		#region Fields

		private const string UrlBuilderConfigurationSectionName = "UrlBuilderConfiguration";
		private const string UrlReplacementToken = "~";

		private static UrlBuilder __instance;

		private UrlBuilderConfigurationSection _urlBuilderConfiguration;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates an instance of UrlBuilder.
		/// </summary>
		private UrlBuilder()
		{
			_urlBuilderConfiguration = (UrlBuilderConfigurationSection)ConfigurationManager.GetSection(UrlBuilder.UrlBuilderConfigurationSectionName);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets a reference to an instance of UrlBuilder.
		/// </summary>
		/// <returns></returns>
		public static UrlBuilder GetInstance()
		{
			if (UrlBuilder.__instance == null)
			{
				UrlBuilder.__instance = new UrlBuilder();
			}

			return UrlBuilder.__instance;
		}

		/// <summary>
		/// Builds the Production - nsCorporate URL.
		/// </summary>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public string BuildProductionCorporate(string replacement = null)
		{
			return BuildUrl(UrlBuilderConfigurationSection.ProductionNsCorporate, replacement);
		}

		/// <summary>
		/// Builds the Production - nsCore URL.
		/// </summary>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public string BuildProductionCore(string replacement = null)
		{
			return BuildUrl(UrlBuilderConfigurationSection.ProductionNsCore, replacement);
		}

		/// <summary>
		/// Builds the Production - nsDistributor URL.
		/// </summary>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public string BuildProductionDistributor(string replacement = null)
		{
			return BuildUrl(UrlBuilderConfigurationSection.ProductionNsDistributor, replacement);
		}

		/// <summary>
		/// Builds the Production - nsBackOffice URL.
		/// </summary>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public string BuildProductionBackOffice(string replacement = null)
		{
			return BuildUrl(UrlBuilderConfigurationSection.ProductionNsBackOffice, replacement);
		}

		/// <summary>
		/// Builds the Staging - nsCorporate URL.
		/// </summary>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public string BuildStagingCorporate(string replacement = null)
		{
			return BuildUrl(UrlBuilderConfigurationSection.StagingNsCorporate, replacement);
		}

		/// <summary>
		/// Builds the Staging - nsCore URL.
		/// </summary>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public string BuildStagingCore(string replacement = null)
		{
			return BuildUrl(UrlBuilderConfigurationSection.StagingNsCore, replacement);
		}

		/// <summary>
		/// Builds the Staging - nsDistributor URL.
		/// </summary>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public string BuildStagingDistributor(string replacement = null)
		{
			return BuildUrl(UrlBuilderConfigurationSection.StagingNsDistributor, replacement);
		}

		/// <summary>
		/// Builds the Staging - nsBackOffice URL.
		/// </summary>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public string BuildStagingBackOffice(string replacement = null)
		{
			return BuildUrl(UrlBuilderConfigurationSection.StagingNsBackOffice, replacement);
		}

		/// <summary>
		/// Builds the QA - nsCorporate URL.
		/// </summary>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public string BuildQaCorporate(string replacement = null)
		{
			return BuildUrl(UrlBuilderConfigurationSection.QaNsCorporate, replacement);
		}

		/// <summary>
		/// Builds the QA - nsCore URL.
		/// </summary>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public string BuildQaCore(string replacement = null)
		{
			return BuildUrl(UrlBuilderConfigurationSection.QaNsCore, replacement);
		}

		/// <summary>
		/// Builds the QA - nsDistributor URL.
		/// </summary>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public string BuildQaDistributor(string replacement = null)
		{
			return BuildUrl(UrlBuilderConfigurationSection.QaNsDistributor, replacement);
		}

		/// <summary>
		/// Builds the QA - nsBackOffice URL.
		/// </summary>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public string BuildQaBackOffice(string replacement = null)
		{
			return BuildUrl(UrlBuilderConfigurationSection.QaNsBackOffice, replacement);
		}

		/// <summary>
		/// Builds the Dev - nsCorporate URL.
		/// </summary>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public string BuildDevCorporate(string replacement = null)
		{
			return BuildUrl(UrlBuilderConfigurationSection.DevNsCorporate, replacement);
		}

		/// <summary>
		/// Builds the Dev - nsCore URL.
		/// </summary>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public string BuildDevCore(string replacement = null)
		{
			return BuildUrl(UrlBuilderConfigurationSection.DevNsCore, replacement);
		}

		/// <summary>
		/// Builds the Dev - nsDistributor URL.
		/// </summary>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public string BuildDevDistributor(string replacement = null)
		{
			return BuildUrl(UrlBuilderConfigurationSection.DevNsDistributor, replacement);
		}

		/// <summary>
		/// Builds the Dev - nsBackOffice URL.
		/// </summary>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public string BuildDevBackOffice(string replacement = null)
		{
			return BuildUrl(UrlBuilderConfigurationSection.DevNsBackOffice, replacement);
		}

		/// <summary>
		/// Builds the Test - nsCorporate URL.
		/// </summary>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public string BuildTestCorporate(string replacement = null)
		{
			return BuildUrl(UrlBuilderConfigurationSection.TestNsCorporate, replacement);
		}

		/// <summary>
		/// Builds the Test - nsCore URL.
		/// </summary>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public string BuildTestCore(string replacement = null)
		{
			return BuildUrl(UrlBuilderConfigurationSection.TestNsCore, replacement);
		}

		/// <summary>
		/// Builds the Test - nsDistributor URL.
		/// </summary>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public string BuildTestDistributor(string replacement = null)
		{
			return BuildUrl(UrlBuilderConfigurationSection.TestNsDistributor, replacement);
		}

		/// <summary>
		/// Builds the Test - nsBackOffice URL.
		/// </summary>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public string BuildTestBackOffice(string replacement = null)
		{
			return BuildUrl(UrlBuilderConfigurationSection.TestNsBackOffice, replacement);
		}

		/// <summary>
		/// Builds a URL given the configuration element key and string replacement if present.
		/// </summary>
		/// <param name="elementKey"></param>
		/// <param name="replacement"></param>
		/// <returns></returns>
		private string BuildUrl(string elementKey, string replacement = null)
		{
			string retVal = String.Empty;
			UrlBuilderConfigurationElement productionCorporateConfig = _urlBuilderConfiguration.UrlBuilderComponents[elementKey];

			if (productionCorporateConfig != null)
			{
				string url = productionCorporateConfig.Value;
				string protocol = productionCorporateConfig.HttpProtocol;

				if (!String.IsNullOrEmpty(replacement) && url.Contains(UrlBuilder.UrlReplacementToken))
				{
					url = url.Replace(UrlBuilder.UrlReplacementToken, replacement);
				}

				retVal = String.Format("{0}://{1}", protocol, url);
			}

			return retVal;
		}

		#endregion
	}
}
