using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class DomainEventQueueItemBusinessLogic
    {
        public override List<string> ValidatedChildPropertiesSetByParent(Repositories.IDomainEventQueueItemRepository repository)
        {
            List<string> list = new List<string>() { "DomainEventQueueItemID", "EventContextID" };
            return list;
        }
    }
}

