using System;
using System.Collections.Concurrent;
using System.Data.SqlClient;
using System.Linq;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Threading;

namespace NetSteps.Common.Base
{
    /// <summary>
    /// Author: John Egbert
    /// Description: This is a CachedList{T} with additional functionality to work with 'business' lists.
    /// Added support to integrate with SqlDependency to expire cache when notified of a change from the DB. - JHE
    /// Ex: GetById method
    /// Created: 05-03-2010
    /// </summary>
    [Serializable]
    public class CachedListBusiness<T, TKeyType> : CachedList<T> where TKeyType : IComparable
    {
        #region Members
        private SqlDependency _sqlDependency = null;

        protected TimeSpan _expiration = TimeSpan.FromMinutes(5);
        protected TimerAction _expireCacheTimer = null;

        protected ConcurrentDictionary<TKeyType, T> _lookupDictionary;
        #endregion

        #region Properties
        /// <summary>
        /// Override this so the GetById method will work:
        /// Example:
        ///     protected override Func{AccountStatus, Int32} GetIdColumn
        ///     {
        ///         get
        ///         {
        ///             return i => i.AccountStatusID;
        ///         }
        ///     }
        /// </summary>
        protected virtual Func<T, TKeyType> GetIdColumnFunc
        {
            get
            {
                return null;
            }
        }

        protected SqlDependency SqlDependency
        {
            get
            {
                return _sqlDependency;
            }
            set
            {
                lock (_lock)
                {
                    _sqlDependency = value;
                    if (_sqlDependency != null)
                    {
                        _sqlDependency.OnChange -= new System.Data.SqlClient.OnChangeEventHandler(SqlDependency_OnChange);
                        _sqlDependency.OnChange += new System.Data.SqlClient.OnChangeEventHandler(SqlDependency_OnChange);
                    }
                    else
                    {
                        // Timer will expire the data if _sqlDependency comes back null (meaning Sql Dependency is off or not working in the DB) - JHE
                        if (_expireCacheTimer == null)
                        {
                            _expireCacheTimer = new TimerAction(_expiration.TotalMilliseconds);
                            _expireCacheTimer.Action = () =>
                            {
                                _expireCacheTimer.Stop();
                                ExpireCache();
                                OnDataChanged(this, null);
                            };
                            StartExpireCacheTimer();
                        }
                        else
                        {
                            _expireCacheTimer.Stop();
                            StartExpireCacheTimer();
                        }

                    }
                }
            }
        }

        public TimeSpan Expiration
        {
            get
            {
                return _expiration;
            }
            set
            {
                // Reset the timeout - JHE
                _expireCacheTimer.Stop();
                _expireCacheTimer.Interval = value.TotalMilliseconds;
                StartExpireCacheTimer();
            }
        }
        #endregion

        #region Events
        public event EventHandler DataChanged;
        protected virtual void OnDataChanged(object sender, EventArgs e)
        {
            if (DataChanged != null)
                DataChanged(this, e);
        }
        #endregion

        #region Methods
        public T GetById(TKeyType id)
        {
            // Use GetIdColumnFunc is set else see if the T is an Business Entity and use it's GetIdColumnFunc - JHE
            if (GetIdColumnFunc != null)
                return List.FirstOrDefault(i => ((IComparable)id).Equals(GetIdColumnFunc(i)));
            else
            {
                if (_lookupDictionary != null && _lookupDictionary.ContainsKey(id))
                    return _lookupDictionary[id];
                else
                    return List.FirstOrDefault(i => (i is IKeyName<T, TKeyType>) ? ((IComparable)id).Equals((i as IKeyName<T, TKeyType>).GetIdColumnFunc(i)) :
                        ((IComparable)id).Equals(GetIdColumnFunc(i)));
            }
        }

        private void StartExpireCacheTimer()
        {
            if (_expireCacheTimer.Interval != 0f && this.List != null)
                _expireCacheTimer.Start();
        }

        public override void ExpireCache()
        {
            lock (this._lock)
            {
                this._list = null;
                this._lookupDictionary = null;
            }
        }

        #endregion

        #region Event Handlers
        protected void SqlDependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (sender == null || (sender as SqlDependency) == null)
            {
                return;
            }

            if (e.Type == SqlNotificationType.Change)
            {
                switch (e.Info)
                {
                    case SqlNotificationInfo.Truncate:
                    case SqlNotificationInfo.Insert:
                    case SqlNotificationInfo.Update:
                    case SqlNotificationInfo.Delete:
                    case SqlNotificationInfo.Drop:
                    case SqlNotificationInfo.Alter:
                        this.ExpireCache();
                        (sender as SqlDependency).OnChange -= new OnChangeEventHandler(this.SqlDependency_OnChange);
                        this.OnDataChanged(this, null);
                        break;
                }
            }
        }
        #endregion
    }
}