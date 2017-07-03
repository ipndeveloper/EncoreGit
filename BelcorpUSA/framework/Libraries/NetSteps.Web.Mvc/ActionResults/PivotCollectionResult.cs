using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using NetSteps.Common.Extensions;

namespace NetSteps.Web.Mvc.ActionResults
{
	public class PivotCollectionResult<T> : ActionResult
	{
		private List<string> _properties = new List<string>();
		private bool _useFields = false;
		private Regex _charactersToEscape = new Regex("[\",\n]", RegexOptions.Compiled);
		private Regex _pascalToSpaced = new Regex("([A-Z])", RegexOptions.Compiled);

		private Type _dataType = null;
		private Type DataType
		{
			get
			{
				return _dataType;
			}
			set
			{
				_dataType = value;
			}
		}

		public string FileName
		{
			get;
			set;
		}

		public IEnumerable<T> Data
		{
			get;
			set;
		}

		public List<string> Properties
		{
			get { return _properties; }
		}

		public PivotCollectionResult(string fileName, IEnumerable<T> data)
		{
			FileName = fileName;
			Data = data;
			_useFields = true;

			if (data != null)
			{
				var first = data.FirstOrDefault();
				if (first != null)
					DataType = first.GetType();
			}

			if (DataType == null)
				DataType = typeof(T);

			foreach (PropertyInfo property in DataType.GetPropertiesCached().Where(p => p.CanRead && p.GetGetMethod().IsPublic))
			{
				Properties.Add(property.Name);
			}
		}

		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}




			string docHeader = "<?xmlversion=\"1.0\"?> <Collection Name=\"Downline Pivot Data Test\" SchemaVersion=\"1.0\" xmlns=\"http://schemas.microsoft.com/collection/metadata/2009\" xmlns:p=\"http://schemas.microsoft.com/livelabs/pivot/collection/2009\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">";

			StringBuilder doc = new StringBuilder(docHeader);



			//StringBuilder result = new StringBuilder();
			if (_useFields)
			{
				if (Properties.Count == 0)
					throw new InvalidOperationException("No properties to pull");
				if (Data.Any())
				{
					doc.Append("<FacetCategories>");
					doc.Append("<FacetCategory Name=\"FirstName\" Type=\"String\"/>");
					doc.Append("<FacetCategory Name=\"LastName\" Type=\"String\"/>");
					doc.Append("</FacetCategories>");

					doc.Append("<Items ImgBase=\"helloworld.dzc\">");

					int i = 0;
					var list = Data as IEnumerable<dynamic>;
					foreach (var item in list)
					{
						doc.Append(string.Format("<Item Img=\"#{0}\" Id=\"{0}\" Href=\"#\" Name=\"{1} {2}\">", i, item.FirstName ?? string.Empty, item.LastName ?? string.Empty));
						doc.Append("<Facets>");
						doc.Append(string.Format("<Facet Name=\"FirstName\"><String Value=\"{0}\"/></Facet>", item.FirstName ?? string.Empty));
						doc.Append(string.Format("<Facet Name=\"LastName\"><String Value=\"{0}\"/></Facet>", item.LastName ?? string.Empty));
						doc.Append("</Facets>");
						doc.Append("</Item>");
						i++;
					}
					doc.Append("</Items>");
					doc.Append("</Collection>");



				}
			}
			else
			{
				foreach (T item in Data)
				{
					doc.Append(item.ToString() + Environment.NewLine);
				}
			}

			context.HttpContext.Response.Clear();
			if (!string.IsNullOrEmpty(FileName))
			{
				ContentDisposition cd = new ContentDisposition()
				{
					FileName = FileName
				};
				context.HttpContext.Response.AddHeader("Content-Disposition", cd.ToString());
			}
			context.HttpContext.Response.ContentType = "text/xml";
			context.HttpContext.Response.Charset = "65001";
			byte[] b = new byte[] { 0xEF, 0xBB, 0xBF };
			context.HttpContext.Response.BinaryWrite(b);
			context.HttpContext.Response.Write(doc.ToString().TrimEnd(Environment.NewLine.ToCharArray()));
			context.HttpContext.Response.Flush();
			context.HttpContext.Response.End();
		}
	}
}
