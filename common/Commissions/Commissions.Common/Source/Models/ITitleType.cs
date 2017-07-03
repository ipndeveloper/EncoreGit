using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for TitleType.
	/// </summary>
	[ContractClass(typeof(Contracts.TitleTypeContracts))]
	public interface ITitleType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The TitleTypeID for this TitleType.
		/// </summary>
		int TitleTypeID { get; set; }
	
		/// <summary>
		/// The TitleTypeCode for this TitleType.
		/// </summary>
		string TitleTypeCode { get; set; }
	
		/// <summary>
		/// The Name for this TitleType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this TitleType.
		/// </summary>
		string TermName { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AccountTitleOverrides for this TitleType.
		/// </summary>
		IEnumerable<IAccountTitleOverride> AccountTitleOverrides { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountTitleOverride"/> to the AccountTitleOverrides collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountTitleOverride"/> to add.</param>
		void AddAccountTitleOverride(IAccountTitleOverride item);
	
		/// <summary>
		/// Removes an <see cref="IAccountTitleOverride"/> from the AccountTitleOverrides collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountTitleOverride"/> to remove.</param>
		void RemoveAccountTitleOverride(IAccountTitleOverride item);
	
		/// <summary>
		/// The AccountTitles for this TitleType.
		/// </summary>
		IEnumerable<IAccountTitle> AccountTitles { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountTitle"/> to the AccountTitles collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountTitle"/> to add.</param>
		void AddAccountTitle(IAccountTitle item);
	
		/// <summary>
		/// Removes an <see cref="IAccountTitle"/> from the AccountTitles collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountTitle"/> to remove.</param>
		void RemoveAccountTitle(IAccountTitle item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ITitleType))]
		internal abstract class TitleTypeContracts : ITitleType
		{
		    #region Primitive properties
		
			int ITitleType.TitleTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ITitleType.TitleTypeCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ITitleType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ITitleType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAccountTitleOverride> ITitleType.AccountTitleOverrides
			{
				get { throw new NotImplementedException(); }
			}
		
			void ITitleType.AddAccountTitleOverride(IAccountTitleOverride item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ITitleType.RemoveAccountTitleOverride(IAccountTitleOverride item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAccountTitle> ITitleType.AccountTitles
			{
				get { throw new NotImplementedException(); }
			}
		
			void ITitleType.AddAccountTitle(IAccountTitle item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ITitleType.RemoveAccountTitle(IAccountTitle item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
