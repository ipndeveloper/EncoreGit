using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Linq;
using System.Xml.Linq;
using NetSteps.Encore.Core.Properties;

namespace NetSteps.Encore.Core.Xml
{
	/// <summary>
	/// Static class for creating dynamic objects over XML
	/// </summary>
	public static class XDynamic
	{
		/// <summary>
		/// Parses the input text and returns a dynamic object.
		/// </summary>
		/// <param name="text">source xml text</param>
		/// <param name="includeRootObject">whether or not the root object is included in the structure of the resulting dynamic</param>
		/// <returns>a dynamic object shaped like the input xml</returns>
		public static dynamic Parse(string text, bool includeRootObject)
		{			
			Contract.Requires(text != null, Resources.Chk_CannotBeNull);
			Contract.Requires(text.Length > 0, Resources.Chk_CannotBeEmpty);

			var xml = XDocument.Parse(text);
			if (includeRootObject)
			{
				var expando = new ExpandoObject() as IDictionary<string, object>;
				expando.Add(xml.Root.Name.LocalName, ToDynamic(xml.Root));
				return expando;
			}
			else
			{
				return ToDynamic(xml.Root);
			}			
		}

		/// <summary>
		/// Parses the input text and returns a dynamic object.
		/// </summary>
		/// <param name="text">source xml text</param>
		/// <returns>a dynamic object shaped like the input xml</returns>
		public static dynamic Parse(string text)
		{
			return Parse(text, false);
		}

		/// <summary>
		/// Creates an object over the XElement given.
		/// </summary>
		/// <param name="elm">the source element</param>
		/// <returns>an object shaped like the input xml</returns>
		public static object ToDynamic(XElement elm)
		{
			if (elm.IsEmpty)
			{
				var expando = new ExpandoObject() as IDictionary<string, object>;
				AddAttributesToDictionary(expando, elm);
				return expando;
			}
			else if (!elm.HasAttributes)
			{
				if (elm.HasElements)
				{
					var expando = new ExpandoObject() as IDictionary<string, object>;
					AddElementsToDictionary(expando, elm);
					return expando;
				}
				else return elm.Value;
			}
			else 
			{
				var expando = new ExpandoObject() as IDictionary<string, object>;
				AddAttributesToDictionary(expando, elm);
				if (elm.HasElements) AddElementsToDictionary(expando, elm);
				else expando.Add("Value", elm.Value);				
				return expando;				
			}
		}

		/// <summary>
		/// Adds attributes from an element into the dictionary given.
		/// </summary>
		/// <param name="expando">target dictionary</param>
		/// <param name="elm">source element</param>
		private static void AddAttributesToDictionary(IDictionary<string, object> expando, XElement elm)
		{
			foreach (var a in elm.Attributes())
			{
				expando.Add(a.Name.LocalName, a.Value);
			}
		}

		/// <summary>
		/// Adds child elements from an element into the dictionary given.
		/// </summary>
		/// <param name="expando">target dictionary</param>
		/// <param name="elm">source element</param>
		private static void AddElementsToDictionary(IDictionary<string, object> expando, XElement elm)
		{
			foreach (var gg in from e in elm.Elements()
												 group e by e.Name.LocalName into g
												 select g)
			{
				if (gg.Count() > 1)
				{
					expando.Add(gg.Key, new List<dynamic>(from item in gg
																								select ToDynamic(item)));
				}
				else expando.Add(gg.Key, ToDynamic(gg.Single()));
			}
		}
	}
}
