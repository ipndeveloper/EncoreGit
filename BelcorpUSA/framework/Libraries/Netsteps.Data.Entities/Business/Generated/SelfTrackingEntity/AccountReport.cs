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
    [KnownType(typeof(AccountReportType))]
    [KnownType(typeof(Account))]
    [KnownType(typeof(User))]
    [Serializable]
    public partial class AccountReport: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void AccountReportIDChanged();
    	public int AccountReportID
    	{
    		get { return _accountReportID; }
    		set
    		{
    			if (_accountReportID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'AccountReportID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_accountReportID = value;
    				AccountReportIDChanged();
    				OnPropertyChanged("AccountReportID");
    			}
    		}
    	}
    	private int _accountReportID;
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
    	partial void AccountReportTypeIDChanged();
    	public short AccountReportTypeID
    	{
    		get { return _accountReportTypeID; }
    		set
    		{
    			if (_accountReportTypeID != value)
    			{
    				ChangeTracker.RecordOriginalValue("AccountReportTypeID", _accountReportTypeID);
    				if (!IsDeserializing)
    				{
    					if (AccountReportType != null && AccountReportType.AccountReportTypeID != value)
    					{
    						AccountReportType = null;
    					}
    				}
    				_accountReportTypeID = value;
    				AccountReportTypeIDChanged();
    				OnPropertyChanged("AccountReportTypeID");
    			}
    		}
    	}
    	private short _accountReportTypeID;
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
    	partial void DataChanged();
    	public byte[] Data
    	{
    		get { return _data; }
    		set
    		{
    			if (_data != value)
    			{
    				ChangeTracker.RecordOriginalValue("Data", _data);
    				_data = value;
    				DataChanged();
    				OnPropertyChanged("Data");
    			}
    		}
    	}
    	private byte[] _data;
    	partial void DateCreatedUTCChanged();
    	public System.DateTime DateCreatedUTC
    	{
    		get { return _dateCreatedUTC; }
    		set
    		{
    			if (_dateCreatedUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("DateCreatedUTC", _dateCreatedUTC);
    				_dateCreatedUTC = value;
    				DateCreatedUTCChanged();
    				OnPropertyChanged("DateCreatedUTC");
    			}
    		}
    	}
    	private System.DateTime _dateCreatedUTC;
    	partial void CreatedByUserIDChanged();
    	public Nullable<int> CreatedByUserID
    	{
    		get { return _createdByUserID; }
    		set
    		{
    			if (_createdByUserID != value)
    			{
    				ChangeTracker.RecordOriginalValue("CreatedByUserID", _createdByUserID);
    				if (!IsDeserializing)
    				{
    					if (User1 != null && User1.UserID != value)
    					{
    						User1 = null;
    					}
    				}
    				_createdByUserID = value;
    				CreatedByUserIDChanged();
    				OnPropertyChanged("CreatedByUserID");
    			}
    		}
    	}
    	private Nullable<int> _createdByUserID;
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
    	partial void IsCorporateChanged();
    	public bool IsCorporate
    	{
    		get { return _isCorporate; }
    		set
    		{
    			if (_isCorporate != value)
    			{
    				ChangeTracker.RecordOriginalValue("IsCorporate", _isCorporate);
    				_isCorporate = value;
    				IsCorporateChanged();
    				OnPropertyChanged("IsCorporate");
    			}
    		}
    	}
    	private bool _isCorporate;

        #endregion
        #region Navigation Properties
    
    	public AccountReportType AccountReportType
    	{
    		get { return _accountReportType; }
    		set
    		{
    			if (!ReferenceEquals(_accountReportType, value))
    			{
    				var previousValue = _accountReportType;
    				_accountReportType = value;
    				FixupAccountReportType(previousValue);
    				OnNavigationPropertyChanged("AccountReportType");
    			}
    		}
    	}
    	private AccountReportType _accountReportType;
    
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
    
    	public User User1
    	{
    		get { return _user1; }
    		set
    		{
    			if (!ReferenceEquals(_user1, value))
    			{
    				var previousValue = _user1;
    				_user1 = value;
    				FixupUser1(previousValue);
    				OnNavigationPropertyChanged("User1");
    			}
    		}
    	}
    	private User _user1;

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
    		AccountReportType = null;
    		Account = null;
    		User = null;
    		User1 = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupAccountReportType(AccountReportType previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.AccountReports.Contains(this))
    		{
    			previousValue.AccountReports.Remove(this);
    		}
    
    		if (AccountReportType != null)
    		{
    			if (!AccountReportType.AccountReports.Contains(this))
    			{
    				AccountReportType.AccountReports.Add(this);
    			}
    
    			AccountReportTypeID = AccountReportType.AccountReportTypeID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("AccountReportType")
    				&& (ChangeTracker.OriginalValues["AccountReportType"] == AccountReportType))
    			{
    				ChangeTracker.OriginalValues.Remove("AccountReportType");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("AccountReportType", previousValue);
    			}
    			if (AccountReportType != null && !AccountReportType.ChangeTracker.ChangeTrackingEnabled)
    			{
    				AccountReportType.StartTracking();
    			}
    		}
    	}
    
    	private void FixupAccount(Account previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.AccountReports.Contains(this))
    		{
    			previousValue.AccountReports.Remove(this);
    		}
    
    		if (Account != null)
    		{
    			if (!Account.AccountReports.Contains(this))
    			{
    				Account.AccountReports.Add(this);
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
    
    	private void FixupUser(User previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.AccountReports.Contains(this))
    		{
    			previousValue.AccountReports.Remove(this);
    		}
    
    		if (User != null)
    		{
    			if (!User.AccountReports.Contains(this))
    			{
    				User.AccountReports.Add(this);
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
    
    	private void FixupUser1(User previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.AccountReports1.Contains(this))
    		{
    			previousValue.AccountReports1.Remove(this);
    		}
    
    		if (User1 != null)
    		{
    			if (!User1.AccountReports1.Contains(this))
    			{
    				User1.AccountReports1.Add(this);
    			}
    
    			CreatedByUserID = User1.UserID;
    		}
    		else if (!skipKeys)
    		{
    			CreatedByUserID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("User1")
    				&& (ChangeTracker.OriginalValues["User1"] == User1))
    			{
    				ChangeTracker.OriginalValues.Remove("User1");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("User1", previousValue);
    			}
    			if (User1 != null && !User1.ChangeTracker.ChangeTrackingEnabled)
    			{
    				User1.StartTracking();
    			}
    		}
    	}

        #endregion
    }
}