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
    [KnownType(typeof(MailMessageAttachment))]
    [KnownType(typeof(MailMessageGroup))]
    [KnownType(typeof(MailFolderType))]
    [KnownType(typeof(MailMessageType))]
    [KnownType(typeof(MailMessagePriority))]
    [Serializable]
    public partial class MailMessage: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        public int MailMessageID
        {
            get { return _mailMessageID; }
            set
            {
                if (_mailMessageID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'MailMessageID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _mailMessageID = value;
                    OnPropertyChanged("MailMessageID");
                }
            }
        }
        private int _mailMessageID;
    
        public string Subject
        {
            get { return _subject; }
            set
            {
                if (_subject != value)
                {
                    ChangeTracker.RecordOriginalValue("Subject", _subject);
                    _subject = value;
                    OnPropertyChanged("Subject");
                }
            }
        }
        private string _subject;
    
        public string Body
        {
            get { return _body; }
            set
            {
                if (_body != value)
                {
                    ChangeTracker.RecordOriginalValue("Body", _body);
                    _body = value;
                    OnPropertyChanged("Body");
                }
            }
        }
        private string _body;
    
        public string HTMLBody
        {
            get { return _hTMLBody; }
            set
            {
                if (_hTMLBody != value)
                {
                    ChangeTracker.RecordOriginalValue("HTMLBody", _hTMLBody);
                    _hTMLBody = value;
                    OnPropertyChanged("HTMLBody");
                }
            }
        }
        private string _hTMLBody;
    
        public string FromAddress
        {
            get { return _fromAddress; }
            set
            {
                if (_fromAddress != value)
                {
                    ChangeTracker.RecordOriginalValue("FromAddress", _fromAddress);
                    _fromAddress = value;
                    OnPropertyChanged("FromAddress");
                }
            }
        }
        private string _fromAddress;
    
        public string FromNickName
        {
            get { return _fromNickName; }
            set
            {
                if (_fromNickName != value)
                {
                    ChangeTracker.RecordOriginalValue("FromNickName", _fromNickName);
                    _fromNickName = value;
                    OnPropertyChanged("FromNickName");
                }
            }
        }
        private string _fromNickName;
    
        public Nullable<int> MailAccountID
        {
            get { return _mailAccountID; }
            set
            {
                if (_mailAccountID != value)
                {
                    ChangeTracker.RecordOriginalValue("MailAccountID", _mailAccountID);
                    _mailAccountID = value;
                    OnPropertyChanged("MailAccountID");
                }
            }
        }
        private Nullable<int> _mailAccountID;
    
        public bool IsOutbound
        {
            get { return _isOutbound; }
            set
            {
                if (_isOutbound != value)
                {
                    ChangeTracker.RecordOriginalValue("IsOutbound", _isOutbound);
                    _isOutbound = value;
                    OnPropertyChanged("IsOutbound");
                }
            }
        }
        private bool _isOutbound;
    
        public short MailMessageTypeID
        {
            get { return _mailMessageTypeID; }
            set
            {
                if (_mailMessageTypeID != value)
                {
                    ChangeTracker.RecordOriginalValue("MailMessageTypeID", _mailMessageTypeID);
                    if (!IsDeserializing)
                    {
                        if (MailMessageType != null && MailMessageType.MailMessageTypeID != value)
                        {
                            MailMessageType = null;
                        }
                    }
                    _mailMessageTypeID = value;
                    OnPropertyChanged("MailMessageTypeID");
                }
            }
        }
        private short _mailMessageTypeID;
    
        public bool BeenRead
        {
            get { return _beenRead; }
            set
            {
                if (_beenRead != value)
                {
                    ChangeTracker.RecordOriginalValue("BeenRead", _beenRead);
                    _beenRead = value;
                    OnPropertyChanged("BeenRead");
                }
            }
        }
        private bool _beenRead;
    
        public short MailMessagePriorityID
        {
            get { return _mailMessagePriorityID; }
            set
            {
                if (_mailMessagePriorityID != value)
                {
                    ChangeTracker.RecordOriginalValue("MailMessagePriorityID", _mailMessagePriorityID);
                    if (!IsDeserializing)
                    {
                        if (MailMessagePriority != null && MailMessagePriority.MailMessagePriorityID != value)
                        {
                            MailMessagePriority = null;
                        }
                    }
                    _mailMessagePriorityID = value;
                    OnPropertyChanged("MailMessagePriorityID");
                }
            }
        }
        private short _mailMessagePriorityID;
    
        public Nullable<int> VisualTemplateID
        {
            get { return _visualTemplateID; }
            set
            {
                if (_visualTemplateID != value)
                {
                    ChangeTracker.RecordOriginalValue("VisualTemplateID", _visualTemplateID);
                    _visualTemplateID = value;
                    OnPropertyChanged("VisualTemplateID");
                }
            }
        }
        private Nullable<int> _visualTemplateID;
    
        public short MailFolderTypeID
        {
            get { return _mailFolderTypeID; }
            set
            {
                if (_mailFolderTypeID != value)
                {
                    ChangeTracker.RecordOriginalValue("MailFolderTypeID", _mailFolderTypeID);
                    if (!IsDeserializing)
                    {
                        if (MailFolderType != null && MailFolderType.MailFolderTypeID != value)
                        {
                            MailFolderType = null;
                        }
                    }
                    _mailFolderTypeID = value;
                    OnPropertyChanged("MailFolderTypeID");
                }
            }
        }
        private short _mailFolderTypeID;
    
        public string AttachmentUniqueID
        {
            get { return _attachmentUniqueID; }
            set
            {
                if (_attachmentUniqueID != value)
                {
                    ChangeTracker.RecordOriginalValue("AttachmentUniqueID", _attachmentUniqueID);
                    _attachmentUniqueID = value;
                    OnPropertyChanged("AttachmentUniqueID");
                }
            }
        }
        private string _attachmentUniqueID;
    
        public Nullable<bool> Locked
        {
            get { return _locked; }
            set
            {
                if (_locked != value)
                {
                    ChangeTracker.RecordOriginalValue("Locked", _locked);
                    _locked = value;
                    OnPropertyChanged("Locked");
                }
            }
        }
        private Nullable<bool> _locked;
    
        public Nullable<int> SiteID
        {
            get { return _siteID; }
            set
            {
                if (_siteID != value)
                {
                    ChangeTracker.RecordOriginalValue("SiteID", _siteID);
                    _siteID = value;
                    OnPropertyChanged("SiteID");
                }
            }
        }
        private Nullable<int> _siteID;
    
        public System.DateTime DateAddedUTC
        {
            get { return _dateAddedUTC; }
            set
            {
                if (_dateAddedUTC != value)
                {
                    ChangeTracker.RecordOriginalValue("DateAddedUTC", _dateAddedUTC);
                    _dateAddedUTC = value;
                    OnPropertyChanged("DateAddedUTC");
                }
            }
        }
        private System.DateTime _dateAddedUTC;
    
        public Nullable<int> CampaignActionID
        {
            get { return _campaignActionID; }
            set
            {
                if (_campaignActionID != value)
                {
                    ChangeTracker.RecordOriginalValue("CampaignActionID", _campaignActionID);
                    _campaignActionID = value;
                    OnPropertyChanged("CampaignActionID");
                }
            }
        }
        private Nullable<int> _campaignActionID;
    
        public bool EnableEventTracking
        {
            get { return _enableEventTracking; }
            set
            {
                if (_enableEventTracking != value)
                {
                    ChangeTracker.RecordOriginalValue("EnableEventTracking", _enableEventTracking);
                    _enableEventTracking = value;
                    OnPropertyChanged("EnableEventTracking");
                }
            }
        }
        private bool _enableEventTracking;
    
        public string ReplyToAddress
        {
            get { return _replyToAddress; }
            set
            {
                if (_replyToAddress != value)
                {
                    ChangeTracker.RecordOriginalValue("ReplyToAddress", _replyToAddress);
                    _replyToAddress = value;
                    OnPropertyChanged("ReplyToAddress");
                }
            }
        }
        private string _replyToAddress;

        #endregion
        #region Navigation Properties
    
            public TrackableCollection<MailMessageAttachment> MailMessageAttachments
        {
            get
            {
                if (_mailMessageAttachments == null)
                {
                    _mailMessageAttachments = new TrackableCollection<MailMessageAttachment>();
                    _mailMessageAttachments.CollectionChanged += FixupMailMessageAttachments;
                }
                return _mailMessageAttachments;
            }
            set
            {
                if (!ReferenceEquals(_mailMessageAttachments, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_mailMessageAttachments != null)
                    {
                        _mailMessageAttachments.CollectionChanged -= FixupMailMessageAttachments;
                    }
                    _mailMessageAttachments = value;
                    if (_mailMessageAttachments != null)
                    {
                        _mailMessageAttachments.CollectionChanged += FixupMailMessageAttachments;
                    }
                    OnNavigationPropertyChanged("MailMessageAttachments");
                }
            }
        }
        private TrackableCollection<MailMessageAttachment> _mailMessageAttachments;
    
            public TrackableCollection<MailMessageGroup> MailMessageGroups
        {
            get
            {
                if (_mailMessageGroups == null)
                {
                    _mailMessageGroups = new TrackableCollection<MailMessageGroup>();
                    _mailMessageGroups.CollectionChanged += FixupMailMessageGroups;
                }
                return _mailMessageGroups;
            }
            set
            {
                if (!ReferenceEquals(_mailMessageGroups, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_mailMessageGroups != null)
                    {
                        _mailMessageGroups.CollectionChanged -= FixupMailMessageGroups;
                    }
                    _mailMessageGroups = value;
                    if (_mailMessageGroups != null)
                    {
                        _mailMessageGroups.CollectionChanged += FixupMailMessageGroups;
                    }
                    OnNavigationPropertyChanged("MailMessageGroups");
                }
            }
        }
        private TrackableCollection<MailMessageGroup> _mailMessageGroups;
    
            public MailFolderType MailFolderType
        {
            get { return _mailFolderType; }
            set
            {
                if (!ReferenceEquals(_mailFolderType, value))
                {
                    var previousValue = _mailFolderType;
                    _mailFolderType = value;
                    FixupMailFolderType(previousValue);
                    OnNavigationPropertyChanged("MailFolderType");
                }
            }
        }
        private MailFolderType _mailFolderType;
    
            public MailMessageType MailMessageType
        {
            get { return _mailMessageType; }
            set
            {
                if (!ReferenceEquals(_mailMessageType, value))
                {
                    var previousValue = _mailMessageType;
                    _mailMessageType = value;
                    FixupMailMessageType(previousValue);
                    OnNavigationPropertyChanged("MailMessageType");
                }
            }
        }
        private MailMessageType _mailMessageType;
    
            public MailMessagePriority MailMessagePriority
        {
            get { return _mailMessagePriority; }
            set
            {
                if (!ReferenceEquals(_mailMessagePriority, value))
                {
                    var previousValue = _mailMessagePriority;
                    _mailMessagePriority = value;
                    FixupMailMessagePriority(previousValue);
                    OnNavigationPropertyChanged("MailMessagePriority");
                }
            }
        }
        private MailMessagePriority _mailMessagePriority;

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
            if (_mailMessageAttachments != null)
    		{
    		    _mailMessageAttachments.CollectionChanged -= FixupMailMessageAttachments;
                _mailMessageAttachments.CollectionChanged += FixupMailMessageAttachments;
            }
            if (_mailMessageGroups != null)
    		{
    		    _mailMessageGroups.CollectionChanged -= FixupMailMessageGroups;
                _mailMessageGroups.CollectionChanged += FixupMailMessageGroups;
            }
        }
    
    
        protected virtual void ClearNavigationProperties()
        {
            MailMessageAttachments.Clear();
            MailMessageGroups.Clear();
            MailFolderType = null;
            MailMessageType = null;
            MailMessagePriority = null;
        }

        #endregion
        #region Association Fixup
    
        private void FixupMailFolderType(MailFolderType previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.MailMessages.Contains(this))
            {
                previousValue.MailMessages.Remove(this);
            }
    
            if (MailFolderType != null)
            {
                if (!MailFolderType.MailMessages.Contains(this))
                {
                    MailFolderType.MailMessages.Add(this);
                }
    
                MailFolderTypeID = MailFolderType.MailFolderTypeID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("MailFolderType")
                    && (ChangeTracker.OriginalValues["MailFolderType"] == MailFolderType))
                {
                    ChangeTracker.OriginalValues.Remove("MailFolderType");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("MailFolderType", previousValue);
                }
                if (MailFolderType != null && !MailFolderType.ChangeTracker.ChangeTrackingEnabled)
                {
                    MailFolderType.StartTracking();
                }
            }
        }
    
        private void FixupMailMessageType(MailMessageType previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.MailMessages.Contains(this))
            {
                previousValue.MailMessages.Remove(this);
            }
    
            if (MailMessageType != null)
            {
                if (!MailMessageType.MailMessages.Contains(this))
                {
                    MailMessageType.MailMessages.Add(this);
                }
    
                MailMessageTypeID = MailMessageType.MailMessageTypeID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("MailMessageType")
                    && (ChangeTracker.OriginalValues["MailMessageType"] == MailMessageType))
                {
                    ChangeTracker.OriginalValues.Remove("MailMessageType");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("MailMessageType", previousValue);
                }
                if (MailMessageType != null && !MailMessageType.ChangeTracker.ChangeTrackingEnabled)
                {
                    MailMessageType.StartTracking();
                }
            }
        }
    
        private void FixupMailMessagePriority(MailMessagePriority previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.MailMessages.Contains(this))
            {
                previousValue.MailMessages.Remove(this);
            }
    
            if (MailMessagePriority != null)
            {
                if (!MailMessagePriority.MailMessages.Contains(this))
                {
                    MailMessagePriority.MailMessages.Add(this);
                }
    
                MailMessagePriorityID = MailMessagePriority.MailMessagePriorityID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("MailMessagePriority")
                    && (ChangeTracker.OriginalValues["MailMessagePriority"] == MailMessagePriority))
                {
                    ChangeTracker.OriginalValues.Remove("MailMessagePriority");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("MailMessagePriority", previousValue);
                }
                if (MailMessagePriority != null && !MailMessagePriority.ChangeTracker.ChangeTrackingEnabled)
                {
                    MailMessagePriority.StartTracking();
                }
            }
        }
    
        private void FixupMailMessageAttachments(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (MailMessageAttachment item in e.NewItems)
                {
                    item.MailMessage = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("MailMessageAttachments", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (MailMessageAttachment item in e.OldItems)
                {
                    if (ReferenceEquals(item.MailMessage, this))
                    {
                        item.MailMessage = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("MailMessageAttachments", item);
                    }
                }
            }
        }
    
        private void FixupMailMessageGroups(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (MailMessageGroup item in e.NewItems)
                {
                    item.MailMessage = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("MailMessageGroups", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (MailMessageGroup item in e.OldItems)
                {
                    if (ReferenceEquals(item.MailMessage, this))
                    {
                        item.MailMessage = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("MailMessageGroups", item);
                    }
                }
            }
        }

        #endregion
    }
}
