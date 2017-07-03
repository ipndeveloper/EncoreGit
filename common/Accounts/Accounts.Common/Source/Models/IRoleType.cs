using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for RoleType.
	/// </summary>
	[ContractClass(typeof(Contracts.RoleTypeContracts))]
	public interface IRoleType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The RoleTypeID for this RoleType.
		/// </summary>
		short RoleTypeID { get; set; }
	
		/// <summary>
		/// The Name for this RoleType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this RoleType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this RoleType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this RoleType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Roles for this RoleType.
		/// </summary>
		IEnumerable<IRole> Roles { get; }
	
		/// <summary>
		/// Adds an <see cref="IRole"/> to the Roles collection.
		/// </summary>
		/// <param name="item">The <see cref="IRole"/> to add.</param>
		void AddRole(IRole item);
	
		/// <summary>
		/// Removes an <see cref="IRole"/> from the Roles collection.
		/// </summary>
		/// <param name="item">The <see cref="IRole"/> to remove.</param>
		void RemoveRole(IRole item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IRoleType))]
		internal abstract class RoleTypeContracts : IRoleType
		{
		    #region Primitive properties
		
			short IRoleType.RoleTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IRoleType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IRoleType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IRoleType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IRoleType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IRole> IRoleType.Roles
			{
				get { throw new NotImplementedException(); }
			}
		
			void IRoleType.AddRole(IRole item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IRoleType.RemoveRole(IRole item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
