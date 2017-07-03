using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ParcelPortResponseAPI
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String PostData = String.Empty;

            using (StreamReader sr = new StreamReader(this.Request.InputStream))
            {
                PostData = HttpUtility.UrlDecode(sr.ReadToEnd());
            }

            Response.Write(PostData);
        }
    }
}
