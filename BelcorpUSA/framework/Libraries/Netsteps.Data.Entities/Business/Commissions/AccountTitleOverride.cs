using System;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Commissions
{
    public partial class AccountTitleOverride
    {
        #region Members

        private bool? _isEditable = null;

        #endregion

        #region Properties

        public bool IsEditable
        {
            get
            {
                if (_isEditable == null)
                {
                    _isEditable = Period.IsEditable(PeriodID);
                }

                return _isEditable.ToBool();
            }
        }

        #endregion

        #region Methods

        public static PaginatedList<AccountTitleOverrideSearchData> Search(AccountTitleOverrideSearchParameters searchParameters)
        {
            try
            {
                return Repository.Search(searchParameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        #endregion

    }
}
