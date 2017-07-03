using System.Configuration;

namespace NetSteps.Accounts.Downline.UI.Service.Configuration
{
	public class DownlineUIConfigurationSection : ConfigurationSection
	{
		[ConfigurationProperty("infoCardExtensions")]
		[ConfigurationCollection(typeof(InfoCardExtensionCollection))]
		public InfoCardExtensionCollection InfoCardExtensions
		{
			get { return (InfoCardExtensionCollection)this["infoCardExtensions"]; }
		}

		[ConfigurationProperty("infoCardListItems")]
		[ConfigurationCollection(typeof(InfoCardListItemCollection))]
		public InfoCardListItemCollection InfoCardListItems
		{
			get { return (InfoCardListItemCollection)this["infoCardListItems"]; }
		}
	}

	public class InfoCardExtensionCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new InfoCardExtensionElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((InfoCardExtensionElement)element).Type;
		}
	}

	public class InfoCardExtensionElement : ConfigurationElement
	{
		[ConfigurationProperty("type", IsKey = true, IsRequired = true)]
		public string Type
		{
			get
			{
				return this["type"] as string;
			}
		}
	}

	public class InfoCardListItemCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new InfoCardListItemElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((InfoCardListItemElement)element).Key;
		}
	}

	public class InfoCardListItemElement : ConfigurationElement
	{
		[ConfigurationProperty("key", IsKey = true, IsRequired = true)]
		public string Key
		{
			get
			{
				return this["key"] as string;
			}
		}
	}
}
