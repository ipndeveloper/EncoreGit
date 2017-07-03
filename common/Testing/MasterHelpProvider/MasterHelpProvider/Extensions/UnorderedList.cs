using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;
using WatiN.Core.Native;

namespace WatiN.Core
{
	/// <summary>
	/// This class provides specialized functionality for a HTML ul element.
	/// </summary>
	[ElementTag("ul")]
	public class UnorderedList : ElementContainer<UnorderedList>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Div"/> class.
		/// Mainly used by WatiN internally.
		/// </summary>
		/// <param name="domContainer">The DOM container.</param>
		/// <param name="htmlUlElement">The HTML ul element.</param>
		public UnorderedList(DomContainer domContainer, INativeElement htmlUlElement) : base(domContainer, htmlUlElement) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="Div"/> class.
		/// Mainly used by WatiN internally.
		/// </summary>
		/// <param name="domContainer">The DOM container.</param>
		/// <param name="finder">The HTML ul element.</param>
		public UnorderedList(DomContainer domContainer, ElementFinder finder) : base(domContainer, finder) { }

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <returns></returns>
		public string GetName()
		{
			return WatinFrameworkConstants.UnorderedListTagName;
		}
	}
}
