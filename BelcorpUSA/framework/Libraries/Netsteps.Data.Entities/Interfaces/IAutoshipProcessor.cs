using System;
using System.Threading;
using NetSteps.Common.Events;

namespace NetSteps.Data.Entities.Interfaces
{
    /// <summary>
    /// Author: John Egbert
    /// Created: 08/18/2010
    /// </summary>
    public interface IAutoshipProcessor
    {
        event EventHandler<ProgressMessageEventArgs> ProgressMessage;
        void ProcessAllAutoships(DateTime runDate, int threads, CancellationTokenSource cancellationTokenSource);
    }
}
