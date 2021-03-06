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
    [Serializable]
    public partial class AccountInfoCache: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void AccountIDChanged();
    	public int AccountID
    	{
    		get { return _accountID; }
    		set
    		{
    			if (_accountID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'AccountID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_accountID = value;
    				AccountIDChanged();
    				OnPropertyChanged("AccountID");
    			}
    		}
    	}
    	private int _accountID;
    	partial void AccountNumberSortableChanged();
    	public string AccountNumberSortable
    	{
    		get { return _accountNumberSortable; }
    		set
    		{
    			if (_accountNumberSortable != value)
    			{
    				ChangeTracker.RecordOriginalValue("AccountNumberSortable", _accountNumberSortable);
    				_accountNumberSortable = value;
    				AccountNumberSortableChanged();
    				OnPropertyChanged("AccountNumberSortable");
    			}
    		}
    	}
    	private string _accountNumberSortable;
    	partial void SponsorAccountNumberChanged();
    	public string SponsorAccountNumber
    	{
    		get { return _sponsorAccountNumber; }
    		set
    		{
    			if (_sponsorAccountNumber != value)
    			{
    				ChangeTracker.RecordOriginalValue("SponsorAccountNumber", _sponsorAccountNumber);
    				_sponsorAccountNumber = value;
    				SponsorAccountNumberChanged();
    				OnPropertyChanged("SponsorAccountNumber");
    			}
    		}
    	}
    	private string _sponsorAccountNumber;
    	partial void SponsorFirstNameChanged();
    	public string SponsorFirstName
    	{
    		get { return _sponsorFirstName; }
    		set
    		{
    			if (_sponsorFirstName != value)
    			{
    				ChangeTracker.RecordOriginalValue("SponsorFirstName", _sponsorFirstName);
    				_sponsorFirstName = value;
    				SponsorFirstNameChanged();
    				OnPropertyChanged("SponsorFirstName");
    			}
    		}
    	}
    	private string _sponsorFirstName;
    	partial void SponsorLastNameChanged();
    	public string SponsorLastName
    	{
    		get { return _sponsorLastName; }
    		set
    		{
    			if (_sponsorLastName != value)
    			{
    				ChangeTracker.RecordOriginalValue("SponsorLastName", _sponsorLastName);
    				_sponsorLastName = value;
    				SponsorLastNameChanged();
    				OnPropertyChanged("SponsorLastName");
    			}
    		}
    	}
    	private string _sponsorLastName;
    	partial void EnrollerAccountNumberChanged();
    	public string EnrollerAccountNumber
    	{
    		get { return _enrollerAccountNumber; }
    		set
    		{
    			if (_enrollerAccountNumber != value)
    			{
    				ChangeTracker.RecordOriginalValue("EnrollerAccountNumber", _enrollerAccountNumber);
    				_enrollerAccountNumber = value;
    				EnrollerAccountNumberChanged();
    				OnPropertyChanged("EnrollerAccountNumber");
    			}
    		}
    	}
    	private string _enrollerAccountNumber;
    	partial void EnrollerFirstNameChanged();
    	public string EnrollerFirstName
    	{
    		get { return _enrollerFirstName; }
    		set
    		{
    			if (_enrollerFirstName != value)
    			{
    				ChangeTracker.RecordOriginalValue("EnrollerFirstName", _enrollerFirstName);
    				_enrollerFirstName = value;
    				EnrollerFirstNameChanged();
    				OnPropertyChanged("EnrollerFirstName");
    			}
    		}
    	}
    	private string _enrollerFirstName;
    	partial void EnrollerLastNameChanged();
    	public string EnrollerLastName
    	{
    		get { return _enrollerLastName; }
    		set
    		{
    			if (_enrollerLastName != value)
    			{
    				ChangeTracker.RecordOriginalValue("EnrollerLastName", _enrollerLastName);
    				_enrollerLastName = value;
    				EnrollerLastNameChanged();
    				OnPropertyChanged("EnrollerLastName");
    			}
    		}
    	}
    	private string _enrollerLastName;
    	partial void Address1Changed();
    	public string Address1
    	{
    		get { return _address1; }
    		set
    		{
    			if (_address1 != value)
    			{
    				ChangeTracker.RecordOriginalValue("Address1", _address1);
    				_address1 = value;
    				Address1Changed();
    				OnPropertyChanged("Address1");
    			}
    		}
    	}
    	private string _address1;
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
    	partial void StateProvinceIDChanged();
    	public Nullable<int> StateProvinceID
    	{
    		get { return _stateProvinceID; }
    		set
    		{
    			if (_stateProvinceID != value)
    			{
    				ChangeTracker.RecordOriginalValue("StateProvinceID", _stateProvinceID);
    				_stateProvinceID = value;
    				StateProvinceIDChanged();
    				OnPropertyChanged("StateProvinceID");
    			}
    		}
    	}
    	private Nullable<int> _stateProvinceID;
    	partial void StateAbbreviationChanged();
    	public string StateAbbreviation
    	{
    		get { return _stateAbbreviation; }
    		set
    		{
    			if (_stateAbbreviation != value)
    			{
    				ChangeTracker.RecordOriginalValue("StateAbbreviation", _stateAbbreviation);
    				_stateAbbreviation = value;
    				StateAbbreviationChanged();
    				OnPropertyChanged("StateAbbreviation");
    			}
    		}
    	}
    	private string _stateAbbreviation;
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
    	partial void CountryIDChanged();
    	public Nullable<int> CountryID
    	{
    		get { return _countryID; }
    		set
    		{
    			if (_countryID != value)
    			{
    				ChangeTracker.RecordOriginalValue("CountryID", _countryID);
    				_countryID = value;
    				CountryIDChanged();
    				OnPropertyChanged("CountryID");
    			}
    		}
    	}
    	private Nullable<int> _countryID;
    	partial void LatitudeChanged();
    	public Nullable<double> Latitude
    	{
    		get { return _latitude; }
    		set
    		{
    			if (_latitude != value)
    			{
    				ChangeTracker.RecordOriginalValue("Latitude", _latitude);
    				_latitude = value;
    				LatitudeChanged();
    				OnPropertyChanged("Latitude");
    			}
    		}
    	}
    	private Nullable<double> _latitude;
    	partial void LongitudeChanged();
    	public Nullable<double> Longitude
    	{
    		get { return _longitude; }
    		set
    		{
    			if (_longitude != value)
    			{
    				ChangeTracker.RecordOriginalValue("Longitude", _longitude);
    				_longitude = value;
    				LongitudeChanged();
    				OnPropertyChanged("Longitude");
    			}
    		}
    	}
    	private Nullable<double> _longitude;
    	partial void PhoneNumberChanged();
    	public string PhoneNumber
    	{
    		get { return _phoneNumber; }
    		set
    		{
    			if (_phoneNumber != value)
    			{
    				ChangeTracker.RecordOriginalValue("PhoneNumber", _phoneNumber);
    				_phoneNumber = value;
    				PhoneNumberChanged();
    				OnPropertyChanged("PhoneNumber");
    			}
    		}
    	}
    	private string _phoneNumber;
    	partial void NextAutoshipRunDateChanged();
    	public Nullable<System.DateTime> NextAutoshipRunDate
    	{
    		get { return _nextAutoshipRunDate; }
    		set
    		{
    			if (_nextAutoshipRunDate != value)
    			{
    				ChangeTracker.RecordOriginalValue("NextAutoshipRunDate", _nextAutoshipRunDate);
    				_nextAutoshipRunDate = value;
    				NextAutoshipRunDateChanged();
    				OnPropertyChanged("NextAutoshipRunDate");
    			}
    		}
    	}
    	private Nullable<System.DateTime> _nextAutoshipRunDate;
    	partial void PwsUrlChanged();
    	public string PwsUrl
    	{
    		get { return _pwsUrl; }
    		set
    		{
    			if (_pwsUrl != value)
    			{
    				ChangeTracker.RecordOriginalValue("PwsUrl", _pwsUrl);
    				_pwsUrl = value;
    				PwsUrlChanged();
    				OnPropertyChanged("PwsUrl");
    			}
    		}
    	}
    	private string _pwsUrl;
    	partial void LastOrderCommissionDateUTCChanged();
    	public Nullable<System.DateTime> LastOrderCommissionDateUTC
    	{
    		get { return _lastOrderCommissionDateUTC; }
    		set
    		{
    			if (_lastOrderCommissionDateUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("LastOrderCommissionDateUTC", _lastOrderCommissionDateUTC);
    				_lastOrderCommissionDateUTC = value;
    				LastOrderCommissionDateUTCChanged();
    				OnPropertyChanged("LastOrderCommissionDateUTC");
    			}
    		}
    	}
    	private Nullable<System.DateTime> _lastOrderCommissionDateUTC;
    	partial void RowHashChanged();
    	public byte[] RowHash
    	{
    		get { return _rowHash; }
    		set
    		{
    			if (_rowHash != value)
    			{
    				ChangeTracker.RecordOriginalValue("RowHash", _rowHash);
    				_rowHash = value;
    				RowHashChanged();
    				OnPropertyChanged("RowHash");
    			}
    		}
    	}
    	private byte[] _rowHash;

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
    	}

        #endregion
    }
}
