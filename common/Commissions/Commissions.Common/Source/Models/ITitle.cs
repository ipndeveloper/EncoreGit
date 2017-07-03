using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Account Commissions Title
	/// </summary>
	public interface ITitle
	{
		/// <summary>
		/// Gets the title identifier.
		/// </summary>
		/// <value>
		/// The title identifier.
		/// </value>
		int TitleId { get; }

		/// <summary>
		/// Gets the name of the title.
		/// </summary>
		/// <value>
		/// The name of the title.
		/// </value>
		string TitleName { get; }

		/// <summary>
		/// Gets the name of the term.
		/// </summary>
		/// <value>
		/// The name of the term.
		/// </value>
		string TermName { get; }

		/// <summary>
		/// Gets or sets the sort order.
		/// </summary>
		/// <value>
		/// The sort order.
		/// </value>
		int SortOrder { get; set; }

		/// <summary>
		/// Gets or sets the title code.
		/// </summary>
		/// <value>
		/// The title code.
		/// </value>
		string TitleCode { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [active].
		/// </summary>
		/// <value>
		///   <c>true</c> if [active]; otherwise, <c>false</c>.
		/// </value>
		bool Active { get; set; }

		/// <summary>
		/// Gets or sets the name of the client.
		/// </summary>
		/// <value>
		/// The name of the client.
		/// </value>
		string ClientName { get; set; }

		/// <summary>
		/// Gets or sets the client code.
		/// </summary>
		/// <value>
		/// The client code.
		/// </value>
		string ClientCode { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [reports visibility].
		/// </summary>
		/// <value>
		///   <c>true</c> if [reports visibility]; otherwise, <c>false</c>.
		/// </value>
		bool ReportsVisibility { get; set; }
	}
}
