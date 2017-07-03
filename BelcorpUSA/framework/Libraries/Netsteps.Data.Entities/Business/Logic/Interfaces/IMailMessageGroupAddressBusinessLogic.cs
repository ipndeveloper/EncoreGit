using System;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Data.Entities.Mail;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IMailMessageGroupAddressBusinessLogic : IBusinessEntityLogic<MailMessageGroupAddress, Int32, IMailMessageGroupAddressRepository>, IBusinessLogic
    {
        MailMessageGroupAddressSearchTotals GetSearchTotals(IMailMessageGroupAddressRepository repository, MailMessageGroupAddressSearchParameters searchParameters);
        PaginatedList<MailMessageGroupAddressSearchData> Search(IMailMessageGroupAddressRepository repository, MailMessageGroupAddressSearchParameters searchParameters);
    }
}
