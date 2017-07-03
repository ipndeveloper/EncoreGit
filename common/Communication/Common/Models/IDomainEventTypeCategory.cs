using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for DomainEventTypeCategory.
	/// </summary>
	[ContractClass(typeof(Contracts.DomainEventTypeCategoryContracts))]
	public interface IDomainEventTypeCategory
	{
	    #region Primitive properties
	
		/// <summary>
		/// The DomainEventTypeCategoryID for this DomainEventTypeCategory.
		/// </summary>
		int DomainEventTypeCategoryID { get; set; }
	
		/// <summary>
		/// The TermName for this DomainEventTypeCategory.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Name for this DomainEventTypeCategory.
		/// </summary>
		string Name { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The DomainEventTypes for this DomainEventTypeCategory.
		/// </summary>
		IEnumerable<IDomainEventType> DomainEventTypes { get; }
	
		/// <summary>
		/// Adds an <see cref="IDomainEventType"/> to the DomainEventTypes collection.
		/// </summary>
		/// <param name="item">The <see cref="IDomainEventType"/> to add.</param>
		void AddDomainEventType(IDomainEventType item);
	
		/// <summary>
		/// Removes an <see cref="IDomainEventType"/> from the DomainEventTypes collection.
		/// </summary>
		/// <param name="item">The <see cref="IDomainEventType"/> to remove.</param>
		void RemoveDomainEventType(IDomainEventType item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IDomainEventTypeCategory))]
		internal abstract class DomainEventTypeCategoryContracts : IDomainEventTypeCategory
		{
		    #region Primitive properties
		
			int IDomainEventTypeCategory.DomainEventTypeCategoryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDomainEventTypeCategory.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDomainEventTypeCategory.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IDomainEventType> IDomainEventTypeCategory.DomainEventTypes
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDomainEventTypeCategory.AddDomainEventType(IDomainEventType item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDomainEventTypeCategory.RemoveDomainEventType(IDomainEventType item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
