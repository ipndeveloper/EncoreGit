using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for PriceRelationshipType.
	/// </summary>
	[ContractClass(typeof(Contracts.PriceRelationshipTypeContracts))]
	public interface IPriceRelationshipType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The PriceRelationshipTypeID for this PriceRelationshipType.
		/// </summary>
		int PriceRelationshipTypeID { get; set; }
	
		/// <summary>
		/// The Name for this PriceRelationshipType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this PriceRelationshipType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this PriceRelationshipType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this PriceRelationshipType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AccountPriceTypes for this PriceRelationshipType.
		/// </summary>
		IEnumerable<IAccountPriceType> AccountPriceTypes { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountPriceType"/> to the AccountPriceTypes collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountPriceType"/> to add.</param>
		void AddAccountPriceType(IAccountPriceType item);
	
		/// <summary>
		/// Removes an <see cref="IAccountPriceType"/> from the AccountPriceTypes collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountPriceType"/> to remove.</param>
		void RemoveAccountPriceType(IAccountPriceType item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IPriceRelationshipType))]
		internal abstract class PriceRelationshipTypeContracts : IPriceRelationshipType
		{
		    #region Primitive properties
		
			int IPriceRelationshipType.PriceRelationshipTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPriceRelationshipType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPriceRelationshipType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPriceRelationshipType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IPriceRelationshipType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAccountPriceType> IPriceRelationshipType.AccountPriceTypes
			{
				get { throw new NotImplementedException(); }
			}
		
			void IPriceRelationshipType.AddAccountPriceType(IAccountPriceType item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IPriceRelationshipType.RemoveAccountPriceType(IAccountPriceType item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
