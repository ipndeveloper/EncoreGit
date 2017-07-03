using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities
{
	public partial class HtmlContent
	{
		#region Fields

		private string _basePath = string.Empty;

		private ImageList _images;

		#endregion

		#region Properties

		public string BasePath
		{
			get { return _basePath; }
			set { _basePath = value; }
		}

		public ImageList Images
		{
			get
			{
				if (_images == null)
					_images = new ImageList(this);
				return _images;
			}
		}

		public string GetCaption()
		{
			return GetFirstHtmlElement(Constants.HtmlElementType.Caption);
		}

		public void SetCaption(string value)
		{
			SetFirstHtmlElement(Constants.HtmlElementType.Caption, value);
		}

		public string GetImage()
		{
			return GetFirstHtmlElement(Constants.HtmlElementType.Image);
		}

		public void SetImage(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				HtmlElement htmlElement = this.FirstOrNewElement(Constants.HtmlElementType.Image);
				htmlElement.StartTracking();
				htmlElement.HtmlContentID = this.HtmlContentID;
				htmlElement.Contents = new XElement("Image", new XElement("Src", value)).ToString();
			}
		}

		public string GetLink()
		{
			return GetFirstHtmlElement(Constants.HtmlElementType.Link);
		}

		public void SetLink(string value)
		{
			SetFirstHtmlElement(Constants.HtmlElementType.Link, value);
		}


		public string GetBody()
		{
			return GetFirstHtmlElement(Constants.HtmlElementType.Body);
		}

		public void SetBody(string value)
		{
			SetFirstHtmlElement(Constants.HtmlElementType.Body, value);
		}

		public string GetTitle()
		{

			return GetFirstHtmlElement(Constants.HtmlElementType.Title);
		}

		public void SetTitle(string value)
		{
			SetFirstHtmlElement(Constants.HtmlElementType.Title, value);
		}


		public string GetHead()
		{
			return GetFirstHtmlElement(Constants.HtmlElementType.Head);
		}
		
		public void SetHead(string value)
		{
			SetFirstHtmlElement(Constants.HtmlElementType.Head, value);
		}


		private string GetFirstHtmlElement(Constants.HtmlElementType htmlElementType)
		{
			// Not calling FirstOrNewElement here because we don't want to create a new empty HtmlElement when reading the value; only when setting the value - JHE
			return this.HtmlElements.Count(n => n.HtmlElementTypeID == (int)htmlElementType && n.Active) > 0 ? this.HtmlElements.First(n => n.HtmlElementTypeID == (int)htmlElementType && n.Active).BuildElement(false) : string.Empty;
		}

		private void SetFirstHtmlElement(Constants.HtmlElementType htmlElementType, string value)
		{
			//if((int)Constants.HtmlElementType.NotSet == 0)
			if (value != null)
			{
				HtmlElement htmlElement = this.FirstOrNewElement(htmlElementType);
				htmlElement.StartTracking();
				htmlElement.HtmlContentID = this.HtmlContentID;
				htmlElement.Contents = value;
			}
		}
		#endregion

		#region Static Properties

		private static string BaseNavigationPath
		{
			get
			{
				Site currentSite = (Site)HttpContext.Current.Session["CurrentSite"];
				string tempPath = currentSite.PrimaryUrl.Url;
				if (HttpContext.Current.Request.Url.Scheme == "https" && !tempPath.StartsWith("https"))
				{
					tempPath.Replace("http://", "https://");
				}

				if (tempPath.EndsWith("/"))
				{
					tempPath = tempPath.Substring(0, tempPath.Length - 1);
				}
				return tempPath;
			}
		}

		#endregion

		#region Methods

		public static HtmlContent FindBySectionNameAndSiteId(string sectionName, int siteID)
		{
			// TODO: Finish implementing this. - JHE
			throw new NotImplementedException();
		}

		public static List<HtmlContentAccountStatus> GetContentAndDistributorNameByStatus(int statusID)
		{
			try
			{
				return Repository.GetContentAndDistributorNameByStatus(statusID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public void Disapprove(string comments)
		{
			try
			{
				this.HtmlContentStatusID = Constants.HtmlContentStatus.Disapproved.ToInt();
				this.Save();

				HtmlContentHistory htmlContentHistory = new HtmlContentHistory()
				{
					HtmlContentID = this.HtmlContentID,
					HtmlContentStatusID = this.HtmlContentStatusID,
					UserID = ApplicationContext.Instance.CurrentUser.UserID,
					HistoryDate = DateTime.Now,
					Comments = comments
				};
				htmlContentHistory.Save();
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public HtmlElement AddNewBodyHtmlElement(string content)
		{
			return AddNewHtmlElement(Constants.HtmlElementType.Body, content);
		}

		public HtmlElement AddNewHtmlElement(Constants.HtmlElementType htmlElementType, string content)
		{
			HtmlElement htmlElement = new HtmlElement();

			htmlElement.StartEntityTracking();
			htmlElement.HtmlContentID = this.HtmlContentID;
			htmlElement.HtmlElementTypeID = htmlElementType.ToByte();
			htmlElement.Contents = content;
			htmlElement.Active = true;

			this.HtmlElements.Add(htmlElement);
			return htmlElement;
		}
		#endregion

		#region Html Helper Methods

		public string ParseElement(string elementName, string elementClass, out int startIndex)
		{
			throw new NotImplementedException("Finish porting this functionality if needed. - JHE");

			//startIndex = Html.IndexOf(String.Format("<{0} class=\"{1}\"", elementName, elementClass));

			//if (startIndex > -1)
			//{
			//    // 11-14-2008 JLS
			//    // The following line was cutting off the last two character of the tag, so instead of
			//    // returning <img ...></img> it was returning <img ...></im
			//    // To fix this I've changed the hardcoded value of 14 to 16.
			//    //int endIndex = Html.IndexOf(String.Format("<!--end{0}-->", elementClass)) + 16 + elementClass.Length;
			//    //2009-07-1 CJH
			//    // The following line was being stupid again...attempting to make it more dynamic
			//    int endIndex = Html.IndexOf(String.Format("<!--end{0}-->", elementClass)) + 10 /*for: <!--end-->*/ + elementClass.Length + 3 /*for: </>*/ + elementName.Length;
			//    return Html.Substring(startIndex, endIndex - startIndex);
			//}
			//return "";    //2008-11-21 BDB - I want to add a non-empty string for testing purposes, and as a possible hack/fix
			////Returning an empty string prevents the image uploader stuff from displaying
		}

		public string ParseAttribute(string element, string attributeName)
		{
			int startIndex = element.IndexOf(String.Format("{0}=\"", attributeName));
			if (startIndex != -1)
			{
				startIndex += attributeName.Length + 2;

				int endIndex = element.IndexOf("\"", startIndex);
				return element.Substring(startIndex, endIndex - startIndex);
			}
			return string.Empty;
		}

		public string ParseElementInnerText(string element, string elementClass)
		{
			int startIndex = element.IndexOf("\">") + 2;
			if (startIndex > 1)
			{
				int endIndex = element.IndexOf(String.Format("<!--end{0}-->", elementClass));
				return element.Substring(startIndex, endIndex - startIndex);
			}
			return string.Empty;
		}

		#endregion

		#region Internal Helper Methods

		// NOTE: The htmlSectionToClone variable passed in must be fully loaded. - JHE
		internal static HtmlContent CloneHtmlContent(HtmlContent existingHtmlContent)
		{
			try
			{
				return BusinessLogic.CloneHtmlContent(existingHtmlContent);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		#endregion
	}
}
