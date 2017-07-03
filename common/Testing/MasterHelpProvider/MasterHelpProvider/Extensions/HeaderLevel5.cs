using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;
using WatiN.Core.Native;

namespace WatiN.Core.Extras
{
	/// <summary>
	/// This class provides specialized functionality for a HTML h5 element.
	/// </summary>
	[ElementTag("h5")]
	public class HeaderLevel5 : ElementContainer<HeaderLevel5>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Div"/> class.
		/// Mainly used by WatiN internally.
		/// </summary>
		/// <param name="domContainer">The DOM container.</param>
		/// <param name="htmlH4Element">The HTML h5 element.</param>
		public HeaderLevel5(DomContainer domContainer, INativeElement htmlH5Element) : base(domContainer, htmlH5Element) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="Div"/> class.
		/// Mainly used by WatiN internally.
		/// </summary>
		/// <param name="domContainer">The DOM container.</param>
		/// <param name="finder">The HTML h5 element.</param>
		public HeaderLevel5(DomContainer domContainer, ElementFinder finder) : base(domContainer, finder) { }

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <returns></returns>
		public string GetName()
		{
			return WatinFrameworkConstants.HeaderLevel5TagName;
		}
	}
}
