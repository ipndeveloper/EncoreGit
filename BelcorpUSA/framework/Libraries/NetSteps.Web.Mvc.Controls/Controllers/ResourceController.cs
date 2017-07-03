using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using NetSteps.Web.Mvc.Filters;
using System.Web.SessionState;

namespace NetSteps.Web.Mvc.Controls.Controllers
{
	[CacheFilter(Cacheability = HttpCacheability.Public, MaximumAge = "7.00:00:00", ValidUntilExpires = true)]
    [SessionState(SessionStateBehavior.Disabled)]
	public class ResourceController : Controller
	{
		public ActionResult Index(string path)
		{
			if (!path.StartsWith("/"))
			{
				path = "/" + path;
			}

			var contentType = GetContentType(path);
			var pathProvider = HostingEnvironment.VirtualPathProvider;

			if (pathProvider.FileExists(path))
			{
				var resourceStream = pathProvider.GetFile(path).Open();
				var result = this.File(resourceStream, contentType);

				return result;
			}

			return new HttpNotFoundResult();
		}

		private string GetContentType(string resourceName)
		{
			var extention = Path.GetExtension(resourceName).ToLower();

			switch (extention)
			{
				case ".bmp":
					return "image/bmp";
				case ".gif":
					return "image/gif";
				case ".ico":
					return "image/x-icon";
				case ".jpg":
				case ".jpeg":
					return "image/jpeg";
				case ".png":
					return "image/png";
				case ".tiff":
					return "image/tiff";
				case ".js":
					return "text/javascript";
				case ".css":
					return "text/css";
				case ".swf":
					return "application/x-shockwave-flash";
				default:
					return "text/html";
			}
		}
	}
}
