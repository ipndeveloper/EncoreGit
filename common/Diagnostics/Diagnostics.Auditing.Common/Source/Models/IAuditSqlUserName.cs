using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Diagnostics.Auditing.Common.Models
{
	/// <summary>
	/// Common interface for AuditSqlUserName.
	/// </summary>
	[ContractClass(typeof(Contracts.AuditSqlUserNameContracts))]
	public interface IAuditSqlUserName
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AuditSqlUserNameID for this AuditSqlUserName.
		/// </summary>
		short AuditSqlUserNameID { get; set; }
	
		/// <summary>
		/// The Name for this AuditSqlUserName.
		/// </summary>
		string Name { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAuditSqlUserName))]
		internal abstract class AuditSqlUserNameContracts : IAuditSqlUserName
		{
		    #region Primitive properties
		
			short IAuditSqlUserName.AuditSqlUserNameID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAuditSqlUserName.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
