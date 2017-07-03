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
    [KnownType(typeof(UserFunctionOverride))]
    [KnownType(typeof(Role))]
    [Serializable]
    public partial class Function: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void FunctionIDChanged();
    	public int FunctionID
    	{
    		get { return _functionID; }
    		set
    		{
    			if (_functionID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'FunctionID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_functionID = value;
    				FunctionIDChanged();
    				OnPropertyChanged("FunctionID");
    			}
    		}
    	}
    	private int _functionID;
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

        #endregion
        #region Navigation Properties
    
    	public TrackableCollection<UserFunctionOverride> UserFunctionOverrides
    	{
    		get
    		{
    			if (_userFunctionOverrides == null)
    			{
    				_userFunctionOverrides = new TrackableCollection<UserFunctionOverride>();
    				_userFunctionOverrides.CollectionChanged += FixupUserFunctionOverrides;
    				_userFunctionOverrides.CollectionChanged += RaiseUserFunctionOverridesChanged;
    			}
    			return _userFunctionOverrides;
    		}
    		set
    		{
    			if (!ReferenceEquals(_userFunctionOverrides, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_userFunctionOverrides != null)
    				{
    					_userFunctionOverrides.CollectionChanged -= FixupUserFunctionOverrides;
    					_userFunctionOverrides.CollectionChanged -= RaiseUserFunctionOverridesChanged;
    				}
    				_userFunctionOverrides = value;
    				if (_userFunctionOverrides != null)
    				{
    					_userFunctionOverrides.CollectionChanged += FixupUserFunctionOverrides;
    					_userFunctionOverrides.CollectionChanged += RaiseUserFunctionOverridesChanged;
    				}
    				OnNavigationPropertyChanged("UserFunctionOverrides");
    			}
    		}
    	}
    	private TrackableCollection<UserFunctionOverride> _userFunctionOverrides;
    	partial void UserFunctionOverridesChanged();
    	private void RaiseUserFunctionOverridesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		UserFunctionOverridesChanged();
    	}
    
    	public TrackableCollection<Role> Roles
    	{
    		get
    		{
    			if (_roles == null)
    			{
    				_roles = new TrackableCollection<Role>();
    				_roles.CollectionChanged += FixupRoles;
    				_roles.CollectionChanged += RaiseRolesChanged;
    			}
    			return _roles;
    		}
    		set
    		{
    			if (!ReferenceEquals(_roles, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_roles != null)
    				{
    					_roles.CollectionChanged -= FixupRoles;
    					_roles.CollectionChanged -= RaiseRolesChanged;
    				}
    				_roles = value;
    				if (_roles != null)
    				{
    					_roles.CollectionChanged += FixupRoles;
    					_roles.CollectionChanged += RaiseRolesChanged;
    				}
    				OnNavigationPropertyChanged("Roles");
    			}
    		}
    	}
    	private TrackableCollection<Role> _roles;
    	partial void RolesChanged();
    	private void RaiseRolesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		RolesChanged();
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
    		if (_userFunctionOverrides != null)
    		{
    			_userFunctionOverrides.CollectionChanged -= FixupUserFunctionOverrides;
    			_userFunctionOverrides.CollectionChanged -= RaiseUserFunctionOverridesChanged;
    			_userFunctionOverrides.CollectionChanged += FixupUserFunctionOverrides;
    			_userFunctionOverrides.CollectionChanged += RaiseUserFunctionOverridesChanged;
    		}
    		if (_roles != null)
    		{
    			_roles.CollectionChanged -= FixupRoles;
    			_roles.CollectionChanged -= RaiseRolesChanged;
    			_roles.CollectionChanged += FixupRoles;
    			_roles.CollectionChanged += RaiseRolesChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		UserFunctionOverrides.Clear();
    		Roles.Clear();
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupUserFunctionOverrides(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (UserFunctionOverride item in e.NewItems)
    			{
    				item.Function = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("UserFunctionOverrides", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (UserFunctionOverride item in e.OldItems)
    			{
    				if (ReferenceEquals(item.Function, this))
    				{
    					item.Function = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("UserFunctionOverrides", item);
    				}
    			}
    		}
    	}
    
    	private void FixupRoles(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (Role item in e.NewItems)
    			{
    				if (!item.Functions.Contains(this))
    				{
    					item.Functions.Add(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Roles", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (Role item in e.OldItems)
    			{
    				if (item.Functions.Contains(this))
    				{
    					item.Functions.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Roles", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}
