using System.Text;
using System.Xml.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Cache;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class HtmlElementBusinessLogic
	{
		public override void DefaultValues(Repositories.IHtmlElementRepository repository, HtmlElement entity)
		{
			base.DefaultValues(repository, entity);
			entity.Active = true;
		}

		public string BuildElement(HtmlElement element, bool wrap = true)
		{
			var builder = new StringBuilder();
			HtmlElementType elementType = SmallCollectionCache.Instance.HtmlElementTypes.GetById(element.HtmlElementTypeID);
			if (!string.IsNullOrEmpty(elementType.ContainerTagName) && wrap)
			{
				builder.Append("<").Append(elementType.ContainerTagName).Append(string.IsNullOrEmpty(elementType.ContainerCssClass) ? "" : " class=\"" + elementType.ContainerCssClass + "\"").Append(">");
			}

			XElement xml;
			switch (elementType.HtmlElementTypeID)
			{
				case (int)Constants.HtmlElementType.Image:
					xml = XElement.Parse(element.Contents);
					builder.Append("<img src=\"").Append(xml.Descendant("Src").Value.ReplaceFileUploadPathToken()).Append("\" alt=\"").Append(xml.Descendant("Alt").Value).Append("\"")
						.Append(!string.IsNullOrEmpty(xml.Descendant("Width").Value) ? " width=\"" + xml.Descendant("Width").Value + "\"" : "")
						.Append(!string.IsNullOrEmpty(xml.Descendant("Height").Value) ? " height=\"" + xml.Descendant("Height").Value + "\"" : "")
						.Append(" />");
					break;
				case (int)Constants.HtmlElementType.Link:
					xml = XElement.Parse(element.Contents);
					builder.Append("<a href=\"").Append(xml.Descendant("Href").Value).Append("\"")
						.Append(!string.IsNullOrEmpty(xml.Descendant("Target").Value) ? " target=\"" + xml.Descendant("Target").Value + "\"" : "")
						.Append(">").Append(xml.Descendant("Text").Value).Append("</a>");
					break;
                case (int)Constants.HtmlElementType.Head:
                    return element.Contents;
				default:
					builder.Append(element.Contents);
					break;
			}

			if (!string.IsNullOrEmpty(elementType.ContainerTagName) && wrap)
				builder.Append("</").Append(elementType.ContainerTagName).Append(">");

			return builder.ToString();
		}
	}
}