using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace EncoreOrderFulfillmentService
{
    class Program
    {
        static void Main(string[] args)
        {
            EncoreOrderFulfillmentService serv = new EncoreOrderFulfillmentService();
            string contents = GetXmlStringFromFilePath(@"C:\Netsteps\ENCORE\OrderFulfillment\XML\NetstepsOrderFulfillmentExampleXML.xml");
            //string s = serv.GetOrdersToFulfill("miche5779", "98jklrgeasg");
            serv.SendFulfillmentDataForOrders(contents, "miche5779", "98jklrgeasg");
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
