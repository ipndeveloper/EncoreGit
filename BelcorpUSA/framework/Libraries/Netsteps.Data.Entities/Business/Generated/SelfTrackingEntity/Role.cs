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
    [KnownType(typeof(RoleType))]
    [KnownType(typeof(Function))]
    [KnownType(typeof(User))]
    [KnownType(typeof(AccountType))]
    [Serializable]
    public partial class Role: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void RoleIDChanged();
    	public int RoleID
    	{
    		get { return _roleID; }
    		set
    		{
    			if (_roleID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'RoleID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_roleID = value;
    				RoleIDChanged();
    				OnPropertyChanged("RoleID");
    			}
    		}
    	}
    	private int _roleID;
    	partial void RoleTypeIDChanged();
    	public short RoleTypeID
    	{
    		get { return _roleTypeID; }
    		set
    		{
    			if (_roleTypeID != value)
    			{
    				ChangeTracker.RecordOriginalValue("RoleTypeID", _roleTypeID);
    				if (!IsDeserializing)
    				{
    					if (RoleType != null && RoleType.RoleTypeID != value)
    					{
    						RoleType = null;
    					}
    				}
    				_roleTypeID = value;
    				RoleTypeIDChanged();
    				OnPropertyChanged("RoleTypeID");
    			}
    		}
    	}
    	private short _roleTypeID;
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
    	partial void TermNameChanged();
    	public string TermName
    	{
    		get { return _termName; }
    		set
    		{
    			if (_termName != value)
    			{
    				ChangeTracker.RecordOriginalValue("TermName", _termName);
    				_termName = value;
    				TermNameChanged();
    				OnPropertyChanged("TermName");
    			}
    		}
    	}
    	private string _termName;
    	partial void DescriptionChanged();
    	public string Description
    	{
    		get { return _description; }
    		set
    		{
    			if (_description != value)
    			{
    				ChangeTracker.RecordOriginalValue("Description", _description);
    				_description = value;
    				DescriptionChanged();
    				OnPropertyChanged("Description");
    			}
    		}
    	}
    	private string _description;
    	partial void StartPageChanged();
    	public string StartPage
    	{
    		get { return _startPage; }
    		set
    		{
    			if (_startPage != value)
    			{
    				ChangeTracker.RecordOriginalValue("StartPage", _startPage);
    				_startPage = value;
    				StartPageChanged();
    				OnPropertyChanged("StartPage");
    			}
    		}
    	}
    	private string _startPage;
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
    	partial void EditableChanged();
    	public bool Editable
    	{
    		get { return _editable; }
    		set
    		{
    			if (_editable != value)
    			{
    				ChangeTracker.RecordOriginalValue("Editable", _editable);
    				_editable = value;
    				EditableChanged();
    				OnPropertyChanged("Editable");
    			}
    		}
    	}
    	private bool _editable;

        #endregion
        #region Navigation Properties
    
    	public RoleType RoleType
    	{
    		get { return _roleType; }
    		set
    		{
    			if (!ReferenceEquals(_roleType, value))
    			{
    				var previousValue = _roleType;
    				_roleType = value;
    				FixupRoleType(previousValue);
    				OnNavigationPropertyChanged("RoleType");
    			}
    		}
    	}
    	private RoleType _roleType;
    
    	public TrackableCollection<Function> Functions
    	{
    		get
    		{
    			if (_functions == null)
    			{
    				_functions = new TrackableCollection<Function>();
    				_functions.CollectionChanged += FixupFunctions;
    				_functions.CollectionChanged += RaiseFunctionsChanged;
    			}
    			return _functions;
    		}
    		set
    		{
    			if (!ReferenceEquals(_functions, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_functions != null)
    				{
    					_functions.CollectionChanged -= FixupFunctions;
    					_functions.CollectionChanged -= RaiseFunctionsChanged;
    				}
    				_functions = value;
    				if (_functions != null)
    				{
    					_functions.CollectionChanged += FixupFunctions;
    					_functions.CollectionChanged += RaiseFunctionsChanged;
    				}
    				OnNavigationPropertyChanged("Functions");
    			}
    		}
    	}
    	private TrackableCollection<Function> _functions;
    	partial void FunctionsChanged();
    	private void RaiseFunctionsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		FunctionsChanged();
    	}
    
    	public TrackableCollection<User> Users
    	{
    		get
    		{
    			if (_users == null)
    			{
    				_users = new TrackableCollection<User>();
    				_users.CollectionChanged += FixupUsers;
    				_users.CollectionChanged += RaiseUsersChanged;
    			}
    			return _users;
    		}
    		set
    		{
    			if (!ReferenceEquals(_users, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_users != null)
    				{
    					_users.CollectionChanged -= FixupUsers;
    					_users.CollectionChanged -= RaiseUsersChanged;
    				}
    				_users = value;
    				if (_users != null)
    				{
    					_users.CollectionChanged += FixupUsers;
    					_users.CollectionChanged += RaiseUsersChanged;
    				}
    				OnNavigationPropertyChanged("Users");
    			}
    		}
    	}
    	private TrackableCollection<User> _users;
    	partial void UsersChanged();
    	private void RaiseUsersChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		UsersChanged();
    	}
    
    	public TrackableCollection<AccountType> AccountTypes
    	{
    		get
    		{
    			if (_accountTypes == null)
    			{
    				_accountTypes = new TrackableCollection<AccountType>();
    				_accountTypes.CollectionChanged += FixupAccountTypes;
    				_accountTypes.CollectionChanged += RaiseAccountTypesChanged;
    			}
    			return _accountTypes;
    		}
    		set
    		{
    			if (!ReferenceEquals(_accountTypes, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_accountTypes != null)
    				{
    					_accountTypes.CollectionChanged -= FixupAccountTypes;
    					_accountTypes.CollectionChanged -= RaiseAccountTypesChanged;
    				}
    				_accountTypes = value;
    				if (_accountTypes != null)
    				{
    					_accountTypes.CollectionChanged += FixupAccountTypes;
    					_accountTypes.CollectionChanged += RaiseAccountTypesChanged;
    				}
    				OnNavigationPropertyChanged("AccountTypes");
    			}
    		}
    	}
    	private TrackableCollection<AccountType> _accountTypes;
    	partial void AccountTypesChanged();
    	private void RaiseAccountTypesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		AccountTypesChanged();
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
    		if (_functions != null)
    		{
    			_functions.CollectionChanged -= FixupFunctions;
    			_functions.CollectionChanged -= RaiseFunctionsChanged;
    			_functions.CollectionChanged += FixupFunctions;
    			_functions.CollectionChanged += RaiseFunctionsChanged;
    		}
    		if (_users != null)
    		{
    			_users.CollectionChanged -= FixupUsers;
    			_users.CollectionChanged -= RaiseUsersChanged;
    			_users.CollectionChanged += FixupUsers;
    			_users.CollectionChanged += RaiseUsersChanged;
    		}
    		if (_accountTypes != null)
    		{
    			_accountTypes.CollectionChanged -= FixupAccountTypes;
    			_accountTypes.CollectionChanged -= RaiseAccountTypesChanged;
    			_accountTypes.CollectionChanged += FixupAccountTypes;
    			_accountTypes.CollectionChanged += RaiseAccountTypesChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		RoleType = null;
    		Functions.Clear();
    		Users.Clear();
    		AccountTypes.Clear();
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupRoleType(RoleType previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.Roles.Contains(this))
    		{
    			previousValue.Roles.Remove(this);
    		}
    
    		if (RoleType != null)
    		{
    			if (!RoleType.Roles.Contains(this))
    			{
    				RoleType.Roles.Add(this);
    			}
    
    			RoleTypeID = RoleType.RoleTypeID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("RoleType")
    				&& (ChangeTracker.OriginalValues["RoleType"] == RoleType))
    			{
    				ChangeTracker.OriginalValues.Remove("RoleType");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("RoleType", previousValue);
    			}
    			if (RoleType != null && !RoleType.ChangeTracker.ChangeTrackingEnabled)
    			{
    				RoleType.StartTracking();
    			}
    		}
    	}
    
    	private void FixupFunctions(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (Function item in e.NewItems)
    			{
    				if (!item.Roles.Contains(this))
    				{
    					item.Roles.Add(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Functions", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (Function item in e.OldItems)
    			{
    				if (item.Roles.Contains(this))
    				{
    					item.Roles.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Functions", item);
    				}
    			}
    		}
    	}
    
    	private void FixupUsers(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (User item in e.NewItems)
    			{
    				if (!item.Roles.Contains(this))
    				{
    					item.Roles.Add(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Users", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (User item in e.OldItems)
    			{
    				if (item.Roles.Contains(this))
    				{
    					item.Roles.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Users", item);
    				}
    			}
    		}
    	}
    
    	private void FixupAccountTypes(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (AccountType item in e.NewItems)
    			{
    				if (!item.Roles.Contains(this))
    				{
    					item.Roles.Add(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("AccountTypes", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (AccountType item in e.OldItems)
    			{
    				if (item.Roles.Contains(this))
    				{
    					item.Roles.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("AccountTypes", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}
