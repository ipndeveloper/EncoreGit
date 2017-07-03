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
    [KnownType(typeof(User))]
    [KnownType(typeof(Site))]
    [Serializable]
    public partial class CorporateUser: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void CorporateUserIDChanged();
    	public int CorporateUserID
    	{
    		get { return _corporateUserID; }
    		set
    		{
    			if (_corporateUserID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'CorporateUserID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_corporateUserID = value;
    				CorporateUserIDChanged();
    				OnPropertyChanged("CorporateUserID");
    			}
    		}
    	}
    	private int _corporateUserID;
    	partial void UserIDChanged();
    	public int UserID
    	{
    		get { return _userID; }
    		set
    		{
    			if (_userID != value)
    			{
    				ChangeTracker.RecordOriginalValue("UserID", _userID);
    				if (!IsDeserializing)
    				{
    					if (User != null && User.UserID != value)
    					{
    						User = null;
    					}
    				}
    				_userID = value;
    				UserIDChanged();
    				OnPropertyChanged("UserID");
    			}
    		}
    	}
    	private int _userID;
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
    	partial void EmailChanged();
    	public string Email
    	{
    		get { return _email; }
    		set
    		{
    			if (_email != value)
    			{
    				ChangeTracker.RecordOriginalValue("Email", _email);
    				_email = value;
    				EmailChanged();
    				OnPropertyChanged("Email");
    			}
    		}
    	}
    	private string _email;
    	partial void PhoneNumberChanged();
    	public string PhoneNumber
    	{
    		get { return _phoneNumber; }
    		set
    		{
    			if (_phoneNumber != value)
    			{
    				ChangeTracker.RecordOriginalValue("PhoneNumber", _phoneNumber);
    				_phoneNumber = value;
    				PhoneNumberChanged();
    				OnPropertyChanged("PhoneNumber");
    			}
    		}
    	}
    	private string _phoneNumber;
    	partial void DataVersionChanged();
    	public byte[] DataVersion
    	{
    		get { return _dataVersion; }
    		set
    		{
    			if (_dataVersion != value)
    			{
    				ChangeTracker.RecordOriginalValue("DataVersion", _dataVersion);
    				_dataVersion = value;
    				DataVersionChanged();
    				OnPropertyChanged("DataVersion");
    			}
    		}
    	}
    	private byte[] _dataVersion;
    	partial void HasAccessToAllSitesChanged();
    	public bool HasAccessToAllSites
    	{
    		get { return _hasAccessToAllSites; }
    		set
    		{
    			if (_hasAccessToAllSites != value)
    			{
    				ChangeTracker.RecordOriginalValue("HasAccessToAllSites", _hasAccessToAllSites);
    				_hasAccessToAllSites = value;
    				HasAccessToAllSitesChanged();
    				OnPropertyChanged("HasAccessToAllSites");
    			}
    		}
    	}
    	private bool _hasAccessToAllSites;
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
    					if (User1 != null && User1.UserID != value)
    					{
    						User1 = null;
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
    
    	public TrackableCollection<Site> Sites
    	{
    		get
    		{
    			if (_sites == null)
    			{
    				_sites = new TrackableCollection<Site>();
    				_sites.CollectionChanged += FixupSites;
    				_sites.CollectionChanged += RaiseSitesChanged;
    			}
    			return _sites;
    		}
    		set
    		{
    			if (!ReferenceEquals(_sites, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_sites != null)
    				{
    					_sites.CollectionChanged -= FixupSites;
    					_sites.CollectionChanged -= RaiseSitesChanged;
    				}
    				_sites = value;
    				if (_sites != null)
    				{
    					_sites.CollectionChanged += FixupSites;
    					_sites.CollectionChanged += RaiseSitesChanged;
    				}
    				OnNavigationPropertyChanged("Sites");
    			}
    		}
    	}
    	private TrackableCollection<Site> _sites;
    	partial void SitesChanged();
    	private void RaiseSitesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		SitesChanged();
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
    		if (_sites != null)
    		{
    			_sites.CollectionChanged -= FixupSites;
    			_sites.CollectionChanged -= RaiseSitesChanged;
    			_sites.CollectionChanged += FixupSites;
    			_sites.CollectionChanged += RaiseSitesChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		User = null;
    		User1 = null;
    		Sites.Clear();
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupUser(User previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.CorporateUsers.Contains(this))
    		{
    			previousValue.CorporateUsers.Remove(this);
    		}
    
    		if (User != null)
    		{
    			if (!User.CorporateUsers.Contains(this))
    			{
    				User.CorporateUsers.Add(this);
    			}
    
    			UserID = User.UserID;
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
    
    		if (previousValue != null && previousValue.CorporateUsers1.Contains(this))
    		{
    			previousValue.CorporateUsers1.Remove(this);
    		}
    
    		if (User1 != null)
    		{
    			if (!User1.CorporateUsers1.Contains(this))
    			{
    				User1.CorporateUsers1.Add(this);
    			}
    
    			ModifiedByUserID = User1.UserID;
    		}
    		else if (!skipKeys)
    		{
    			ModifiedByUserID = null;
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
    
    	private void FixupSites(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (Site item in e.NewItems)
    			{
    				if (!item.CorporateUsers.Contains(this))
    				{
    					item.CorporateUsers.Add(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Sites", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (Site item in e.OldItems)
    			{
    				if (item.CorporateUsers.Contains(this))
    				{
    					item.CorporateUsers.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Sites", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}
