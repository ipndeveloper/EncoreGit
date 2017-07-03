using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for FileResourceType.
	/// </summary>
	[ContractClass(typeof(Contracts.FileResourceTypeContracts))]
	public interface IFileResourceType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The FileResourceTypeID for this FileResourceType.
		/// </summary>
		int FileResourceTypeID { get; set; }
	
		/// <summary>
		/// The Name for this FileResourceType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this FileResourceType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this FileResourceType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this FileResourceType.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The Editable for this FileResourceType.
		/// </summary>
		bool Editable { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The FileResources for this FileResourceType.
		/// </summary>
		IEnumerable<IFileResource> FileResources { get; }
	
		/// <summary>
		/// Adds an <see cref="IFileResource"/> to the FileResources collection.
		/// </summary>
		/// <param name="item">The <see cref="IFileResource"/> to add.</param>
		void AddFileResource(IFileResource item);
	
		/// <summary>
		/// Removes an <see cref="IFileResource"/> from the FileResources collection.
		/// </summary>
		/// <param name="item">The <see cref="IFileResource"/> to remove.</param>
		void RemoveFileResource(IFileResource item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IFileResourceType))]
		internal abstract class FileResourceTypeContracts : IFileResourceType
		{
		    #region Primitive properties
		
			int IFileResourceType.FileResourceTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IFileResourceType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IFileResourceType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IFileResourceType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IFileResourceType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IFileResourceType.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IFileResource> IFileResourceType.FileResources
			{
				get { throw new NotImplementedException(); }
			}
		
			void IFileResourceType.AddFileResource(IFileResource item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IFileResourceType.RemoveFileResource(IFileResource item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
