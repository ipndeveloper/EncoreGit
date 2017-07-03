using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using NetSteps.Common.Extensions;
using NetSteps.Web.Base;

namespace NetSteps.Web.Handlers
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Generated a image of a gradient of specified colors in parameters.
	/// Based off of code from: http://www.codeproject.com/KB/aspnet/GradientHandler.aspx
	/// Created: 03-18-2009
	/// </summary>
	public class GradientHandler : BaseHttpHandler
	{
		#region Constants
		private const string ORIENTATION_PARAM = "o";
		private const string LENGTH_PARAM = "l";
		private const string WIDTH_PARAM = "w";
		private const string START_COLOR_PARAM = "StartColor";
		private const string FINISH_COLOR_PARAM = "FinishColor";
		private const string ROUND_CORNER_DIAMATER = "rc";
		#endregion

		#region Members
		private ImageFormat _formatType = ImageFormat.Png;
		private bool _cacheImages = WebContext.WebConfig.CacheHandlerImages;
		private TimeSpan _cacheTime = new TimeSpan(60, 0, 0, 0);
		private bool _roundCorners = false;
		private int _roundCornersDiamater = 0;
		#endregion

		#region Methods
		private Color GetColorFromHexString(string colorString)
		{
			int colorInt = int.Parse(colorString, NumberStyles.HexNumber);
			Color baseColor = Color.FromArgb(colorInt);
			return Color.FromArgb(byte.MaxValue, baseColor);
		}

		public static string GetUrl(string baseUrl, NameValueCollection parameters)
		{
			return baseUrl + "?" + UrlEncode(parameters);
		}

		public static string GetUrl(string baseUrl, string param1Name, object param1Value)
		{
			NameValueCollection nvc = new NameValueCollection(1);
			nvc.Add(param1Name, param1Value == null ? null : param1Value.ToString());

			return GetUrl(baseUrl, nvc);
		}

		public static string UrlEncode(NameValueCollection parameters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool first = true;
			foreach (string parameter in parameters)
			{
				if (first)
				{
					first = false;
				}
				else
				{
					stringBuilder.Append("&");
				}
				stringBuilder.Append(parameter);
				stringBuilder.Append("=");

				string value = parameters[parameter];
				if (value != null)
				{
					string encodedValue = HttpUtility.UrlEncode(value);
					encodedValue = encodedValue.Replace("+", "%20");
					stringBuilder.Append(encodedValue);
				}
			}
			return stringBuilder.ToString();
		}

		public System.Drawing.Image RoundCorners(System.Drawing.Image src, int roundedDiameter)
		{
			_mimeType = "image/png";
			_formatType = ImageFormat.Png;

			return src.RoundCorners(roundedDiameter);
		}
		#endregion Methods

		#region BaseHttpHandler Overrides
		public override void SetResponseCachePolicy(HttpCachePolicy cache)
		{
			cache.SetCacheability(HttpCacheability.Public);
			//cache.SetNoStore();
			cache.SetExpires(DateTime.Now.AddDays(60));
		}

		/// <summary>
		/// Main interface for reacting to the Thumbnailer request.
		/// </summary>
		/// <param name="context"></param>
		protected override void HandleRequest(HttpContext context)
		{
			_mimeType = "image/png";
			_formatType = ImageFormat.Png;

			bool useCache = false;
			if (context.Cache[Cache] != null && _cacheImages)
				useCache = true;

			if (!useCache)
			{
				//SetMimeType(ImageFormat.Png);

				context.Response.Cache.SetCacheability(HttpCacheability.Private);
				context.Response.Cache.SetExpires(DateTime.Now.AddYears(100));

				Orientation orientation = GetQueryStringVar<Orientation>(ORIENTATION_PARAM, Orientation.Vertical);
				string orientationString = context.Request.QueryString[ORIENTATION_PARAM];
				int length = GetQueryStringVar<int>(LENGTH_PARAM, 100);
				int imageWidth = GetQueryStringVar<int>(WIDTH_PARAM, 0);
				string startColorString = context.Request.QueryString[START_COLOR_PARAM];
				string finishColorString = context.Request.QueryString[FINISH_COLOR_PARAM];

				_roundCorners = false;
				_roundCornersDiamater = GetQueryStringVar<int>(ROUND_CORNER_DIAMATER, 0);
				if (_roundCornersDiamater != 0)
					_roundCorners = true;

				// parse query string parameters
				Color startColor = (string.IsNullOrEmpty(startColorString) ? Color.Black : this.GetColorFromHexString(startColorString));
				Color finishColor = (string.IsNullOrEmpty(finishColorString) ? Color.White : this.GetColorFromHexString(finishColorString));

				// calculate geometry based on orientation and length
				int defaultWidth = (imageWidth != 0) ? imageWidth : 1;
				int width = (orientation == Orientation.Horizontal ? length : defaultWidth);
				int height = (orientation == Orientation.Horizontal ? defaultWidth : length);
				Point endPoint = new Point(orientation == Orientation.Horizontal ? length : 0, orientation == Orientation.Horizontal ? 0 : length);
				Rectangle rectangle = new Rectangle(Point.Empty, new Size(width, height));

				using (Bitmap bitmap = new Bitmap(width, height))
				using (Graphics graphics = Graphics.FromImage(bitmap))
				using (Brush brush = new LinearGradientBrush(Point.Empty, endPoint, startColor, finishColor))
				using (MemoryStream memoryStream = new MemoryStream())
				{
					context.Response.ContentType = "image/png";

					graphics.FillRectangle(brush, rectangle);

					System.Drawing.Image result = (System.Drawing.Image)bitmap.Clone();

					if (_roundCorners)
						result = (System.Drawing.Image)RoundCorners(bitmap, _roundCornersDiamater);

					// memory stream required because of bug in GDI+ when saving PNG directly to Response.OutputStream - JHE
					context.Cache.Insert(Cache, (System.Drawing.Image)result.Clone(), null, System.Web.Caching.Cache.NoAbsoluteExpiration, _cacheTime);

					context.Response.ContentType = ContentMimeType;
					EnsureIsImageMimeType(context);

					result.Save(memoryStream, ImageFormat.Png);

					memoryStream.WriteTo(context.Response.OutputStream);
				}
			}
			else
			{
				if (!string.IsNullOrEmpty(context.Request.ServerVariables["HTTP_IF_NONE_MATCH"]) &&
				context.Request.ServerVariables["HTTP_IF_NONE_MATCH"].Equals(UrlHash))
				{
					context.Response.Status = "304 Not Modified";
				}
				else
				{
					using (MemoryStream ms = new MemoryStream())
					{
						((System.Drawing.Image)context.Cache[Cache]).Save(ms, _formatType);
						context.Response.ContentType = ContentMimeType;
						EnsureIsImageMimeType(context);
						ms.WriteTo(context.Response.OutputStream);
						ms.Dispose();
					}
				}
			}
		}
		#endregion BaseHttpHandler Overrides
	}
}
