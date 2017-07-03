using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace NetSteps.Accounts.Downline.Common.Models
{
	/// <summary>
	/// Contains data related to the DownlineLoading event.
	/// </summary>
	public class DownlineLoadingEventArgs : EventArgs
	{
		/// <summary>
		/// Contains specification parameters.
		/// </summary>
		public IGetDownlineContext Context { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="DownlineLoadingEventArgs"/> class.
		/// </summary>
		/// <param name="context">Contains specification parameters.</param>
		public DownlineLoadingEventArgs(
			IGetDownlineContext context)
		{
			Contract.Requires<ArgumentNullException>(context != null);

			Context = context;
		}
	}
}