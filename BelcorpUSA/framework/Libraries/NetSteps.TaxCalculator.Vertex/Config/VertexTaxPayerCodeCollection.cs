using System.Configuration;

namespace NetSteps.TaxCalculator.Vertex.Config
{
	public class VertexTaxPayerCodeCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new VertexTaxPayerCodeElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((VertexTaxPayerCodeElement)element).CountryCode;
		}

		internal void Add(VertexTaxPayerCodeElement element)
		{
			BaseAdd(element, true);
		}

		internal VertexTaxPayerCodeElement Get(string countryCode)
		{
			return (VertexTaxPayerCodeElement)BaseGet(countryCode);
		}
	}
}
