using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;
using WatiN.Core.Extras;

namespace TestMasterHelpProvider.Pages
{
	public class AspNetErrorPage : Page
	{
		#region Fields

		private const string ServerErrorPageHeaderText = "Server Error in '/' Application.";

		#endregion

		#region Properties

		/// <summary>
		/// Gets the Server Error H1.
		/// </summary>
		public HeaderLevel1 ServerErrorPageHeader
		{
			get { return Document.HeaderLevel1(Find.ByText(ServerErrorPageHeaderText)); }
		}

		/// <summary>
		/// Gets the stack trace Pre.
		/// </summary>
		public PreFormattedText StackTrace
		{
			get { return Document.PreFormattedText(Find.ByIndex(0)); }
		}

		#endregion
	}
}
