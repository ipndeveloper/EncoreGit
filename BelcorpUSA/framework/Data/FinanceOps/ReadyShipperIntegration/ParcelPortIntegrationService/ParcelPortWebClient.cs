using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace ParcelPortIntegrationService
{
    [System.ComponentModel.DesignerCategory("")]
    class ParcelPortWebClient : WebClient
    {
        public bool TestOnly = true;

        public ParcelPortWebClient()
        {
            this.Encoding = System.Text.Encoding.UTF8;

        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest req = base.GetWebRequest(address);
            if (TestOnly && req.Method == "GET")
            {
                req.Method = "HEAD";
            }
            else
            {
                req.ContentType = "application/x-www-form-urlencoded";
            }
            return req;
        }


        public String PostOrder(NameValueCollection formData, String URL)
        {
            TestOnly = false;
            
            byte[] responseBytes = UploadData(URL, "POST", Encoding.UTF8.GetBytes(System.Web.HttpUtility.UrlEncode(formData["xmlpost"])));
            return Encoding.UTF8.GetString(responseBytes);
        }
    }
}
