using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for AccountReportType.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountReportTypeContracts))]
	public interface IAccountReportType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountReportTypeID for this AccountReportType.
		/// </summary>
		short AccountReportTypeID { get; set; }
	
		/// <summary>
		/// The Name for this AccountReportType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this AccountReportType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this AccountReportType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this AccountReportType.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this AccountReportType.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The User for this AccountReportType.
		/// </summary>
	    IUser User { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AccountReports for this AccountReportType.
		/// </summary>
		IEnumerable<IAccountReport> AccountReports { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountReport"/> to the AccountReports collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountReport"/> to add.</param>
		void AddAccountReport(IAccountReport item);
	
		/// <summary>
		/// Removes an <see cref="IAccountReport"/> from the AccountReports collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountReport"/> to remove.</param>
		void RemoveAccountReport(IAccountReport item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountReportType))]
		internal abstract class AccountReportTypeContracts : IAccountReportType
		{
		    #region Primitive properties
		
			short IAccountReportType.AccountReportTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountReportType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountReportType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountReportType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountReportType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountReportType.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IUser IAccountReportType.User
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAccountReport> IAccountReportType.AccountReports
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccountReportType.AddAccountReport(IAccountReport item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccountReportType.RemoveAccountReport(IAccountReport item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
