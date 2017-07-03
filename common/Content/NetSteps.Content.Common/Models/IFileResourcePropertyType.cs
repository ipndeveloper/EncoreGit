using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for FileResourcePropertyType.
	/// </summary>
	[ContractClass(typeof(Contracts.FileResourcePropertyTypeContracts))]
	public interface IFileResourcePropertyType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The FileResourcePropertyTypeID for this FileResourcePropertyType.
		/// </summary>
		int FileResourcePropertyTypeID { get; set; }
	
		/// <summary>
		/// The Name for this FileResourcePropertyType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The DataType for this FileResourcePropertyType.
		/// </summary>
		string DataType { get; set; }
	
		/// <summary>
		/// The Required for this FileResourcePropertyType.
		/// </summary>
		bool Required { get; set; }
	
		/// <summary>
		/// The SortIndex for this FileResourcePropertyType.
		/// </summary>
		int SortIndex { get; set; }
	
		/// <summary>
		/// The TermName for this FileResourcePropertyType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this FileResourcePropertyType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this FileResourcePropertyType.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The Editable for this FileResourcePropertyType.
		/// </summary>
		bool Editable { get; set; }
	
		/// <summary>
		/// The IsInternal for this FileResourcePropertyType.
		/// </summary>
		bool IsInternal { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The FileResourceProperties for this FileResourcePropertyType.
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
		[ContractClassFor(typeof(IFileResourcePropertyType))]
		internal abstract class FileResourcePropertyTypeContracts : IFileResourcePropertyType
		{
		    #region Primitive properties
		
			int IFileResourcePropertyType.FileResourcePropertyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IFileResourcePropertyType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IFileResourcePropertyType.DataType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IFileResourcePropertyType.Required
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IFileResourcePropertyType.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IFileResourcePropertyType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IFileResourcePropertyType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IFileResourcePropertyType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IFileResourcePropertyType.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IFileResourcePropertyType.IsInternal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IFileResourceProperty> IFileResourcePropertyType.FileResourceProperties
			{
				get { throw new NotImplementedException(); }
			}
		
			void IFileResourcePropertyType.AddFileResourceProperty(IFileResourceProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IFileResourcePropertyType.RemoveFileResourceProperty(IFileResourceProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
