using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Web;
using NetSteps.Common;
using NetSteps.Common.Extensions;
using NetSteps.Web.Base;
using NetSteps.Web.Imaging;

namespace NetSteps.Web.Handlers
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Handler that a URL parameter to a web site and outputs an screen shot(image) of the site.
	/// Created: 04-03-2009
	/// </summary>
	public class WebPageThumbnailHandler : BaseHttpHandler
	{
		#region Enums
		/// <summary>
		/// An internal enumeration defining the thumbnail sizes.
		/// </summary>
		internal enum ThumbnailSizeType
		{
			Small = 75,
			Medium = 150,
			Large = 200
		}
		#endregion

		#region Constants
		private const string IMG_PARAM = "img"; // image parameter
		private const string SIZE_PARAM = "size"; // size parameter
		private const string MAXH_PARAM = "h"; // size parameter
		private const string MAXW_PARAM = "w"; // size parameter
		private const string TRIM_PARAM = "trim";
		private const string ROUND_CORNER_DIAMATER = "rc";
		private const string DEFAULT_THUMBNAIL = "ImageNotFound.jpg";
		private const string LOCATED_ON_SAN = "san";
		private const string PAGE_URL = "url";
		private const string CROP_H_PARAM = "ch";
		private const string CROP_W_PARAM = "cw";
		#endregion

		#region Members
		private bool _cacheImages = WebContext.WebConfig.CacheHandlerImages;
		private TimeSpan _cacheTime = new TimeSpan(60, 0, 0, 0);

		private ImageFormat _formatType = ImageFormat.Gif;
		private int _maxSize = 0;
		private int _maxWidth = -1;
		private int _maxHeight = -1;
		private bool _isSizeSet = false;
		private bool _trimWhiteSpace = false;
		private string _pageUrl = string.Empty;
		private int _cropHeight = -1;
		private int _cropWidth = -1;
		#endregion

		#region Methods
		/// <summary>
		/// Determines if the img parameter is a valid image.
		/// </summary>
		/// <param name="fileName">File name from the img parameter.</param>
		/// <returns>
		///   <c>true</c> if valid image, otherwise <c>false</c>
		/// </returns>
		private bool IsValidImage(string fileName)
		{
			string ext = IO.GetFileExtention(fileName);
			bool isValid = false;
			switch (ext)
			{
				case ".jpg":
				case ".jpeg":
					isValid = true;
					_mimeType = "image/jpeg";
					_formatType = ImageFormat.Jpeg;
					break;
				case ".gif":
					isValid = true;
					_mimeType = "image/gif";
					_formatType = ImageFormat.Gif;
					break;
				case ".png":
					isValid = true;
					_mimeType = "image/png";
					_formatType = ImageFormat.Png;
					break;
				case ".bmp":
					isValid = true;
					_mimeType = "image/bmp";
					_formatType = ImageFormat.Bmp;
					break;
				default:
					isValid = false;
					break;
			}
			return isValid;
		}

		private int GetSize(string size)
		{
			int sizeVal;
			if (!Int32.TryParse(size.Trim(), System.Globalization.NumberStyles.Integer, null, out sizeVal))
				sizeVal = (int)ThumbnailSizeType.Small;

			try
			{
				return sizeVal;
			}
			catch
			{
				return -1;
			}
		}

		/// <summary>
		/// This method generates the actual thumbnail.
		/// </summary>
		/// <param name="image"></param>
		/// <returns>Thumbnail image</returns>
		private System.Drawing.Image CreateThumbnail(System.Drawing.Image image)
		{
			int maxSize = _maxSize;

			int w = image.Width;
			int h = image.Height;

			if (_isSizeSet)
			{
				if (w > maxSize || h > maxSize)
				{
					if (_maxWidth > 0)
					{
						if (w > _maxWidth)
						{
							h = (h * _maxWidth) / w;
							w = _maxWidth;
						}
					}
					else
					{
						if (w > maxSize)
						{
							h = (h * maxSize) / w;
							w = maxSize;
						}
					}

					if (_maxHeight > 0)
					{
						if (h > _maxHeight)
						{
							w = (w * _maxHeight) / h;
							h = _maxHeight;
						}
					}
					else
					{
						if (h > maxSize)
						{
							w = (w * maxSize) / h;
							h = maxSize;
						}
					}
				}
			}

			// The third parameter is required and is of type delegate.  Rather then create a method that
			// does nothing, .NET 2.0 allows for anonymous delegate (similar to anonymous functions in other languages).
			return image.GetThumbnailImage(w, h, delegate() { return false; }, IntPtr.Zero);
		}

		/// <summary>
		/// Get default image.
		/// </summary>
		/// <remarks>
		/// This method is only invoked when there is a problem with the parameters.
		/// </remarks>
		/// <param name="context"></param>
		private void GetDefaultImage(HttpContext context)
		{
			IsValidImage(DEFAULT_THUMBNAIL);
			string file = string.Format("NetSteps.WebControls.Resources.{0}", DEFAULT_THUMBNAIL);
			string cache = file;

			using (Stream imgStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(file))
			{
				if (imgStream != null)
				{
					using (Bitmap bmp = (Bitmap.FromStream(imgStream) as Bitmap))
					using (System.Drawing.Image tn = CreateThumbnail((System.Drawing.Image)bmp))
					{
						tn.Save(context.Response.OutputStream, _formatType);
						imgStream.Close();
					}
				}
			}
		}
		#endregion Methods

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
			_mimeType = "image/png";
			_formatType = ImageFormat.Jpeg;

			#region Get Size
			_isSizeSet = false;
			if (!string.IsNullOrEmpty(context.Request.QueryString[MAXH_PARAM]))
			{
				_maxHeight = GetSize(context.Request.QueryString[MAXH_PARAM]);
				_isSizeSet = true;
			}
			if (!string.IsNullOrEmpty(context.Request.QueryString[MAXW_PARAM]))
			{
				_maxWidth = GetSize(context.Request.QueryString[MAXW_PARAM]);
				_isSizeSet = true;
			}
			if (!string.IsNullOrEmpty(context.Request.QueryString[SIZE_PARAM]))
			{
				_isSizeSet = true;
				_maxSize = GetSize(context.Request.QueryString[SIZE_PARAM]);
			}

			if (_maxSize == 0)
			{
				if (_maxHeight <= 0 && _maxWidth != 0)
					_maxSize = _maxWidth;
				else if (_maxHeight != 0 && _maxWidth <= 0)
					_maxSize = _maxHeight;
			}

			_cropHeight = GetQueryStringVar<int>(CROP_H_PARAM, -1);
			_cropWidth = GetQueryStringVar<int>(CROP_W_PARAM, -1);
			_pageUrl = GetQueryStringVar<string>(PAGE_URL);
			#endregion

			bool isTinyImage = false;
			if (_maxSize <= 100)
				isTinyImage = true;

			if (isTinyImage)
				_cacheImages = WebContext.WebConfig.CacheTinyHandlerImages;
			else
				_cacheImages = WebContext.WebConfig.CacheHandlerImages;

			bool useCache = false;
			if (context.Cache[Cache] != null && _cacheImages)
				useCache = true;

			if (!useCache)
			{
				_trimWhiteSpace = GetQueryStringVar<bool>(TRIM_PARAM, false);

				// Write Image - JHE
				WebPageThumbnail webPageThumbnail = new WebPageThumbnail();
				using (Bitmap im = webPageThumbnail.GenerateBitmap(_pageUrl, _cropWidth, _cropHeight))
				using (System.Drawing.Image tn = CreateThumbnail((_trimWhiteSpace) ? im.TrimWhiteSpace() : im))
				{
					MemoryStream ms = new MemoryStream();

					Image result = (Image)tn.Clone();
					AddImageToCache(context, result, _cacheTime);
					result.Save(ms, _formatType);

					context.Response.ContentType = ContentMimeType;
					ms.WriteTo(context.Response.OutputStream);
					ms.Dispose();
				}
			}
			else
			{
				if (!string.IsNullOrEmpty(context.Request.ServerVariables["HTTP_IF_NONE_MATCH"]) &&
				context.Request.ServerVariables["HTTP_IF_NONE_MATCH"].Equals(UrlHash))
				{
					context.Response.ClearHeaders();
					context.Response.ContentType = ContentMimeType;
					context.Response.AppendHeader("Etag", UrlHash);
					context.Response.Status = "304 Not Modified";
					context.Response.AppendHeader("Content-Length", "0");
				}
				else
				{
					using (MemoryStream ms = new MemoryStream())
					{
						((Image)context.Cache[Cache]).Save(ms, _formatType);
						context.Response.ContentType = ContentMimeType;
						ms.WriteTo(context.Response.OutputStream);
						ms.Dispose();
					}
				}
			}
		}
		#endregion
	}
}
