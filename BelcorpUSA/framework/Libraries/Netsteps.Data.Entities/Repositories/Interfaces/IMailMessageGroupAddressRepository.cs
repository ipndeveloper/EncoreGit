using System;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Mail;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IMailMessageGroupAddressRepository : IBaseRepository<MailMessageGroupAddress, Int32>
    {
        MailMessageGroupAddressSearchTotals GetSearchTotals(MailMessageGroupAddressSearchParameters searchParameters);
        PaginatedList<MailMessageGroupAddressSearchData> Search(MailMessageGroupAddressSearchParameters searchParameters);
    }
}
