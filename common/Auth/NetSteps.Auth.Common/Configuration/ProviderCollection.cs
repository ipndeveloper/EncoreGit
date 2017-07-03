using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace NetSteps.Auth.Common.Configuration
{
	[ConfigurationCollection(typeof(ProviderElement), AddItemName="addProvider", RemoveItemName="removeProvider", CollectionType=ConfigurationElementCollectionType.BasicMap)]
	public class ProviderCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new ProviderElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ProviderElement)element).Name;
		}

		public ProviderElement this[int index]
		{
			get
			{
				return (ProviderElement)base.BaseGet(index);
			}
		}
	}
}
