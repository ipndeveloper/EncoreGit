using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for DistributionListType.
	/// </summary>
	[ContractClass(typeof(Contracts.DistributionListTypeContracts))]
	public interface IDistributionListType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The DistributionListTypeID for this DistributionListType.
		/// </summary>
		short DistributionListTypeID { get; set; }
	
		/// <summary>
		/// The Name for this DistributionListType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this DistributionListType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this DistributionListType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this DistributionListType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The DistributionLists for this DistributionListType.
		/// </summary>
		IEnumerable<IDistributionList> DistributionLists { get; }
	
		/// <summary>
		/// Adds an <see cref="IDistributionList"/> to the DistributionLists collection.
		/// </summary>
		/// <param name="item">The <see cref="IDistributionList"/> to add.</param>
		void AddDistributionList(IDistributionList item);
	
		/// <summary>
		/// Removes an <see cref="IDistributionList"/> from the DistributionLists collection.
		/// </summary>
		/// <param name="item">The <see cref="IDistributionList"/> to remove.</param>
		void RemoveDistributionList(IDistributionList item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IDistributionListType))]
		internal abstract class DistributionListTypeContracts : IDistributionListType
		{
		    #region Primitive properties
		
			short IDistributionListType.DistributionListTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDistributionListType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDistributionListType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDistributionListType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IDistributionListType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IDistributionList> IDistributionListType.DistributionLists
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDistributionListType.AddDistributionList(IDistributionList item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDistributionListType.RemoveDistributionList(IDistributionList item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
