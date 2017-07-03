using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Diagnostics.Auditing.Common.Models
{
	/// <summary>
	/// Common interface for AuditLog.
	/// </summary>
	[ContractClass(typeof(Contracts.AuditLogContracts))]
	public interface IAuditLog
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AuditLogID for this AuditLog.
		/// </summary>
		long AuditLogID { get; set; }
	
		/// <summary>
		/// The AuditTableID for this AuditLog.
		/// </summary>
		short AuditTableID { get; set; }
	
		/// <summary>
		/// The AuditChangeTypeID for this AuditLog.
		/// </summary>
		byte AuditChangeTypeID { get; set; }
	
		/// <summary>
		/// The AuditMachineNameID for this AuditLog.
		/// </summary>
		short AuditMachineNameID { get; set; }
	
		/// <summary>
		/// The AuditSqlUserNameID for this AuditLog.
		/// </summary>
		short AuditSqlUserNameID { get; set; }
	
		/// <summary>
		/// The AuditTableColumnID for this AuditLog.
		/// </summary>
		short AuditTableColumnID { get; set; }
	
		/// <summary>
		/// The ApplicationID for this AuditLog.
		/// </summary>
		short ApplicationID { get; set; }
	
		/// <summary>
		/// The UserID for this AuditLog.
		/// </summary>
		Nullable<int> UserID { get; set; }
	
		/// <summary>
		/// The PK for this AuditLog.
		/// </summary>
		int PK { get; set; }
	
		/// <summary>
		/// The PKs for this AuditLog.
		/// </summary>
		string PKs { get; set; }
	
		/// <summary>
		/// The OldValue for this AuditLog.
		/// </summary>
		string OldValue { get; set; }
	
		/// <summary>
		/// The NewValue for this AuditLog.
		/// </summary>
		string NewValue { get; set; }
	
		/// <summary>
		/// The DateChanged for this AuditLog.
		/// </summary>
		System.DateTime DateChanged { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAuditLog))]
		internal abstract class AuditLogContracts : IAuditLog
		{
		    #region Primitive properties
		
			long IAuditLog.AuditLogID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IAuditLog.AuditTableID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte IAuditLog.AuditChangeTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IAuditLog.AuditMachineNameID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IAuditLog.AuditSqlUserNameID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IAuditLog.AuditTableColumnID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IAuditLog.ApplicationID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAuditLog.UserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAuditLog.PK
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAuditLog.PKs
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAuditLog.OldValue
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAuditLog.NewValue
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IAuditLog.DateChanged
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
