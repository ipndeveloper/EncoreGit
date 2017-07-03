using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Common.Models
{
	/// <summary>
	/// Common interface for NoteType.
	/// </summary>
	[ContractClass(typeof(Contracts.NoteTypeContracts))]
	public interface INoteType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The NoteTypeID for this NoteType.
		/// </summary>
		int NoteTypeID { get; set; }
	
		/// <summary>
		/// The Name for this NoteType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this NoteType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this NoteType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this NoteType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Notes for this NoteType.
		/// </summary>
		IEnumerable<INote> Notes { get; }
	
		/// <summary>
		/// Adds an <see cref="INote"/> to the Notes collection.
		/// </summary>
		/// <param name="item">The <see cref="INote"/> to add.</param>
		void AddNote(INote item);
	
		/// <summary>
		/// Removes an <see cref="INote"/> from the Notes collection.
		/// </summary>
		/// <param name="item">The <see cref="INote"/> to remove.</param>
		void RemoveNote(INote item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(INoteType))]
		internal abstract class NoteTypeContracts : INoteType
		{
		    #region Primitive properties
		
			int INoteType.NoteTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string INoteType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string INoteType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string INoteType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool INoteType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<INote> INoteType.Notes
			{
				get { throw new NotImplementedException(); }
			}
		
			void INoteType.AddNote(INote item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void INoteType.RemoveNote(INote item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
