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
    [KnownType(typeof(Party))]
    [KnownType(typeof(PartyRsvp))]
    [Serializable]
    public partial class PartyGuest: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void PartyGuestIDChanged();
    	public int PartyGuestID
    	{
    		get { return _partyGuestID; }
    		set
    		{
    			if (_partyGuestID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'PartyGuestID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_partyGuestID = value;
    				PartyGuestIDChanged();
    				OnPropertyChanged("PartyGuestID");
    			}
    		}
    	}
    	private int _partyGuestID;
    	partial void PartyIDChanged();
    	public int PartyID
    	{
    		get { return _partyID; }
    		set
    		{
    			if (_partyID != value)
    			{
    				ChangeTracker.RecordOriginalValue("PartyID", _partyID);
    				if (!IsDeserializing)
    				{
    					if (Party != null && Party.PartyID != value)
    					{
    						Party = null;
    					}
    				}
    				_partyID = value;
    				PartyIDChanged();
    				OnPropertyChanged("PartyID");
    			}
    		}
    	}
    	private int _partyID;
    	partial void AccountIDChanged();
    	public Nullable<int> AccountID
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
    	private Nullable<int> _accountID;
    	partial void FirstNameChanged();
    	public string FirstName
    	{
    		get { return _firstName; }
    		set
    		{
    			if (_firstName != value)
    			{
    				ChangeTracker.RecordOriginalValue("FirstName", _firstName);
    				_firstName = value;
    				FirstNameChanged();
    				OnPropertyChanged("FirstName");
    			}
    		}
    	}
    	private string _firstName;
    	partial void LastNameChanged();
    	public string LastName
    	{
    		get { return _lastName; }
    		set
    		{
    			if (_lastName != value)
    			{
    				ChangeTracker.RecordOriginalValue("LastName", _lastName);
    				_lastName = value;
    				LastNameChanged();
    				OnPropertyChanged("LastName");
    			}
    		}
    	}
    	private string _lastName;
    	partial void EmailAddressChanged();
    	public string EmailAddress
    	{
    		get { return _emailAddress; }
    		set
    		{
    			if (_emailAddress != value)
    			{
    				ChangeTracker.RecordOriginalValue("EmailAddress", _emailAddress);
    				_emailAddress = value;
    				EmailAddressChanged();
    				OnPropertyChanged("EmailAddress");
    			}
    		}
    	}
    	private string _emailAddress;
    	partial void ETLNaturalKeyChanged();
    	public string ETLNaturalKey
    	{
    		get { return _eTLNaturalKey; }
    		set
    		{
    			if (_eTLNaturalKey != value)
    			{
    				ChangeTracker.RecordOriginalValue("ETLNaturalKey", _eTLNaturalKey);
    				_eTLNaturalKey = value;
    				ETLNaturalKeyChanged();
    				OnPropertyChanged("ETLNaturalKey");
    			}
    		}
    	}
    	private string _eTLNaturalKey;
    	partial void ETLHashChanged();
    	public string ETLHash
    	{
    		get { return _eTLHash; }
    		set
    		{
    			if (_eTLHash != value)
    			{
    				ChangeTracker.RecordOriginalValue("ETLHash", _eTLHash);
    				_eTLHash = value;
    				ETLHashChanged();
    				OnPropertyChanged("ETLHash");
    			}
    		}
    	}
    	private string _eTLHash;
    	partial void ETLPhaseChanged();
    	public string ETLPhase
    	{
    		get { return _eTLPhase; }
    		set
    		{
    			if (_eTLPhase != value)
    			{
    				ChangeTracker.RecordOriginalValue("ETLPhase", _eTLPhase);
    				_eTLPhase = value;
    				ETLPhaseChanged();
    				OnPropertyChanged("ETLPhase");
    			}
    		}
    	}
    	private string _eTLPhase;
    	partial void ETLDateChanged();
    	public Nullable<System.DateTime> ETLDate
    	{
    		get { return _eTLDate; }
    		set
    		{
    			if (_eTLDate != value)
    			{
    				ChangeTracker.RecordOriginalValue("ETLDate", _eTLDate);
    				_eTLDate = value;
    				ETLDateChanged();
    				OnPropertyChanged("ETLDate");
    			}
    		}
    	}
    	private Nullable<System.DateTime> _eTLDate;

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
    
    	public Party Party
    	{
    		get { return _party; }
    		set
    		{
    			if (!ReferenceEquals(_party, value))
    			{
    				var previousValue = _party;
    				_party = value;
    				FixupParty(previousValue);
    				OnNavigationPropertyChanged("Party");
    			}
    		}
    	}
    	private Party _party;
    
    	public TrackableCollection<PartyRsvp> PartyRsvps
    	{
    		get
    		{
    			if (_partyRsvps == null)
    			{
    				_partyRsvps = new TrackableCollection<PartyRsvp>();
    				_partyRsvps.CollectionChanged += FixupPartyRsvps;
    				_partyRsvps.CollectionChanged += RaisePartyRsvpsChanged;
    			}
    			return _partyRsvps;
    		}
    		set
    		{
    			if (!ReferenceEquals(_partyRsvps, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_partyRsvps != null)
    				{
    					_partyRsvps.CollectionChanged -= FixupPartyRsvps;
    					_partyRsvps.CollectionChanged -= RaisePartyRsvpsChanged;
    				}
    				_partyRsvps = value;
    				if (_partyRsvps != null)
    				{
    					_partyRsvps.CollectionChanged += FixupPartyRsvps;
    					_partyRsvps.CollectionChanged += RaisePartyRsvpsChanged;
    				}
    				OnNavigationPropertyChanged("PartyRsvps");
    			}
    		}
    	}
    	private TrackableCollection<PartyRsvp> _partyRsvps;
    	partial void PartyRsvpsChanged();
    	private void RaisePartyRsvpsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		PartyRsvpsChanged();
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
    		if (_partyRsvps != null)
    		{
    			_partyRsvps.CollectionChanged -= FixupPartyRsvps;
    			_partyRsvps.CollectionChanged -= RaisePartyRsvpsChanged;
    			_partyRsvps.CollectionChanged += FixupPartyRsvps;
    			_partyRsvps.CollectionChanged += RaisePartyRsvpsChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		Account = null;
    		Party = null;
    		PartyRsvps.Clear();
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupAccount(Account previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.PartyGuests.Contains(this))
    		{
    			previousValue.PartyGuests.Remove(this);
    		}
    
    		if (Account != null)
    		{
    			if (!Account.PartyGuests.Contains(this))
    			{
    				Account.PartyGuests.Add(this);
    			}
    
    			AccountID = Account.AccountID;
    		}
    		else if (!skipKeys)
    		{
    			AccountID = null;
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
    
    	private void FixupParty(Party previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.PartyGuests.Contains(this))
    		{
    			previousValue.PartyGuests.Remove(this);
    		}
    
    		if (Party != null)
    		{
    			if (!Party.PartyGuests.Contains(this))
    			{
    				Party.PartyGuests.Add(this);
    			}
    
    			PartyID = Party.PartyID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Party")
    				&& (ChangeTracker.OriginalValues["Party"] == Party))
    			{
    				ChangeTracker.OriginalValues.Remove("Party");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Party", previousValue);
    			}
    			if (Party != null && !Party.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Party.StartTracking();
    			}
    		}
    	}
    
    	private void FixupPartyRsvps(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (PartyRsvp item in e.NewItems)
    			{
    				item.PartyGuest = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("PartyRsvps", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (PartyRsvp item in e.OldItems)
    			{
    				if (ReferenceEquals(item.PartyGuest, this))
    				{
    					item.PartyGuest = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("PartyRsvps", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}