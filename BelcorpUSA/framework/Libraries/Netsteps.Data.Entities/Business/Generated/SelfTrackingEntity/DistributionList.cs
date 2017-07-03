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
    [KnownType(typeof(Account))]
    [KnownType(typeof(DistributionListType))]
    [KnownType(typeof(DistributionSubscriber))]
    [Serializable]
    public partial class DistributionList: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void DistributionListIDChanged();
    	public int DistributionListID
    	{
    		get { return _distributionListID; }
    		set
    		{
    			if (_distributionListID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'DistributionListID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_distributionListID = value;
    				DistributionListIDChanged();
    				OnPropertyChanged("DistributionListID");
    			}
    		}
    	}
    	private int _distributionListID;
    	partial void DistributionListTypeIDChanged();
    	public short DistributionListTypeID
    	{
    		get { return _distributionListTypeID; }
    		set
    		{
    			if (_distributionListTypeID != value)
    			{
    				ChangeTracker.RecordOriginalValue("DistributionListTypeID", _distributionListTypeID);
    				if (!IsDeserializing)
    				{
    					if (DistributionListType != null && DistributionListType.DistributionListTypeID != value)
    					{
    						DistributionListType = null;
    					}
    				}
    				_distributionListTypeID = value;
    				DistributionListTypeIDChanged();
    				OnPropertyChanged("DistributionListTypeID");
    			}
    		}
    	}
    	private short _distributionListTypeID;
    	partial void AccountIDChanged();
    	public int AccountID
    	{
    		get { return _accountID; }
    		set
    		{
    			if (_accountID != value)
    			{
    				ChangeTracker.RecordOriginalValue("AccountID", _accountID);
    				if (!IsDeserializing)
    				{
    					if (Account != null && Account.AccountID != value)
    					{
    						Account = null;
    					}
    				}
    				_accountID = value;
    				AccountIDChanged();
    				OnPropertyChanged("AccountID");
    			}
    		}
    	}
    	private int _accountID;
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

        #endregion
        #region Navigation Properties
    
    	public Account Account
    	{
    		get { return _account; }
    		set
    		{
    			if (!ReferenceEquals(_account, value))
    			{
    				var previousValue = _account;
    				_account = value;
    				FixupAccount(previousValue);
    				OnNavigationPropertyChanged("Account");
    			}
    		}
    	}
    	private Account _account;
    
    	public DistributionListType DistributionListType
    	{
    		get { return _distributionListType; }
    		set
    		{
    			if (!ReferenceEquals(_distributionListType, value))
    			{
    				var previousValue = _distributionListType;
    				_distributionListType = value;
    				FixupDistributionListType(previousValue);
    				OnNavigationPropertyChanged("DistributionListType");
    			}
    		}
    	}
    	private DistributionListType _distributionListType;
    
    	public TrackableCollection<DistributionSubscriber> DistributionSubscribers
    	{
    		get
    		{
    			if (_distributionSubscribers == null)
    			{
    				_distributionSubscribers = new TrackableCollection<DistributionSubscriber>();
    				_distributionSubscribers.CollectionChanged += FixupDistributionSubscribers;
    				_distributionSubscribers.CollectionChanged += RaiseDistributionSubscribersChanged;
    			}
    			return _distributionSubscribers;
    		}
    		set
    		{
    			if (!ReferenceEquals(_distributionSubscribers, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_distributionSubscribers != null)
    				{
    					_distributionSubscribers.CollectionChanged -= FixupDistributionSubscribers;
    					_distributionSubscribers.CollectionChanged -= RaiseDistributionSubscribersChanged;
    				}
    				_distributionSubscribers = value;
    				if (_distributionSubscribers != null)
    				{
    					_distributionSubscribers.CollectionChanged += FixupDistributionSubscribers;
    					_distributionSubscribers.CollectionChanged += RaiseDistributionSubscribersChanged;
    				}
    				OnNavigationPropertyChanged("DistributionSubscribers");
    			}
    		}
    	}
    	private TrackableCollection<DistributionSubscriber> _distributionSubscribers;
    	partial void DistributionSubscribersChanged();
    	private void RaiseDistributionSubscribersChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		DistributionSubscribersChanged();
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
    		if (_distributionSubscribers != null)
    		{
    			_distributionSubscribers.CollectionChanged -= FixupDistributionSubscribers;
    			_distributionSubscribers.CollectionChanged -= RaiseDistributionSubscribersChanged;
    			_distributionSubscribers.CollectionChanged += FixupDistributionSubscribers;
    			_distributionSubscribers.CollectionChanged += RaiseDistributionSubscribersChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		Account = null;
    		DistributionListType = null;
    		DistributionSubscribers.Clear();
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupAccount(Account previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.DistributionLists.Contains(this))
    		{
    			previousValue.DistributionLists.Remove(this);
    		}
    
    		if (Account != null)
    		{
    			if (!Account.DistributionLists.Contains(this))
    			{
    				Account.DistributionLists.Add(this);
    			}
    
    			AccountID = Account.AccountID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Account")
    				&& (ChangeTracker.OriginalValues["Account"] == Account))
    			{
    				ChangeTracker.OriginalValues.Remove("Account");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Account", previousValue);
    			}
    			if (Account != null && !Account.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Account.StartTracking();
    			}
    		}
    	}
    
    	private void FixupDistributionListType(DistributionListType previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.DistributionLists.Contains(this))
    		{
    			previousValue.DistributionLists.Remove(this);
    		}
    
    		if (DistributionListType != null)
    		{
    			if (!DistributionListType.DistributionLists.Contains(this))
    			{
    				DistributionListType.DistributionLists.Add(this);
    			}
    
    			DistributionListTypeID = DistributionListType.DistributionListTypeID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("DistributionListType")
    				&& (ChangeTracker.OriginalValues["DistributionListType"] == DistributionListType))
    			{
    				ChangeTracker.OriginalValues.Remove("DistributionListType");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("DistributionListType", previousValue);
    			}
    			if (DistributionListType != null && !DistributionListType.ChangeTracker.ChangeTrackingEnabled)
    			{
    				DistributionListType.StartTracking();
    			}
    		}
    	}
    
    	private void FixupDistributionSubscribers(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (DistributionSubscriber item in e.NewItems)
    			{
    				item.DistributionList = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("DistributionSubscribers", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (DistributionSubscriber item in e.OldItems)
    			{
    				if (ReferenceEquals(item.DistributionList, this))
    				{
    					item.DistributionList = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("DistributionSubscribers", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}