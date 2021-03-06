//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;

namespace NetSteps.Data.Entities
{
    [KnownType(typeof(DomainEventType))]
    [KnownType(typeof(EventContext))]
    [KnownType(typeof(QueueItemPriority))]
    [KnownType(typeof(QueueItemStatus))]
    [KnownType(typeof(DeviceNotification))]
    [Serializable]
    public partial class DomainEventQueueItem: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void DomainEventQueueItemIDChanged();
    	public int DomainEventQueueItemID
    	{
    		get { return _domainEventQueueItemID; }
    		set
    		{
    			if (_domainEventQueueItemID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'DomainEventQueueItemID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_domainEventQueueItemID = value;
    				DomainEventQueueItemIDChanged();
    				OnPropertyChanged("DomainEventQueueItemID");
    			}
    		}
    	}
    	private int _domainEventQueueItemID;
    	partial void DomainEventTypeIDChanged();
    	public short DomainEventTypeID
    	{
    		get { return _domainEventTypeID; }
    		set
    		{
    			if (_domainEventTypeID != value)
    			{
    				ChangeTracker.RecordOriginalValue("DomainEventTypeID", _domainEventTypeID);
    				if (!IsDeserializing)
    				{
    					if (DomainEventType != null && DomainEventType.DomainEventTypeID != value)
    					{
    						DomainEventType = null;
    					}
    				}
    				_domainEventTypeID = value;
    				DomainEventTypeIDChanged();
    				OnPropertyChanged("DomainEventTypeID");
    			}
    		}
    	}
    	private short _domainEventTypeID;
    	partial void QueueItemStatusIDChanged();
    	public short QueueItemStatusID
    	{
    		get { return _queueItemStatusID; }
    		set
    		{
    			if (_queueItemStatusID != value)
    			{
    				ChangeTracker.RecordOriginalValue("QueueItemStatusID", _queueItemStatusID);
    				if (!IsDeserializing)
    				{
    					if (QueueItemStatus != null && QueueItemStatus.QueueItemStatusID != value)
    					{
    						QueueItemStatus = null;
    					}
    				}
    				_queueItemStatusID = value;
    				QueueItemStatusIDChanged();
    				OnPropertyChanged("QueueItemStatusID");
    			}
    		}
    	}
    	private short _queueItemStatusID;
    	partial void QueueItemPriorityIDChanged();
    	public short QueueItemPriorityID
    	{
    		get { return _queueItemPriorityID; }
    		set
    		{
    			if (_queueItemPriorityID != value)
    			{
    				ChangeTracker.RecordOriginalValue("QueueItemPriorityID", _queueItemPriorityID);
    				if (!IsDeserializing)
    				{
    					if (QueueItemPriority != null && QueueItemPriority.QueueItemPriorityID != value)
    					{
    						QueueItemPriority = null;
    					}
    				}
    				_queueItemPriorityID = value;
    				QueueItemPriorityIDChanged();
    				OnPropertyChanged("QueueItemPriorityID");
    			}
    		}
    	}
    	private short _queueItemPriorityID;
    	partial void EventContextIDChanged();
    	public int EventContextID
    	{
    		get { return _eventContextID; }
    		set
    		{
    			if (_eventContextID != value)
    			{
    				ChangeTracker.RecordOriginalValue("EventContextID", _eventContextID);
    				if (!IsDeserializing)
    				{
    					if (EventContext != null && EventContext.EventContextID != value)
    					{
    						EventContext = null;
    					}
    				}
    				_eventContextID = value;
    				EventContextIDChanged();
    				OnPropertyChanged("EventContextID");
    			}
    		}
    	}
    	private int _eventContextID;
    	partial void AttemptCountChanged();
    	public byte AttemptCount
    	{
    		get { return _attemptCount; }
    		set
    		{
    			if (_attemptCount != value)
    			{
    				ChangeTracker.RecordOriginalValue("AttemptCount", _attemptCount);
    				_attemptCount = value;
    				AttemptCountChanged();
    				OnPropertyChanged("AttemptCount");
    			}
    		}
    	}
    	private byte _attemptCount;
    	partial void LastRunDateUTCChanged();
    	public Nullable<System.DateTime> LastRunDateUTC
    	{
    		get { return _lastRunDateUTC; }
    		set
    		{
    			if (_lastRunDateUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("LastRunDateUTC", _lastRunDateUTC);
    				_lastRunDateUTC = value;
    				LastRunDateUTCChanged();
    				OnPropertyChanged("LastRunDateUTC");
    			}
    		}
    	}
    	private Nullable<System.DateTime> _lastRunDateUTC;

        #endregion
        #region Navigation Properties
    
    	public DomainEventType DomainEventType
    	{
    		get { return _domainEventType; }
    		set
    		{
    			if (!ReferenceEquals(_domainEventType, value))
    			{
    				var previousValue = _domainEventType;
    				_domainEventType = value;
    				FixupDomainEventType(previousValue);
    				OnNavigationPropertyChanged("DomainEventType");
    			}
    		}
    	}
    	private DomainEventType _domainEventType;
    
    	public EventContext EventContext
    	{
    		get { return _eventContext; }
    		set
    		{
    			if (!ReferenceEquals(_eventContext, value))
    			{
    				var previousValue = _eventContext;
    				_eventContext = value;
    				FixupEventContext(previousValue);
    				OnNavigationPropertyChanged("EventContext");
    			}
    		}
    	}
    	private EventContext _eventContext;
    
    	public QueueItemPriority QueueItemPriority
    	{
    		get { return _queueItemPriority; }
    		set
    		{
    			if (!ReferenceEquals(_queueItemPriority, value))
    			{
    				var previousValue = _queueItemPriority;
    				_queueItemPriority = value;
    				FixupQueueItemPriority(previousValue);
    				OnNavigationPropertyChanged("QueueItemPriority");
    			}
    		}
    	}
    	private QueueItemPriority _queueItemPriority;
    
    	public QueueItemStatus QueueItemStatus
    	{
    		get { return _queueItemStatus; }
    		set
    		{
    			if (!ReferenceEquals(_queueItemStatus, value))
    			{
    				var previousValue = _queueItemStatus;
    				_queueItemStatus = value;
    				FixupQueueItemStatus(previousValue);
    				OnNavigationPropertyChanged("QueueItemStatus");
    			}
    		}
    	}
    	private QueueItemStatus _queueItemStatus;
    
    	public TrackableCollection<DeviceNotification> DeviceNotifications
    	{
    		get
    		{
    			if (_deviceNotifications == null)
    			{
    				_deviceNotifications = new TrackableCollection<DeviceNotification>();
    				_deviceNotifications.CollectionChanged += FixupDeviceNotifications;
    				_deviceNotifications.CollectionChanged += RaiseDeviceNotificationsChanged;
    			}
    			return _deviceNotifications;
    		}
    		set
    		{
    			if (!ReferenceEquals(_deviceNotifications, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_deviceNotifications != null)
    				{
    					_deviceNotifications.CollectionChanged -= FixupDeviceNotifications;
    					_deviceNotifications.CollectionChanged -= RaiseDeviceNotificationsChanged;
    				}
    				_deviceNotifications = value;
    				if (_deviceNotifications != null)
    				{
    					_deviceNotifications.CollectionChanged += FixupDeviceNotifications;
    					_deviceNotifications.CollectionChanged += RaiseDeviceNotificationsChanged;
    				}
    				OnNavigationPropertyChanged("DeviceNotifications");
    			}
    		}
    	}
    	private TrackableCollection<DeviceNotification> _deviceNotifications;
    	partial void DeviceNotificationsChanged();
    	private void RaiseDeviceNotificationsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		DeviceNotificationsChanged();
    	}

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
    		if (_deviceNotifications != null)
    		{
    			_deviceNotifications.CollectionChanged -= FixupDeviceNotifications;
    			_deviceNotifications.CollectionChanged -= RaiseDeviceNotificationsChanged;
    			_deviceNotifications.CollectionChanged += FixupDeviceNotifications;
    			_deviceNotifications.CollectionChanged += RaiseDeviceNotificationsChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		DomainEventType = null;
    		EventContext = null;
    		QueueItemPriority = null;
    		QueueItemStatus = null;
    		DeviceNotifications.Clear();
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupDomainEventType(DomainEventType previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.DomainEventQueueItems.Contains(this))
    		{
    			previousValue.DomainEventQueueItems.Remove(this);
    		}
    
    		if (DomainEventType != null)
    		{
    			if (!DomainEventType.DomainEventQueueItems.Contains(this))
    			{
    				DomainEventType.DomainEventQueueItems.Add(this);
    			}
    
    			DomainEventTypeID = DomainEventType.DomainEventTypeID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("DomainEventType")
    				&& (ChangeTracker.OriginalValues["DomainEventType"] == DomainEventType))
    			{
    				ChangeTracker.OriginalValues.Remove("DomainEventType");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("DomainEventType", previousValue);
    			}
    			if (DomainEventType != null && !DomainEventType.ChangeTracker.ChangeTrackingEnabled)
    			{
    				DomainEventType.StartTracking();
    			}
    		}
    	}
    
    	private void FixupEventContext(EventContext previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.DomainEventQueueItems.Contains(this))
    		{
    			previousValue.DomainEventQueueItems.Remove(this);
    		}
    
    		if (EventContext != null)
    		{
    			if (!EventContext.DomainEventQueueItems.Contains(this))
    			{
    				EventContext.DomainEventQueueItems.Add(this);
    			}
    
    			EventContextID = EventContext.EventContextID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("EventContext")
    				&& (ChangeTracker.OriginalValues["EventContext"] == EventContext))
    			{
    				ChangeTracker.OriginalValues.Remove("EventContext");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("EventContext", previousValue);
    			}
    			if (EventContext != null && !EventContext.ChangeTracker.ChangeTrackingEnabled)
    			{
    				EventContext.StartTracking();
    			}
    		}
    	}
    
    	private void FixupQueueItemPriority(QueueItemPriority previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.DomainEventQueueItems.Contains(this))
    		{
    			previousValue.DomainEventQueueItems.Remove(this);
    		}
    
    		if (QueueItemPriority != null)
    		{
    			if (!QueueItemPriority.DomainEventQueueItems.Contains(this))
    			{
    				QueueItemPriority.DomainEventQueueItems.Add(this);
    			}
    
    			QueueItemPriorityID = QueueItemPriority.QueueItemPriorityID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("QueueItemPriority")
    				&& (ChangeTracker.OriginalValues["QueueItemPriority"] == QueueItemPriority))
    			{
    				ChangeTracker.OriginalValues.Remove("QueueItemPriority");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("QueueItemPriority", previousValue);
    			}
    			if (QueueItemPriority != null && !QueueItemPriority.ChangeTracker.ChangeTrackingEnabled)
    			{
    				QueueItemPriority.StartTracking();
    			}
    		}
    	}
    
    	private void FixupQueueItemStatus(QueueItemStatus previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.DomainEventQueueItems.Contains(this))
    		{
    			previousValue.DomainEventQueueItems.Remove(this);
    		}
    
    		if (QueueItemStatus != null)
    		{
    			if (!QueueItemStatus.DomainEventQueueItems.Contains(this))
    			{
    				QueueItemStatus.DomainEventQueueItems.Add(this);
    			}
    
    			QueueItemStatusID = QueueItemStatus.QueueItemStatusID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("QueueItemStatus")
    				&& (ChangeTracker.OriginalValues["QueueItemStatus"] == QueueItemStatus))
    			{
    				ChangeTracker.OriginalValues.Remove("QueueItemStatus");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("QueueItemStatus", previousValue);
    			}
    			if (QueueItemStatus != null && !QueueItemStatus.ChangeTracker.ChangeTrackingEnabled)
    			{
    				QueueItemStatus.StartTracking();
    			}
    		}
    	}
    
    	private void FixupDeviceNotifications(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (DeviceNotification item in e.NewItems)
    			{
    				item.DomainEventQueueItem = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("DeviceNotifications", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (DeviceNotification item in e.OldItems)
    			{
    				if (ReferenceEquals(item.DomainEventQueueItem, this))
    				{
    					item.DomainEventQueueItem = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("DeviceNotifications", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}
