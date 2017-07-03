using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace ReadyShipperIntegrationService
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadyShipperIntegrationService serv = new ReadyShipperIntegrationService();
            string contents = GetXmlStringFromFilePath(@"C:\Development\Shipped.xml");
            string s = serv.ImportOrdersIntoReadyShipper("readyShipper4592", "r92rgeWEW457");

            XmlDocument xdoc = new XmlDocument();

            s = s.Replace("<ordercollection xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">", "<ordercollection xsi:noNamespaceSchemaLocation=\"http://www.trueship.com/xml/TrueOrder1.6.2.xsd\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");

            xdoc.LoadXml(s);
            xdoc.Save("C:\\Development\\ItWorks\\test.xml");

            serv.ExportOrdersFromReadyShipper(contents, "readyShipper4592", "r92rgeWEW457");
        }

        public static string GetXmlStringFromFilePath(string strFile)
        {
            // Load the xml file into XmlDocument object.
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(strFile);
            }
            catch (XmlException e)
            {
                Console.WriteLine(e.Message);
            }
            // Now create StringWriter object to get data from xml document.
            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);
            xmlDoc.WriteTo(xw);
            return sw.ToString();
        }
    }
}
