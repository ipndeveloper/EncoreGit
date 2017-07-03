using System.Configuration;
using System.Xml;

namespace NetSteps.Security
{
	// http://www.yaldex.com/asp_net_tutorial/html/bfa00166-2e56-4234-a596-30cf9d197792.htm - JHE
	// http://www.wrox.com/WileyCDA/Section/Redirecting-Configuration-with-a-Custom-Provider.id-291932.html - JHE
	/// <summary>
	/// Author: John Egbert
	/// Description: Custom WebConfig section encryption provider
	/// Created: 09-20-2011
	/// </summary>
	public class NetStepsProtectedConfigurationProvider : ProtectedConfigurationProvider
	{
		public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
		{
			base.Initialize(name, config);
		}

		public override System.Xml.XmlNode Decrypt(System.Xml.XmlNode encryptedNode)
		{
			string decryptedData = Encryption.DecryptAES(encryptedNode.InnerText);

			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.PreserveWhitespace = true;
			xmlDoc.LoadXml(decryptedData);

			return xmlDoc.DocumentElement;
		}

		public override System.Xml.XmlNode Encrypt(System.Xml.XmlNode node)
		{
			string encryptedData = Encryption.EncryptAES(node.OuterXml);

			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.PreserveWhitespace = true;
			xmlDoc.LoadXml("<EncryptedData>" + encryptedData + "</EncryptedData>");

			return xmlDoc.DocumentElement;
		}
	}
}
