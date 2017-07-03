using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Email.MIME
{
    public class MIMEMessage
    {
        public MIMEMessage()
        {
        }

        public MIMEMessage(string Input)
        {
            LoadMessage(Input);
        }

        public void LoadMessage(string Input)
        {
            _Content = Input;
            int HeaderEnd = Input.IndexOf("\r\n\r\n");
            Header = new MIMEHeader(Input.Substring(0, HeaderEnd + 2));
            Input = Input.Substring(HeaderEnd + 2);
            Body = new MIMEBody(Input, Header);
        }

        public MIMEHeader Header
        {
            get { return _Header; }
            set { _Header = value; }
        }

        private MIMEHeader _Header = null;

        public MIMEBody Body
        {
            get { return _Body; }
            set { _Body = value; }
        }

        private MIMEBody _Body = null;

        public string Subject
        {
            get { try { return Header[Constants.Subject].Attributes[0].Value; } catch { return String.Empty; } }
        }

        public string To
        {
            get { try { return Header[Constants.To].Attributes[0].Value; } catch { return String.Empty; } }
        }

        public string From
        {
            get { try { return Header[Constants.From].Attributes[0].Value; } catch { return String.Empty; } }
        }

        public string BodyText
        {
            get
            {
                try
                {
                    if (GetMediaType(Header)==MediaEnum.MEDIA_TEXT)
                    {
                        return Body.Content;
                    }
                    foreach (MIMEMessage TempMessage in Body.Boundries)
                    {
                        if (!string.IsNullOrEmpty(TempMessage.BodyText))
                        {
                            return TempMessage.BodyText;
                        }
                    }
                    return String.Empty;
                }
                catch
                {
                    return String.Empty;
                }
            }
        }

        private string GetContentType(MIMEHeader Header)
        {
            if (Header != null && Header[Constants.ContentType] != null && Header[Constants.ContentType].Attributes.Count > 0)
            {
                string ContentType = Header[Constants.ContentType].Attributes[0].Value;
                if (null != ContentType)
                {
                    int Index = ContentType.IndexOf('/', 0);
                    if (Index != -1)
                    {
                        return ContentType.Substring(0, Index);
                    }
                    else
                    {
                        return ContentType;
                    }
                }
            }
            return "text";
        }

        private MediaEnum GetMediaType(MIMEHeader Header)
        {
            string ContentType = GetContentType(Header);
            int x = 0;
            foreach (string TempType in MIMEType.TypeTable)
            {
                if (TempType.Equals(ContentType, StringComparison.InvariantCultureIgnoreCase))
                {
                    return (MediaEnum)x;
                }
                ++x;
            }
            return (MediaEnum)MIMEType.TypeTable.Length - 1;
        }

        private string _Content=String.Empty;

        public override string ToString()
        {
            return _Content;
        }
    }
}