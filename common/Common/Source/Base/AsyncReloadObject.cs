using System;
using System.Threading;
using NetSteps.Common.Exceptions;

namespace NetSteps.Common.Base
{
    /// <summary>
    /// Asynchronously load type <typeparamref name="T"/> in the background, blocking only on initial load.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncReloadObject<T> : IDisposable where T : class
    {
        #region Private Fields

        private readonly Func<T> _loadForReal;
        private volatile object _value;
        private static readonly object _dummy = new object();

        private readonly object _loadLock = new object();
        private readonly TimeSpan _reloadInterval;
        private readonly Timer _reloadTimer;
        private const int MAX_TIME = int.MaxValue;

        private int _isDisposed;

        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lazyLoaderFunction">This function should be thread safe</param>
        /// <param name="reloadInterval">Defaults to 1 day when zero</param>
        public AsyncReloadObject(Func<T> lazyLoaderFunction, TimeSpan reloadInterval)
        {
            _loadForReal = lazyLoaderFunction;
            _value = _dummy;

            _reloadInterval = (long) reloadInterval.TotalMilliseconds < (long) MAX_TIME
                                  ? reloadInterval
                                  : TimeSpan.FromMilliseconds(MAX_TIME);
            if (_reloadInterval <= TimeSpan.Zero)
            {
                _reloadInterval = TimeSpan.FromDays(1);
            }

            _reloadTimer = new Timer(obj => Reload(), null, Timeout.Infinite, Timeout.Infinite);
        }

        #endregion

        #region Properties

        public bool DataLoaded
        {
            get { return !object.ReferenceEquals(_dummy, _value); }
        }

        #endregion

        #region Disposal

        ~AsyncReloadObject()
        {
            Dispose(false);
        }

        private void Dispose(bool isDisposing)
        {
            if (Interlocked.Exchange(ref _isDisposed, 1) == 1)
            {
                if (isDisposing)
                {
                    _reloadTimer.Dispose();
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        #endregion

        #region Public Methods

        public T Value
        {
            get
            {
                object t = _value;
                if (object.ReferenceEquals(_dummy, t))
                {
                    lock (_loadLock)
                    {
                        t = _value;
                        if (object.ReferenceEquals(_dummy, t))
                        {
                            try
                            {
                                _value = t = _loadForReal();
                                _reloadTimer.Change(_reloadInterval, _reloadInterval);
                            }
                            catch (Exception ex)
                            {
                                _reloadTimer.Change(TimeSpan.FromMinutes(1), _reloadInterval);
                                throw new AsyncReloadLoadException(ex);
                            }
                        }
                    }
                }

                return t as T;
            }
        }

        private void Reload()
        {
            object t = _value;
            lock (_loadLock)
            {
                if (object.ReferenceEquals(t, _value))
                {
                    try
                    {
                        _value = _loadForReal();
                        _reloadTimer.Change(_reloadInterval, _reloadInterval);
                    }
                    catch (Exception ex)
                    {
                        _reloadTimer.Change(TimeSpan.FromMinutes(1), _reloadInterval);
                        throw new AsyncReloadLoadException(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Trigger an async reload.  Resets the timer controlling background reloads. Expensive now (in a background thread), cheap later (not lazy). See <seealso cref="InvalidateData"/>
        /// </summary>
        public void AsyncReload()
        {
            _reloadTimer.Change(0, Timeout.Infinite);
        }

        /// <summary>
        /// Invalidate the data and stop the timer.  The next call to <see cref="Value"/> will restart the timer and block until the data has loaded. Cheap now, expensive later (lazy). See <seealso cref="AsyncReload"/>
        /// </summary>
        public void InvalidateData()
        {
            _reloadTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _value = _dummy;
        }
        #endregion
    }
}
