using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NetSteps.Content.Common;
using NetSteps.Diagnostics.Logging.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Modules.AccountNotes.Common;
using NetSteps.Web.API.Base.Common;
using NetSteps.Web.API.AccountNotes.Common.Model;

namespace NetSteps.Web.API.AccountNotes.Common
{
    /// <summary>
    /// functions for Orders
    /// </summary>
    public class AccountsController : BaseController
    {

        #region Declarations

        //private IAccount _accountModule;
        private IAccountNotesRepositoryAdapter _accountModule;
        private ILogResolver _logResolver;
        private ITermResolver _termResolver;
        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an instance
        /// </summary>
        public AccountsController()
            : this(Create.New<IAccountNotesRepositoryAdapter>(), Create.New<ILogResolver>(), Create.New<ITermResolver>())
        {
            Contract.Ensures(_accountModule != null);
            Contract.Ensures(_termResolver != null);
            Contract.Ensures(_logResolver != null);
        }
        /// <summary>
        /// Create an instance
        /// </summary>
        /// <param name="accountModule">Account Note Module</param>
        /// <param name="lResolver">Log Resolver</param>
        /// <param name="tResolver">Term Resolver</param>
        public AccountsController(IAccountNotesRepositoryAdapter accountModule, ILogResolver lResolver, ITermResolver tResolver)
        {
            _accountModule = accountModule;
            _termResolver = tResolver;
            _logResolver = lResolver;
        }

        #endregion

        #region Methods

		private bool ValidateAccountNote(int accountID, ref string message)
		{
			bool isValid = true;

			if (accountID == 0)
			{
				isValid = false;
				string term = _termResolver.Term("AccountNote_Invalid_UserID", "Invalid UserID:");
				message = string.Format("{0} {1}", term, accountID);
			}

			return isValid;
		}

        private bool ValidateAccountNote(CreateAccountNoteModel model, ref string message)
        {
            bool isValid = true;

			if (model.AccountID == 0)
            {
                isValid = false;
                string term = _termResolver.Term("AccountNote_Invalid_UserID", "Invalid UserID:");
				message = string.Format("{0} {1}", term, model.AccountID);
            }
			if (model.Subject == null)
			{
				isValid = false;
				message = _termResolver.Term("AccountNote_Invalid_Subject", "Invalid Subject");
			}
			if (model.NoteText == null)
			{
				isValid = false;
				message = _termResolver.Term("AccountNote_Invalid_Note_Text", "Invalid Note Text");
			}

            return isValid;
        }


        /// <summary>
        /// Load account notes from an account
        /// 
        /// eg. http://yourdomain.com/account/{accountID}/accountnotes/{noteID}
        /// </summary>
        /// <param name="accountID">Account whose notes you want to load</param>
        /// <returns>ActionResult</returns>
        /// <seealso cref="ActionResult"/>
        [HttpGet]
		[ActionName("AccountNotes")]
        [ApiAccessKeyFilter]
        public ActionResult LoadAccountNotes(int accountID)
        {
            try
            {
                string message = string.Empty;

                bool isValidUserID = ValidateAccountNote(accountID, ref message);

                if (isValidUserID)
                {
                    IEnumerable<INote> result = _accountModule.LoadAccountNotes(accountID);
                    return this.Result_200_OK(result);
                }

                return this.Result_400_BadRequest(message);
            }
            catch (Exception ex)
            {
                _logResolver.LogException(ex, true);

                return this.Result_400_BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create an account note
        /// 
        /// eg. http://yourdomain.com/account/{accountID}/accountnotes
        /// </summary>
		/// <param name="accountNote">Account note to create</param>        
        /// <returns>ActionResult</returns>
        /// <seealso cref="ActionResult"/>
        [HttpPost]
		[ActionName("AccountNotes")]
        [ApiAccessKeyFilter]
        public ActionResult CreateAccountNote(CreateAccountNoteModel accountNote)
        {
            try
            {
                string message = string.Empty;

				bool isValidUserID = ValidateAccountNote(accountNote, ref message);

                if (isValidUserID)
                {
					bool noteIsInternal = accountNote.IsInternal.HasValue ? accountNote.IsInternal.Value : false;

                    ICreateAccountNoteResult result;

					result = _accountModule.CreateAccountNote(accountNote.AccountID, accountNote.Subject, accountNote.NoteText, 1, accountNote.ParentID, noteIsInternal);

                    return this.Result_200_OK(result);
                }

                return this.Result_400_BadRequest(message);
            }
            catch (Exception ex)
            {
                _logResolver.LogException(ex, true);

                return this.Result_400_BadRequest(ex.Message);
            }
        }

        #endregion

    }
}
