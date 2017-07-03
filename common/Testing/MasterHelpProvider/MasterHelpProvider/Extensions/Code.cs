using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;
using WatiN.Core.Native;

namespace WatiN.Core.Extras
{
	/// <summary>
	/// This class provides specialized functionality for a HTML code element.
	/// </summary>
	[ElementTag("code")]
	public class Code : ElementContainer<Code>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Code"/> class.
		/// Mainly used by WatiN internally.
		/// </summary>
		/// <param name="domContainer">The DOM container.</param>
		/// <param name="htmlCodeElement">The HTML code element.</param>
		public Code(DomContainer domContainer, INativeElement htmlCodeElement) : base(domContainer, htmlCodeElement) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="Code"/> class.
		/// Mainly used by WatiN internally.
		/// </summary>
		/// <param name="domContainer">The DOM container.</param>
		/// <param name="finder">The HTML code element.</param>
		public Code(DomContainer domContainer, ElementFinder finder) : base(domContainer, finder) { }

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <returns></returns>
		public string GetName()
		{
			return WatinFrameworkConstants.CodeTagName;
		}
	}
}
