using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Diagnostics.Auditing.Common.Models
{
	/// <summary>
	/// Common interface for AuditTableColumn.
	/// </summary>
	[ContractClass(typeof(Contracts.AuditTableColumnContracts))]
	public interface IAuditTableColumn
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AuditTableColumnID for this AuditTableColumn.
		/// </summary>
		short AuditTableColumnID { get; set; }
	
		/// <summary>
		/// The AuditTableID for this AuditTableColumn.
		/// </summary>
		short AuditTableID { get; set; }
	
		/// <summary>
		/// The ColumnName for this AuditTableColumn.
		/// </summary>
		string ColumnName { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAuditTableColumn))]
		internal abstract class AuditTableColumnContracts : IAuditTableColumn
		{
		    #region Primitive properties
		
			short IAuditTableColumn.AuditTableColumnID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IAuditTableColumn.AuditTableID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAuditTableColumn.ColumnName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
