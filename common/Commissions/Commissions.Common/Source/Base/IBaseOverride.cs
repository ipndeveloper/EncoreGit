using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Base
{
	/// <summary>
	/// This is a base override object; I do not know if this is really shared but the properties
	/// of all overrides seem to have the same properties.
	/// </summary>
	public interface IBaseOverride
	{
		/// <summary>
		/// Gets or sets the account identifier.
		/// </summary>
		/// <value>
		/// The account identifier.
		/// </value>
		int AccountId { get; set; }

		/// <summary>
		/// Gets or sets the override reason.
		/// </summary>
		/// <value>
		/// The override reason.
		/// </value>
		IOverrideReason OverrideReason { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [is editable].
		/// </summary>
		/// <value>
		///   <c>true</c> if [is editable]; otherwise, <c>false</c>.
		/// </value>
		bool IsEditable { get; }

		/// <summary>
		/// Gets or sets the application source identifier.
		/// </summary>
		/// <value>
		/// The application source identifier.
		/// </value>
		int ApplicationSourceId { get; set; }

		/// <summary>
		/// Gets or sets the created date UTC.
		/// </summary>
		/// <value>
		/// The created date UTC.
		/// </value>
		DateTime CreatedDateUTC { get; set; }

		/// <summary>
		/// Gets or sets the notes.
		/// </summary>
		/// <value>
		/// The notes.
		/// </value>
		string Notes { get; set; }

		/// <summary>
		/// Gets or sets the updated date UTC.
		/// </summary>
		/// <value>
		/// The updated date UTC.
		/// </value>
		DateTime UpdatedDateUTC { get; set; }

		/// <summary>
		/// Gets or sets the user identifier.
		/// </summary>
		/// <value>
		/// The user identifier.
		/// </value>
		int UserId { get; set; }

		/// <summary>
		/// Gets or sets the kind of the override.
		/// </summary>
		/// <value>
		/// The kind of the override.
		/// </value>
		IOverrideKind OverrideKind { get; set; }
	}
}
