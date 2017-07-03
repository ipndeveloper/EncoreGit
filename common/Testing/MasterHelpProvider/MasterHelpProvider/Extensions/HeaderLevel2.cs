using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;
using WatiN.Core.Native;

namespace WatiN.Core.Extras
{
	/// <summary>
	/// This class provides specialized functionality for a HTML h2 element.
	/// </summary>
	[ElementTag("h2")]
	public class HeaderLevel2 : ElementContainer<HeaderLevel2>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Div"/> class.
		/// Mainly used by WatiN internally.
		/// </summary>
		/// <param name="domContainer">The DOM container.</param>
		/// <param name="htmlH2Element">The HTML h2 element.</param>
		public HeaderLevel2(DomContainer domContainer, INativeElement htmlH2Element) : base(domContainer, htmlH2Element) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="Div"/> class.
		/// Mainly used by WatiN internally.
		/// </summary>
		/// <param name="domContainer">The DOM container.</param>
		/// <param name="finder">The HTML h2 element.</param>
		public HeaderLevel2(DomContainer domContainer, ElementFinder finder) : base(domContainer, finder) { }

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <returns></returns>
		public string GetName()
		{
			return WatinFrameworkConstants.HeaderLevel2TagName;
		}
	}
}
