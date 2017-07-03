using System;
using System.Collections.Generic;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Generated;
using NetSteps.QueueProcessing.Modules.DomainEvent.Interfaces;

namespace NetSteps.QueueProcessing.Modules.DomainEvent.Class
{
    class DomainEventTypeTaskHandlerFactory : DomainEventTaskHandlerFactoryBase<Constants.DomainEventType, Func<IDomainEventTaskHandler>>
    {
        private IDictionary<Constants.DomainEventType, Func<IDomainEventTaskHandler>> _domainEventTaskHandlers =
         new Dictionary<Constants.DomainEventType, Func<IDomainEventTaskHandler>>();

        public override void Register(ConstantsGenerated.DomainEventType type, Func<IDomainEventTaskHandler> resolver)
        {
            if (HandlerExists(type))
                throw new Exception(String.Format("A domain event type task handler for {0} has already been registered", type));
            _domainEventTaskHandlers.Add(type, resolver);
        }

        public override Func<IDomainEventTaskHandler> Get(ConstantsGenerated.DomainEventType type)
        {
            Func<IDomainEventTaskHandler> handler = () => null;
            _domainEventTaskHandlers.TryGetValue(type, out handler);
            return handler;
        }

        public override bool HandlerExists(ConstantsGenerated.DomainEventType type)
        {
            return _domainEventTaskHandlers.ContainsKey(type);
        }
    }
}