using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace NetSteps.Common.Email
{
    /// <summary>
    /// Author: John Egbert
    /// Created: 04/13/2010
    /// </summary>
    public class EmailHelpers
    {
        public static MailMessage AttachImages(MailMessage message)
        {
            if (!message.IsBodyHtml)
                throw new ApplicationException("Message format needs to be HTML");

            Regex reg = new Regex("src[^>]*[^/].(?:jpg|bmp|gif)(?:\"|\')");

            MailMessage messageOut = message;

            foreach (Match match in reg.Matches(message.Body))
            {
                string cid = Guid.NewGuid().ToString("N");
                Stream srcStream;

                string src = match.Value;
                src = src.Replace("src=", string.Empty);
                src = src.Replace("\"", string.Empty);

                if (src.StartsWith("http"))
                {
                    WebRequest request = HttpWebRequest.Create(src);
                    srcStream = request.GetResponse().GetResponseStream();
                }
                else
                {
                    srcStream = new MemoryStream(File.ReadAllBytes(src));
                }

                Attachment newAttachment = new Attachment(srcStream, "image/" + Path.GetExtension(src).ToLower().Replace(".", string.Empty));
                newAttachment.ContentId = cid;
                messageOut.Attachments.Add(newAttachment);

                messageOut.Body = messageOut.Body.Replace(src, "cid:" + cid);
            }

            return messageOut;
        }
    }
}
