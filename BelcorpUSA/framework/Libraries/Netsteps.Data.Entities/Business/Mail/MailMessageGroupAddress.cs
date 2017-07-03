using System;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.Logic.Interfaces;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Mail
{
    public partial class MailMessageGroupAddress : EntityBusinessBase<MailMessageGroupAddress, Int32, IMailMessageGroupAddressRepository, IMailMessageGroupAddressBusinessLogic>
    {
        #region Methods
        public static MailMessageGroupAddressSearchTotals GetSearchTotals(MailMessageGroupAddressSearchParameters searchParameters)
        {
            return BusinessLogic.GetSearchTotals(Repository, searchParameters);
        }

        public static PaginatedList<MailMessageGroupAddressSearchData> Search(MailMessageGroupAddressSearchParameters searchParameters)
        {
            return BusinessLogic.Search(Repository, searchParameters);
        }
        #endregion
    }
}
