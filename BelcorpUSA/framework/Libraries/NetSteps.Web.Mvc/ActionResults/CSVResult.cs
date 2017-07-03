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
	public class CSVResult<T> : ActionResult
	{
		private List<string> _properties = new List<string>();
		private bool _useFields = false;
		private Regex _charactersToEscape = new Regex("[\",\n]", RegexOptions.Compiled);
		private Regex _pascalToSpaced = new Regex("([A-Z])", RegexOptions.Compiled);

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

		public CSVResult(string fileName, IEnumerable<T> data, params string[] fields)
		{
			FileName = fileName;
			Data = data;
			if (fields.Length > 0)
			{
				_useFields = true;
				_properties = fields.ToList();
			}
		}

		public CSVResult(string fileName, IEnumerable<T> data)
		{
			FileName = fileName;
			Data = data;
			_useFields = true;

			foreach (PropertyInfo property in typeof(T).GetPropertiesCached().Where(p => p.CanRead && p.GetGetMethod().IsPublic))
			{
				Properties.Add(property.Name);
			}
		}

		public CSVResult(string fileName, IEnumerable<T> data, bool useFields)
		{
			FileName = fileName;
			Data = data;
			_useFields = useFields;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			StringBuilder result = new StringBuilder();
			if (_useFields)
			{
				if (Properties.Count == 0)
					throw new InvalidOperationException("No properties to pull");
				if (Data.Any())
				{
					foreach (string property in Properties)
					{
						result.Append(_pascalToSpaced.Replace(property, " $1").Trim()).Append(",");
					}
					result.Remove(result.Length - 1, 1).Append(Environment.NewLine);
					int length = result.Length;
					Type tType = typeof(T);
					foreach (T item in Data)
					{
						foreach (string property in Properties)
						{
							var prop = tType.GetPropertyCached(property);
							if (prop == null)
								throw new InvalidOperationException(string.Format("Property '{0}' does not exist on type '{1}'", property, tType.Name));
							var val = prop.GetValue(item, null);
							if (val != null)
							{
								string value;
								if (val is DateTime || val is DateTime?)
									value = string.Format("\"{0}\"", val.ToString());
								else
								{
									value = val.ToString();
									if (_charactersToEscape.IsMatch(value))
									{
										value = string.Format("\"{0}\"", value.Replace("\"", "\"\""));
									}
								}
								result.Append(value);
							}
							result.Append(",");
						}
						if (result.Length > length)
							result.Remove(result.Length - 1, 1).Append(Environment.NewLine);
					}
				}
			}
			else
			{
				foreach (T item in Data)
				{
					result.Append(item.ToString() + Environment.NewLine);
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
			context.HttpContext.Response.ContentType = "text/csv";
			//context.HttpContext.Response.Charset = "65001";
			//byte[] b = new byte[] { 0xEF, 0xBB, 0xBF };
			//context.HttpContext.Response.BinaryWrite(b);
			context.HttpContext.Response.Write(result.ToString().TrimEnd(Environment.NewLine.ToCharArray()));
			context.HttpContext.Response.Flush();
			context.HttpContext.Response.End();
		}
	}
}
