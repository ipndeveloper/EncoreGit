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
    [KnownType(typeof(Campaign))]
    [Serializable]
    public partial class CampaignEmail: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void CampaignEmailIDChanged();
    	public int CampaignEmailID
    	{
    		get { return _campaignEmailID; }
    		set
    		{
    			if (_campaignEmailID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'CampaignEmailID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_campaignEmailID = value;
    				CampaignEmailIDChanged();
    				OnPropertyChanged("CampaignEmailID");
    			}
    		}
    	}
    	private int _campaignEmailID;
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
    	partial void EmailNameChanged();
    	public string EmailName
    	{
    		get { return _emailName; }
    		set
    		{
    			if (_emailName != value)
    			{
    				ChangeTracker.RecordOriginalValue("EmailName", _emailName);
    				_emailName = value;
    				EmailNameChanged();
    				OnPropertyChanged("EmailName");
    			}
    		}
    	}
    	private string _emailName;
    	partial void DateToBeSentUTCChanged();
    	public Nullable<System.DateTime> DateToBeSentUTC
    	{
    		get { return _dateToBeSentUTC; }
    		set
    		{
    			if (_dateToBeSentUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("DateToBeSentUTC", _dateToBeSentUTC);
    				_dateToBeSentUTC = value;
    				DateToBeSentUTCChanged();
    				OnPropertyChanged("DateToBeSentUTC");
    			}
    		}
    	}
    	private Nullable<System.DateTime> _dateToBeSentUTC;
    	partial void WaitTimeInDaysChanged();
    	public Nullable<short> WaitTimeInDays
    	{
    		get { return _waitTimeInDays; }
    		set
    		{
    			if (_waitTimeInDays != value)
    			{
    				ChangeTracker.RecordOriginalValue("WaitTimeInDays", _waitTimeInDays);
    				_waitTimeInDays = value;
    				WaitTimeInDaysChanged();
    				OnPropertyChanged("WaitTimeInDays");
    			}
    		}
    	}
    	private Nullable<short> _waitTimeInDays;
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
    	partial void SentCountChanged();
    	public Nullable<int> SentCount
    	{
    		get { return _sentCount; }
    		set
    		{
    			if (_sentCount != value)
    			{
    				ChangeTracker.RecordOriginalValue("SentCount", _sentCount);
    				_sentCount = value;
    				SentCountChanged();
    				OnPropertyChanged("SentCount");
    			}
    		}
    	}
    	private Nullable<int> _sentCount;

        #endregion
        #region Navigation Properties
    
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
    		Campaign = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupCampaign(Campaign previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.CampaignEmails.Contains(this))
    		{
    			previousValue.CampaignEmails.Remove(this);
    		}
    
    		if (Campaign != null)
    		{
    			if (!Campaign.CampaignEmails.Contains(this))
    			{
    				Campaign.CampaignEmails.Add(this);
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
