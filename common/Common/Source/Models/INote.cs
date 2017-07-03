using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Common.Models
{
	/// <summary>
	/// Common interface for Note.
	/// </summary>
	[ContractClass(typeof(Contracts.NoteContracts))]
	public interface INote
	{
	    #region Primitive properties
	
		/// <summary>
		/// The NoteID for this Note.
		/// </summary>
		int NoteID { get; set; }
	
		/// <summary>
		/// The NoteTypeID for this Note.
		/// </summary>
		int NoteTypeID { get; set; }
	
		/// <summary>
		/// The UserID for this Note.
		/// </summary>
		Nullable<int> UserID { get; set; }
	
		/// <summary>
		/// The ParentID for this Note.
		/// </summary>
		Nullable<int> ParentID { get; set; }
	
		/// <summary>
		/// The DateCreatedUTC for this Note.
		/// </summary>
		System.DateTime DateCreatedUTC { get; set; }
	
		/// <summary>
		/// The Subject for this Note.
		/// </summary>
		string Subject { get; set; }
	
		/// <summary>
		/// The NoteText for this Note.
		/// </summary>
		string NoteText { get; set; }
	
		/// <summary>
		/// The DataVersion for this Note.
		/// </summary>
		byte[] DataVersion { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this Note.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The IsInternal for this Note.
		/// </summary>
		bool IsInternal { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Note1 for this Note.
		/// </summary>
	    INote Note1 { get; set; }
	
		/// <summary>
		/// The NoteType for this Note.
		/// </summary>
	    INoteType NoteType { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Notes1 for this Note.
		/// </summary>
		IEnumerable<INote> Notes1 { get; }
	
		/// <summary>
		/// Adds an <see cref="INote"/> to the Notes1 collection.
		/// </summary>
		/// <param name="item">The <see cref="INote"/> to add.</param>
		void AddNotes1(INote item);
	
		/// <summary>
		/// Removes an <see cref="INote"/> from the Notes1 collection.
		/// </summary>
		/// <param name="item">The <see cref="INote"/> to remove.</param>
		void RemoveNotes1(INote item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(INote))]
		internal abstract class NoteContracts : INote
		{
		    #region Primitive properties
		
			int INote.NoteID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int INote.NoteTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> INote.UserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> INote.ParentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime INote.DateCreatedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string INote.Subject
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string INote.NoteText
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] INote.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> INote.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool INote.IsInternal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    INote INote.Note1
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    INoteType INote.NoteType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<INote> INote.Notes1
			{
				get { throw new NotImplementedException(); }
			}
		
			void INote.AddNotes1(INote item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void INote.RemoveNotes1(INote item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
