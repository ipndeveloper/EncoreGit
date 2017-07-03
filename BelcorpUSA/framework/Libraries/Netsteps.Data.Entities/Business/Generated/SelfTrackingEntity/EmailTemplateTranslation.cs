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
    [KnownType(typeof(Language))]
    [KnownType(typeof(EmailTemplate))]
    [Serializable]
    public partial class EmailTemplateTranslation: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void EmailTemplateTranslationIDChanged();
    	public int EmailTemplateTranslationID
    	{
    		get { return _emailTemplateTranslationID; }
    		set
    		{
    			if (_emailTemplateTranslationID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'EmailTemplateTranslationID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_emailTemplateTranslationID = value;
    				EmailTemplateTranslationIDChanged();
    				OnPropertyChanged("EmailTemplateTranslationID");
    			}
    		}
    	}
    	private int _emailTemplateTranslationID;
    	partial void EmailTemplateIDChanged();
    	public int EmailTemplateID
    	{
    		get { return _emailTemplateID; }
    		set
    		{
    			if (_emailTemplateID != value)
    			{
    				ChangeTracker.RecordOriginalValue("EmailTemplateID", _emailTemplateID);
    				if (!IsDeserializing)
    				{
    					if (EmailTemplate != null && EmailTemplate.EmailTemplateID != value)
    					{
    						EmailTemplate = null;
    					}
    				}
    				_emailTemplateID = value;
    				EmailTemplateIDChanged();
    				OnPropertyChanged("EmailTemplateID");
    			}
    		}
    	}
    	private int _emailTemplateID;
    	partial void LanguageIDChanged();
    	public int LanguageID
    	{
    		get { return _languageID; }
    		set
    		{
    			if (_languageID != value)
    			{
    				ChangeTracker.RecordOriginalValue("LanguageID", _languageID);
    				if (!IsDeserializing)
    				{
    					if (Language != null && Language.LanguageID != value)
    					{
    						Language = null;
    					}
    				}
    				_languageID = value;
    				LanguageIDChanged();
    				OnPropertyChanged("LanguageID");
    			}
    		}
    	}
    	private int _languageID;
    	partial void SubjectChanged();
    	public string Subject
    	{
    		get { return _subject; }
    		set
    		{
    			if (_subject != value)
    			{
    				ChangeTracker.RecordOriginalValue("Subject", _subject);
    				_subject = value;
    				SubjectChanged();
    				OnPropertyChanged("Subject");
    			}
    		}
    	}
    	private string _subject;
    	partial void BodyChanged();
    	public string Body
    	{
    		get { return _body; }
    		set
    		{
    			if (_body != value)
    			{
    				ChangeTracker.RecordOriginalValue("Body", _body);
    				_body = value;
    				BodyChanged();
    				OnPropertyChanged("Body");
    			}
    		}
    	}
    	private string _body;
    	partial void FromAddressChanged();
    	public string FromAddress
    	{
    		get { return _fromAddress; }
    		set
    		{
    			if (_fromAddress != value)
    			{
    				ChangeTracker.RecordOriginalValue("FromAddress", _fromAddress);
    				_fromAddress = value;
    				FromAddressChanged();
    				OnPropertyChanged("FromAddress");
    			}
    		}
    	}
    	private string _fromAddress;
    	partial void AttachmentPathChanged();
    	public string AttachmentPath
    	{
    		get { return _attachmentPath; }
    		set
    		{
    			if (_attachmentPath != value)
    			{
    				ChangeTracker.RecordOriginalValue("AttachmentPath", _attachmentPath);
    				_attachmentPath = value;
    				AttachmentPathChanged();
    				OnPropertyChanged("AttachmentPath");
    			}
    		}
    	}
    	private string _attachmentPath;
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

        #endregion
        #region Navigation Properties
    
    	public Language Language
    	{
    		get { return _language; }
    		set
    		{
    			if (!ReferenceEquals(_language, value))
    			{
    				var previousValue = _language;
    				_language = value;
    				FixupLanguage(previousValue);
    				OnNavigationPropertyChanged("Language");
    			}
    		}
    	}
    	private Language _language;
    
    	public EmailTemplate EmailTemplate
    	{
    		get { return _emailTemplate; }
    		set
    		{
    			if (!ReferenceEquals(_emailTemplate, value))
    			{
    				var previousValue = _emailTemplate;
    				_emailTemplate = value;
    				FixupEmailTemplate(previousValue);
    				OnNavigationPropertyChanged("EmailTemplate");
    			}
    		}
    	}
    	private EmailTemplate _emailTemplate;

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
    		Language = null;
    		EmailTemplate = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupLanguage(Language previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.EmailTemplateTranslations.Contains(this))
    		{
    			previousValue.EmailTemplateTranslations.Remove(this);
    		}
    
    		if (Language != null)
    		{
    			if (!Language.EmailTemplateTranslations.Contains(this))
    			{
    				Language.EmailTemplateTranslations.Add(this);
    			}
    
    			LanguageID = Language.LanguageID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Language")
    				&& (ChangeTracker.OriginalValues["Language"] == Language))
    			{
    				ChangeTracker.OriginalValues.Remove("Language");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Language", previousValue);
    			}
    			if (Language != null && !Language.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Language.StartTracking();
    			}
    		}
    	}
    
    	private void FixupEmailTemplate(EmailTemplate previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.EmailTemplateTranslations.Contains(this))
    		{
    			previousValue.EmailTemplateTranslations.Remove(this);
    		}
    
    		if (EmailTemplate != null)
    		{
    			if (!EmailTemplate.EmailTemplateTranslations.Contains(this))
    			{
    				EmailTemplate.EmailTemplateTranslations.Add(this);
    			}
    
    			EmailTemplateID = EmailTemplate.EmailTemplateID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("EmailTemplate")
    				&& (ChangeTracker.OriginalValues["EmailTemplate"] == EmailTemplate))
    			{
    				ChangeTracker.OriginalValues.Remove("EmailTemplate");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("EmailTemplate", previousValue);
    			}
    			if (EmailTemplate != null && !EmailTemplate.ChangeTracker.ChangeTrackingEnabled)
    			{
    				EmailTemplate.StartTracking();
    			}
    		}
    	}

        #endregion
    }
}
