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
    [KnownType(typeof(Autoresponder))]
    [KnownType(typeof(Language))]
    [Serializable]
    public partial class AutoresponderTranslation: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void AutoresponderTranslationIDChanged();
    	public int AutoresponderTranslationID
    	{
    		get { return _autoresponderTranslationID; }
    		set
    		{
    			if (_autoresponderTranslationID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'AutoresponderTranslationID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_autoresponderTranslationID = value;
    				AutoresponderTranslationIDChanged();
    				OnPropertyChanged("AutoresponderTranslationID");
    			}
    		}
    	}
    	private int _autoresponderTranslationID;
    	partial void AutoresponderIDChanged();
    	public int AutoresponderID
    	{
    		get { return _autoresponderID; }
    		set
    		{
    			if (_autoresponderID != value)
    			{
    				ChangeTracker.RecordOriginalValue("AutoresponderID", _autoresponderID);
    				if (!IsDeserializing)
    				{
    					if (Autoresponder != null && Autoresponder.AutoresponderID != value)
    					{
    						Autoresponder = null;
    					}
    				}
    				_autoresponderID = value;
    				AutoresponderIDChanged();
    				OnPropertyChanged("AutoresponderID");
    			}
    		}
    	}
    	private int _autoresponderID;
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
    	partial void DisplayTextChanged();
    	public string DisplayText
    	{
    		get { return _displayText; }
    		set
    		{
    			if (_displayText != value)
    			{
    				ChangeTracker.RecordOriginalValue("DisplayText", _displayText);
    				_displayText = value;
    				DisplayTextChanged();
    				OnPropertyChanged("DisplayText");
    			}
    		}
    	}
    	private string _displayText;

        #endregion
        #region Navigation Properties
    
    	public Autoresponder Autoresponder
    	{
    		get { return _autoresponder; }
    		set
    		{
    			if (!ReferenceEquals(_autoresponder, value))
    			{
    				var previousValue = _autoresponder;
    				_autoresponder = value;
    				FixupAutoresponder(previousValue);
    				OnNavigationPropertyChanged("Autoresponder");
    			}
    		}
    	}
    	private Autoresponder _autoresponder;
    
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
    		Autoresponder = null;
    		Language = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupAutoresponder(Autoresponder previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.Translations.Contains(this))
    		{
    			previousValue.Translations.Remove(this);
    		}
    
    		if (Autoresponder != null)
    		{
    			if (!Autoresponder.Translations.Contains(this))
    			{
    				Autoresponder.Translations.Add(this);
    			}
    
    			AutoresponderID = Autoresponder.AutoresponderID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Autoresponder")
    				&& (ChangeTracker.OriginalValues["Autoresponder"] == Autoresponder))
    			{
    				ChangeTracker.OriginalValues.Remove("Autoresponder");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Autoresponder", previousValue);
    			}
    			if (Autoresponder != null && !Autoresponder.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Autoresponder.StartTracking();
    			}
    		}
    	}
    
    	private void FixupLanguage(Language previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.AutoresponderTranslations.Contains(this))
    		{
    			previousValue.AutoresponderTranslations.Remove(this);
    		}
    
    		if (Language != null)
    		{
    			if (!Language.AutoresponderTranslations.Contains(this))
    			{
    				Language.AutoresponderTranslations.Add(this);
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
