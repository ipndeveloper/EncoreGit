using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Bonus kind
	/// </summary>
	public interface IBonusKind
	{
        /// <summary>
        /// Gets or sets the bonus kind identifier.
        /// </summary>
        /// <value>
        /// The bonus kind identifier.
        /// </value>
        int BonusKindId { get; set; }
        
        /// <summary>
		/// Gets or sets the bonus code.
		/// </summary>
		/// <value>
		/// The bonus code.
		/// </value>
		string BonusCode { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the term.
        /// </summary>
        /// <value>
        /// The name of the term.
        /// </value>
        string TermName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is enabled]; otherwise, <c>false</c>.
        /// </value>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is editable].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is editable]; otherwise, <c>false</c>.
        /// </value>
        bool IsEditable { get; set; }

        /// <summary>
        /// Gets or sets the disbursement frequency.
        /// </summary>
        /// <value>
        /// The disbursement frequency.
        /// </value>
        int PlanId { get; set; }

        /// <summary>
		/// Gets or sets the earnings kind identifier.
		/// </summary>
		/// <value>
		/// The earnings kind identifier.
		/// </value>
        int? EarningsKindId { get; set; }

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
	}
}
