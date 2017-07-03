using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;
using WatiN.Core.Native;

namespace WatiN.Core
{
	/// <summary>
	/// This class provides specialized functionality for a HTML ol element.
	/// </summary>
	[ElementTag("ol")]
	public class OrderedList : ElementContainer<OrderedList>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Div"/> class.
		/// Mainly used by WatiN internally.
		/// </summary>
		/// <param name="domContainer">The DOM container.</param>
		/// <param name="htmlOlElement">The HTML ol element.</param>
		public OrderedList(DomContainer domContainer, INativeElement htmlOlElement) : base(domContainer, htmlOlElement) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="Div"/> class.
		/// Mainly used by WatiN internally.
		/// </summary>
		/// <param name="domContainer">The DOM container.</param>
		/// <param name="finder">The HTML ol element.</param>
		public OrderedList(DomContainer domContainer, ElementFinder finder) : base(domContainer, finder) { }

		/// <summary>
		/// Gets the name of this Watin element.
		/// </summary>
		/// <returns></returns>
		public string GetName()
		{
			return WatinFrameworkConstants.OrderedListTagName;
		}
	}
}