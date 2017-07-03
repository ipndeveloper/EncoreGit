using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Email.MIME
{
	public class MIMEType
	{
		public MIMEType()
		{
		}

		public static readonly string[] TypeTable = { "text", "image", "audio", "vedio", "application", "multipart", "message", null };

		public static readonly MediaType[] TypeCvtTable =
			new MediaType[] {
								   // media-type, sub-type, file extension
								   new MediaType( MediaEnum.MEDIA_APPLICATION, "xml", "xml" ),
								   new MediaType( MediaEnum.MEDIA_APPLICATION, "msword", "doc" ),
								   new MediaType( MediaEnum.MEDIA_APPLICATION, "rtf", "rtf" ),
								   new MediaType( MediaEnum.MEDIA_APPLICATION, "vnd.ms-excel", "xls" ),
								   new MediaType( MediaEnum.MEDIA_APPLICATION, "vnd.ms-powerpoint", "ppt" ),
								   new MediaType( MediaEnum.MEDIA_APPLICATION, "pdf", "pdf" ),
								   new MediaType( MediaEnum.MEDIA_APPLICATION, "zip", "zip" ),

								   new MediaType( MediaEnum.MEDIA_IMAGE, "jpeg", "jpeg" ),
								   new MediaType( MediaEnum.MEDIA_IMAGE, "jpeg", "jpg" ),
								   new MediaType( MediaEnum.MEDIA_IMAGE, "gif", "gif" ),
								   new MediaType( MediaEnum.MEDIA_IMAGE, "tiff", "tif" ),
								   new MediaType( MediaEnum.MEDIA_IMAGE, "tiff", "tiff" ),

								   new MediaType( MediaEnum.MEDIA_AUDIO, "basic", "wav" ),
								   new MediaType( MediaEnum.MEDIA_AUDIO, "basic", "mp3" ),

								   new MediaType( MediaEnum.MEDIA_VEDIO, "mpeg", "mpg" ),
								   new MediaType( MediaEnum.MEDIA_VEDIO, "mpeg", "mpeg" ),

								   new MediaType( MediaEnum.MEDIA_UNKNOWN, String.Empty, String.Empty )		// add new subtypes before this line
							   };
	}
}