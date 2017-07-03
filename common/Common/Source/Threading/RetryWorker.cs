using System;
using System.Threading;

namespace NetSteps.Common.Threading
{
    public sealed class RetryWorker<THandback>
    {
        readonly static int __max_attempts = 3;
        readonly static double __backoff_factor = 1.3d;

        int _attempts = 0;
        int _millisecond_wait = 2000;
        DateTime _execute_after = DateTime.Now;
        readonly Func<RetryWorker<THandback>, Exception, int, bool> _retryTest;
        readonly Action<RetryWorker<THandback>, Exception> _failureHandler;
        
        Action<THandback> _action;
        THandback _handback;

        public RetryWorker() : this(null, null)
        {
        }
        public RetryWorker(Func<RetryWorker<THandback>, Exception, int, bool> retryTest,
            Action<RetryWorker<THandback>, Exception> failureHandler)
        {
            this._retryTest = retryTest;
            this._failureHandler = failureHandler;
        }
        public RetryWorker<THandback> Fork(Action<THandback> action)
        {
            return Fork(action, default(THandback));
        }
        public RetryWorker<THandback> Fork(Action<THandback> action, THandback handback)
        {
            if (action == null) throw new ArgumentNullException("action");
            if (_action != null) throw new InvalidOperationException("the action has already been scheduled");
            this._handback = handback;
            _action = action;
            ThreadPool.QueueUserWorkItem(new WaitCallback(AttemptBackgroundWork), this);
            return this;
        }

        bool CanAttempt { get { return DateTime.Now >= _execute_after; } }
        bool ShouldRetry(Exception ex)
        {
            return _attempts < __max_attempts
                && (_retryTest == null || _retryTest(this, ex, _attempts));
        }
        RetryWorker<THandback> IncrementAttempts()
        {
            _attempts++;
            return this;
        }
        RetryWorker<THandback> ScheduleRetry()
        {
            _millisecond_wait = Convert.ToInt32(_millisecond_wait * __backoff_factor);
            _execute_after = DateTime.Now.Add(TimeSpan.FromMilliseconds(_millisecond_wait));
            ThreadPool.QueueUserWorkItem(AttemptBackgroundWork, this);
            return this;
        }
        void Failure(Exception ex)
        {
            if (_failureHandler != null) _failureHandler(this, ex);
        }
        static void AttemptBackgroundWork(object state)
        {
            var worker = (RetryWorker<THandback>)state;

            if (worker.CanAttempt)
            {
                try
                {
                    worker.IncrementAttempts();
                    worker._action(worker._handback);
                }
                catch (Exception ex)
                {
                    if (worker.ShouldRetry(ex))
                    {
                        worker.ScheduleRetry();
                    }
                    else
                    {
                        worker.Failure(ex);
                    }
                }
            }
            else
            {   // Delay. This amounts to spinning, it returns the thread to the threadpool
                // so other activity can continue; it will attempt after "spinning" up to the wait timeout
                ThreadPool.QueueUserWorkItem(AttemptBackgroundWork, worker);
            }
        }
    }
}
