using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Common.Models;
using NetSteps.Content.Common.Models;

namespace NetSteps.Marketing.Common.Models
{
	/// <summary>
	/// Common interface for Archive.
	/// </summary>
	[ContractClass(typeof(Contracts.ArchiveContracts))]
	public interface IArchive
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ArchiveID for this Archive.
		/// </summary>
		int ArchiveID { get; set; }
	
		/// <summary>
		/// The ArchiveTypeID for this Archive.
		/// </summary>
		short ArchiveTypeID { get; set; }
	
		/// <summary>
		/// The ArchiveDateUTC for this Archive.
		/// </summary>
		Nullable<System.DateTime> ArchiveDateUTC { get; set; }
	
		/// <summary>
		/// The StartDateUTC for this Archive.
		/// </summary>
		Nullable<System.DateTime> StartDateUTC { get; set; }
	
		/// <summary>
		/// The EndDateUTC for this Archive.
		/// </summary>
		Nullable<System.DateTime> EndDateUTC { get; set; }
	
		/// <summary>
		/// The ArchiveIcon for this Archive.
		/// </summary>
		string ArchiveIcon { get; set; }
	
		/// <summary>
		/// The ArchiveImage for this Archive.
		/// </summary>
		string ArchiveImage { get; set; }
	
		/// <summary>
		/// The ArchivePath for this Archive.
		/// </summary>
		string ArchivePath { get; set; }
	
		/// <summary>
		/// The TotalDownloads for this Archive.
		/// </summary>
		int TotalDownloads { get; set; }
	
		/// <summary>
		/// The Active for this Archive.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The IsEmailable for this Archive.
		/// </summary>
		bool IsEmailable { get; set; }
	
		/// <summary>
		/// The IsDownloadable for this Archive.
		/// </summary>
		bool IsDownloadable { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this Archive.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The AccessibilityInfo for this Archive.
		/// </summary>
		string AccessibilityInfo { get; set; }
	
		/// <summary>
		/// The LanguageID for this Archive.
		/// </summary>
		int LanguageID { get; set; }
	
		/// <summary>
		/// The IsFeatured for this Archive.
		/// </summary>
		bool IsFeatured { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The ArchiveType for this Archive.
		/// </summary>
	    IArchiveType ArchiveType { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Categories for this Archive.
		/// </summary>
		IEnumerable<ICategory> Categories { get; }
	
		/// <summary>
		/// Adds an <see cref="ICategory"/> to the Categories collection.
		/// </summary>
		/// <param name="item">The <see cref="ICategory"/> to add.</param>
		void AddCategory(ICategory item);
	
		/// <summary>
		/// Removes an <see cref="ICategory"/> from the Categories collection.
		/// </summary>
		/// <param name="item">The <see cref="ICategory"/> to remove.</param>
		void RemoveCategory(ICategory item);
	
		/// <summary>
		/// The Translations for this Archive.
		/// </summary>
		IEnumerable<IDescriptionTranslation> Translations { get; }
	
		/// <summary>
		/// Adds an <see cref="IDescriptionTranslation"/> to the Translations collection.
		/// </summary>
		/// <param name="item">The <see cref="IDescriptionTranslation"/> to add.</param>
		void AddTranslation(IDescriptionTranslation item);
	
		/// <summary>
		/// Removes an <see cref="IDescriptionTranslation"/> from the Translations collection.
		/// </summary>
		/// <param name="item">The <see cref="IDescriptionTranslation"/> to remove.</param>
		void RemoveTranslation(IDescriptionTranslation item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IArchive))]
		internal abstract class ArchiveContracts : IArchive
		{
		    #region Primitive properties
		
			int IArchive.ArchiveID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IArchive.ArchiveTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IArchive.ArchiveDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IArchive.StartDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IArchive.EndDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IArchive.ArchiveIcon
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IArchive.ArchiveImage
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IArchive.ArchivePath
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IArchive.TotalDownloads
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IArchive.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IArchive.IsEmailable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IArchive.IsDownloadable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IArchive.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IArchive.AccessibilityInfo
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IArchive.LanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IArchive.IsFeatured
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IArchiveType IArchive.ArchiveType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ICategory> IArchive.Categories
			{
				get { throw new NotImplementedException(); }
			}
		
			void IArchive.AddCategory(ICategory item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IArchive.RemoveCategory(ICategory item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IDescriptionTranslation> IArchive.Translations
			{
				get { throw new NotImplementedException(); }
			}
		
			void IArchive.AddTranslation(IDescriptionTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IArchive.RemoveTranslation(IDescriptionTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
