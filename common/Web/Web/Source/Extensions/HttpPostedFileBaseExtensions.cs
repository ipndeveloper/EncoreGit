using System.Web;

namespace NetSteps.Web.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: HttpPostedFileBase Extensions
	/// Created: 12-20-2010
	/// </summary>
	public static class HttpPostedFileBaseExtensions
	{
		public static string GetFileContents(this HttpPostedFileBase httpPostedFileBase)
		{
			byte[] bs = new byte[httpPostedFileBase.ContentLength];
			httpPostedFileBase.InputStream.Read(bs, 0, httpPostedFileBase.ContentLength);
			string fileContents = System.Text.Encoding.ASCII.GetString(bs);
			return fileContents;
		}
	}
}
