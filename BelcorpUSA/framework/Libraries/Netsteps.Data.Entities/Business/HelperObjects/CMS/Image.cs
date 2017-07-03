using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Business
{
	public class Image
	{
		internal Image(HtmlElement element)
		{
			XElement e = XElement.Parse(element.Contents);
			Src = e.Descendant("Src").Value;
			Width = e.Descendant("Width").Value.ToIntNullable();
			Height = e.Descendant("Height").Value.ToIntNullable();
			Folder = e.Descendant("Folder").Value;

			HtmlElement = element;
		}

		public Image(NetSteps.Common.Constants.ImageFolder folder)
		{
			Folder = folder.ToString();
		}

		public Image(string folder)
		{
			Folder = folder;
		}

		public HtmlElement HtmlElement { get; set; }

		private string _src;
		public string Src
		{
			get { return string.IsNullOrEmpty(_src) ? "" : _src.ReplaceFileUploadPathToken(); }
			set
			{
				if (!string.IsNullOrEmpty(Folder) && (value.Contains(ConfigurationManager.GetWebFolder(Folder)) || value.Contains(ConfigurationManager.GetAbsoluteFolder(Folder))))
					value = value.AddFileUploadPathToken().Replace(ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.FileUploadWebPath), "<!--filepath-->");
				if (_src != value)
				{
					_src = value;
					UpdateElementContents();
				}
			}
		}

		private string _folder;
		public string Folder
		{
			get { return _folder; }
			set
			{
				if (_folder != value)
				{
					_folder = value;
					UpdateElementContents();
				}
			}
		}

		private int? _width;
		public int? Width
		{
			get { return _width; }
			set
			{
				if (_width != value)
				{
					_width = value;
					UpdateElementContents();
				}
			}
		}

		private int? _height;
		public int? Height
		{
			get { return _height; }
			set
			{
				if (_height != value)
				{
					_height = value;
					UpdateElementContents();
				}
			}
		}

		private void UpdateElementContents()
		{
			if (HtmlElement != null)
				HtmlElement.Contents = this.ToString();
		}

		public override string ToString()
		{
			XElement element = new XElement("Image", new XElement("Src", Src));
			if (Width.HasValue)
				element.Add(new XElement("Width", Width.Value));
			if (Height.HasValue)
				element.Add(new XElement("Height", Height.Value));
			if (!string.IsNullOrEmpty(Folder))
				element.Add(new XElement("Folder", Folder));
			return element.ToString();
		}
	}

    [System.Serializable]
	public class ImageList : List<Image>
	{
		private HtmlContent _content;

		public ImageList(HtmlContent content)
		{
			_content = content;
			this.AddRange(content.HtmlElements.Where(e => e.HtmlElementTypeID == (int)Constants.HtmlElementType.Image).Select(e => new Image(e)));
		}

        public ImageList UpdateFolder(NetSteps.Common.Constants.ImageFolder folder)
		{
			foreach (Image i in this)
				i.Folder = folder.ToString();
			return this;
		}

		new public void Add(Image image)
		{
			base.Add(image);

			HtmlElement element;
			if (image.HtmlElement != null)
				element = _content.HtmlElements.First(e => e.HtmlElementID == image.HtmlElement.HtmlElementID);
			else
			{
				element = new HtmlElement();
				_content.HtmlElements.Add(element);
			}

			element.Contents = image.ToString();
			element.HtmlElementTypeID = (int)Constants.HtmlElementType.Image;
			element.Active = true;

			image.HtmlElement = element;
		}

		new public void Remove(Image image)
		{
			_content.HtmlElements.RemoveAndMarkAsDeleted(image.HtmlElement);
			base.Remove(image);
		}
	}
}
