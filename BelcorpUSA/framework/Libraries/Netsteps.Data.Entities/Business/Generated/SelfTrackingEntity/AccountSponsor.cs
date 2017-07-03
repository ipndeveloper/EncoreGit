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
    [KnownType(typeof(AccountSponsorType))]
    [KnownType(typeof(User))]
    [Serializable]
    public partial class AccountSponsor: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void AccountIDChanged();
    	public int AccountID
    	{
    		get { return _accountID; }
    		set
    		{
    			if (_accountID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'AccountID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
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
    	partial void SponsorIDChanged();
    	public Nullable<int> SponsorID
    	{
    		get { return _sponsorID; }
    		set
    		{
    			if (_sponsorID != value)
    			{
    				ChangeTracker.RecordOriginalValue("SponsorID", _sponsorID);
    				if (!IsDeserializing)
    				{
    					if (Account1 != null && Account1.AccountID != value)
    					{
    						Account1 = null;
    					}
    				}
    				_sponsorID = value;
    				SponsorIDChanged();
    				OnPropertyChanged("SponsorID");
    			}
    		}
    	}
    	private Nullable<int> _sponsorID;
    	partial void AccountSponsorTypeIDChanged();
    	public int AccountSponsorTypeID
    	{
    		get { return _accountSponsorTypeID; }
    		set
    		{
    			if (_accountSponsorTypeID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'AccountSponsorTypeID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				if (!IsDeserializing)
    				{
    					if (AccountSponsorType != null && AccountSponsorType.AccountSponsorTypeID != value)
    					{
    						AccountSponsorType = null;
    					}
    				}
    				_accountSponsorTypeID = value;
    				AccountSponsorTypeIDChanged();
    				OnPropertyChanged("AccountSponsorTypeID");
    			}
    		}
    	}
    	private int _accountSponsorTypeID;
    	partial void PositionChanged();
    	public int Position
    	{
    		get { return _position; }
    		set
    		{
    			if (_position != value)
    			{
    				ChangeTracker.RecordOriginalValue("Position", _position);
    				_position = value;
    				PositionChanged();
    				OnPropertyChanged("Position");
    			}
    		}
    	}
    	private int _position;
    	partial void EffectiveDateUTCChanged();
    	public System.DateTime EffectiveDateUTC
    	{
    		get { return _effectiveDateUTC; }
    		set
    		{
    			if (_effectiveDateUTC != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'EffectiveDateUTC' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_effectiveDateUTC = value;
    				EffectiveDateUTCChanged();
    				OnPropertyChanged("EffectiveDateUTC");
    			}
    		}
    	}
    	private System.DateTime _effectiveDateUTC;
    	partial void ModifiedByUserIDChanged();
    	public Nullable<int> ModifiedByUserID
    	{
    		get { return _modifiedByUserID; }
    		set
    		{
    			if (_modifiedByUserID != value)
    			{
    				ChangeTracker.RecordOriginalValue("ModifiedByUserID", _modifiedByUserID);
    				if (!IsDeserializing)
    				{
    					if (User != null && User.UserID != value)
    					{
    						User = null;
    					}
    				}
    				_modifiedByUserID = value;
    				ModifiedByUserIDChanged();
    				OnPropertyChanged("ModifiedByUserID");
    			}
    		}
    	}
    	private Nullable<int> _modifiedByUserID;

        #endregion
        #region Navigation Properties
    
    	public Account Account
    	{
    		get { return _account; }
    		set
    		{
    			if (!ReferenceEquals(_account, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added && value != null)
    				{
    					// This the dependent end of an identifying relationship, so the principal end cannot be changed if it is already set,
    					// otherwise it can only be set to an entity with a primary key that is the same value as the dependent's foreign key.
    					if (AccountID != value.AccountID)
    					{
    						throw new InvalidOperationException("The principal end of an identifying relationship can only be changed when the dependent end is in the Added state.");
    					}
    				}
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
    
    	public AccountSponsorType AccountSponsorType
    	{
    		get { return _accountSponsorType; }
    		set
    		{
    			if (!ReferenceEquals(_accountSponsorType, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added && value != null)
    				{
    					// This the dependent end of an identifying relationship, so the principal end cannot be changed if it is already set,
    					// otherwise it can only be set to an entity with a primary key that is the same value as the dependent's foreign key.
    					if (AccountSponsorTypeID != value.AccountSponsorTypeID)
    					{
    						throw new InvalidOperationException("The principal end of an identifying relationship can only be changed when the dependent end is in the Added state.");
    					}
    				}
    				var previousValue = _accountSponsorType;
    				_accountSponsorType = value;
    				FixupAccountSponsorType(previousValue);
    				OnNavigationPropertyChanged("AccountSponsorType");
    			}
    		}
    	}
    	private AccountSponsorType _accountSponsorType;
    
    	public User User
    	{
    		get { return _user; }
    		set
    		{
    			if (!ReferenceEquals(_user, value))
    			{
    				var previousValue = _user;
    				_user = value;
    				FixupUser(previousValue);
    				OnNavigationPropertyChanged("User");
    			}
    		}
    	}
    	private User _user;

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
    
    	// This entity type is the dependent end in at least one association that performs cascade deletes.
    	// This event handler will process notifications that occur when the principal end is deleted.
    	internal void HandleCascadeDelete(object sender, ObjectStateChangingEventArgs e)
    	{
    		if (e.NewState == ObjectState.Deleted)
    		{
    			this.MarkAsDeleted();
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
    		AccountSponsorType = null;
    		User = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupAccount(Account previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.AccountSponsors.Contains(this))
    		{
    			previousValue.AccountSponsors.Remove(this);
    		}
    
    		if (Account != null)
    		{
    			if (!Account.AccountSponsors.Contains(this))
    			{
    				Account.AccountSponsors.Add(this);
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
    
    	private void FixupAccount1(Account previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.AccountSponsors1.Contains(this))
    		{
    			previousValue.AccountSponsors1.Remove(this);
    		}
    
    		if (Account1 != null)
    		{
    			if (!Account1.AccountSponsors1.Contains(this))
    			{
    				Account1.AccountSponsors1.Add(this);
    			}
    
    			SponsorID = Account1.AccountID;
    		}
    		else if (!skipKeys)
    		{
    			SponsorID = null;
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
    
    	private void FixupAccountSponsorType(AccountSponsorType previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.AccountSponsors.Contains(this))
    		{
    			previousValue.AccountSponsors.Remove(this);
    		}
    
    		if (AccountSponsorType != null)
    		{
    			if (!AccountSponsorType.AccountSponsors.Contains(this))
    			{
    				AccountSponsorType.AccountSponsors.Add(this);
    			}
    
    			AccountSponsorTypeID = AccountSponsorType.AccountSponsorTypeID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("AccountSponsorType")
    				&& (ChangeTracker.OriginalValues["AccountSponsorType"] == AccountSponsorType))
    			{
    				ChangeTracker.OriginalValues.Remove("AccountSponsorType");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("AccountSponsorType", previousValue);
    			}
    			if (AccountSponsorType != null && !AccountSponsorType.ChangeTracker.ChangeTrackingEnabled)
    			{
    				AccountSponsorType.StartTracking();
    			}
    		}
    	}
    
    	private void FixupUser(User previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.AccountSponsors.Contains(this))
    		{
    			previousValue.AccountSponsors.Remove(this);
    		}
    
    		if (User != null)
    		{
    			if (!User.AccountSponsors.Contains(this))
    			{
    				User.AccountSponsors.Add(this);
    			}
    
    			ModifiedByUserID = User.UserID;
    		}
    		else if (!skipKeys)
    		{
    			ModifiedByUserID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("User")
    				&& (ChangeTracker.OriginalValues["User"] == User))
    			{
    				ChangeTracker.OriginalValues.Remove("User");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("User", previousValue);
    			}
    			if (User != null && !User.ChangeTracker.ChangeTrackingEnabled)
    			{
    				User.StartTracking();
    			}
    		}
    	}

        #endregion
    }
}