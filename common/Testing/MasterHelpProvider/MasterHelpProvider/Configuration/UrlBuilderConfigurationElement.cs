using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace TestMasterHelpProvider.Configuration
{
	public class UrlBuilderConfigurationElement : ConfigurationElement
	{
		#region Fields

		private const string NameAttribute = "Name";
		private const string ValueAttribute = "Value";
		private const string HttpProtocolAttribute = "HttpProtocol";
		
		private static readonly ConfigurationProperty UrlBuilderElementName = new ConfigurationProperty(UrlBuilderConfigurationElement.NameAttribute, typeof(string), string.Empty, ConfigurationPropertyOptions.IsKey);
		private static readonly ConfigurationProperty UrlBuilderElementValue = new ConfigurationProperty(UrlBuilderConfigurationElement.ValueAttribute, typeof(string), string.Empty, ConfigurationPropertyOptions.IsRequired);
		private static readonly ConfigurationProperty UrlBuilderElementHttpProtocol = new ConfigurationProperty(UrlBuilderConfigurationElement.ValueAttribute, typeof(string), "https", ConfigurationPropertyOptions.None);

		#endregion

		#region Properties

		/// <summary>
		/// Gets the name of this URL Builder configuration element.
		/// </summary>
		[ConfigurationProperty(UrlBuilderConfigurationElement.NameAttribute, IsRequired = true)]
		public string Name
		{
			get { return (string)this[UrlBuilderElementName]; }
		}

		/// <summary>
		/// Gets the value of this URL Builder configuration element.
		/// </summary>
		[ConfigurationProperty(UrlBuilderConfigurationElement.ValueAttribute, IsRequired = true)]
		public string Value
		{
			get { return (string)this[UrlBuilderElementValue]; }
		}

		/// <summary>
		/// Gets the http protocol of this URL Builder configuration element.
		/// </summary>
		[ConfigurationProperty(UrlBuilderConfigurationElement.HttpProtocolAttribute, IsRequired = true)]
		public string HttpProtocol
		{
			get { return (string)this[UrlBuilderElementHttpProtocol]; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of UrlConfigurationElement.
		/// </summary>
		public UrlBuilderConfigurationElement()
		{
			base.Properties.Add(UrlBuilderElementName);
			base.Properties.Add(UrlBuilderElementValue);
		}

		#endregion
	}
}
