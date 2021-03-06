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
    [KnownType(typeof(AlertTemplate))]
    [KnownType(typeof(EventContext))]
    [Serializable]
    public partial class AccountAlert: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void AccountAlertIDChanged();
    	public int AccountAlertID
    	{
    		get { return _accountAlertID; }
    		set
    		{
    			if (_accountAlertID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'AccountAlertID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_accountAlertID = value;
    				AccountAlertIDChanged();
    				OnPropertyChanged("AccountAlertID");
    			}
    		}
    	}
    	private int _accountAlertID;
    	partial void EventContextIDChanged();
    	public int EventContextID
    	{
    		get { return _eventContextID; }
    		set
    		{
    			if (_eventContextID != value)
    			{
    				ChangeTracker.RecordOriginalValue("EventContextID", _eventContextID);
    				if (!IsDeserializing)
    				{
    					if (EventContext != null && EventContext.EventContextID != value)
    					{
    						EventContext = null;
    					}
    				}
    				_eventContextID = value;
    				EventContextIDChanged();
    				OnPropertyChanged("EventContextID");
    			}
    		}
    	}
    	private int _eventContextID;
    	partial void DismissedChanged();
    	public bool Dismissed
    	{
    		get { return _dismissed; }
    		set
    		{
    			if (_dismissed != value)
    			{
    				ChangeTracker.RecordOriginalValue("Dismissed", _dismissed);
    				_dismissed = value;
    				DismissedChanged();
    				OnPropertyChanged("Dismissed");
    			}
    		}
    	}
    	private bool _dismissed;
    	partial void DismissedDateChanged();
    	public Nullable<System.DateTime> DismissedDate
    	{
    		get { return _dismissedDate; }
    		set
    		{
    			if (_dismissedDate != value)
    			{
    				ChangeTracker.RecordOriginalValue("DismissedDate", _dismissedDate);
    				_dismissedDate = value;
    				DismissedDateChanged();
    				OnPropertyChanged("DismissedDate");
    			}
    		}
    	}
    	private Nullable<System.DateTime> _dismissedDate;
    	partial void AlertTemplateIDChanged();
    	public int AlertTemplateID
    	{
    		get { return _alertTemplateID; }
    		set
    		{
    			if (_alertTemplateID != value)
    			{
    				ChangeTracker.RecordOriginalValue("AlertTemplateID", _alertTemplateID);
    				if (!IsDeserializing)
    				{
    					if (AlertTemplate != null && AlertTemplate.AlertTemplateID != value)
    					{
    						AlertTemplate = null;
    					}
    				}
    				_alertTemplateID = value;
    				AlertTemplateIDChanged();
    				OnPropertyChanged("AlertTemplateID");
    			}
    		}
    	}
    	private int _alertTemplateID;
    	partial void CanBeDismissedChanged();
    	public bool CanBeDismissed
    	{
    		get { return _canBeDismissed; }
    		set
    		{
    			if (_canBeDismissed != value)
    			{
    				ChangeTracker.RecordOriginalValue("CanBeDismissed", _canBeDismissed);
    				_canBeDismissed = value;
    				CanBeDismissedChanged();
    				OnPropertyChanged("CanBeDismissed");
    			}
    		}
    	}
    	private bool _canBeDismissed;

        #endregion
        #region Navigation Properties
    
    	public AlertTemplate AlertTemplate
    	{
    		get { return _alertTemplate; }
    		set
    		{
    			if (!ReferenceEquals(_alertTemplate, value))
    			{
    				var previousValue = _alertTemplate;
    				_alertTemplate = value;
    				FixupAlertTemplate(previousValue);
    				OnNavigationPropertyChanged("AlertTemplate");
    			}
    		}
    	}
    	private AlertTemplate _alertTemplate;
    
    	public EventContext EventContext
    	{
    		get { return _eventContext; }
    		set
    		{
    			if (!ReferenceEquals(_eventContext, value))
    			{
    				var previousValue = _eventContext;
    				_eventContext = value;
    				FixupEventContext(previousValue);
    				OnNavigationPropertyChanged("EventContext");
    			}
    		}
    	}
    	private EventContext _eventContext;

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
    		AlertTemplate = null;
    		EventContext = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupAlertTemplate(AlertTemplate previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.AccountAlerts.Contains(this))
    		{
    			previousValue.AccountAlerts.Remove(this);
    		}
    
    		if (AlertTemplate != null)
    		{
    			if (!AlertTemplate.AccountAlerts.Contains(this))
    			{
    				AlertTemplate.AccountAlerts.Add(this);
    			}
    
    			AlertTemplateID = AlertTemplate.AlertTemplateID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("AlertTemplate")
    				&& (ChangeTracker.OriginalValues["AlertTemplate"] == AlertTemplate))
    			{
    				ChangeTracker.OriginalValues.Remove("AlertTemplate");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("AlertTemplate", previousValue);
    			}
    			if (AlertTemplate != null && !AlertTemplate.ChangeTracker.ChangeTrackingEnabled)
    			{
    				AlertTemplate.StartTracking();
    			}
    		}
    	}
    
    	private void FixupEventContext(EventContext previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.AccountAlerts.Contains(this))
    		{
    			previousValue.AccountAlerts.Remove(this);
    		}
    
    		if (EventContext != null)
    		{
    			if (!EventContext.AccountAlerts.Contains(this))
    			{
    				EventContext.AccountAlerts.Add(this);
    			}
    
    			EventContextID = EventContext.EventContextID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("EventContext")
    				&& (ChangeTracker.OriginalValues["EventContext"] == EventContext))
    			{
    				ChangeTracker.OriginalValues.Remove("EventContext");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("EventContext", previousValue);
    			}
    			if (EventContext != null && !EventContext.ChangeTracker.ChangeTrackingEnabled)
    			{
    				EventContext.StartTracking();
    			}
    		}
    	}

        #endregion
    }
}
