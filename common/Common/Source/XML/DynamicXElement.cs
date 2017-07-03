using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NetSteps.Common.XML
{
	/// <summary>
	/// Dynamic wrapper for XML to be able to use the DLR to parse XML
	/// Author: Daniel Stafford
	/// Date: 8/2/2010
	/// </summary>
	public class DynamicXElement : DynamicObject
	{
		XElement node;

		public DynamicXElement(XElement node)
		{
			this.node = node;
		}

		public DynamicXElement()
		{
		}

		public DynamicXElement(string name)
		{
			node = new XElement(name);
		}

		/// <summary>
		/// Set an attribute on an element (i.e. iso.PostalCodeValidation().Enabled = true;)
		/// </summary>
		/// <param name="binder"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			var attributes = node.Attributes(binder.Name);
			int count = attributes.Count();

			if (count > 0)
				Parallel.ForEach(attributes, (a) => a.SetValue(value));
			else
				node.Add(new XAttribute(binder.Name, value));

			return true;
		}

		/// <summary>
		/// Get an attribute on an element (i.e. string enabled = iso.PostalCodeValidation().Enabled)
		/// </summary>
		/// <param name="binder"></param>
		/// <param name="result"></param>
		/// <returns>Always returns a string of the value of the attribute</returns>
		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			var attributes = node.Attributes(binder.Name).Select(attr => attr.Value);
			int count = attributes.Count();

			if (count > 0)
			{
				if (count > 1)
					result = attributes;
				else
					result = attributes.FirstOrDefault();

				return true;
			}

			result = "";
			return true;
		}

		/// <summary>
		/// Handle explicit casting of DynamicXElement to some other value
		/// </summary>
		/// <param name="binder"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public override bool TryConvert(ConvertBinder binder, out object result)
		{
			if (binder.Type == typeof(string))
			{
				result = node.Value;
			}
			else if (binder.Type == typeof(XElement))
			{
				result = node;
			}
			else if (binder.Type == typeof(IEnumerable) || (binder.Type.IsGenericType && binder.Type.GetGenericTypeDefinition() == typeof(IEnumerable<>)) || binder.Type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
			{
				List<dynamic> nodes = new List<dynamic>();
				if (node != null && (node.HasAttributes || node.HasElements))
					nodes.Add(this);
				result = nodes;
			}
			else if (binder.Type != typeof(DynamicXElement))
			{
				TypeConverter converter = TypeDescriptor.GetConverter(binder.Type);
				if (converter == null)
					result = null;
				else
					result = converter.ConvertFrom(node.Value);
			}
			else
			{
				result = null;
				return false;
			}
			return true;
		}

		/// <summary>
		/// Uses method notation to:
		/// 1) retrieve child elements (i.e. iso.PostalCodeValidation())
		/// 2) set child elements (i.e. iso.PostalCodeValidation("Content"))
		/// 3) add child elements (i.e. iso.Add(new XElement("Child"))
		/// 4) invoke xpath (i.e. iso.XPath("//element"))
		/// </summary>
		/// <param name="binder"></param>
		/// <param name="args"></param>
		/// <param name="result"></param>
		/// <returns>A DynamicXElement object containing the specified node(s)</returns>
		public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
		{
			if (binder.Name.Equals("Add", StringComparison.InvariantCultureIgnoreCase))
			{
				foreach (object arg in args)
					node.Add(arg);
				result = this;
				return true;
			}
			if (binder.Name.Equals("XPath", StringComparison.InvariantCultureIgnoreCase) && args.Length > 0)
			{
				result = node.XPathEvaluate(args[0].ToString());
				//TODO: make new DynamicXElements out of the results if the result of the xpath is an IEnumerable<XElement>
				var results = result as IEnumerable;
				if (results == null)
					result = new DynamicXElement(new XElement(binder.Name));
				else
				{
					var e = results.GetEnumerator();

					if (e.MoveNext())
					{
						if (e.Current is XElement)
						{
							var xElements = results.Cast<XElement>();
							if (xElements.Count() == 1)
								result = new DynamicXElement(xElements.First());
							else
								result = xElements.Select(el => new DynamicXElement(el));
						}
						else if (e.Current is XAttribute)
						{
							var xAttributes = results.Cast<XAttribute>();
							if (xAttributes.Count() == 1)
								result = xAttributes.First().Value;
							else
								result = xAttributes.Select(a => a.Value);
						}
						else
							result = e.Current;
					}
				}
				//result = (result as IEnumerable<XElement>);

				//result = node.XPathSelectElements(args[0].ToString()).Select(e => new DynamicXElement(e));
				return true;
			}

			var elements = node.Elements(binder.Name);

			int count = elements.Count();
			if (count > 0)
			{
				if (count > 1)
				{
					if (args.Length > 0)
						Parallel.ForEach(elements, (e) => e.SetValue(args[0]));
					result = elements.Select(e => new DynamicXElement(e));
				}
				else
				{
					XElement element = elements.First();
					if (args.Length > 0)
						element.SetValue(args[0]);
					result = new DynamicXElement(element);
				}

				return true;
			}

			result = new DynamicXElement(new XElement(binder.Name));
			return true;
		}

		public DynamicXElement this[int index]
		{
			get
			{
				if (index == 0)
					return this;
				throw new ArgumentOutOfRangeException("index", "The index was out of bounds.");
			}
		}
	}
}
