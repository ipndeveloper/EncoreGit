using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;
using WatiN.Core.Native;
using WatiN.Core.Extras;

namespace WatiN.Core
{
	/// <summary>
	/// This class provides specialized functionality for a pre element.
	/// </summary>
	[ElementTag("pre")]
	public class PreFormattedText : ElementContainer<Code>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PreFormattedText"/> class.
		/// Mainly used by WatiN internally.
		/// </summary>
		/// <param name="domContainer">The DOM container.</param>
		/// <param name="htmlCodeElement">The HTML pre element.</param>
		public PreFormattedText(DomContainer domContainer, INativeElement htmlCodeElement) : base(domContainer, htmlCodeElement) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="PreFormattedText"/> class.
		/// Mainly used by WatiN internally.
		/// </summary>
		/// <param name="domContainer">The DOM container.</param>
		/// <param name="finder">The HTML pre element.</param>
		public PreFormattedText(DomContainer domContainer, ElementFinder finder) : base(domContainer, finder) { }

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <returns></returns>
		public string GetName()
		{
			return WatinFrameworkConstants.PreformattedTextTagName;
		}
	}
}
