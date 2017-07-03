using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace NetSteps.Accounts.Downline.Common.Models
{
	/// <summary>
	/// Contains data related to the DownlineSearching event.
	/// </summary>
	public class DownlineSearchingEventArgs : EventArgs
	{
		/// <summary>
		/// Contains specification parameters.
		/// </summary>
		public ISearchDownlineContext Context { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="DownlineSearchingEventArgs"/> class.
		/// </summary>
		/// <param name="context">Contains specification parameters.</param>
		public DownlineSearchingEventArgs(
			ISearchDownlineContext context)
		{
			Contract.Requires<ArgumentNullException>(context != null);

			Context = context;
		}
	}
}