using System.Xml.Linq;

namespace UPSQuery.Common
{
	public static class UPSQueryProcessorResultExtensions
	{
		public static string ToXml(this IUPSQueryProcessorResult processorResults)
		{
			var node = new XElement("Customer");
			node.Add(new XElement("Address1", processorResults.Address1));
			node.Add(new XElement("Address2", processorResults.Address2));
			node.Add(new XElement("Address3", processorResults.Address3));
			node.Add(new XElement("City", processorResults.City));
			node.Add(new XElement("Country", processorResults.Country));
			node.Add(new XElement("CustomerName", processorResults.CustomerName));
			node.Add(new XElement("PostalCode", processorResults.PostalCode));
			node.Add(new XElement("State", processorResults.State));
			node.Add(new XElement("Telephone", processorResults.Telephone));
			return node.ToString();
		}
	}
}
