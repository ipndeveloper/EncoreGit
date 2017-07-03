using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Modules.AccountNotes.Common
{
    /// <summary>
    /// Default Implementation of IAccountNote
    /// </summary>
    [ContainerRegister(typeof(IAccount), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class DefaultAccount : IAccount
    {

        #region Constructors

        private IAccountNotesRepositoryAdapter _repository;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DefaultAccount() : this(Create.New<IAccountNotesRepositoryAdapter>()) { }

        /// <summary>
        /// Constructer with an Adapter
        /// </summary>
        /// <param name="repository"></param>
        public DefaultAccount(IAccountNotesRepositoryAdapter repository)
        {
            _repository = repository ?? Create.New<IAccountNotesRepositoryAdapter>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load Account Notes for a given account
        /// </summary>
        /// <param name="accountID">AccountID</param>
        /// <returns></returns>
        public IEnumerable<INote> LoadAccountNotes(int accountID)
        {
            Contract.Ensures(accountID > 0);
            var accountNotes = _repository.LoadAccountNotes(accountID);

            return accountNotes;
        }

        /// <summary>
        /// Create Account Note
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="subject"></param>
        /// <param name="noteText"></param>
        /// <returns></returns>
        public ICreateAccountNoteResult CreateAccountNote(int accountID, string subject, string noteText, int? ParentID = null)
        {
            Contract.Requires<ArgumentException>(accountID > 0);
			Contract.Requires<ArgumentNullException>(subject != null);
			Contract.Requires<ArgumentNullException>(noteText != null);

            var result = _repository.CreateAccountNote(accountID, subject, noteText, 1, ParentID, false);

            return result;
        }

        /// <summary>
        /// Create Internal Account Note
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="subject"></param>
        /// <param name="noteText"></param>
        /// <returns></returns>
        public ICreateAccountNoteResult CreateInternalAccountNote(int accountID, string subject, string noteText, int? ParentID = null)
        {
			Contract.Requires<ArgumentException>(accountID > 0);
			Contract.Requires<ArgumentNullException>(subject != null);
			Contract.Requires<ArgumentNullException>(noteText != null);

            var result = _repository.CreateAccountNote(accountID, subject, noteText, 1, ParentID, true);

            return result;
        }

        #endregion
    }
}
