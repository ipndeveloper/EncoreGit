using ParcelPortIntegrationService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml;

namespace ParcelPortIntegration
{
    public partial class ShipmentPush : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String PostData = String.Empty;
            String ResponseData = String.Empty;
            XDocument ShipmentPush;

            XElement ErrorResponse = new XElement("response");
            XElement ErrorResponseStatus = new XElement("status");
            XElement ErrorResponseError = new XElement("error");

            using (StreamReader sr = new StreamReader(this.Request.InputStream))
            {
                PostData = HttpUtility.UrlDecode(sr.ReadToEnd());
            }

            try
            {
                try
                {
                    try
                    {
                        ShipmentPush = XDocument.Parse(PostData);
                    }
                    catch (Exception ParsingException)
                    {
                        throw new XmlException();
                    }

                    if (ShipmentPush != null)
                    {
                        ParcelPortIntegrationService.ParcelPortIntegrationService service = new ParcelPortIntegrationService.ParcelPortIntegrationService();
                        Response.Write(service.ShipmentPush(ShipmentPush));
                        return;
                    }
                }
                catch (XmlException xmlException)
                {
                    throw new Exception("Error parsing XML Document");
                }
                catch (Exception GeneralException)
                {
                    throw new Exception("General Error");
                }
            }
            catch (Exception FinalException)
            {
                ErrorResponseError.Value = FinalException.Message;
                ErrorResponseStatus.Value = "NOK";
                ErrorResponse.Add(ErrorResponseStatus);
                ErrorResponse.Add(ErrorResponseError);
                Response.Write(ErrorResponse.ToString());
            }
        }
    }
}