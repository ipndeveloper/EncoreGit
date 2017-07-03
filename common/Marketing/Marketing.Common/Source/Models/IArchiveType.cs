using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Marketing.Common.Models
{
	/// <summary>
	/// Common interface for ArchiveType.
	/// </summary>
	[ContractClass(typeof(Contracts.ArchiveTypeContracts))]
	public interface IArchiveType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ArchiveTypeID for this ArchiveType.
		/// </summary>
		short ArchiveTypeID { get; set; }
	
		/// <summary>
		/// The Name for this ArchiveType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Description for this ArchiveType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Editable for this ArchiveType.
		/// </summary>
		bool Editable { get; set; }
	
		/// <summary>
		/// The Active for this ArchiveType.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The TermName for this ArchiveType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this ArchiveType.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IArchiveType))]
		internal abstract class ArchiveTypeContracts : IArchiveType
		{
		    #region Primitive properties
		
			short IArchiveType.ArchiveTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IArchiveType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IArchiveType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IArchiveType.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IArchiveType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IArchiveType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IArchiveType.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
