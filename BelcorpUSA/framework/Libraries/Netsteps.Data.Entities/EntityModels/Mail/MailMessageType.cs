//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace NetSteps.Data.Entities.Mail
{
    [KnownType(typeof(MailMessage))]
    [Serializable]
    public partial class MailMessageType: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        public short MailMessageTypeID
        {
            get { return _mailMessageTypeID; }
            set
            {
                if (_mailMessageTypeID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'MailMessageTypeID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _mailMessageTypeID = value;
                    OnPropertyChanged("MailMessageTypeID");
                }
            }
        }
        private short _mailMessageTypeID;
    
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    ChangeTracker.RecordOriginalValue("Description", _description);
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        private string _description;

        #endregion
        #region Navigation Properties
    
            public TrackableCollection<MailMessage> MailMessages
        {
            get
            {
                if (_mailMessages == null)
                {
                    _mailMessages = new TrackableCollection<MailMessage>();
                    _mailMessages.CollectionChanged += FixupMailMessages;
                }
                return _mailMessages;
            }
            set
            {
                if (!ReferenceEquals(_mailMessages, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_mailMessages != null)
                    {
                        _mailMessages.CollectionChanged -= FixupMailMessages;
                    }
                    _mailMessages = value;
                    if (_mailMessages != null)
                    {
                        _mailMessages.CollectionChanged += FixupMailMessages;
                    }
                    OnNavigationPropertyChanged("MailMessages");
                }
            }
        }
        private TrackableCollection<MailMessage> _mailMessages;

        #endregion
        #region ChangeTracking
    
        protected virtual void OnPropertyChanged(String propertyName)
        {
            if (ChangeTracker.State != ObjectState.Added && ChangeTracker.State != ObjectState.Deleted)
            {
                ChangeTracker.State = ObjectState.Modified;
            }
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
        protected virtual void OnNavigationPropertyChanged(String propertyName)
        {
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged{ add { _propertyChanged += value; } remove { _propertyChanged -= value; } }
        private event PropertyChangedEventHandler _propertyChanged;
        private ObjectChangeTracker _changeTracker;
    
        public ObjectChangeTracker ChangeTracker
        {
            get
            {
                if (_changeTracker == null)
                {
                    _changeTracker = new ObjectChangeTracker();
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
                return _changeTracker;
            }
            set
            {
                if(_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging -= HandleObjectStateChanging;
                }
                _changeTracker = value;
                if(_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
            }
        }
    
        private void HandleObjectStateChanging(object sender, ObjectStateChangingEventArgs e)
        {
            if (e.NewState == ObjectState.Deleted)
            {
                ClearNavigationProperties();
            }
        }
    
        protected bool IsDeserializing { get; private set; }
    
        [OnDeserializing]
        public void OnDeserializingMethod(StreamingContext context)
        {
            IsDeserializing = true;
        }
    
        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            IsDeserializing = false;
            ChangeTracker.ChangeTrackingEnabled = true;
            if (_mailMessages != null)
    		{
    		    _mailMessages.CollectionChanged -= FixupMailMessages;
                _mailMessages.CollectionChanged += FixupMailMessages;
            }
        }
    
    
        protected virtual void ClearNavigationProperties()
        {
            MailMessages.Clear();
        }

        #endregion
        #region Association Fixup
    
        private void FixupMailMessages(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (MailMessage item in e.NewItems)
                {
                    item.MailMessageType = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("MailMessages", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (MailMessage item in e.OldItems)
                {
                    if (ReferenceEquals(item.MailMessageType, this))
                    {
                        item.MailMessageType = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("MailMessages", item);
                    }
                }
            }
        }

        #endregion
    }
}