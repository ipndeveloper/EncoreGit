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
    [KnownType(typeof(Category))]
    [KnownType(typeof(Language))]
    [KnownType(typeof(HtmlContent))]
    [Serializable]
    public partial class CategoryTranslation: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void CategoryTranslationIDChanged();
    	public int CategoryTranslationID
    	{
    		get { return _categoryTranslationID; }
    		set
    		{
    			if (_categoryTranslationID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'CategoryTranslationID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_categoryTranslationID = value;
    				CategoryTranslationIDChanged();
    				OnPropertyChanged("CategoryTranslationID");
    			}
    		}
    	}
    	private int _categoryTranslationID;
    	partial void CategoryIDChanged();
    	public int CategoryID
    	{
    		get { return _categoryID; }
    		set
    		{
    			if (_categoryID != value)
    			{
    				ChangeTracker.RecordOriginalValue("CategoryID", _categoryID);
    				if (!IsDeserializing)
    				{
    					if (Category != null && Category.CategoryID != value)
    					{
    						Category = null;
    					}
    				}
    				_categoryID = value;
    				CategoryIDChanged();
    				OnPropertyChanged("CategoryID");
    			}
    		}
    	}
    	private int _categoryID;
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
    	partial void HtmlContentIDChanged();
    	public Nullable<int> HtmlContentID
    	{
    		get { return _htmlContentID; }
    		set
    		{
    			if (_htmlContentID != value)
    			{
    				ChangeTracker.RecordOriginalValue("HtmlContentID", _htmlContentID);
    				if (!IsDeserializing)
    				{
    					if (HtmlContent != null && HtmlContent.HtmlContentID != value)
    					{
    						HtmlContent = null;
    					}
    				}
    				_htmlContentID = value;
    				HtmlContentIDChanged();
    				OnPropertyChanged("HtmlContentID");
    			}
    		}
    	}
    	private Nullable<int> _htmlContentID;
    	partial void ETLNaturalKeyChanged();
    	public string ETLNaturalKey
    	{
    		get { return _eTLNaturalKey; }
    		set
    		{
    			if (_eTLNaturalKey != value)
    			{
    				ChangeTracker.RecordOriginalValue("ETLNaturalKey", _eTLNaturalKey);
    				_eTLNaturalKey = value;
    				ETLNaturalKeyChanged();
    				OnPropertyChanged("ETLNaturalKey");
    			}
    		}
    	}
    	private string _eTLNaturalKey;
    	partial void ETLHashChanged();
    	public string ETLHash
    	{
    		get { return _eTLHash; }
    		set
    		{
    			if (_eTLHash != value)
    			{
    				ChangeTracker.RecordOriginalValue("ETLHash", _eTLHash);
    				_eTLHash = value;
    				ETLHashChanged();
    				OnPropertyChanged("ETLHash");
    			}
    		}
    	}
    	private string _eTLHash;
    	partial void ETLPhaseChanged();
    	public string ETLPhase
    	{
    		get { return _eTLPhase; }
    		set
    		{
    			if (_eTLPhase != value)
    			{
    				ChangeTracker.RecordOriginalValue("ETLPhase", _eTLPhase);
    				_eTLPhase = value;
    				ETLPhaseChanged();
    				OnPropertyChanged("ETLPhase");
    			}
    		}
    	}
    	private string _eTLPhase;
    	partial void ETLDateChanged();
    	public Nullable<System.DateTime> ETLDate
    	{
    		get { return _eTLDate; }
    		set
    		{
    			if (_eTLDate != value)
    			{
    				ChangeTracker.RecordOriginalValue("ETLDate", _eTLDate);
    				_eTLDate = value;
    				ETLDateChanged();
    				OnPropertyChanged("ETLDate");
    			}
    		}
    	}
    	private Nullable<System.DateTime> _eTLDate;

        #endregion
        #region Navigation Properties
    
    	public Category Category
    	{
    		get { return _category; }
    		set
    		{
    			if (!ReferenceEquals(_category, value))
    			{
    				var previousValue = _category;
    				_category = value;
    				FixupCategory(previousValue);
    				OnNavigationPropertyChanged("Category");
    			}
    		}
    	}
    	private Category _category;
    
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
    
    	public HtmlContent HtmlContent
    	{
    		get { return _htmlContent; }
    		set
    		{
    			if (!ReferenceEquals(_htmlContent, value))
    			{
    				var previousValue = _htmlContent;
    				_htmlContent = value;
    				FixupHtmlContent(previousValue);
    				OnNavigationPropertyChanged("HtmlContent");
    			}
    		}
    	}
    	private HtmlContent _htmlContent;

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
    		Category = null;
    		Language = null;
    		HtmlContent = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupCategory(Category previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.Translations.Contains(this))
    		{
    			previousValue.Translations.Remove(this);
    		}
    
    		if (Category != null)
    		{
    			if (!Category.Translations.Contains(this))
    			{
    				Category.Translations.Add(this);
    			}
    
    			CategoryID = Category.CategoryID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Category")
    				&& (ChangeTracker.OriginalValues["Category"] == Category))
    			{
    				ChangeTracker.OriginalValues.Remove("Category");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Category", previousValue);
    			}
    			if (Category != null && !Category.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Category.StartTracking();
    			}
    		}
    	}
    
    	private void FixupLanguage(Language previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.CategoryTranslations.Contains(this))
    		{
    			previousValue.CategoryTranslations.Remove(this);
    		}
    
    		if (Language != null)
    		{
    			if (!Language.CategoryTranslations.Contains(this))
    			{
    				Language.CategoryTranslations.Add(this);
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
    
    	private void FixupHtmlContent(HtmlContent previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.CategoryTranslations.Contains(this))
    		{
    			previousValue.CategoryTranslations.Remove(this);
    		}
    
    		if (HtmlContent != null)
    		{
    			if (!HtmlContent.CategoryTranslations.Contains(this))
    			{
    				HtmlContent.CategoryTranslations.Add(this);
    			}
    
    			HtmlContentID = HtmlContent.HtmlContentID;
    		}
    		else if (!skipKeys)
    		{
    			HtmlContentID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("HtmlContent")
    				&& (ChangeTracker.OriginalValues["HtmlContent"] == HtmlContent))
    			{
    				ChangeTracker.OriginalValues.Remove("HtmlContent");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("HtmlContent", previousValue);
    			}
    			if (HtmlContent != null && !HtmlContent.ChangeTracker.ChangeTrackingEnabled)
    			{
    				HtmlContent.StartTracking();
    			}
    		}
    	}

        #endregion
    }
}
