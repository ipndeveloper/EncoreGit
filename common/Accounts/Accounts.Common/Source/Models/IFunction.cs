using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for Function.
	/// </summary>
	[ContractClass(typeof(Contracts.FunctionContracts))]
	public interface IFunction
	{
	    #region Primitive properties
	
		/// <summary>
		/// The FunctionID for this Function.
		/// </summary>
		int FunctionID { get; set; }
	
		/// <summary>
		/// The Name for this Function.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Active for this Function.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The TermName for this Function.
		/// </summary>
		string TermName { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Roles for this Function.
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
	
		/// <summary>
		/// The UserFunctionOverrides for this Function.
		/// </summary>
		IEnumerable<IUserFunctionOverride> UserFunctionOverrides { get; }
	
		/// <summary>
		/// Adds an <see cref="IUserFunctionOverride"/> to the UserFunctionOverrides collection.
		/// </summary>
		/// <param name="item">The <see cref="IUserFunctionOverride"/> to add.</param>
		void AddUserFunctionOverride(IUserFunctionOverride item);
	
		/// <summary>
		/// Removes an <see cref="IUserFunctionOverride"/> from the UserFunctionOverrides collection.
		/// </summary>
		/// <param name="item">The <see cref="IUserFunctionOverride"/> to remove.</param>
		void RemoveUserFunctionOverride(IUserFunctionOverride item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IFunction))]
		internal abstract class FunctionContracts : IFunction
		{
		    #region Primitive properties
		
			int IFunction.FunctionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IFunction.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IFunction.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IFunction.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IRole> IFunction.Roles
			{
				get { throw new NotImplementedException(); }
			}
		
			void IFunction.AddRole(IRole item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IFunction.RemoveRole(IRole item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IUserFunctionOverride> IFunction.UserFunctionOverrides
			{
				get { throw new NotImplementedException(); }
			}
		
			void IFunction.AddUserFunctionOverride(IUserFunctionOverride item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IFunction.RemoveUserFunctionOverride(IUserFunctionOverride item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
