using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for FileResourceProperty.
	/// </summary>
	[ContractClass(typeof(Contracts.FileResourcePropertyContracts))]
	public interface IFileResourceProperty
	{
	    #region Primitive properties
	
		/// <summary>
		/// The FileResourcePropertyID for this FileResourceProperty.
		/// </summary>
		int FileResourcePropertyID { get; set; }
	
		/// <summary>
		/// The FileResourceID for this FileResourceProperty.
		/// </summary>
		int FileResourceID { get; set; }
	
		/// <summary>
		/// The FileResourcePropertyTypeID for this FileResourceProperty.
		/// </summary>
		int FileResourcePropertyTypeID { get; set; }
	
		/// <summary>
		/// The PropertyValue for this FileResourceProperty.
		/// </summary>
		string PropertyValue { get; set; }
	
		/// <summary>
		/// The Active for this FileResourceProperty.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The FileResource for this FileResourceProperty.
		/// </summary>
	    IFileResource FileResource { get; set; }
	
		/// <summary>
		/// The FileResourcePropertyType for this FileResourceProperty.
		/// </summary>
	    IFileResourcePropertyType FileResourcePropertyType { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IFileResourceProperty))]
		internal abstract class FileResourcePropertyContracts : IFileResourceProperty
		{
		    #region Primitive properties
		
			int IFileResourceProperty.FileResourcePropertyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IFileResourceProperty.FileResourceID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IFileResourceProperty.FileResourcePropertyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IFileResourceProperty.PropertyValue
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IFileResourceProperty.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IFileResource IFileResourceProperty.FileResource
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IFileResourcePropertyType IFileResourceProperty.FileResourcePropertyType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
