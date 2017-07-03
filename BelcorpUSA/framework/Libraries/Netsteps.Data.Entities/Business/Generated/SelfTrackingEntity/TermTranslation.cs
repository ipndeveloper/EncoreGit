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
    [Serializable]
    public partial class TermTranslation: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void TermTranslationIDChanged();
    	public int TermTranslationID
    	{
    		get { return _termTranslationID; }
    		set
    		{
    			if (_termTranslationID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'TermTranslationID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_termTranslationID = value;
    				TermTranslationIDChanged();
    				OnPropertyChanged("TermTranslationID");
    			}
    		}
    	}
    	private int _termTranslationID;
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
    	partial void TermChanged();
    	public string Term
    	{
    		get { return _term; }
    		set
    		{
    			if (_term != value)
    			{
    				ChangeTracker.RecordOriginalValue("Term", _term);
    				_term = value;
    				TermChanged();
    				OnPropertyChanged("Term");
    			}
    		}
    	}
    	private string _term;
    	partial void LastUpdatedUTCChanged();
    	public Nullable<System.DateTime> LastUpdatedUTC
    	{
    		get { return _lastUpdatedUTC; }
    		set
    		{
    			if (_lastUpdatedUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("LastUpdatedUTC", _lastUpdatedUTC);
    				_lastUpdatedUTC = value;
    				LastUpdatedUTCChanged();
    				OnPropertyChanged("LastUpdatedUTC");
    			}
    		}
    	}
    	private Nullable<System.DateTime> _lastUpdatedUTC;
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
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupLanguage(Language previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.TermTranslations.Contains(this))
    		{
    			previousValue.TermTranslations.Remove(this);
    		}
    
    		if (Language != null)
    		{
    			if (!Language.TermTranslations.Contains(this))
    			{
    				Language.TermTranslations.Add(this);
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

        #endregion
    }
}