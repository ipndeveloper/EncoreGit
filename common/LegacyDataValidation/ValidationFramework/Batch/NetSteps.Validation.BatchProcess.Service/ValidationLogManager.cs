using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetSteps.Validation.BatchProcess.Common;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;
using System.Diagnostics.Contracts;
using System.Collections.Concurrent;

namespace NetSteps.Validation.BatchProcess.Service
{
    public class ValidationLogManager : IRecordValidationResultManager
    {
        protected readonly ConcurrentStack<IRecord> ValidationStack;
        private bool _closeOnCompletingCurrentRecords;
        private readonly Thread _processThread;
        private EventWaitHandle _resetEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
        private IResultOutputHandler _handler;

        public ValidationLogManager(IResultOutputHandler handler)
        {
            _handler = handler;

            ValidationStack = new ConcurrentStack<IRecord>();
            _processThread = new Thread(new ThreadStart(HandleValidationStack));
            _processThread.Start();
        }

        private bool _isComplete
        {
            get
            {
                // such a kluge.... if _closeOnCompletingCurrentRecords is set, there are other threads still writing to the
                // stack.  So we can't check "Any" without having exceptions enumerating the stack.  Once that is set to
                // true, we should have exclusive on the validation stack and can safely enumerate and call Any().
                if (!_closeOnCompletingCurrentRecords)
                {
                    return false;
                }
                else
                {
                    return !ValidationStack.Any();
                }
            }
        }
        private void HandleValidationStack()
        {
            while (!_isComplete)
            {
                IRecord record;
                if (ValidationStack.TryPop(out record))
                {
                    HandleValidation(record);
                }
                else
                {
                    _resetEvent.WaitOne();
                }
            }
            _handler.Close();
        }

        protected virtual void HandleValidation(IRecord record)
        {
            Contract.Assert(record != null, "Attempted to write a handle validation with a null record.");

            _handler.Handle(record);
        }

        public void AddValidation(IRecord record)
        {
            Contract.Assert(record != null, "Attempted to write a null record to the validation log manager.");

            ValidationStack.Push(record);
            _resetEvent.Set();
        }

        public void NotifyValidationComplete()
        {
            _closeOnCompletingCurrentRecords = true;
            _resetEvent.Set();
        }


        public void OnFinished()
        {
            _processThread.Join();
        }
    }
}
