using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Web;


namespace ParcelPortIntegrationService
{
    public class ParcelPortCommunication
    {
        private String ParcelPortURL;
        private String ParcelPortPostAttribute;

        public ParcelPortCommunication(String _ParcelPortURL, String _ParcelPortPostAttributute)
        {
            this.ParcelPortURL = _ParcelPortURL;
            this.ParcelPortPostAttribute = _ParcelPortPostAttributute;

            if (!_ValidateURL())
            {
                throw new Exception("URL Provided for communicating with Parcel Port is not valid!");
            }
        }

        public XElement SendOrderToParcelPort(XDocument xOrder)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings xws = new XmlWriterSettings();
            xws.OmitXmlDeclaration = true;
            xws.Indent = true;
            String Response;
            
            using (XmlWriter xw = XmlWriter.Create(sb, xws))
            {
                xOrder.Save(xw);
            }

            sb.Insert(0, "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>");
            
            NameValueCollection formData = new NameValueCollection();
            formData.Add(ParcelPortPostAttribute, sb.ToString());
            

            using (ParcelPortWebClient client = new ParcelPortWebClient())
            {
                client.TestOnly = false;
                Response = client.PostOrder(formData, ParcelPortURL);
            }

            XElement XResponse = XElement.Parse(Response);

            return XResponse;
        }

        private bool _ValidateURL()
        {
            using (ParcelPortWebClient client = new ParcelPortWebClient())
            {
                client.TestOnly = true;
                try
                {
                    byte[] body = client.DownloadData(ParcelPortURL);
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }
    }
}
