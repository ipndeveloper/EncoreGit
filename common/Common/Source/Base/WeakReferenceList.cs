using System;

namespace NetSteps.Common.Base
{
    /// <summary>
    /// Author: John Egbert
    /// Description: List of classes by weak reference to not prevent them from being Garbage collected - JHE
    /// Created: 12/14/2009
    /// </summary>
    [Serializable]
    public class WeakReferenceList<T> : CachedList<WeakReferenceObject<T>> where T : class
    {
        #region Members
        #endregion

        #region Properties
        public new T this[int index]
        {
            get
            {
                Touch();
                return this.List[index].RootObject;
            }
            set
            {
                lock (_lock)
                {
                    lock (_lock)
                    {
                        if (this.Contains(value))
                            this.Remove(value);
                        this.List[index] = new WeakReferenceObject<T>(value);
                    }
                }
            }
        }
        #endregion

        #region Methods
        public void Add(T item)
        {
            lock (_lock)
            {
                if (!this.Contains(item))
                    List.Add(new WeakReferenceObject<T>(item));
            }
        }

        public bool Remove(T item)
        {
            if (this.Contains(item))
            {
                lock (_lock)
                {
                    foreach (var listItem in List)
                        if (listItem.RootObject == item)
                            return List.Remove(listItem);
                }
            }

            return false;
        }

        public bool Contains(T item)
        {
            Touch();
            foreach (var listItem in List)
                return (listItem.RootObject == item);

            return false;
        }

        public void Insert(int index, T item)
        {
            Touch();
            lock (_lock)
            {
                List.Insert(index, new WeakReferenceObject<T>(item));
            }
        }

        protected void Touch()
        {
            CleanOutDeadReferences();
        }

        public void CleanOutDeadReferences()
        {
            lock (_lock)
            {
                for (int i = 0; i < this.Count; i++)
                    if (!List[i].Reference.IsAlive)
                        List.RemoveAt(i--);
            }
        }
        #endregion
    }

    [Serializable]
    public class WeakReferenceObject<T> where T : class
    {
        private WeakReference _rootObject = null;
        public WeakReference Reference
        {
            get
            {
                return _rootObject;
            }
        }

        public T RootObject
        {
            get
            {
                return (T)_rootObject.Target;
            }
        }

        public WeakReferenceObject(object target)
        {
            _rootObject = new WeakReference(target);
        }
    }
}