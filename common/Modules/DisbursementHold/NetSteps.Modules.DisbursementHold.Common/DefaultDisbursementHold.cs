using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Content.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Modules.DisbursementHold.Common.Models;
using NetSteps.Modules.DisbursementHold.Common.Results;

namespace NetSteps.Modules.DisbursementHold.Common
{
    /// <summary>
    /// Default Implementation of IDisbursementHold
    /// </summary>
     [ContainerRegister(typeof(IDisbursementHold), RegistrationBehaviors.IfNoneOther, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class DefaultDisbursementHold : IDisbursementHold
    {

        #region Declarations

        private ICheckHoldRepositoryAdapter _checkHoldRepository;

        private ITermResolver _termTranslation;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DefaultDisbursementHold() : this(Create.New<ICheckHoldRepositoryAdapter>(), Create.New<ITermResolver>()) { }

        /// <summary>
        /// Constructor with Dependency Injection
        /// </summary>
        /// <param name="checkHoldRepository"></param>
        /// <param name="termTranslation"></param>
        public DefaultDisbursementHold(ICheckHoldRepositoryAdapter checkHoldRepository, ITermResolver termTranslation)
        {
            _checkHoldRepository = checkHoldRepository ?? Create.New<ICheckHoldRepositoryAdapter>();
            _termTranslation = termTranslation ?? Create.New<ITermResolver>();
        }

        #endregion

        #region Methods

        private ICheckHoldResult CreateNewCheckHoldResult()
        {
            var result = Create.New<ICheckHoldResult>();
            result.ErrorMessages = new List<string>();
            result.Success = false;

            return result;
        }

        private ICheckHold TranslateCheckHoldResult(ICheckHoldResult checkHoldresult, ICheckHoldValues checkHoldValues, DateTime? holdUntil)
        {
            ICheckHold checkHold = Create.New<ICheckHold>();

            DateTime maxDate = new DateTime(9999, 12, 31);
            checkHold.HoldUntil = holdUntil.HasValue ? holdUntil.Value : maxDate;
            checkHold.UpdatedDate = DateTime.Today;

            checkHold.AccountID          = checkHoldresult.AccountID;
            checkHold.DisbursementHoldID = checkHoldresult.DisbursementHoldID;
            checkHold.HoldUntil          = checkHoldresult.HoldUntil;
            checkHold.CreatedDate        = checkHoldresult.CreatedDate;
            checkHold.StartDate          = checkHoldresult.StartDate;
            
            checkHold.ApplicationID      = checkHoldValues.ApplicationID;
            checkHold.Notes              = checkHoldValues.Notes;
            checkHold.OverrideReasonID   = checkHoldValues.OverrideReasonID;
            checkHold.UserID             = checkHoldValues.UserID;

            return checkHold;
        }

        public ICheckHoldResult LoadDisbursementHold(int accountID)
        {
            return _checkHoldRepository.LoadDisbursementHold(accountID);
        }       

        public ICheckHoldResult SaveDisbursementHold(int accountID, DateTime? holdUntil)
        {
            ICheckHoldResult result = CreateNewCheckHoldResult();

            ICheckHoldValues checkHoldValues = _checkHoldRepository.LoadCheckHoldValues();
            ICheckHoldResult checkHoldResult = _checkHoldRepository.LoadDisbursementHold(accountID);
            ICheckHold checkHold = TranslateCheckHoldResult(checkHoldResult, checkHoldValues, holdUntil);            

            if (checkHold.DisbursementHoldID != 0) 
            {
                // Update Disbursement Hold                
                result = _checkHoldRepository.UpdateDisbursementHold(checkHold);
            }
            else 
            {   
                // Save Disbursement Hold            
                DateTime today = DateTime.Today;
                checkHold.StartDate = today;
                checkHold.CreatedDate = today; 
                result = _checkHoldRepository.SaveDisbursementHold(checkHold);                
            }

            return result;
        }
        
        #endregion

    }
}
