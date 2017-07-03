using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NetSteps.Common.Base
{
    /// <summary>
    /// Author: John Egbert
    /// Description: ObservableCollection class with AddRange and RemoveRange to reduce ChangeNotifications 
    /// http://blogs.windowsclient.net/tamirk/archive/2008/05/13/how-to-addrange-removerange-in-silverlight-observablecollection-lt-t-gt.aspx
    /// Created: 02-23-2010
    /// </summary>
    [Serializable]
    public sealed class BulkObservableCollection<T> : ObservableCollection<T>
    {
        private bool _suppressEvents;
        public bool SuppressEvents
        {
            get { return _suppressEvents; }
            set { _suppressEvents = value; }
        }

        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!SuppressEvents)
            {
                base.OnCollectionChanged(e);
            }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            SuppressEvents = true;
            foreach (T itm in collection)
            {
                this.Add(itm);
            }
            SuppressEvents = false;
            OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
        }

        public void RemoveRange(IEnumerable<T> collection)
        {
            SuppressEvents = true;
            foreach (T itm in collection)
            {
                this.Remove(itm);
            }
            SuppressEvents = false;
            OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
        }
    }
}
