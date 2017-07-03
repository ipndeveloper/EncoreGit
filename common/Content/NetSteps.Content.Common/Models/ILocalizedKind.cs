using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for LocalizedKind.
	/// </summary>
	[ContractClass(typeof(Contracts.LocalizedKindContracts))]
	public interface ILocalizedKind
	{
	    #region Primitive properties
	
		/// <summary>
		/// The LocalizedKindTableId for this LocalizedKind.
		/// </summary>
		int LocalizedKindTableId { get; set; }
	
		/// <summary>
		/// The KindId for this LocalizedKind.
		/// </summary>
		int KindId { get; set; }
	
		/// <summary>
		/// The LanguageId for this LocalizedKind.
		/// </summary>
		int LanguageId { get; set; }
	
		/// <summary>
		/// The LocalizedName for this LocalizedKind.
		/// </summary>
		string LocalizedName { get; set; }
	
		/// <summary>
		/// The SortIndex for this LocalizedKind.
		/// </summary>
		int SortIndex { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ILocalizedKind))]
		internal abstract class LocalizedKindContracts : ILocalizedKind
		{
		    #region Primitive properties
		
			int ILocalizedKind.LocalizedKindTableId
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ILocalizedKind.KindId
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ILocalizedKind.LanguageId
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ILocalizedKind.LocalizedName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ILocalizedKind.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
