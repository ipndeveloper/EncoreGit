using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for EmailType.
	/// </summary>
	[ContractClass(typeof(Contracts.EmailTypeContracts))]
	public interface IEmailType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The EmailTypeID for this EmailType.
		/// </summary>
		short EmailTypeID { get; set; }
	
		/// <summary>
		/// The Name for this EmailType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this EmailType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this EmailType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this EmailType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AccountEmailLogs for this EmailType.
		/// </summary>
		IEnumerable<IAccountEmailLog> AccountEmailLogs { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountEmailLog"/> to the AccountEmailLogs collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountEmailLog"/> to add.</param>
		void AddAccountEmailLog(IAccountEmailLog item);
	
		/// <summary>
		/// Removes an <see cref="IAccountEmailLog"/> from the AccountEmailLogs collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountEmailLog"/> to remove.</param>
		void RemoveAccountEmailLog(IAccountEmailLog item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IEmailType))]
		internal abstract class EmailTypeContracts : IEmailType
		{
		    #region Primitive properties
		
			short IEmailType.EmailTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IEmailType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAccountEmailLog> IEmailType.AccountEmailLogs
			{
				get { throw new NotImplementedException(); }
			}
		
			void IEmailType.AddAccountEmailLog(IAccountEmailLog item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IEmailType.RemoveAccountEmailLog(IAccountEmailLog item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
