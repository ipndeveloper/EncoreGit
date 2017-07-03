using System;
using System.Web;
using NetSteps.Common.Extensions;
using NetSteps.Web.Base;

namespace NetSteps.Web.Handlers
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Outputs a binary data as CSV from cache, put there by other logic. 
	/// Created: 04-25-2009
	/// </summary>
	public class ExcelHandler : BaseHttpHandler
	{
		#region Constants
		private const string GUID_PARAM = "guid";
		private const string FILENAME_PARAM = "fn";
		#endregion

		#region Members
		public string ReportName = "report.xls";
		#endregion

		#region Methods
		#endregion

		#region BaseHttpHandler Overrides
		public override void SetResponseCachePolicy(HttpCachePolicy cache)
		{
			cache.SetCacheability(HttpCacheability.Public);
			cache.SetExpires(DateTime.Now.AddDays(60));
			//cache.SetETagFromFileDependencies();
		}

		/// <summary>
		/// Main interface for reacting to the Thumbnailer request.
		/// </summary>
		/// <param name="context"></param>
		protected override void HandleRequest(HttpContext context)
		{
			string guid = GetQueryStringVar<string>(GUID_PARAM, string.Empty);
			string fileName = GetQueryStringVar<string>(FILENAME_PARAM, ReportName);

			if (!string.IsNullOrEmpty(guid) && context.Cache[guid] != null && context.Cache[guid] is byte[])
			{
				byte[] byteArray = (byte[])context.Cache[guid];

				_response.Clear();
				_response.ClearContent();
				_response.ContentType = "application/octet-stream";
				//_response.ContentType = "application/xml";
				_response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
				//_response.OutputStream.Write(fileByteArray, 0, fileByteArray.Length);
				_response.BinaryWrite(byteArray);
				_response.End();
			}
			else if (!string.IsNullOrEmpty(guid) && context.Cache[guid] != null && context.Cache[guid] is HandlerDocument)
			{
				HandlerDocument handlerDocument = (HandlerDocument)context.Cache[guid];
				fileName = GetQueryStringVar<string>(FILENAME_PARAM, handlerDocument.FileName.IsNullOrEmpty() ? ReportName : handlerDocument.FileName);

				_response.Clear();
				_response.ClearContent();
				_response.ContentType = handlerDocument.ContentType.IsNullOrEmpty() ? "application/octet-stream" : handlerDocument.ContentType;
				_response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
				//_response.OutputStream.Write(fileByteArray, 0, fileByteArray.Length);
				_response.BinaryWrite(handlerDocument.DocumentData);
				_response.End();
			}
		}
		#endregion
	}
}
