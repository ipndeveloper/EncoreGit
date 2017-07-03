using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Modules.AccountNotes.Common
{
    /// <summary>
    /// Order Adapter
    /// </summary>
    public interface IAccountNotesRepositoryAdapter
    {
        /// <summary>
        /// Load Account Notes for a given account
        /// </summary>
        /// <param name="accountID">AccountID</param>
        /// <returns></returns>
        IEnumerable<INote> LoadAccountNotes(int accountID);

        /// <summary>
        /// create Account Notes for a given account
        /// </summary>
        /// <param name="accountID">AccountID</param>
        /// <param name="subject"></param>
        /// <param name="noteText"></param>
        /// <returns></returns>
        ICreateAccountNoteResult CreateAccountNote(int accountID, string subject, string noteText, int? NoteTypeID, int? ParentID, bool? IsInternal);
    }
}
