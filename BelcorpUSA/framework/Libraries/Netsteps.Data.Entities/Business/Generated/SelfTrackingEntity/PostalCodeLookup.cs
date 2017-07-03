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
    [KnownType(typeof(Country))]
    [Serializable]
    public partial class PostalCodeLookup: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void PostalCodeIDChanged();
    	public int PostalCodeID
    	{
    		get { return _postalCodeID; }
    		set
    		{
    			if (_postalCodeID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'PostalCodeID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_postalCodeID = value;
    				PostalCodeIDChanged();
    				OnPropertyChanged("PostalCodeID");
    			}
    		}
    	}
    	private int _postalCodeID;
    	partial void PostalCodeChanged();
    	public string PostalCode
    	{
    		get { return _postalCode; }
    		set
    		{
    			if (_postalCode != value)
    			{
    				ChangeTracker.RecordOriginalValue("PostalCode", _postalCode);
    				_postalCode = value;
    				PostalCodeChanged();
    				OnPropertyChanged("PostalCode");
    			}
    		}
    	}
    	private string _postalCode;
    	partial void TypeChanged();
    	public string Type
    	{
    		get { return _type; }
    		set
    		{
    			if (_type != value)
    			{
    				ChangeTracker.RecordOriginalValue("Type", _type);
    				_type = value;
    				TypeChanged();
    				OnPropertyChanged("Type");
    			}
    		}
    	}
    	private string _type;
    	partial void CityChanged();
    	public string City
    	{
    		get { return _city; }
    		set
    		{
    			if (_city != value)
    			{
    				ChangeTracker.RecordOriginalValue("City", _city);
    				_city = value;
    				CityChanged();
    				OnPropertyChanged("City");
    			}
    		}
    	}
    	private string _city;
    	partial void StateProvinceChanged();
    	public string StateProvince
    	{
    		get { return _stateProvince; }
    		set
    		{
    			if (_stateProvince != value)
    			{
    				ChangeTracker.RecordOriginalValue("StateProvince", _stateProvince);
    				_stateProvince = value;
    				StateProvinceChanged();
    				OnPropertyChanged("StateProvince");
    			}
    		}
    	}
    	private string _stateProvince;
    	partial void StateProvinceAbbreviationChanged();
    	public string StateProvinceAbbreviation
    	{
    		get { return _stateProvinceAbbreviation; }
    		set
    		{
    			if (_stateProvinceAbbreviation != value)
    			{
    				ChangeTracker.RecordOriginalValue("StateProvinceAbbreviation", _stateProvinceAbbreviation);
    				_stateProvinceAbbreviation = value;
    				StateProvinceAbbreviationChanged();
    				OnPropertyChanged("StateProvinceAbbreviation");
    			}
    		}
    	}
    	private string _stateProvinceAbbreviation;
    	partial void CountyChanged();
    	public string County
    	{
    		get { return _county; }
    		set
    		{
    			if (_county != value)
    			{
    				ChangeTracker.RecordOriginalValue("County", _county);
    				_county = value;
    				CountyChanged();
    				OnPropertyChanged("County");
    			}
    		}
    	}
    	private string _county;
    	partial void CountryIDChanged();
    	public int CountryID
    	{
    		get { return _countryID; }
    		set
    		{
    			if (_countryID != value)
    			{
    				ChangeTracker.RecordOriginalValue("CountryID", _countryID);
    				if (!IsDeserializing)
    				{
    					if (Country != null && Country.CountryID != value)
    					{
    						Country = null;
    					}
    				}
    				_countryID = value;
    				CountryIDChanged();
    				OnPropertyChanged("CountryID");
    			}
    		}
    	}
    	private int _countryID;
    	partial void DateCreatedUTCChanged();
    	public System.DateTime DateCreatedUTC
    	{
    		get { return _dateCreatedUTC; }
    		set
    		{
    			if (_dateCreatedUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("DateCreatedUTC", _dateCreatedUTC);
    				_dateCreatedUTC = value;
    				DateCreatedUTCChanged();
    				OnPropertyChanged("DateCreatedUTC");
    			}
    		}
    	}
    	private System.DateTime _dateCreatedUTC;
    	partial void DateLastModifiedUTCChanged();
    	public Nullable<System.DateTime> DateLastModifiedUTC
    	{
    		get { return _dateLastModifiedUTC; }
    		set
    		{
    			if (_dateLastModifiedUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("DateLastModifiedUTC", _dateLastModifiedUTC);
    				_dateLastModifiedUTC = value;
    				DateLastModifiedUTCChanged();
    				OnPropertyChanged("DateLastModifiedUTC");
    			}
    		}
    	}
    	private Nullable<System.DateTime> _dateLastModifiedUTC;

        #endregion
        #region Navigation Properties
    
    	public Country Country
    	{
    		get { return _country; }
    		set
    		{
    			if (!ReferenceEquals(_country, value))
    			{
    				var previousValue = _country;
    				_country = value;
    				FixupCountry(previousValue);
    				OnNavigationPropertyChanged("Country");
    			}
    		}
    	}
    	private Country _country;

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
    		Country = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupCountry(Country previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.PostalCodeLookups.Contains(this))
    		{
    			previousValue.PostalCodeLookups.Remove(this);
    		}
    
    		if (Country != null)
    		{
    			if (!Country.PostalCodeLookups.Contains(this))
    			{
    				Country.PostalCodeLookups.Add(this);
    			}
    
    			CountryID = Country.CountryID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Country")
    				&& (ChangeTracker.OriginalValues["Country"] == Country))
    			{
    				ChangeTracker.OriginalValues.Remove("Country");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Country", previousValue);
    			}
    			if (Country != null && !Country.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Country.StartTracking();
    			}
    		}
    	}

        #endregion
    }
}
