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
    [KnownType(typeof(MailMessageGroup))]
    [KnownType(typeof(RecipientType))]
    [KnownType(typeof(RecipientStatus))]
    [KnownType(typeof(AddressType))]
    [KnownType(typeof(MailMessageRecipientEvent))]
    [Serializable]
    public partial class MailMessageGroupAddress: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        public int MailMessageGroupAddressID
        {
            get { return _mailMessageGroupAddressID; }
            set
            {
                if (_mailMessageGroupAddressID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'MailMessageGroupAddressID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _mailMessageGroupAddressID = value;
                    OnPropertyChanged("MailMessageGroupAddressID");
                }
            }
        }
        private int _mailMessageGroupAddressID;
    
        public int MailMessageGroupID
        {
            get { return _mailMessageGroupID; }
            set
            {
                if (_mailMessageGroupID != value)
                {
                    ChangeTracker.RecordOriginalValue("MailMessageGroupID", _mailMessageGroupID);
                    if (!IsDeserializing)
                    {
                        if (MailMessageGroup != null && MailMessageGroup.MailMessageGroupID != value)
                        {
                            MailMessageGroup = null;
                        }
                    }
                    _mailMessageGroupID = value;
                    OnPropertyChanged("MailMessageGroupID");
                }
            }
        }
        private int _mailMessageGroupID;
    
        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                if (_emailAddress != value)
                {
                    ChangeTracker.RecordOriginalValue("EmailAddress", _emailAddress);
                    _emailAddress = value;
                    OnPropertyChanged("EmailAddress");
                }
            }
        }
        private string _emailAddress;
    
        public string NickName
        {
            get { return _nickName; }
            set
            {
                if (_nickName != value)
                {
                    ChangeTracker.RecordOriginalValue("NickName", _nickName);
                    _nickName = value;
                    OnPropertyChanged("NickName");
                }
            }
        }
        private string _nickName;
    
        public Nullable<short> AddressTypeID
        {
            get { return _addressTypeID; }
            set
            {
                if (_addressTypeID != value)
                {
                    ChangeTracker.RecordOriginalValue("AddressTypeID", _addressTypeID);
                    if (!IsDeserializing)
                    {
                        if (AddressType != null && AddressType.AddressTypeID != value)
                        {
                            AddressType = null;
                        }
                    }
                    _addressTypeID = value;
                    OnPropertyChanged("AddressTypeID");
                }
            }
        }
        private Nullable<short> _addressTypeID;
    
        public Nullable<short> RecipientTypeID
        {
            get { return _recipientTypeID; }
            set
            {
                if (_recipientTypeID != value)
                {
                    ChangeTracker.RecordOriginalValue("RecipientTypeID", _recipientTypeID);
                    if (!IsDeserializing)
                    {
                        if (RecipientType != null && RecipientType.RecipientTypeID != value)
                        {
                            RecipientType = null;
                        }
                    }
                    _recipientTypeID = value;
                    OnPropertyChanged("RecipientTypeID");
                }
            }
        }
        private Nullable<short> _recipientTypeID;
    
        public Nullable<short> RecipientStatusID
        {
            get { return _recipientStatusID; }
            set
            {
                if (_recipientStatusID != value)
                {
                    ChangeTracker.RecordOriginalValue("RecipientStatusID", _recipientStatusID);
                    if (!IsDeserializing)
                    {
                        if (RecipientStatus != null && RecipientStatus.RecipientStatusID != value)
                        {
                            RecipientStatus = null;
                        }
                    }
                    _recipientStatusID = value;
                    OnPropertyChanged("RecipientStatusID");
                }
            }
        }
        private Nullable<short> _recipientStatusID;
    
        public Nullable<int> AccountID
        {
            get { return _accountID; }
            set
            {
                if (_accountID != value)
                {
                    ChangeTracker.RecordOriginalValue("AccountID", _accountID);
                    _accountID = value;
                    OnPropertyChanged("AccountID");
                }
            }
        }
        private Nullable<int> _accountID;

        #endregion
        #region Navigation Properties
    
            public MailMessageGroup MailMessageGroup
        {
            get { return _mailMessageGroup; }
            set
            {
                if (!ReferenceEquals(_mailMessageGroup, value))
                {
                    var previousValue = _mailMessageGroup;
                    _mailMessageGroup = value;
                    FixupMailMessageGroup(previousValue);
                    OnNavigationPropertyChanged("MailMessageGroup");
                }
            }
        }
        private MailMessageGroup _mailMessageGroup;
    
            public RecipientType RecipientType
        {
            get { return _recipientType; }
            set
            {
                if (!ReferenceEquals(_recipientType, value))
                {
                    var previousValue = _recipientType;
                    _recipientType = value;
                    FixupRecipientType(previousValue);
                    OnNavigationPropertyChanged("RecipientType");
                }
            }
        }
        private RecipientType _recipientType;
    
            public RecipientStatus RecipientStatus
        {
            get { return _recipientStatus; }
            set
            {
                if (!ReferenceEquals(_recipientStatus, value))
                {
                    var previousValue = _recipientStatus;
                    _recipientStatus = value;
                    FixupRecipientStatus(previousValue);
                    OnNavigationPropertyChanged("RecipientStatus");
                }
            }
        }
        private RecipientStatus _recipientStatus;
    
            public AddressType AddressType
        {
            get { return _addressType; }
            set
            {
                if (!ReferenceEquals(_addressType, value))
                {
                    var previousValue = _addressType;
                    _addressType = value;
                    FixupAddressType(previousValue);
                    OnNavigationPropertyChanged("AddressType");
                }
            }
        }
        private AddressType _addressType;
    
            public TrackableCollection<MailMessageRecipientEvent> MailMessageRecipientEvents
        {
            get
            {
                if (_mailMessageRecipientEvents == null)
                {
                    _mailMessageRecipientEvents = new TrackableCollection<MailMessageRecipientEvent>();
                    _mailMessageRecipientEvents.CollectionChanged += FixupMailMessageRecipientEvents;
                }
                return _mailMessageRecipientEvents;
            }
            set
            {
                if (!ReferenceEquals(_mailMessageRecipientEvents, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_mailMessageRecipientEvents != null)
                    {
                        _mailMessageRecipientEvents.CollectionChanged -= FixupMailMessageRecipientEvents;
                    }
                    _mailMessageRecipientEvents = value;
                    if (_mailMessageRecipientEvents != null)
                    {
                        _mailMessageRecipientEvents.CollectionChanged += FixupMailMessageRecipientEvents;
                    }
                    OnNavigationPropertyChanged("MailMessageRecipientEvents");
                }
            }
        }
        private TrackableCollection<MailMessageRecipientEvent> _mailMessageRecipientEvents;

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
            if (_mailMessageRecipientEvents != null)
    		{
    		    _mailMessageRecipientEvents.CollectionChanged -= FixupMailMessageRecipientEvents;
                _mailMessageRecipientEvents.CollectionChanged += FixupMailMessageRecipientEvents;
            }
        }
    
    
        protected virtual void ClearNavigationProperties()
        {
            MailMessageGroup = null;
            RecipientType = null;
            RecipientStatus = null;
            AddressType = null;
            MailMessageRecipientEvents.Clear();
        }

        #endregion
        #region Association Fixup
    
        private void FixupMailMessageGroup(MailMessageGroup previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.MailMessageGroupAddresses.Contains(this))
            {
                previousValue.MailMessageGroupAddresses.Remove(this);
            }
    
            if (MailMessageGroup != null)
            {
                if (!MailMessageGroup.MailMessageGroupAddresses.Contains(this))
                {
                    MailMessageGroup.MailMessageGroupAddresses.Add(this);
                }
    
                MailMessageGroupID = MailMessageGroup.MailMessageGroupID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("MailMessageGroup")
                    && (ChangeTracker.OriginalValues["MailMessageGroup"] == MailMessageGroup))
                {
                    ChangeTracker.OriginalValues.Remove("MailMessageGroup");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("MailMessageGroup", previousValue);
                }
                if (MailMessageGroup != null && !MailMessageGroup.ChangeTracker.ChangeTrackingEnabled)
                {
                    MailMessageGroup.StartTracking();
                }
            }
        }
    
        private void FixupRecipientType(RecipientType previousValue, bool skipKeys = false)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.MailMessageGroupAddresses.Contains(this))
            {
                previousValue.MailMessageGroupAddresses.Remove(this);
            }
    
            if (RecipientType != null)
            {
                if (!RecipientType.MailMessageGroupAddresses.Contains(this))
                {
                    RecipientType.MailMessageGroupAddresses.Add(this);
                }
    
                RecipientTypeID = RecipientType.RecipientTypeID;
            }
            else if (!skipKeys)
            {
                RecipientTypeID = null;
            }
    
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("RecipientType")
                    && (ChangeTracker.OriginalValues["RecipientType"] == RecipientType))
                {
                    ChangeTracker.OriginalValues.Remove("RecipientType");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("RecipientType", previousValue);
                }
                if (RecipientType != null && !RecipientType.ChangeTracker.ChangeTrackingEnabled)
                {
                    RecipientType.StartTracking();
                }
            }
        }
    
        private void FixupRecipientStatus(RecipientStatus previousValue, bool skipKeys = false)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.MailMessageGroupAddresses.Contains(this))
            {
                previousValue.MailMessageGroupAddresses.Remove(this);
            }
    
            if (RecipientStatus != null)
            {
                if (!RecipientStatus.MailMessageGroupAddresses.Contains(this))
                {
                    RecipientStatus.MailMessageGroupAddresses.Add(this);
                }
    
                RecipientStatusID = RecipientStatus.RecipientStatusID;
            }
            else if (!skipKeys)
            {
                RecipientStatusID = null;
            }
    
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("RecipientStatus")
                    && (ChangeTracker.OriginalValues["RecipientStatus"] == RecipientStatus))
                {
                    ChangeTracker.OriginalValues.Remove("RecipientStatus");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("RecipientStatus", previousValue);
                }
                if (RecipientStatus != null && !RecipientStatus.ChangeTracker.ChangeTrackingEnabled)
                {
                    RecipientStatus.StartTracking();
                }
            }
        }
    
        private void FixupAddressType(AddressType previousValue, bool skipKeys = false)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.MailMessageGroupAddresses.Contains(this))
            {
                previousValue.MailMessageGroupAddresses.Remove(this);
            }
    
            if (AddressType != null)
            {
                if (!AddressType.MailMessageGroupAddresses.Contains(this))
                {
                    AddressType.MailMessageGroupAddresses.Add(this);
                }
    
                AddressTypeID = AddressType.AddressTypeID;
            }
            else if (!skipKeys)
            {
                AddressTypeID = null;
            }
    
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("AddressType")
                    && (ChangeTracker.OriginalValues["AddressType"] == AddressType))
                {
                    ChangeTracker.OriginalValues.Remove("AddressType");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("AddressType", previousValue);
                }
                if (AddressType != null && !AddressType.ChangeTracker.ChangeTrackingEnabled)
                {
                    AddressType.StartTracking();
                }
            }
        }
    
        private void FixupMailMessageRecipientEvents(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (MailMessageRecipientEvent item in e.NewItems)
                {
                    item.MailMessageGroupAddress = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("MailMessageRecipientEvents", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (MailMessageRecipientEvent item in e.OldItems)
                {
                    if (ReferenceEquals(item.MailMessageGroupAddress, this))
                    {
                        item.MailMessageGroupAddress = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("MailMessageRecipientEvents", item);
                    }
                }
            }
        }

        #endregion
    }
}
