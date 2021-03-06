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
    [KnownType(typeof(CampaignActionQueueItem))]
    [KnownType(typeof(CampaignActionType))]
    [KnownType(typeof(TimeUnitType))]
    [KnownType(typeof(CampaignActionTokenValue))]
    [KnownType(typeof(Campaign))]
    [KnownType(typeof(EmailCampaignAction))]
    [KnownType(typeof(AlertCampaignAction))]
    [Serializable]
    public partial class CampaignAction: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void CampaignActionIDChanged();
    	public int CampaignActionID
    	{
    		get { return _campaignActionID; }
    		set
    		{
    			if (_campaignActionID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'CampaignActionID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_campaignActionID = value;
    				CampaignActionIDChanged();
    				OnPropertyChanged("CampaignActionID");
    			}
    		}
    	}
    	private int _campaignActionID;
    	partial void CampaignActionTypeIDChanged();
    	public short CampaignActionTypeID
    	{
    		get { return _campaignActionTypeID; }
    		set
    		{
    			if (_campaignActionTypeID != value)
    			{
    				ChangeTracker.RecordOriginalValue("CampaignActionTypeID", _campaignActionTypeID);
    				if (!IsDeserializing)
    				{
    					if (CampaignActionType != null && CampaignActionType.CampaignActionTypeID != value)
    					{
    						CampaignActionType = null;
    					}
    				}
    				_campaignActionTypeID = value;
    				CampaignActionTypeIDChanged();
    				OnPropertyChanged("CampaignActionTypeID");
    			}
    		}
    	}
    	private short _campaignActionTypeID;
    	partial void CampaignIDChanged();
    	public int CampaignID
    	{
    		get { return _campaignID; }
    		set
    		{
    			if (_campaignID != value)
    			{
    				ChangeTracker.RecordOriginalValue("CampaignID", _campaignID);
    				if (!IsDeserializing)
    				{
    					if (Campaign != null && Campaign.CampaignID != value)
    					{
    						Campaign = null;
    					}
    				}
    				_campaignID = value;
    				CampaignIDChanged();
    				OnPropertyChanged("CampaignID");
    			}
    		}
    	}
    	private int _campaignID;
    	partial void NameChanged();
    	public string Name
    	{
    		get { return _name; }
    		set
    		{
    			if (_name != value)
    			{
    				ChangeTracker.RecordOriginalValue("Name", _name);
    				_name = value;
    				NameChanged();
    				OnPropertyChanged("Name");
    			}
    		}
    	}
    	private string _name;
    	partial void IntervalTimeUnitTypeIDChanged();
    	public Nullable<short> IntervalTimeUnitTypeID
    	{
    		get { return _intervalTimeUnitTypeID; }
    		set
    		{
    			if (_intervalTimeUnitTypeID != value)
    			{
    				ChangeTracker.RecordOriginalValue("IntervalTimeUnitTypeID", _intervalTimeUnitTypeID);
    				if (!IsDeserializing)
    				{
    					if (TimeUnitType != null && TimeUnitType.TimeUnitTypeID != value)
    					{
    						TimeUnitType = null;
    					}
    				}
    				_intervalTimeUnitTypeID = value;
    				IntervalTimeUnitTypeIDChanged();
    				OnPropertyChanged("IntervalTimeUnitTypeID");
    			}
    		}
    	}
    	private Nullable<short> _intervalTimeUnitTypeID;
    	partial void IntervalChanged();
    	public Nullable<int> Interval
    	{
    		get { return _interval; }
    		set
    		{
    			if (_interval != value)
    			{
    				ChangeTracker.RecordOriginalValue("Interval", _interval);
    				_interval = value;
    				IntervalChanged();
    				OnPropertyChanged("Interval");
    			}
    		}
    	}
    	private Nullable<int> _interval;
    	partial void RunImmediatelyChanged();
    	public bool RunImmediately
    	{
    		get { return _runImmediately; }
    		set
    		{
    			if (_runImmediately != value)
    			{
    				ChangeTracker.RecordOriginalValue("RunImmediately", _runImmediately);
    				_runImmediately = value;
    				RunImmediatelyChanged();
    				OnPropertyChanged("RunImmediately");
    			}
    		}
    	}
    	private bool _runImmediately;
    	partial void NextRunDateUTCChanged();
    	public Nullable<System.DateTime> NextRunDateUTC
    	{
    		get { return _nextRunDateUTC; }
    		set
    		{
    			if (_nextRunDateUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("NextRunDateUTC", _nextRunDateUTC);
    				_nextRunDateUTC = value;
    				NextRunDateUTCChanged();
    				OnPropertyChanged("NextRunDateUTC");
    			}
    		}
    	}
    	private Nullable<System.DateTime> _nextRunDateUTC;
    	partial void SortIndexChanged();
    	public short SortIndex
    	{
    		get { return _sortIndex; }
    		set
    		{
    			if (_sortIndex != value)
    			{
    				ChangeTracker.RecordOriginalValue("SortIndex", _sortIndex);
    				_sortIndex = value;
    				SortIndexChanged();
    				OnPropertyChanged("SortIndex");
    			}
    		}
    	}
    	private short _sortIndex;
    	partial void ActiveChanged();
    	public bool Active
    	{
    		get { return _active; }
    		set
    		{
    			if (_active != value)
    			{
    				ChangeTracker.RecordOriginalValue("Active", _active);
    				_active = value;
    				ActiveChanged();
    				OnPropertyChanged("Active");
    			}
    		}
    	}
    	private bool _active;
    	partial void IsRunningChanged();
    	public bool IsRunning
    	{
    		get { return _isRunning; }
    		set
    		{
    			if (_isRunning != value)
    			{
    				ChangeTracker.RecordOriginalValue("IsRunning", _isRunning);
    				_isRunning = value;
    				IsRunningChanged();
    				OnPropertyChanged("IsRunning");
    			}
    		}
    	}
    	private bool _isRunning;
    	partial void IsCompletedChanged();
    	public bool IsCompleted
    	{
    		get { return _isCompleted; }
    		set
    		{
    			if (_isCompleted != value)
    			{
    				ChangeTracker.RecordOriginalValue("IsCompleted", _isCompleted);
    				_isCompleted = value;
    				IsCompletedChanged();
    				OnPropertyChanged("IsCompleted");
    			}
    		}
    	}
    	private bool _isCompleted;
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
    
    	public TrackableCollection<CampaignActionQueueItem> CampaignActionQueueItems
    	{
    		get
    		{
    			if (_campaignActionQueueItems == null)
    			{
    				_campaignActionQueueItems = new TrackableCollection<CampaignActionQueueItem>();
    				_campaignActionQueueItems.CollectionChanged += FixupCampaignActionQueueItems;
    				_campaignActionQueueItems.CollectionChanged += RaiseCampaignActionQueueItemsChanged;
    			}
    			return _campaignActionQueueItems;
    		}
    		set
    		{
    			if (!ReferenceEquals(_campaignActionQueueItems, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_campaignActionQueueItems != null)
    				{
    					_campaignActionQueueItems.CollectionChanged -= FixupCampaignActionQueueItems;
    					_campaignActionQueueItems.CollectionChanged -= RaiseCampaignActionQueueItemsChanged;
    				}
    				_campaignActionQueueItems = value;
    				if (_campaignActionQueueItems != null)
    				{
    					_campaignActionQueueItems.CollectionChanged += FixupCampaignActionQueueItems;
    					_campaignActionQueueItems.CollectionChanged += RaiseCampaignActionQueueItemsChanged;
    				}
    				OnNavigationPropertyChanged("CampaignActionQueueItems");
    			}
    		}
    	}
    	private TrackableCollection<CampaignActionQueueItem> _campaignActionQueueItems;
    	partial void CampaignActionQueueItemsChanged();
    	private void RaiseCampaignActionQueueItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		CampaignActionQueueItemsChanged();
    	}
    
    	public CampaignActionType CampaignActionType
    	{
    		get { return _campaignActionType; }
    		set
    		{
    			if (!ReferenceEquals(_campaignActionType, value))
    			{
    				var previousValue = _campaignActionType;
    				_campaignActionType = value;
    				FixupCampaignActionType(previousValue);
    				OnNavigationPropertyChanged("CampaignActionType");
    			}
    		}
    	}
    	private CampaignActionType _campaignActionType;
    
    	public TimeUnitType TimeUnitType
    	{
    		get { return _timeUnitType; }
    		set
    		{
    			if (!ReferenceEquals(_timeUnitType, value))
    			{
    				var previousValue = _timeUnitType;
    				_timeUnitType = value;
    				FixupTimeUnitType(previousValue);
    				OnNavigationPropertyChanged("TimeUnitType");
    			}
    		}
    	}
    	private TimeUnitType _timeUnitType;
    
    	public TrackableCollection<CampaignActionTokenValue> CampaignActionTokenValues
    	{
    		get
    		{
    			if (_campaignActionTokenValues == null)
    			{
    				_campaignActionTokenValues = new TrackableCollection<CampaignActionTokenValue>();
    				_campaignActionTokenValues.CollectionChanged += FixupCampaignActionTokenValues;
    				_campaignActionTokenValues.CollectionChanged += RaiseCampaignActionTokenValuesChanged;
    			}
    			return _campaignActionTokenValues;
    		}
    		set
    		{
    			if (!ReferenceEquals(_campaignActionTokenValues, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_campaignActionTokenValues != null)
    				{
    					_campaignActionTokenValues.CollectionChanged -= FixupCampaignActionTokenValues;
    					_campaignActionTokenValues.CollectionChanged -= RaiseCampaignActionTokenValuesChanged;
    				}
    				_campaignActionTokenValues = value;
    				if (_campaignActionTokenValues != null)
    				{
    					_campaignActionTokenValues.CollectionChanged += FixupCampaignActionTokenValues;
    					_campaignActionTokenValues.CollectionChanged += RaiseCampaignActionTokenValuesChanged;
    				}
    				OnNavigationPropertyChanged("CampaignActionTokenValues");
    			}
    		}
    	}
    	private TrackableCollection<CampaignActionTokenValue> _campaignActionTokenValues;
    	partial void CampaignActionTokenValuesChanged();
    	private void RaiseCampaignActionTokenValuesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		CampaignActionTokenValuesChanged();
    	}
    
    	public Campaign Campaign
    	{
    		get { return _campaign; }
    		set
    		{
    			if (!ReferenceEquals(_campaign, value))
    			{
    				var previousValue = _campaign;
    				_campaign = value;
    				FixupCampaign(previousValue);
    				OnNavigationPropertyChanged("Campaign");
    			}
    		}
    	}
    	private Campaign _campaign;
    
    	public TrackableCollection<EmailCampaignAction> EmailCampaignActions
    	{
    		get
    		{
    			if (_emailCampaignActions == null)
    			{
    				_emailCampaignActions = new TrackableCollection<EmailCampaignAction>();
    				_emailCampaignActions.CollectionChanged += FixupEmailCampaignActions;
    				_emailCampaignActions.CollectionChanged += RaiseEmailCampaignActionsChanged;
    			}
    			return _emailCampaignActions;
    		}
    		set
    		{
    			if (!ReferenceEquals(_emailCampaignActions, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_emailCampaignActions != null)
    				{
    					_emailCampaignActions.CollectionChanged -= FixupEmailCampaignActions;
    					_emailCampaignActions.CollectionChanged -= RaiseEmailCampaignActionsChanged;
    				}
    				_emailCampaignActions = value;
    				if (_emailCampaignActions != null)
    				{
    					_emailCampaignActions.CollectionChanged += FixupEmailCampaignActions;
    					_emailCampaignActions.CollectionChanged += RaiseEmailCampaignActionsChanged;
    				}
    				OnNavigationPropertyChanged("EmailCampaignActions");
    			}
    		}
    	}
    	private TrackableCollection<EmailCampaignAction> _emailCampaignActions;
    	partial void EmailCampaignActionsChanged();
    	private void RaiseEmailCampaignActionsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		EmailCampaignActionsChanged();
    	}
    
    	public TrackableCollection<AlertCampaignAction> AlertCampaignActions
    	{
    		get
    		{
    			if (_alertCampaignActions == null)
    			{
    				_alertCampaignActions = new TrackableCollection<AlertCampaignAction>();
    				_alertCampaignActions.CollectionChanged += FixupAlertCampaignActions;
    				_alertCampaignActions.CollectionChanged += RaiseAlertCampaignActionsChanged;
    			}
    			return _alertCampaignActions;
    		}
    		set
    		{
    			if (!ReferenceEquals(_alertCampaignActions, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_alertCampaignActions != null)
    				{
    					_alertCampaignActions.CollectionChanged -= FixupAlertCampaignActions;
    					_alertCampaignActions.CollectionChanged -= RaiseAlertCampaignActionsChanged;
    				}
    				_alertCampaignActions = value;
    				if (_alertCampaignActions != null)
    				{
    					_alertCampaignActions.CollectionChanged += FixupAlertCampaignActions;
    					_alertCampaignActions.CollectionChanged += RaiseAlertCampaignActionsChanged;
    				}
    				OnNavigationPropertyChanged("AlertCampaignActions");
    			}
    		}
    	}
    	private TrackableCollection<AlertCampaignAction> _alertCampaignActions;
    	partial void AlertCampaignActionsChanged();
    	private void RaiseAlertCampaignActionsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		AlertCampaignActionsChanged();
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
    		if (_campaignActionQueueItems != null)
    		{
    			_campaignActionQueueItems.CollectionChanged -= FixupCampaignActionQueueItems;
    			_campaignActionQueueItems.CollectionChanged -= RaiseCampaignActionQueueItemsChanged;
    			_campaignActionQueueItems.CollectionChanged += FixupCampaignActionQueueItems;
    			_campaignActionQueueItems.CollectionChanged += RaiseCampaignActionQueueItemsChanged;
    		}
    		if (_campaignActionTokenValues != null)
    		{
    			_campaignActionTokenValues.CollectionChanged -= FixupCampaignActionTokenValues;
    			_campaignActionTokenValues.CollectionChanged -= RaiseCampaignActionTokenValuesChanged;
    			_campaignActionTokenValues.CollectionChanged += FixupCampaignActionTokenValues;
    			_campaignActionTokenValues.CollectionChanged += RaiseCampaignActionTokenValuesChanged;
    		}
    		if (_emailCampaignActions != null)
    		{
    			_emailCampaignActions.CollectionChanged -= FixupEmailCampaignActions;
    			_emailCampaignActions.CollectionChanged -= RaiseEmailCampaignActionsChanged;
    			_emailCampaignActions.CollectionChanged += FixupEmailCampaignActions;
    			_emailCampaignActions.CollectionChanged += RaiseEmailCampaignActionsChanged;
    		}
    		if (_alertCampaignActions != null)
    		{
    			_alertCampaignActions.CollectionChanged -= FixupAlertCampaignActions;
    			_alertCampaignActions.CollectionChanged -= RaiseAlertCampaignActionsChanged;
    			_alertCampaignActions.CollectionChanged += FixupAlertCampaignActions;
    			_alertCampaignActions.CollectionChanged += RaiseAlertCampaignActionsChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		CampaignActionQueueItems.Clear();
    		CampaignActionType = null;
    		TimeUnitType = null;
    		CampaignActionTokenValues.Clear();
    		Campaign = null;
    		EmailCampaignActions.Clear();
    		AlertCampaignActions.Clear();
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupCampaignActionType(CampaignActionType previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.CampaignActions.Contains(this))
    		{
    			previousValue.CampaignActions.Remove(this);
    		}
    
    		if (CampaignActionType != null)
    		{
    			if (!CampaignActionType.CampaignActions.Contains(this))
    			{
    				CampaignActionType.CampaignActions.Add(this);
    			}
    
    			CampaignActionTypeID = CampaignActionType.CampaignActionTypeID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("CampaignActionType")
    				&& (ChangeTracker.OriginalValues["CampaignActionType"] == CampaignActionType))
    			{
    				ChangeTracker.OriginalValues.Remove("CampaignActionType");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("CampaignActionType", previousValue);
    			}
    			if (CampaignActionType != null && !CampaignActionType.ChangeTracker.ChangeTrackingEnabled)
    			{
    				CampaignActionType.StartTracking();
    			}
    		}
    	}
    
    	private void FixupTimeUnitType(TimeUnitType previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.CampaignActions.Contains(this))
    		{
    			previousValue.CampaignActions.Remove(this);
    		}
    
    		if (TimeUnitType != null)
    		{
    			if (!TimeUnitType.CampaignActions.Contains(this))
    			{
    				TimeUnitType.CampaignActions.Add(this);
    			}
    
    			IntervalTimeUnitTypeID = TimeUnitType.TimeUnitTypeID;
    		}
    		else if (!skipKeys)
    		{
    			IntervalTimeUnitTypeID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("TimeUnitType")
    				&& (ChangeTracker.OriginalValues["TimeUnitType"] == TimeUnitType))
    			{
    				ChangeTracker.OriginalValues.Remove("TimeUnitType");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("TimeUnitType", previousValue);
    			}
    			if (TimeUnitType != null && !TimeUnitType.ChangeTracker.ChangeTrackingEnabled)
    			{
    				TimeUnitType.StartTracking();
    			}
    		}
    	}
    
    	private void FixupCampaign(Campaign previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.CampaignActions.Contains(this))
    		{
    			previousValue.CampaignActions.Remove(this);
    		}
    
    		if (Campaign != null)
    		{
    			if (!Campaign.CampaignActions.Contains(this))
    			{
    				Campaign.CampaignActions.Add(this);
    			}
    
    			CampaignID = Campaign.CampaignID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Campaign")
    				&& (ChangeTracker.OriginalValues["Campaign"] == Campaign))
    			{
    				ChangeTracker.OriginalValues.Remove("Campaign");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Campaign", previousValue);
    			}
    			if (Campaign != null && !Campaign.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Campaign.StartTracking();
    			}
    		}
    	}
    
    	private void FixupCampaignActionQueueItems(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (CampaignActionQueueItem item in e.NewItems)
    			{
    				item.CampaignAction = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("CampaignActionQueueItems", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (CampaignActionQueueItem item in e.OldItems)
    			{
    				if (ReferenceEquals(item.CampaignAction, this))
    				{
    					item.CampaignAction = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("CampaignActionQueueItems", item);
    				}
    			}
    		}
    	}
    
    	private void FixupCampaignActionTokenValues(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (CampaignActionTokenValue item in e.NewItems)
    			{
    				item.CampaignAction = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("CampaignActionTokenValues", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (CampaignActionTokenValue item in e.OldItems)
    			{
    				if (ReferenceEquals(item.CampaignAction, this))
    				{
    					item.CampaignAction = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("CampaignActionTokenValues", item);
    				}
    			}
    		}
    	}
    
    	private void FixupEmailCampaignActions(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (EmailCampaignAction item in e.NewItems)
    			{
    				item.CampaignAction = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("EmailCampaignActions", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (EmailCampaignAction item in e.OldItems)
    			{
    				if (ReferenceEquals(item.CampaignAction, this))
    				{
    					item.CampaignAction = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("EmailCampaignActions", item);
    				}
    			}
    		}
    	}
    
    	private void FixupAlertCampaignActions(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (AlertCampaignAction item in e.NewItems)
    			{
    				item.CampaignAction = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("AlertCampaignActions", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (AlertCampaignAction item in e.OldItems)
    			{
    				if (ReferenceEquals(item.CampaignAction, this))
    				{
    					item.CampaignAction = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("AlertCampaignActions", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}
