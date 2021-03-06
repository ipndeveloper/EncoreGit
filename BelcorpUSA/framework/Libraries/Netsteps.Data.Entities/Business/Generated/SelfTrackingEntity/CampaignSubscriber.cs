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
    [KnownType(typeof(Campaign))]
    [Serializable]
    public partial class CampaignSubscriber: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void CampaignSubscriberIDChanged();
    	public int CampaignSubscriberID
    	{
    		get { return _campaignSubscriberID; }
    		set
    		{
    			if (_campaignSubscriberID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'CampaignSubscriberID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_campaignSubscriberID = value;
    				CampaignSubscriberIDChanged();
    				OnPropertyChanged("CampaignSubscriberID");
    			}
    		}
    	}
    	private int _campaignSubscriberID;
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
    	partial void AddedByAccountIDChanged();
    	public int AddedByAccountID
    	{
    		get { return _addedByAccountID; }
    		set
    		{
    			if (_addedByAccountID != value)
    			{
    				ChangeTracker.RecordOriginalValue("AddedByAccountID", _addedByAccountID);
    				if (!IsDeserializing)
    				{
    					if (Account1 != null && Account1.AccountID != value)
    					{
    						Account1 = null;
    					}
    				}
    				_addedByAccountID = value;
    				AddedByAccountIDChanged();
    				OnPropertyChanged("AddedByAccountID");
    			}
    		}
    	}
    	private int _addedByAccountID;
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
    	partial void DateAddedUTCChanged();
    	public System.DateTime DateAddedUTC
    	{
    		get { return _dateAddedUTC; }
    		set
    		{
    			if (_dateAddedUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("DateAddedUTC", _dateAddedUTC);
    				_dateAddedUTC = value;
    				DateAddedUTCChanged();
    				OnPropertyChanged("DateAddedUTC");
    			}
    		}
    	}
    	private System.DateTime _dateAddedUTC;

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
    
    	public Account Account1
    	{
    		get { return _account1; }
    		set
    		{
    			if (!ReferenceEquals(_account1, value))
    			{
    				var previousValue = _account1;
    				_account1 = value;
    				FixupAccount1(previousValue);
    				OnNavigationPropertyChanged("Account1");
    			}
    		}
    	}
    	private Account _account1;
    
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
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		Account = null;
    		Account1 = null;
    		Campaign = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupAccount(Account previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.CampaignSubscribers.Contains(this))
    		{
    			previousValue.CampaignSubscribers.Remove(this);
    		}
    
    		if (Account != null)
    		{
    			if (!Account.CampaignSubscribers.Contains(this))
    			{
    				Account.CampaignSubscribers.Add(this);
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
    
    	private void FixupAccount1(Account previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.CampaignSubscribers1.Contains(this))
    		{
    			previousValue.CampaignSubscribers1.Remove(this);
    		}
    
    		if (Account1 != null)
    		{
    			if (!Account1.CampaignSubscribers1.Contains(this))
    			{
    				Account1.CampaignSubscribers1.Add(this);
    			}
    
    			AddedByAccountID = Account1.AccountID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Account1")
    				&& (ChangeTracker.OriginalValues["Account1"] == Account1))
    			{
    				ChangeTracker.OriginalValues.Remove("Account1");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Account1", previousValue);
    			}
    			if (Account1 != null && !Account1.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Account1.StartTracking();
    			}
    		}
    	}
    
    	private void FixupCampaign(Campaign previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.CampaignSubscribers.Contains(this))
    		{
    			previousValue.CampaignSubscribers.Remove(this);
    		}
    
    		if (Campaign != null)
    		{
    			if (!Campaign.CampaignSubscribers.Contains(this))
    			{
    				Campaign.CampaignSubscribers.Add(this);
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

        #endregion
    }
}
