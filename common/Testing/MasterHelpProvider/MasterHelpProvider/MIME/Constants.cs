using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Email.MIME
{
    public static class Constants
    {
		public static string MimeVersion { get{return "MIME-Version";} }
		public static string ContentType { get{ return "Content-Type";} }
		public static string TransferEncoding { get{return "Content-Transfer-Encoding";} }
		public static string ContentID { get{return "Content-ID";} }
		public static string ContentDescription { get{return "Content-Description";} }
		public static string ContentDisposition { get{return "Content-Disposition";} }
		public static string Charset { get{return "charset";} }
        public static string Subject { get { return "Subject"; } }
        public static string To { get { return "To"; } }
        public static string From { get { return "From"; } }
		public static string Name { get{return "name";} }
		public static string Filename { get{return "filename";} }
		public static string Boundary { get{return "boundary";} }
		public static string Encoding7Bit { get{return "7bit";} }
		public static string Encoding8Bit { get{return "8bit";} }
		public static string EncodingBinary { get{return "binary";} }
		public static string EncodingQP { get{return "quoted-printable";} }
		public static string EncodingBase64 { get{return "base64";} }
		public static string MediaText { get{return "text";} }
		public static string MediaImage { get{return "image";} }
		public static string MediaAudio { get{return "audio";} }
		public static string MediaVideo { get{return "vedio";} }
		public static string MediaApplication { get{return "application";} }
		public static string MediaMultiPart { get{return "multipart";} }
		public static string MediaMessage { get{return "message";} }
    }
}