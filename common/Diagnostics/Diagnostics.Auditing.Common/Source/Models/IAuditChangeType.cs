using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Diagnostics.Auditing.Common.Models
{
	/// <summary>
	/// Common interface for AuditChangeType.
	/// </summary>
	[ContractClass(typeof(Contracts.AuditChangeTypeContracts))]
	public interface IAuditChangeType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AuditChangeTypeID for this AuditChangeType.
		/// </summary>
		byte AuditChangeTypeID { get; set; }
	
		/// <summary>
		/// The Name for this AuditChangeType.
		/// </summary>
		string Name { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAuditChangeType))]
		internal abstract class AuditChangeTypeContracts : IAuditChangeType
		{
		    #region Primitive properties
		
			byte IAuditChangeType.AuditChangeTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAuditChangeType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
