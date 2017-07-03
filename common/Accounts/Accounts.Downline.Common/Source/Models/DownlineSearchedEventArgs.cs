using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace NetSteps.Accounts.Downline.Common.Models
{
	/// <summary>
	/// Contains data related to the DownlineSearched event.
	/// </summary>
	public class DownlineSearchedEventArgs : EventArgs
	{
		/// <summary>
		/// The downline data returned from the database.
		/// </summary>
		public IList<IDownlineData> Data { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="DownlineSearchedEventArgs"/> class.
		/// </summary>
		/// <param name="data">The downline data returned from the database.</param>
		public DownlineSearchedEventArgs(
			IList<IDownlineData> data)
		{
			Contract.Requires<ArgumentNullException>(data != null);

			Data = data;
		}
	}
}