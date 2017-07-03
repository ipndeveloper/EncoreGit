using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;

namespace NetSteps.Common.Extensions
{
	public static class XElementExtensions
	{
		public static T Attribute<T>(this XElement element, string name, T defaultValue = default(T))
		{
			XAttribute attribute = element.Attribute(name);
			if (attribute == null)
				return defaultValue;

			try
			{
				return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(attribute.Value);
			}
			catch
			{
				return defaultValue;
			}
		}

		public static XElement Descendant(this XElement element, XName name)
		{
			return element.Descendants(name).FirstOrDefault() ?? new XElement(name);
		}
	}
}
