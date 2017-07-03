using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace TestMasterHelpProvider.Configuration
{
	[ConfigurationCollection(typeof(UrlBuilderConfigurationElement), AddItemName = "UrlBuilderComponent")]
	public class UrlBuilderConfigurationElementCollection : ConfigurationElementCollection
	{
		#region Properties

		/// <summary>
		/// Gets a UrlBuilderConfigurationElement from the collection by name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public new UrlBuilderConfigurationElement this[string name]
		{
			get { return (UrlBuilderConfigurationElement)BaseGet(name); }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Creates a new empty UrlBuilderConfigurationElement.
		/// </summary>
		/// <returns></returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new UrlBuilderConfigurationElement();
		}

		/// <summary>
		/// Gets the key for a particular UrlBuilderConfigurationElement.
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((UrlBuilderConfigurationElement)element).Name;
		}

		#endregion
	}
}
