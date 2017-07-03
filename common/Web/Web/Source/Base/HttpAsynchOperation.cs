using System;
using System.Threading;
using System.Web;

namespace NetSteps.Web.Base
{
    /// <summary>
    /// Author: John Egbert
    /// Description: A generic implementation of an HTTP IAsyncResult result.
    /// Taken from http://msdn.microsoft.com/en-us/library/ms227433.aspx
    /// Created: 04-22-2010
    /// </summary>
    public class HttpAsynchOperation : IAsyncResult
    {
        #region Members
        private bool _completed;
        private Object _state;
        private AsyncCallback _callback;
        private HttpContext _context;
        private Action<HttpContext> _action;
        #endregion

        #region Properties
        bool IAsyncResult.IsCompleted { get { return _completed; } }
        WaitHandle IAsyncResult.AsyncWaitHandle { get { return null; } }
        Object IAsyncResult.AsyncState { get { return _state; } }
        bool IAsyncResult.CompletedSynchronously { get { return false; } }
        #endregion

        #region Methods
        public HttpAsynchOperation(AsyncCallback callback, HttpContext context, Object state, Action<HttpContext> action)
        {
            _callback = callback;
            _context = context;
            _state = state;
            _action = action;
            _completed = false;
        }

        public void StartAsyncWork()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(StartAsyncTask), null);
        }

        private void StartAsyncTask(Object workItemState)
        {
            //_context.Response.Write("1. <p>Completion IsThreadPoolThread is " + Thread.CurrentThread.IsThreadPoolThread + "</p>\r\n");
            //_context.Response.Write("1. Hello World from Async Handler!");

            if (_action != null)
                _action(_context);

            _completed = true;
            _callback(this);
        }
        #endregion
    }
}
