using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace NetSteps.Web.API.Base.Common
{
	public static class HttpRequestExtensions
	{
		public static bool CanAcceptType(this HttpRequestBase request, string acceptType)
		{
			var accept_with_media_type_sep = String.Concat(acceptType, ';');
			foreach (var a in request.AcceptTypes)
			{
				if (a.StartsWith("*/*")
					|| String.Equals(acceptType, a)
					|| a.StartsWith(accept_with_media_type_sep))
					return true;
			}
			return false;
		}

		public static bool IsContentType(this HttpRequestBase request, string contentType)
		{
			var content_type_with_media_type_sep = String.Concat(contentType, ';');
			var current = request.ContentType;

			return (current.StartsWith("*/*")
					|| String.Equals(contentType, current)
					|| current.StartsWith(content_type_with_media_type_sep));
		}
	}
}
