using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Email.MIME
{
    public class MIMEBody
    {
        public MIMEBody()
        {
        }

        public MIMEBody(string Input,MIMEHeader Header)
        {
            if (string.IsNullOrEmpty(Input))
            {
                throw new ArgumentNullException("Input can not be null");
            }
            MediaEnum ContentType;
            ContentType=GetMediaType(Header);
            if(MediaEnum.MEDIA_MULTIPART==ContentType)
            {
                string CurrentBoundry=GetBoundryMarker(Header);
                if (string.IsNullOrEmpty(CurrentBoundry))
                    return;
                CurrentBoundry = CurrentBoundry.Replace("\"", String.Empty);
                
                string BoundryStart = "--"+CurrentBoundry;
                string BoundryEnd = BoundryStart+"--";

                int StartIndex = Input.IndexOf(BoundryStart, 0);
                if (StartIndex == -1) return;
                int EndIndex = Input.IndexOf(BoundryEnd, 0);
                if (EndIndex == -1) EndIndex = Input.Length;

                Content = Input.Substring(0, StartIndex);
                while (StartIndex < EndIndex)
                {
                    StartIndex += BoundryStart.Length + 2;
                    if (StartIndex >= EndIndex)
                        break;
                    int TempIndex = Input.IndexOf(BoundryStart, StartIndex);
                    if (TempIndex != -1)
                    {
                        Boundries.Add(new MIMEMessage(Input.Substring(StartIndex, TempIndex - StartIndex)));
                    }
                    else
                        break;
                    StartIndex = TempIndex;
                }
            }
            else
            {
                Content = Input;
            }
            string Encoding = String.Empty;
            try
            {
                Encoding = Header[Constants.TransferEncoding].Attributes[0].Value;
            }
            catch
            {
                Encoding = Constants.Encoding7Bit;
            }
            Code CodeUsing = CodeManager.Instance[Encoding];
            CodeUsing.CharacterSet = Header[Constants.ContentType][Constants.Charset];
            CodeUsing.Decode(Content, out _Content);
        }

        private MediaEnum GetMediaType(MIMEHeader Header)
        {
            string ContentType = GetContentType(Header);
            int x=0;
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

        private string GetBoundryMarker(MIMEHeader Header)
        {
            return Header[Constants.ContentType][Constants.Boundary];
        }

        public List<MIMEMessage> Boundries
        {
            get { return _Boundries; }
            set { _Boundries = value; }
        }
        private List<MIMEMessage> _Boundries = new List<MIMEMessage>();

        public string Content
        {
            get { return _Content; }
            set { _Content = value; }
        }
        private string _Content = String.Empty;
    }
}