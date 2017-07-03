namespace NetSteps.QueueProcessing.Host
{
    using NetSteps.Data.Entities;
    using NetSteps.Data.Entities.Exceptions;
    using NetSteps.Encore.Core.IoC;
    using NetSteps.QueueProcessing.Common;

    /// <summary>
    /// This service creates the QueueProcessor instances to handle polling needs.
    ///   1) Sending Email Queue Items
    /// Each QueueProcessor has it's own queue and worker threads that service the queue. This allows each QueueProcess to work completely separately, 
    /// and in parallel.
    /// </summary>
    public class QueueProcessingHost : DebuggableService // Must use DebuggableService in order for /i, /u, /debug command line options to work
    {
        IQueueProcessingService _service;

        public override void OnStart(string[] args)
        {
            try
            {
                this._service = Create.New<IQueueProcessingService>();
                this._service.StartQueues();
            }
            catch (System.Exception excp)
            {
                this.Logger.Info(excp.Message + " " + excp.StackTrace);
                EntityExceptionHelper.GetAndLogNetStepsException(excp, Constants.NetStepsExceptionType.NetStepsDataException);
                throw excp;
            }
        }

        public override void OnStop()
        {
            this._service.StopQueues();
        }
    }
}
