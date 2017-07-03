using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;
using WatiN.Core.Native;

namespace WatiN.Core.Extras
{
	/// <summary>
	/// This class provides specialized functionality for a HTML li element.
	/// </summary>
	[ElementTag("li")]
	public class ListItem : ElementContainer<ListItem>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ListItem"/> class.
		/// Mainly used by WatiN internally.
		/// </summary>
		/// <param name="domContainer">The DOM container.</param>
		/// <param name="htmlLiElement">The HTML li element.</param>
		public ListItem(DomContainer domContainer, INativeElement htmlLiElement) : base(domContainer, htmlLiElement) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="ListItem"/> class.
		/// Mainly used by WatiN internally.
		/// </summary>
		/// <param name="domContainer">The DOM container.</param>
		/// <param name="finder">The HTML li element.</param>
		public ListItem(DomContainer domContainer, ElementFinder finder) : base(domContainer, finder) { }

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <returns></returns>
		public string GetName()
		{
			return WatinFrameworkConstants.ListItemTagName;
		}
	}
}