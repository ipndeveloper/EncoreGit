using System;
using NetSteps.Common.Base;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Business.Logic.Interfaces;
using NetSteps.Data.Entities.Mail;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Business.Logic
{
    [ContainerRegister(typeof(IMailMessageGroupAddressBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class MailMessageGroupAddressBusinessLogic : BusinessLogicBase<MailMessageGroupAddress, Int32, IMailMessageGroupAddressRepository, IMailMessageGroupAddressBusinessLogic>, IMailMessageGroupAddressBusinessLogic, IDefaultImplementation
    {
        public virtual MailMessageGroupAddressSearchTotals GetSearchTotals(IMailMessageGroupAddressRepository repository, MailMessageGroupAddressSearchParameters searchParameters)
        {
            return repository.GetSearchTotals(searchParameters);
        }

        public PaginatedList<MailMessageGroupAddressSearchData> Search(IMailMessageGroupAddressRepository repository, MailMessageGroupAddressSearchParameters searchParameters)
        {
            return repository.Search(searchParameters);
        }
    }
}
