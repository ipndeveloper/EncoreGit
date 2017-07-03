using NetSteps.Data.Entities;

namespace NetSteps.QueueProcessing.Modules.DomainEvent.Interfaces
{
    public interface IDomainEventTaskHandler
    {
        bool Run(DomainEventQueueItem taskItem);
    }
}
