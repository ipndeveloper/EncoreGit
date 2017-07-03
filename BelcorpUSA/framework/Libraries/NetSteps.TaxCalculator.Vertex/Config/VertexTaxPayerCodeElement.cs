using System.Configuration;

namespace NetSteps.TaxCalculator.Vertex.Config
{
	public class VertexTaxPayerCodeElement : ConfigurationElement
	{
		private const string Property_countryCode = "countryCode";
		private const string Property_taxpayerCode = "taxPayerCode";

		public VertexTaxPayerCodeElement()
		{
		}

		public VertexTaxPayerCodeElement(string countryCode, string taxpayerCode)
		{
			CountryCode = countryCode;
			TaxPayerCode = taxpayerCode;
		}

		[ConfigurationProperty(Property_countryCode, IsKey = true, IsRequired = true)]
		public string CountryCode
		{
			get { return (string) this[Property_countryCode]; }
			set { this[Property_countryCode] = value; }
		}

		[ConfigurationProperty(Property_taxpayerCode, IsKey = false, IsRequired = true)]
		public string TaxPayerCode
		{
			get { return (string) this[Property_taxpayerCode]; }
			set { this[Property_taxpayerCode] = value; }
		}
	}
}