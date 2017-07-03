using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for AccountReport.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountReportContracts))]
	public interface IAccountReport
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountReportID for this AccountReport.
		/// </summary>
		int AccountReportID { get; set; }
	
		/// <summary>
		/// The AccountID for this AccountReport.
		/// </summary>
		Nullable<int> AccountID { get; set; }
	
		/// <summary>
		/// The AccountReportTypeID for this AccountReport.
		/// </summary>
		short AccountReportTypeID { get; set; }
	
		/// <summary>
		/// The Name for this AccountReport.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Data for this AccountReport.
		/// </summary>
		byte[] Data { get; set; }
	
		/// <summary>
		/// The DateCreatedUTC for this AccountReport.
		/// </summary>
		System.DateTime DateCreatedUTC { get; set; }
	
		/// <summary>
		/// The CreatedByUserID for this AccountReport.
		/// </summary>
		Nullable<int> CreatedByUserID { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this AccountReport.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The IsCorporate for this AccountReport.
		/// </summary>
		bool IsCorporate { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Account for this AccountReport.
		/// </summary>
	    IAccount Account { get; set; }
	
		/// <summary>
		/// The AccountReportType for this AccountReport.
		/// </summary>
	    IAccountReportType AccountReportType { get; set; }
	
		/// <summary>
		/// The User for this AccountReport.
		/// </summary>
	    IUser User { get; set; }
	
		/// <summary>
		/// The User1 for this AccountReport.
		/// </summary>
	    IUser User1 { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountReport))]
		internal abstract class AccountReportContracts : IAccountReport
		{
		    #region Primitive properties
		
			int IAccountReport.AccountReportID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountReport.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IAccountReport.AccountReportTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountReport.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] IAccountReport.Data
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IAccountReport.DateCreatedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountReport.CreatedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountReport.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountReport.IsCorporate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAccount IAccountReport.Account
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IAccountReportType IAccountReport.AccountReportType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IUser IAccountReport.User
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IUser IAccountReport.User1
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
