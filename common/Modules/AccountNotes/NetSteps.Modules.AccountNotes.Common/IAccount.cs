using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.AccountNotes.Common
{
    /// <summary>
    /// AccountNote
    /// </summary>    
    public interface IAccount
    {
        #region Methods
        /// <summary>
        /// Load Account Notes for a given account
        /// </summary>
        /// <param name="accountID">AccountID</param>
        /// <returns></returns>
        IEnumerable<INote> LoadAccountNotes(int accountID);

        /// <summary>
        /// Create Account Note
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="subject"></param>
        /// <param name="noteText"></param>
        /// <returns></returns>
        ICreateAccountNoteResult CreateAccountNote(int accountID, string subject, string noteText, int? ParentID = null);

        /// <summary>
        /// Create Internal Account Note
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="subject"></param>
        /// <param name="noteText"></param>
        /// <returns></returns>
        ICreateAccountNoteResult CreateInternalAccountNote(int accountID, string subject, string noteText, int? ParentID = null);

        #endregion
    }
}
