using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for FileResource.
	/// </summary>
	[ContractClass(typeof(Contracts.FileResourceContracts))]
	public interface IFileResource
	{
	    #region Primitive properties
	
		/// <summary>
		/// The FileResourceID for this FileResource.
		/// </summary>
		int FileResourceID { get; set; }
	
		/// <summary>
		/// The FileResourceTypeID for this FileResource.
		/// </summary>
		int FileResourceTypeID { get; set; }
	
		/// <summary>
		/// The FileResourcePath for this FileResource.
		/// </summary>
		string FileResourcePath { get; set; }
	
		/// <summary>
		/// The DateCreatedUTC for this FileResource.
		/// </summary>
		System.DateTime DateCreatedUTC { get; set; }
	
		/// <summary>
		/// The Active for this FileResource.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this FileResource.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The FileResourceProperties for this FileResource.
		/// </summary>
		IEnumerable<IFileResourceProperty> FileResourceProperties { get; }
	
		/// <summary>
		/// Adds an <see cref="IFileResourceProperty"/> to the FileResourceProperties collection.
		/// </summary>
		/// <param name="item">The <see cref="IFileResourceProperty"/> to add.</param>
		void AddFileResourceProperty(IFileResourceProperty item);
	
		/// <summary>
		/// Removes an <see cref="IFileResourceProperty"/> from the FileResourceProperties collection.
		/// </summary>
		/// <param name="item">The <see cref="IFileResourceProperty"/> to remove.</param>
		void RemoveFileResourceProperty(IFileResourceProperty item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IFileResource))]
		internal abstract class FileResourceContracts : IFileResource
		{
		    #region Primitive properties
		
			int IFileResource.FileResourceID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IFileResource.FileResourceTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IFileResource.FileResourcePath
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IFileResource.DateCreatedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IFileResource.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IFileResource.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IFileResourceProperty> IFileResource.FileResourceProperties
			{
				get { throw new NotImplementedException(); }
			}
		
			void IFileResource.AddFileResourceProperty(IFileResourceProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IFileResource.RemoveFileResourceProperty(IFileResourceProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
