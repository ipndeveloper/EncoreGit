using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Diagnostics.Auditing.Common.Models
{
	/// <summary>
	/// Common interface for AuditTable.
	/// </summary>
	[ContractClass(typeof(Contracts.AuditTableContracts))]
	public interface IAuditTable
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AuditTableID for this AuditTable.
		/// </summary>
		short AuditTableID { get; set; }
	
		/// <summary>
		/// The Name for this AuditTable.
		/// </summary>
		string Name { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAuditTable))]
		internal abstract class AuditTableContracts : IAuditTable
		{
		    #region Primitive properties
		
			short IAuditTable.AuditTableID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAuditTable.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
