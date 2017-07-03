using System;

namespace NetSteps.QueueProcessing.Modules.DomainEvent.Class
{
    abstract class DomainEventTaskHandlerFactoryBase<EventType, HandlerResolver>
            where EventType : struct
    {
        public abstract void Register(EventType type, HandlerResolver resolver);
        public abstract HandlerResolver Get(EventType type);
        public abstract bool HandlerExists(EventType type);
        public virtual HandlerResolver Get(string eventTypeTermName)
        {
            EventType result;
            Enum.TryParse<EventType>(eventTypeTermName, out result);
            return Get(result);
        }
    }
}
