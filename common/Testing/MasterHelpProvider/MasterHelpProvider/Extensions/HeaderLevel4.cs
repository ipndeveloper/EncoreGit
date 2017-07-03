using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;
using WatiN.Core.Native;

namespace WatiN.Core.Extras
{
	/// <summary>
	/// This class provides specialized functionality for a HTML h4 element.
	/// </summary>
	[ElementTag("h4")]
	public class HeaderLevel4 : ElementContainer<HeaderLevel4>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Div"/> class.
		/// Mainly used by WatiN internally.
		/// </summary>
		/// <param name="domContainer">The DOM container.</param>
		/// <param name="htmlH5Element">The HTML h4 element.</param>
		public HeaderLevel4(DomContainer domContainer, INativeElement htmlH4Element) : base(domContainer, htmlH4Element) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="Div"/> class.
		/// Mainly used by WatiN internally.
		/// </summary>
		/// <param name="domContainer">The DOM container.</param>
		/// <param name="finder">The HTML h4 element.</param>
		public HeaderLevel4(DomContainer domContainer, ElementFinder finder) : base(domContainer, finder) { }

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <returns></returns>
		public string GetName()
		{
			return WatinFrameworkConstants.HeaderLevel4TagName;
		}
	}
}
