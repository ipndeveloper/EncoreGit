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
    [KnownType(typeof(Application))]
    [KnownType(typeof(User))]
    [Serializable]
    public partial class ApplicationUsageLog: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void ApplicationUsageLogIDChanged();
    	public int ApplicationUsageLogID
    	{
    		get { return _applicationUsageLogID; }
    		set
    		{
    			if (_applicationUsageLogID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'ApplicationUsageLogID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_applicationUsageLogID = value;
    				ApplicationUsageLogIDChanged();
    				OnPropertyChanged("ApplicationUsageLogID");
    			}
    		}
    	}
    	private int _applicationUsageLogID;
    	partial void ApplicationIDChanged();
    	public short ApplicationID
    	{
    		get { return _applicationID; }
    		set
    		{
    			if (_applicationID != value)
    			{
    				ChangeTracker.RecordOriginalValue("ApplicationID", _applicationID);
    				if (!IsDeserializing)
    				{
    					if (Application != null && Application.ApplicationID != value)
    					{
    						Application = null;
    					}
    				}
    				_applicationID = value;
    				ApplicationIDChanged();
    				OnPropertyChanged("ApplicationID");
    			}
    		}
    	}
    	private short _applicationID;
    	partial void UserIDChanged();
    	public Nullable<int> UserID
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
    	private Nullable<int> _userID;
    	partial void UsageDateUTCChanged();
    	public System.DateTime UsageDateUTC
    	{
    		get { return _usageDateUTC; }
    		set
    		{
    			if (_usageDateUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("UsageDateUTC", _usageDateUTC);
    				_usageDateUTC = value;
    				UsageDateUTCChanged();
    				OnPropertyChanged("UsageDateUTC");
    			}
    		}
    	}
    	private System.DateTime _usageDateUTC;
    	partial void AssemblyNameChanged();
    	public string AssemblyName
    	{
    		get { return _assemblyName; }
    		set
    		{
    			if (_assemblyName != value)
    			{
    				ChangeTracker.RecordOriginalValue("AssemblyName", _assemblyName);
    				_assemblyName = value;
    				AssemblyNameChanged();
    				OnPropertyChanged("AssemblyName");
    			}
    		}
    	}
    	private string _assemblyName;
    	partial void MachineNameChanged();
    	public string MachineName
    	{
    		get { return _machineName; }
    		set
    		{
    			if (_machineName != value)
    			{
    				ChangeTracker.RecordOriginalValue("MachineName", _machineName);
    				_machineName = value;
    				MachineNameChanged();
    				OnPropertyChanged("MachineName");
    			}
    		}
    	}
    	private string _machineName;
    	partial void ClassNameChanged();
    	public string ClassName
    	{
    		get { return _className; }
    		set
    		{
    			if (_className != value)
    			{
    				ChangeTracker.RecordOriginalValue("ClassName", _className);
    				_className = value;
    				ClassNameChanged();
    				OnPropertyChanged("ClassName");
    			}
    		}
    	}
    	private string _className;
    	partial void MethodNameChanged();
    	public string MethodName
    	{
    		get { return _methodName; }
    		set
    		{
    			if (_methodName != value)
    			{
    				ChangeTracker.RecordOriginalValue("MethodName", _methodName);
    				_methodName = value;
    				MethodNameChanged();
    				OnPropertyChanged("MethodName");
    			}
    		}
    	}
    	private string _methodName;
    	partial void MillisecondDurationChanged();
    	public Nullable<double> MillisecondDuration
    	{
    		get { return _millisecondDuration; }
    		set
    		{
    			if (_millisecondDuration != value)
    			{
    				ChangeTracker.RecordOriginalValue("MillisecondDuration", _millisecondDuration);
    				_millisecondDuration = value;
    				MillisecondDurationChanged();
    				OnPropertyChanged("MillisecondDuration");
    			}
    		}
    	}
    	private Nullable<double> _millisecondDuration;

        #endregion
        #region Navigation Properties
    
    	public Application Application
    	{
    		get { return _application; }
    		set
    		{
    			if (!ReferenceEquals(_application, value))
    			{
    				var previousValue = _application;
    				_application = value;
    				FixupApplication(previousValue);
    				OnNavigationPropertyChanged("Application");
    			}
    		}
    	}
    	private Application _application;
    
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
    		Application = null;
    		User = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupApplication(Application previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.ApplicationUsageLogs.Contains(this))
    		{
    			previousValue.ApplicationUsageLogs.Remove(this);
    		}
    
    		if (Application != null)
    		{
    			if (!Application.ApplicationUsageLogs.Contains(this))
    			{
    				Application.ApplicationUsageLogs.Add(this);
    			}
    
    			ApplicationID = Application.ApplicationID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Application")
    				&& (ChangeTracker.OriginalValues["Application"] == Application))
    			{
    				ChangeTracker.OriginalValues.Remove("Application");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Application", previousValue);
    			}
    			if (Application != null && !Application.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Application.StartTracking();
    			}
    		}
    	}
    
    	private void FixupUser(User previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.ApplicationUsageLogs.Contains(this))
    		{
    			previousValue.ApplicationUsageLogs.Remove(this);
    		}
    
    		if (User != null)
    		{
    			if (!User.ApplicationUsageLogs.Contains(this))
    			{
    				User.ApplicationUsageLogs.Add(this);
    			}
    
    			UserID = User.UserID;
    		}
    		else if (!skipKeys)
    		{
    			UserID = null;
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
