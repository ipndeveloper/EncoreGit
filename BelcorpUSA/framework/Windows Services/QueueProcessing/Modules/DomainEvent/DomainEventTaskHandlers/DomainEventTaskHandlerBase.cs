using NetSteps.Data.Entities;
using NetSteps.Encore.Core.IoC;
using NetSteps.QueueProcessing.Common;
using NetSteps.QueueProcessing.Modules.DomainEvent.Interfaces;

namespace NetSteps.QueueProcessing.Modules.DomainEvent.DomainEventTaskHandlers
{
    public abstract class DomainEventTaskHandlerBase : IDomainEventTaskHandler
    {
        protected IQueueProcessorLogger Logger { get; private set; }

        public DomainEventTaskHandlerBase()
        {
            Logger = Create.New<IQueueProcessorLogger>();
        }

        protected abstract bool Run(DomainEventQueueItem taskItem);

        #region IDomainEventTaskhandler Members

        bool IDomainEventTaskHandler.Run(DomainEventQueueItem taskItem)
        {
            return Run(taskItem);
        }

        #endregion
    }
}