using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Diagnostics.Auditing.Common.Models
{
	/// <summary>
	/// Common interface for AuditMachineName.
	/// </summary>
	[ContractClass(typeof(Contracts.AuditMachineNameContracts))]
	public interface IAuditMachineName
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AuditMachineNameID for this AuditMachineName.
		/// </summary>
		short AuditMachineNameID { get; set; }
	
		/// <summary>
		/// The Name for this AuditMachineName.
		/// </summary>
		string Name { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAuditMachineName))]
		internal abstract class AuditMachineNameContracts : IAuditMachineName
		{
		    #region Primitive properties
		
			short IAuditMachineName.AuditMachineNameID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAuditMachineName.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
