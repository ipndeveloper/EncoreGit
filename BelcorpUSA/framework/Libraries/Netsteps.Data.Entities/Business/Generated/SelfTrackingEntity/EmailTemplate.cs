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
    [KnownType(typeof(EmailBodyTextType))]
    [KnownType(typeof(EmailCampaignAction))]
    [KnownType(typeof(EmailTemplateType))]
    [KnownType(typeof(EmailTemplateTranslation))]
    [Serializable]
    public partial class EmailTemplate: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void EmailTemplateIDChanged();
    	public int EmailTemplateID
    	{
    		get { return _emailTemplateID; }
    		set
    		{
    			if (_emailTemplateID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'EmailTemplateID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_emailTemplateID = value;
    				EmailTemplateIDChanged();
    				OnPropertyChanged("EmailTemplateID");
    			}
    		}
    	}
    	private int _emailTemplateID;
    	partial void EmailTemplateTypeIDChanged();
    	public short EmailTemplateTypeID
    	{
    		get { return _emailTemplateTypeID; }
    		set
    		{
    			if (_emailTemplateTypeID != value)
    			{
    				ChangeTracker.RecordOriginalValue("EmailTemplateTypeID", _emailTemplateTypeID);
    				if (!IsDeserializing)
    				{
    					if (EmailTemplateType != null && EmailTemplateType.EmailTemplateTypeID != value)
    					{
    						EmailTemplateType = null;
    					}
    				}
    				_emailTemplateTypeID = value;
    				EmailTemplateTypeIDChanged();
    				OnPropertyChanged("EmailTemplateTypeID");
    			}
    		}
    	}
    	private short _emailTemplateTypeID;
    	partial void EmailBodyTextTypeIDChanged();
    	public Nullable<short> EmailBodyTextTypeID
    	{
    		get { return _emailBodyTextTypeID; }
    		set
    		{
    			if (_emailBodyTextTypeID != value)
    			{
    				ChangeTracker.RecordOriginalValue("EmailBodyTextTypeID", _emailBodyTextTypeID);
    				if (!IsDeserializing)
    				{
    					if (EmailBodyTextType != null && EmailBodyTextType.EmailBodyTextTypeID != value)
    					{
    						EmailBodyTextType = null;
    					}
    				}
    				_emailBodyTextTypeID = value;
    				EmailBodyTextTypeIDChanged();
    				OnPropertyChanged("EmailBodyTextTypeID");
    			}
    		}
    	}
    	private Nullable<short> _emailBodyTextTypeID;
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
    	partial void HitsChanged();
    	public Nullable<int> Hits
    	{
    		get { return _hits; }
    		set
    		{
    			if (_hits != value)
    			{
    				ChangeTracker.RecordOriginalValue("Hits", _hits);
    				_hits = value;
    				HitsChanged();
    				OnPropertyChanged("Hits");
    			}
    		}
    	}
    	private Nullable<int> _hits;
    	partial void ActiveChanged();
    	public Nullable<bool> Active
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
    	private Nullable<bool> _active;

        #endregion
        #region Navigation Properties
    
    	public EmailBodyTextType EmailBodyTextType
    	{
    		get { return _emailBodyTextType; }
    		set
    		{
    			if (!ReferenceEquals(_emailBodyTextType, value))
    			{
    				var previousValue = _emailBodyTextType;
    				_emailBodyTextType = value;
    				FixupEmailBodyTextType(previousValue);
    				OnNavigationPropertyChanged("EmailBodyTextType");
    			}
    		}
    	}
    	private EmailBodyTextType _emailBodyTextType;
    
    	public TrackableCollection<EmailCampaignAction> EmailCampaignActions
    	{
    		get
    		{
    			if (_emailCampaignActions == null)
    			{
    				_emailCampaignActions = new TrackableCollection<EmailCampaignAction>();
    				_emailCampaignActions.CollectionChanged += FixupEmailCampaignActions;
    				_emailCampaignActions.CollectionChanged += RaiseEmailCampaignActionsChanged;
    			}
    			return _emailCampaignActions;
    		}
    		set
    		{
    			if (!ReferenceEquals(_emailCampaignActions, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_emailCampaignActions != null)
    				{
    					_emailCampaignActions.CollectionChanged -= FixupEmailCampaignActions;
    					_emailCampaignActions.CollectionChanged -= RaiseEmailCampaignActionsChanged;
    				}
    				_emailCampaignActions = value;
    				if (_emailCampaignActions != null)
    				{
    					_emailCampaignActions.CollectionChanged += FixupEmailCampaignActions;
    					_emailCampaignActions.CollectionChanged += RaiseEmailCampaignActionsChanged;
    				}
    				OnNavigationPropertyChanged("EmailCampaignActions");
    			}
    		}
    	}
    	private TrackableCollection<EmailCampaignAction> _emailCampaignActions;
    	partial void EmailCampaignActionsChanged();
    	private void RaiseEmailCampaignActionsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		EmailCampaignActionsChanged();
    	}
    
    	public EmailTemplateType EmailTemplateType
    	{
    		get { return _emailTemplateType; }
    		set
    		{
    			if (!ReferenceEquals(_emailTemplateType, value))
    			{
    				var previousValue = _emailTemplateType;
    				_emailTemplateType = value;
    				FixupEmailTemplateType(previousValue);
    				OnNavigationPropertyChanged("EmailTemplateType");
    			}
    		}
    	}
    	private EmailTemplateType _emailTemplateType;
    
    	public TrackableCollection<EmailTemplateTranslation> EmailTemplateTranslations
    	{
    		get
    		{
    			if (_emailTemplateTranslations == null)
    			{
    				_emailTemplateTranslations = new TrackableCollection<EmailTemplateTranslation>();
    				_emailTemplateTranslations.CollectionChanged += FixupEmailTemplateTranslations;
    				_emailTemplateTranslations.CollectionChanged += RaiseEmailTemplateTranslationsChanged;
    			}
    			return _emailTemplateTranslations;
    		}
    		set
    		{
    			if (!ReferenceEquals(_emailTemplateTranslations, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_emailTemplateTranslations != null)
    				{
    					_emailTemplateTranslations.CollectionChanged -= FixupEmailTemplateTranslations;
    					_emailTemplateTranslations.CollectionChanged -= RaiseEmailTemplateTranslationsChanged;
    				}
    				_emailTemplateTranslations = value;
    				if (_emailTemplateTranslations != null)
    				{
    					_emailTemplateTranslations.CollectionChanged += FixupEmailTemplateTranslations;
    					_emailTemplateTranslations.CollectionChanged += RaiseEmailTemplateTranslationsChanged;
    				}
    				OnNavigationPropertyChanged("EmailTemplateTranslations");
    			}
    		}
    	}
    	private TrackableCollection<EmailTemplateTranslation> _emailTemplateTranslations;
    	partial void EmailTemplateTranslationsChanged();
    	private void RaiseEmailTemplateTranslationsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		EmailTemplateTranslationsChanged();
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
    		if (_emailCampaignActions != null)
    		{
    			_emailCampaignActions.CollectionChanged -= FixupEmailCampaignActions;
    			_emailCampaignActions.CollectionChanged -= RaiseEmailCampaignActionsChanged;
    			_emailCampaignActions.CollectionChanged += FixupEmailCampaignActions;
    			_emailCampaignActions.CollectionChanged += RaiseEmailCampaignActionsChanged;
    		}
    		if (_emailTemplateTranslations != null)
    		{
    			_emailTemplateTranslations.CollectionChanged -= FixupEmailTemplateTranslations;
    			_emailTemplateTranslations.CollectionChanged -= RaiseEmailTemplateTranslationsChanged;
    			_emailTemplateTranslations.CollectionChanged += FixupEmailTemplateTranslations;
    			_emailTemplateTranslations.CollectionChanged += RaiseEmailTemplateTranslationsChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		EmailBodyTextType = null;
    		EmailCampaignActions.Clear();
    		EmailTemplateType = null;
    		EmailTemplateTranslations.Clear();
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupEmailBodyTextType(EmailBodyTextType previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.EmailTemplates.Contains(this))
    		{
    			previousValue.EmailTemplates.Remove(this);
    		}
    
    		if (EmailBodyTextType != null)
    		{
    			if (!EmailBodyTextType.EmailTemplates.Contains(this))
    			{
    				EmailBodyTextType.EmailTemplates.Add(this);
    			}
    
    			EmailBodyTextTypeID = EmailBodyTextType.EmailBodyTextTypeID;
    		}
    		else if (!skipKeys)
    		{
    			EmailBodyTextTypeID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("EmailBodyTextType")
    				&& (ChangeTracker.OriginalValues["EmailBodyTextType"] == EmailBodyTextType))
    			{
    				ChangeTracker.OriginalValues.Remove("EmailBodyTextType");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("EmailBodyTextType", previousValue);
    			}
    			if (EmailBodyTextType != null && !EmailBodyTextType.ChangeTracker.ChangeTrackingEnabled)
    			{
    				EmailBodyTextType.StartTracking();
    			}
    		}
    	}
    
    	private void FixupEmailTemplateType(EmailTemplateType previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.EmailTemplates.Contains(this))
    		{
    			previousValue.EmailTemplates.Remove(this);
    		}
    
    		if (EmailTemplateType != null)
    		{
    			if (!EmailTemplateType.EmailTemplates.Contains(this))
    			{
    				EmailTemplateType.EmailTemplates.Add(this);
    			}
    
    			EmailTemplateTypeID = EmailTemplateType.EmailTemplateTypeID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("EmailTemplateType")
    				&& (ChangeTracker.OriginalValues["EmailTemplateType"] == EmailTemplateType))
    			{
    				ChangeTracker.OriginalValues.Remove("EmailTemplateType");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("EmailTemplateType", previousValue);
    			}
    			if (EmailTemplateType != null && !EmailTemplateType.ChangeTracker.ChangeTrackingEnabled)
    			{
    				EmailTemplateType.StartTracking();
    			}
    		}
    	}
    
    	private void FixupEmailCampaignActions(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (EmailCampaignAction item in e.NewItems)
    			{
    				item.EmailTemplate = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("EmailCampaignActions", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (EmailCampaignAction item in e.OldItems)
    			{
    				if (ReferenceEquals(item.EmailTemplate, this))
    				{
    					item.EmailTemplate = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("EmailCampaignActions", item);
    				}
    			}
    		}
    	}
    
    	private void FixupEmailTemplateTranslations(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (EmailTemplateTranslation item in e.NewItems)
    			{
    				item.EmailTemplate = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("EmailTemplateTranslations", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (EmailTemplateTranslation item in e.OldItems)
    			{
    				if (ReferenceEquals(item.EmailTemplate, this))
    				{
    					item.EmailTemplate = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("EmailTemplateTranslations", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}
