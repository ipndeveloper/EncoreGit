using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Diagnostics.Auditing.Common.Models
{
	/// <summary>
	/// Common interface for AuditLogRow.
	/// </summary>
	public interface IAuditLogRow
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ChangeType for this AuditLogRow.
		/// </summary>
		string ChangeType { get; set; }
	
		/// <summary>
		/// The TableName for this AuditLogRow.
		/// </summary>
		string TableName { get; set; }
	
		/// <summary>
		/// The PK for this AuditLogRow.
		/// </summary>
		int PK { get; set; }
	
		/// <summary>
		/// The ColumnName for this AuditLogRow.
		/// </summary>
		string ColumnName { get; set; }
	
		/// <summary>
		/// The OldValue for this AuditLogRow.
		/// </summary>
		string OldValue { get; set; }
	
		/// <summary>
		/// The NewValue for this AuditLogRow.
		/// </summary>
		string NewValue { get; set; }
	
		/// <summary>
		/// The DateChanged for this AuditLogRow.
		/// </summary>
		System.DateTime DateChanged { get; set; }
	
		/// <summary>
		/// The Username for this AuditLogRow.
		/// </summary>
		string Username { get; set; }
	
		/// <summary>
		/// The SqlUserName for this AuditLogRow.
		/// </summary>
		string SqlUserName { get; set; }
	
		/// <summary>
		/// The MachineName for this AuditLogRow.
		/// </summary>
		string MachineName { get; set; }
	
		/// <summary>
		/// The ApplicationName for this AuditLogRow.
		/// </summary>
		string ApplicationName { get; set; }
	
		/// <summary>
		/// The row_number for this AuditLogRow.
		/// </summary>
		Nullable<long> row_number { get; set; }

	    #endregion
	}
}
