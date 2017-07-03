using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Web.API.AccountNotes.Common.Model
{
	/// <summary>
	/// Model for creating a note
	/// </summary>
	public class CreateAccountNoteModel
	{
		/// <summary>
		/// Account to create the note fore
		/// </summary>
		public int AccountID { get; set; }
		/// <summary>
		/// Subject of the note
		/// </summary>
		public string Subject { get; set; }
		/// <summary>
		/// text of the note
		/// </summary>
		public string NoteText { get; set; }
		/// <summary>
		/// If the note is internal
		/// </summary>
		public bool? IsInternal { get; set; }
		/// <summary>
		/// parent note id
		/// </summary>
		public int? ParentID { get; set; }
	}
}
