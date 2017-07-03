using System;

using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Extensions
{
	using System.Linq;
	using System.Text;

	public static class HtmlContentExtensions
	{
		public static string BuildContent(this HtmlContent content)
		{
			StringBuilder builder = new StringBuilder();
			if (content != null && content.HtmlElements != null)
			{
				foreach (HtmlElement element in content.HtmlElements.OrderBy(e => e.SortIndex))
				{
					try
					{
						builder.Append(element.BuildElement());
					}
					catch (Exception ex)
					{
						EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
					}
				}
			}
			return builder.ToString();
		}

		public static HtmlElement FirstOrEmptyElement(this HtmlContent content, Constants.HtmlElementType elementType)
		{
			if (content == null || content.HtmlElements == null)
				return new HtmlElement()
				{
					HtmlElementTypeID = (byte)elementType
				};
			return content.HtmlElements.FirstOrDefault(e => e.HtmlElementTypeID == (int)elementType) ?? new HtmlElement()
			{
				HtmlElementTypeID = (byte)elementType
			};
		}

		public static HtmlElement FirstOrNewElement(this HtmlContent content, Constants.HtmlElementType elementType)
		{
			if (content == null || content.HtmlElements == null)
				content.HtmlElements = new TrackableCollection<HtmlElement>();
			HtmlElement element = content.HtmlElements.FirstOrDefault(e => e.HtmlElementTypeID == (int)elementType);
			if (element == default(HtmlElement))
			{
				element = new HtmlElement()
				{
					HtmlElementTypeID = (byte)elementType,
					SortIndex = content.HtmlElements.Count == 0 ? (byte)0 : (byte)(content.HtmlElements.Max(e => e.SortIndex) + 1),
					Active = true
				};
				content.HtmlElements.Add(element);
			}
			return element;
		}

		public static string Title(this HtmlContent content)
		{
			return content.FirstOrEmptyElement(Constants.HtmlElementType.Title).Contents;
		}

		public static string Body(this HtmlContent content)
		{
			return content.FirstOrEmptyElement(Constants.HtmlElementType.Body).Contents;
		}

		public static string Caption(this HtmlContent content)
		{
			return content.FirstOrEmptyElement(Constants.HtmlElementType.Caption).Contents;
		}
	}
}
