using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web;

namespace NetSteps.Web
{
    public class ErrorNotifier
    {
        private HttpApplication app;
        public ErrorNotifier(HttpApplication app)
        {
            this.app = app;
        }

        public void Notify()
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(this.MailFrom);
            foreach (string email in MailTo)
            {
                message.To.Add(new MailAddress(email));
            }
            message.Subject = this.MailSubject;
            message.IsBodyHtml = true;
            message.Body = String.Format(GetErrorMessageTemplate(),
                                            this.PagePath(),
                                            this.PageReferrer(),
                                            this.QuerystringValues(),
                                            this.FormValues(),
                                            this.CookieValues(),
                                            this.ServerValues(),
                                            this.SessionValues(),
                                            this.CurrentUser(),
                                            this.CallStack(),
                                            this.ExceptionTrace());

            SmtpClient sender = new SmtpClient();
            sender.Send(message);
        }

        private string mailFrom = String.Empty;
        public string MailFrom
        {
            get { return mailFrom; }
            set { mailFrom = value; }
        }

        private List<String> mailTo = new List<String>();
        public List<String> MailTo
        {
            get { return mailTo; }
        }

        private string mailSubject = String.Empty;
        public string MailSubject
        {
            get { return mailSubject; }
            set { mailSubject = value; }
        }

        private string PagePath()
        {
            try
            {
                return app.Context.Request.FilePath;
            }
            catch
            {
                return "Error Retrieving Data";
            }
        }

        private string PageReferrer()
        {
            try
            {
                return app.Context.Request.UrlReferrer.OriginalString;
            }
            catch
            {
                return "Error Retrieving Data";
            }
        }

        private string QuerystringValues()
        {
            try
            {
                System.IO.StringWriter qs = new System.IO.StringWriter();

                // Decrypt any of the Encrypted QueryString parameters
                //if (app.Context.Request.QueryString[Util.URL_NAME] != null)
                //{
                //    SecureQueryString q = new SecureQueryString(app.Context.Request.QueryString[Util.URL_NAME]);

                //    foreach (string key in q.Keys)
                //    {
                //        qs.WriteLine("[Key] = " + key + " [Value] = " + q[key]);
                //    }
                //}

                // Extract All querystring params
                foreach (string key in app.Context.Request.QueryString.Keys)
                {
                    qs.WriteLine("[Key] = " + key + " [Value] = " + app.Context.Request.QueryString[key]);
                }

                return qs.GetStringBuilder().ToString();
            }
            catch
            {
                return "Error Retrieving Data";
            }
        }

        private string FormValues()
        {
            try
            {
                System.IO.StringWriter sw = new System.IO.StringWriter();

                foreach (string key in app.Context.Request.Form.Keys)
                {
                    sw.WriteLine("[Key] = " + key + " [Value] = " + app.Context.Request.Form[key]);
                }

                return sw.GetStringBuilder().ToString();
            }
            catch
            {
                return "Error Retrieving Data";
            }
        }

        private string CookieValues()
        {
            try
            {
                System.IO.StringWriter sw = new System.IO.StringWriter();

                foreach (string key in app.Context.Request.Cookies.Keys)
                {
                    sw.WriteLine("[Key] = " + key + " [Value] = " + app.Context.Request.Cookies[key].Value);
                }

                return sw.GetStringBuilder().ToString();
            }
            catch
            {
                return "Error Retrieving Data";
            }
        }

        private string ServerValues()
        {
            try
            {
                System.IO.StringWriter sw = new System.IO.StringWriter();

                for (int i = 0; i < app.Context.Request.ServerVariables.Keys.Count; i++)
                {
                    sw.WriteLine("[Key] = " + app.Context.Request.ServerVariables.Keys[i] + " [Value] = " + app.Context.Request.ServerVariables[i]);
                }

                return sw.GetStringBuilder().ToString();
            }
            catch
            {
                return "Error Retrieving Data";
            }
        }

        private string SessionValues()
        {
            try
            {
                System.IO.StringWriter sw = new System.IO.StringWriter();

                foreach (string key in app.Context.Session.Keys)
                {
                    sw.WriteLine("[Key] = " + key + " [Value] = " + app.Context.Session[key]);
                }

                return sw.GetStringBuilder().ToString();
            }
            catch
            {
                return "Error Retrieving Data";
            }
        }

        private string CurrentUser()
        {
            try
            {
                if (app.Context.User != null)
                {
                    return app.Context.User.Identity.Name;
                }
                else
                {
                    return String.Empty;
                }
            }
            catch
            {
                return "Error Retrieving Data";
            }
        }

        private string CallStack()
        {
            try
            {
                return app.Context.Error.StackTrace;
            }
            catch
            {
                return "Error Retrieving Data";
            }
        }

        private string ExceptionTrace()
        {
            try
            {
                System.IO.StringWriter sw = new System.IO.StringWriter();

                sw.WriteLine("==========================================================");

                Exception e = app.Context.Error;
                sw.WriteLine("Stack Order: 1");
                sw.WriteLine("Error Message: " + e.Message);
                sw.WriteLine("Error Source: " + e.Source);
                sw.WriteLine("Target Module: " + e.TargetSite.Module.FullyQualifiedName);
                sw.WriteLine("Target Method: " + e.TargetSite.Name);
                sw.WriteLine("Custom Data: ");
                foreach (object key in e.Data)
                {
                    sw.WriteLine(key + "=>" + e.Data[key]);
                }
                sw.WriteLine("Stack Trace:");
                sw.WriteLine(e.StackTrace);
                sw.WriteLine("==========================================================");

                int order = 1;
                while (e.InnerException != null)
                {
                    order++;
                    e = e.InnerException;
                    sw.WriteLine("Stack Order: " + order.ToString());
                    sw.WriteLine("Error Message: " + e.Message);
                    sw.WriteLine("Error Source: " + e.Source);
                    sw.WriteLine("Target Module: " + e.TargetSite.Module.FullyQualifiedName);
                    sw.WriteLine("Target Method: " + e.TargetSite.Name);
                    sw.WriteLine("Custom Data: ");
                    foreach (object key in e.Data)
                    {
                        sw.WriteLine(key + "=>" + e.Data[key]);
                    }
                    sw.WriteLine("Stack Trace:");
                    sw.WriteLine(e.StackTrace);
                    sw.WriteLine("==========================================================");
                }

                return sw.GetStringBuilder().ToString();
            }
            catch
            {
                return "Error Retrieving Data";
            }
        }

        private string GetErrorMessageTemplate()
        {
            System.IO.StringWriter t = new System.IO.StringWriter();

            t.WriteLine("<html>                                                                                                                                                                 ");
            t.WriteLine("  <body>                                                                                                                                                               ");
            t.WriteLine("      <table border='0' cellpadding='2' cellspacing='1' width='500px' height='262' bgcolor='#000000' style='font-family: arial; border: solid 1px black;'>              ");
            t.WriteLine("        <tr><td height='38' bgcolor='#093570'><span style='font-size: 16pt; color: white; font-family: Arial'>&nbsp;Detailed Error Report</span></td></tr>             ");
            t.WriteLine("        <tr><td height='38' bgcolor='#CCCCCC' align='left'><span style='font-size: 12pt; font-weight: bold'>Page Path</span></td></tr>                                ");
            t.WriteLine("        <tr><td height='39' bgcolor='#FFFFFF'><pre>{0}</pre></td></tr>                                                                                                 ");
            t.WriteLine("        <tr><td height='38' bgcolor='#CCCCCC' align='left'><span style='font-size: 12pt; font-weight: bold'>Referring Page</span></td></tr>                           ");
            t.WriteLine("        <tr><td height='39' bgcolor='#FFFFFF'><pre>{1}</pre></td></tr>                                                                                                 ");
            t.WriteLine("        <tr><td height='38' bgcolor='#CCCCCC' align='left'><span style='font-size: 12pt; font-weight: bold'>Querystring Details</span></td></tr>                      ");
            t.WriteLine("        <tr><td height='39' bgcolor='#FFFFFF'><pre>{2}</pre></td></tr>                                                                                                 ");
            t.WriteLine("        <tr><td height='38' bgcolor='#CCCCCC' align='left'><span style='font-size: 12pt; font-weight: bold'>Form Details</span></td></tr>                             ");
            t.WriteLine("        <tr><td height='39' bgcolor='#FFFFFF'><pre>{3}</pre></td></tr>                                                                                                 ");
            t.WriteLine("        <tr><td height='38' bgcolor='#CCCCCC' align='left'><span style='font-size: 12pt; font-weight: bold'>Cookies</span></td></tr>                                  ");
            t.WriteLine("        <tr><td height='39' bgcolor='#FFFFFF'><pre>{4}</pre></td></tr>                                                                                                 ");
            t.WriteLine("        <tr><td height='38' bgcolor='#CCCCCC' align='left'><span style='font-size: 12pt; font-weight: bold'>Server Variables</span></td></tr>                         ");
            t.WriteLine("        <tr><td height='39' bgcolor='#FFFFFF'><pre>{5}</pre></td></tr>                                                                                                 ");
            t.WriteLine("        <tr><td height='38' bgcolor='#CCCCCC' align='left'><span style='font-size: 12pt; font-weight: bold'>Session Variables</span></td></tr>                        ");
            t.WriteLine("        <tr><td height='39' bgcolor='#FFFFFF'><pre>{6}</pre></td></tr>                                                                                                 ");
            t.WriteLine("        <tr><td height='38' bgcolor='#CCCCCC' align='left'><span style='font-size: 12pt; font-weight: bold'>Current User</span></td></tr>                                ");
            t.WriteLine("        <tr><td height='39' bgcolor='#FFFFFF'><pre>{7}</pre></td></tr>                                                                                                 ");
            t.WriteLine("        <tr><td height='38' bgcolor='#CCCCCC' align='left'><span style='font-size: 12pt; font-weight: bold'>Raw Call Stack</span></td></tr>                           ");
            t.WriteLine("        <tr><td height='39' bgcolor='#FFFFFF'><pre>{8}</pre></td></tr>                                                                                                 ");
            t.WriteLine("        <tr><td height='38' bgcolor='#CCCCCC' align='left'><span style='font-size: 12pt; font-weight: bold'>Exception Tree</span></td></tr>                           ");
            t.WriteLine("        <tr><td height='39' bgcolor='#FFFFFF'><pre>{9}</pre></td></tr>                                                                                                 ");
            t.WriteLine("      </table>                                                                                                                                                         ");
            t.WriteLine("  </body>                                                                                                                                                              ");
            t.WriteLine("</html>                                                                                                                                                                ");

            return t.GetStringBuilder().ToString();
        }
    }
}
