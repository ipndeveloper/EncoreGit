using System;
using System.Collections.Generic;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Generated;

namespace NetSteps.QueueProcessing.Modules.DomainEvent.Class
{
    using NetSteps.QueueProcessing.Modules.DomainEvent.Interfaces;

    class DomainEventCategoryTaskHandlerFactory : DomainEventTaskHandlerFactoryBase<Constants.DomainEventTypeCategory, Func<DomainEventQueueItem, IDomainEventTaskHandler>>
    {
        private IDictionary<Constants.DomainEventTypeCategory, Func<DomainEventQueueItem, IDomainEventTaskHandler>> _domainEventCategoryTaskHandlers =
            new Dictionary<Constants.DomainEventTypeCategory, Func<DomainEventQueueItem, IDomainEventTaskHandler>>();

        public override void Register(Constants.DomainEventTypeCategory category, Func<DomainEventQueueItem, IDomainEventTaskHandler> resolver)
        {
            if (HandlerExists(category))
                throw new Exception(String.Format("A domain event category task handler for {0} has already been registered", category));
            _domainEventCategoryTaskHandlers.Add(category, resolver);
        }

        public override bool HandlerExists(ConstantsGenerated.DomainEventTypeCategory category)
        {
            return _domainEventCategoryTaskHandlers.ContainsKey(category);
        }

        public override Func<DomainEventQueueItem, IDomainEventTaskHandler> Get(Constants.DomainEventTypeCategory category)
        {
            Func<DomainEventQueueItem, IDomainEventTaskHandler> handler = (d) => null;
            _domainEventCategoryTaskHandlers.TryGetValue(category, out handler);
            return handler;
        }
    }
}