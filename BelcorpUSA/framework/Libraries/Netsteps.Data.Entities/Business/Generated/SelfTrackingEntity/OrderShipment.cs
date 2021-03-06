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
    [KnownType(typeof(OrderCustomer))]
    [KnownType(typeof(ShippingMethod))]
    [KnownType(typeof(StateProvince))]
    [KnownType(typeof(OrderShipmentStatus))]
    [KnownType(typeof(User))]
    [KnownType(typeof(Order))]
    [KnownType(typeof(OrderShipmentPackage))]
    [KnownType(typeof(LogisticsProvider))]
    [Serializable]
    public partial class OrderShipment: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void OrderShipmentIDChanged();
    	public int OrderShipmentID
    	{
    		get { return _orderShipmentID; }
    		set
    		{
    			if (_orderShipmentID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'OrderShipmentID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_orderShipmentID = value;
    				OrderShipmentIDChanged();
    				OnPropertyChanged("OrderShipmentID");
    			}
    		}
    	}
    	private int _orderShipmentID;
    	partial void OrderIDChanged();
    	public int OrderID
    	{
    		get { return _orderID; }
    		set
    		{
    			if (_orderID != value)
    			{
    				ChangeTracker.RecordOriginalValue("OrderID", _orderID);
    				if (!IsDeserializing)
    				{
    					if (Order != null && Order.OrderID != value)
    					{
    						Order = null;
    					}
    				}
    				_orderID = value;
    				OrderIDChanged();
    				OnPropertyChanged("OrderID");
    			}
    		}
    	}
    	private int _orderID;
    	partial void OrderCustomerIDChanged();
    	public Nullable<int> OrderCustomerID
    	{
    		get { return _orderCustomerID; }
    		set
    		{
    			if (_orderCustomerID != value)
    			{
    				ChangeTracker.RecordOriginalValue("OrderCustomerID", _orderCustomerID);
    				if (!IsDeserializing)
    				{
    					if (OrderCustomer != null && OrderCustomer.OrderCustomerID != value)
    					{
    						OrderCustomer = null;
    					}
    				}
    				_orderCustomerID = value;
    				OrderCustomerIDChanged();
    				OnPropertyChanged("OrderCustomerID");
    			}
    		}
    	}
    	private Nullable<int> _orderCustomerID;
    	partial void ShippingMethodIDChanged();
    	public Nullable<int> ShippingMethodID
    	{
    		get { return _shippingMethodID; }
    		set
    		{
    			if (_shippingMethodID != value)
    			{
    				ChangeTracker.RecordOriginalValue("ShippingMethodID", _shippingMethodID);
    				if (!IsDeserializing)
    				{
    					if (ShippingMethod != null && ShippingMethod.ShippingMethodID != value)
    					{
    						ShippingMethod = null;
    					}
    				}
    				_shippingMethodID = value;
    				ShippingMethodIDChanged();
    				OnPropertyChanged("ShippingMethodID");
    			}
    		}
    	}
    	private Nullable<int> _shippingMethodID;
    	partial void OrderShipmentStatusIDChanged();
    	public short OrderShipmentStatusID
    	{
    		get { return _orderShipmentStatusID; }
    		set
    		{
    			if (_orderShipmentStatusID != value)
    			{
    				ChangeTracker.RecordOriginalValue("OrderShipmentStatusID", _orderShipmentStatusID);
    				if (!IsDeserializing)
    				{
    					if (OrderShipmentStatus != null && OrderShipmentStatus.OrderShipmentStatusID != value)
    					{
    						OrderShipmentStatus = null;
    					}
    				}
    				_orderShipmentStatusID = value;
    				OrderShipmentStatusIDChanged();
    				OnPropertyChanged("OrderShipmentStatusID");
    			}
    		}
    	}
    	private short _orderShipmentStatusID;
    	partial void FirstNameChanged();
    	public string FirstName
    	{
    		get { return _firstName; }
    		set
    		{
    			if (_firstName != value)
    			{
    				ChangeTracker.RecordOriginalValue("FirstName", _firstName);
    				_firstName = value;
    				FirstNameChanged();
    				OnPropertyChanged("FirstName");
    			}
    		}
    	}
    	private string _firstName;
    	partial void LastNameChanged();
    	public string LastName
    	{
    		get { return _lastName; }
    		set
    		{
    			if (_lastName != value)
    			{
    				ChangeTracker.RecordOriginalValue("LastName", _lastName);
    				_lastName = value;
    				LastNameChanged();
    				OnPropertyChanged("LastName");
    			}
    		}
    	}
    	private string _lastName;
    	partial void AttentionChanged();
    	public string Attention
    	{
    		get { return _attention; }
    		set
    		{
    			if (_attention != value)
    			{
    				ChangeTracker.RecordOriginalValue("Attention", _attention);
    				_attention = value;
    				AttentionChanged();
    				OnPropertyChanged("Attention");
    			}
    		}
    	}
    	private string _attention;
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
    	partial void Address2Changed();
    	public string Address2
    	{
    		get { return _address2; }
    		set
    		{
    			if (_address2 != value)
    			{
    				ChangeTracker.RecordOriginalValue("Address2", _address2);
    				_address2 = value;
    				Address2Changed();
    				OnPropertyChanged("Address2");
    			}
    		}
    	}
    	private string _address2;
    	partial void Address3Changed();
    	public string Address3
    	{
    		get { return _address3; }
    		set
    		{
    			if (_address3 != value)
    			{
    				ChangeTracker.RecordOriginalValue("Address3", _address3);
    				_address3 = value;
    				Address3Changed();
    				OnPropertyChanged("Address3");
    			}
    		}
    	}
    	private string _address3;
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
    	partial void StateChanged();
    	public string State
    	{
    		get { return _state; }
    		set
    		{
    			if (_state != value)
    			{
    				ChangeTracker.RecordOriginalValue("State", _state);
    				_state = value;
    				StateChanged();
    				OnPropertyChanged("State");
    			}
    		}
    	}
    	private string _state;
    	partial void StateProvinceIDChanged();
    	public Nullable<int> StateProvinceID
    	{
    		get { return _stateProvinceID; }
    		set
    		{
    			if (_stateProvinceID != value)
    			{
    				ChangeTracker.RecordOriginalValue("StateProvinceID", _stateProvinceID);
    				if (!IsDeserializing)
    				{
    					if (StateProvince != null && StateProvince.StateProvinceID != value)
    					{
    						StateProvince = null;
    					}
    				}
    				_stateProvinceID = value;
    				StateProvinceIDChanged();
    				OnPropertyChanged("StateProvinceID");
    			}
    		}
    	}
    	private Nullable<int> _stateProvinceID;
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
    	partial void EmailChanged();
    	public string Email
    	{
    		get { return _email; }
    		set
    		{
    			if (_email != value)
    			{
    				ChangeTracker.RecordOriginalValue("Email", _email);
    				_email = value;
    				EmailChanged();
    				OnPropertyChanged("Email");
    			}
    		}
    	}
    	private string _email;
    	partial void DayPhoneChanged();
    	public string DayPhone
    	{
    		get { return _dayPhone; }
    		set
    		{
    			if (_dayPhone != value)
    			{
    				ChangeTracker.RecordOriginalValue("DayPhone", _dayPhone);
    				_dayPhone = value;
    				DayPhoneChanged();
    				OnPropertyChanged("DayPhone");
    			}
    		}
    	}
    	private string _dayPhone;
    	partial void EveningPhoneChanged();
    	public string EveningPhone
    	{
    		get { return _eveningPhone; }
    		set
    		{
    			if (_eveningPhone != value)
    			{
    				ChangeTracker.RecordOriginalValue("EveningPhone", _eveningPhone);
    				_eveningPhone = value;
    				EveningPhoneChanged();
    				OnPropertyChanged("EveningPhone");
    			}
    		}
    	}
    	private string _eveningPhone;
    	partial void TrackingNumberChanged();
    	public string TrackingNumber
    	{
    		get { return _trackingNumber; }
    		set
    		{
    			if (_trackingNumber != value)
    			{
    				ChangeTracker.RecordOriginalValue("TrackingNumber", _trackingNumber);
    				_trackingNumber = value;
    				TrackingNumberChanged();
    				OnPropertyChanged("TrackingNumber");
    			}
    		}
    	}
    	private string _trackingNumber;
    	partial void TrackingURLChanged();
    	public string TrackingURL
    	{
    		get { return _trackingURL; }
    		set
    		{
    			if (_trackingURL != value)
    			{
    				ChangeTracker.RecordOriginalValue("TrackingURL", _trackingURL);
    				_trackingURL = value;
    				TrackingURLChanged();
    				OnPropertyChanged("TrackingURL");
    			}
    		}
    	}
    	private string _trackingURL;
    	partial void DateShippedUTCChanged();
    	public Nullable<System.DateTime> DateShippedUTC
    	{
    		get { return _dateShippedUTC; }
    		set
    		{
    			if (_dateShippedUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("DateShippedUTC", _dateShippedUTC);
    				_dateShippedUTC = value;
    				DateShippedUTCChanged();
    				OnPropertyChanged("DateShippedUTC");
    			}
    		}
    	}
    	private Nullable<System.DateTime> _dateShippedUTC;
    	partial void GovernmentReceiptNumberChanged();
    	public string GovernmentReceiptNumber
    	{
    		get { return _governmentReceiptNumber; }
    		set
    		{
    			if (_governmentReceiptNumber != value)
    			{
    				ChangeTracker.RecordOriginalValue("GovernmentReceiptNumber", _governmentReceiptNumber);
    				_governmentReceiptNumber = value;
    				GovernmentReceiptNumberChanged();
    				OnPropertyChanged("GovernmentReceiptNumber");
    			}
    		}
    	}
    	private string _governmentReceiptNumber;
    	partial void IsDirectShipmentChanged();
    	public bool IsDirectShipment
    	{
    		get { return _isDirectShipment; }
    		set
    		{
    			if (_isDirectShipment != value)
    			{
    				ChangeTracker.RecordOriginalValue("IsDirectShipment", _isDirectShipment);
    				_isDirectShipment = value;
    				IsDirectShipmentChanged();
    				OnPropertyChanged("IsDirectShipment");
    			}
    		}
    	}
    	private bool _isDirectShipment;
    	partial void IsWillCallChanged();
    	public bool IsWillCall
    	{
    		get { return _isWillCall; }
    		set
    		{
    			if (_isWillCall != value)
    			{
    				ChangeTracker.RecordOriginalValue("IsWillCall", _isWillCall);
    				_isWillCall = value;
    				IsWillCallChanged();
    				OnPropertyChanged("IsWillCall");
    			}
    		}
    	}
    	private bool _isWillCall;
    	partial void DataVersionChanged();
    	public byte[] DataVersion
    	{
    		get { return _dataVersion; }
    		set
    		{
    			if (_dataVersion != value)
    			{
    				ChangeTracker.RecordOriginalValue("DataVersion", _dataVersion);
    				_dataVersion = value;
    				DataVersionChanged();
    				OnPropertyChanged("DataVersion");
    			}
    		}
    	}
    	private byte[] _dataVersion;
    	partial void ModifiedByUserIDChanged();
    	public Nullable<int> ModifiedByUserID
    	{
    		get { return _modifiedByUserID; }
    		set
    		{
    			if (_modifiedByUserID != value)
    			{
    				ChangeTracker.RecordOriginalValue("ModifiedByUserID", _modifiedByUserID);
    				if (!IsDeserializing)
    				{
    					if (User != null && User.UserID != value)
    					{
    						User = null;
    					}
    				}
    				_modifiedByUserID = value;
    				ModifiedByUserIDChanged();
    				OnPropertyChanged("ModifiedByUserID");
    			}
    		}
    	}
    	private Nullable<int> _modifiedByUserID;
    	partial void SourceAddressIDChanged();
    	public Nullable<int> SourceAddressID
    	{
    		get { return _sourceAddressID; }
    		set
    		{
    			if (_sourceAddressID != value)
    			{
    				ChangeTracker.RecordOriginalValue("SourceAddressID", _sourceAddressID);
    				_sourceAddressID = value;
    				SourceAddressIDChanged();
    				OnPropertyChanged("SourceAddressID");
    			}
    		}
    	}
    	private Nullable<int> _sourceAddressID;
    	partial void PickupPointCodeChanged();
    	public string PickupPointCode
    	{
    		get { return _pickupPointCode; }
    		set
    		{
    			if (_pickupPointCode != value)
    			{
    				ChangeTracker.RecordOriginalValue("PickupPointCode", _pickupPointCode);
    				_pickupPointCode = value;
    				PickupPointCodeChanged();
    				OnPropertyChanged("PickupPointCode");
    			}
    		}
    	}
    	private string _pickupPointCode;
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
    	public System.DateTime DateLastModifiedUTC
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
    	private System.DateTime _dateLastModifiedUTC;
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
    	partial void StreetChanged();
    	public string Street
    	{
    		get { return _street; }
    		set
    		{
    			if (_street != value)
    			{
    				ChangeTracker.RecordOriginalValue("Street", _street);
    				_street = value;
    				StreetChanged();
    				OnPropertyChanged("Street");
    			}
    		}
    	}
    	private string _street;
    	partial void LogisticsProviderIDChanged();
    	public Nullable<short> LogisticsProviderID
    	{
    		get { return _logisticsProviderID; }
    		set
    		{
    			if (_logisticsProviderID != value)
    			{
    				ChangeTracker.RecordOriginalValue("LogisticsProviderID", _logisticsProviderID);
    				if (!IsDeserializing)
    				{
    					if (LogisticsProvider != null && LogisticsProvider.LogisticsProviderID != value)
    					{
    						LogisticsProvider = null;
    					}
    				}
    				_logisticsProviderID = value;
    				LogisticsProviderIDChanged();
    				OnPropertyChanged("LogisticsProviderID");
    			}
    		}
    	}
    	private Nullable<short> _logisticsProviderID;
    	partial void EstimatedDateUTCChanged();
    	public Nullable<System.DateTime> EstimatedDateUTC
    	{
    		get { return _estimatedDateUTC; }
    		set
    		{
    			if (_estimatedDateUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("EstimatedDateUTC", _estimatedDateUTC);
    				_estimatedDateUTC = value;
    				EstimatedDateUTCChanged();
    				OnPropertyChanged("EstimatedDateUTC");
    			}
    		}
    	}
    	private Nullable<System.DateTime> _estimatedDateUTC;
    	partial void DeliveryDateUTCChanged();
    	public Nullable<System.DateTime> DeliveryDateUTC
    	{
    		get { return _deliveryDateUTC; }
    		set
    		{
    			if (_deliveryDateUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("DeliveryDateUTC", _deliveryDateUTC);
    				_deliveryDateUTC = value;
    				DeliveryDateUTCChanged();
    				OnPropertyChanged("DeliveryDateUTC");
    			}
    		}
    	}
    	private Nullable<System.DateTime> _deliveryDateUTC;
    	partial void WareHouseIDChanged();
    	public Nullable<int> WareHouseID
    	{
    		get { return _wareHouseID; }
    		set
    		{
    			if (_wareHouseID != value)
    			{
    				ChangeTracker.RecordOriginalValue("WareHouseID", _wareHouseID);
    				_wareHouseID = value;
    				WareHouseIDChanged();
    				OnPropertyChanged("WareHouseID");
    			}
    		}
    	}
    	private Nullable<int> _wareHouseID;
    	partial void ShippingOrderTypeIDChanged();
    	public Nullable<int> ShippingOrderTypeID
    	{
    		get { return _shippingOrderTypeID; }
    		set
    		{
    			if (_shippingOrderTypeID != value)
    			{
    				ChangeTracker.RecordOriginalValue("ShippingOrderTypeID", _shippingOrderTypeID);
    				_shippingOrderTypeID = value;
    				ShippingOrderTypeIDChanged();
    				OnPropertyChanged("ShippingOrderTypeID");
    			}
    		}
    	}
    	private Nullable<int> _shippingOrderTypeID;

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
    
    	public OrderCustomer OrderCustomer
    	{
    		get { return _orderCustomer; }
    		set
    		{
    			if (!ReferenceEquals(_orderCustomer, value))
    			{
    				var previousValue = _orderCustomer;
    				_orderCustomer = value;
    				FixupOrderCustomer(previousValue);
    				OnNavigationPropertyChanged("OrderCustomer");
    			}
    		}
    	}
    	private OrderCustomer _orderCustomer;
    
    	public ShippingMethod ShippingMethod
    	{
    		get { return _shippingMethod; }
    		set
    		{
    			if (!ReferenceEquals(_shippingMethod, value))
    			{
    				var previousValue = _shippingMethod;
    				_shippingMethod = value;
    				FixupShippingMethod(previousValue);
    				OnNavigationPropertyChanged("ShippingMethod");
    			}
    		}
    	}
    	private ShippingMethod _shippingMethod;
    
    	public StateProvince StateProvince
    	{
    		get { return _stateProvince; }
    		set
    		{
    			if (!ReferenceEquals(_stateProvince, value))
    			{
    				var previousValue = _stateProvince;
    				_stateProvince = value;
    				FixupStateProvince(previousValue);
    				OnNavigationPropertyChanged("StateProvince");
    			}
    		}
    	}
    	private StateProvince _stateProvince;
    
    	public OrderShipmentStatus OrderShipmentStatus
    	{
    		get { return _orderShipmentStatus; }
    		set
    		{
    			if (!ReferenceEquals(_orderShipmentStatus, value))
    			{
    				var previousValue = _orderShipmentStatus;
    				_orderShipmentStatus = value;
    				FixupOrderShipmentStatus(previousValue);
    				OnNavigationPropertyChanged("OrderShipmentStatus");
    			}
    		}
    	}
    	private OrderShipmentStatus _orderShipmentStatus;
    
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
    
    	public Order Order
    	{
    		get { return _order; }
    		set
    		{
    			if (!ReferenceEquals(_order, value))
    			{
    				var previousValue = _order;
    				_order = value;
    				FixupOrder(previousValue);
    				OnNavigationPropertyChanged("Order");
    			}
    		}
    	}
    	private Order _order;
    
    	public TrackableCollection<OrderShipmentPackage> OrderShipmentPackages
    	{
    		get
    		{
    			if (_orderShipmentPackages == null)
    			{
    				_orderShipmentPackages = new TrackableCollection<OrderShipmentPackage>();
    				_orderShipmentPackages.CollectionChanged += FixupOrderShipmentPackages;
    				_orderShipmentPackages.CollectionChanged += RaiseOrderShipmentPackagesChanged;
    			}
    			return _orderShipmentPackages;
    		}
    		set
    		{
    			if (!ReferenceEquals(_orderShipmentPackages, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_orderShipmentPackages != null)
    				{
    					_orderShipmentPackages.CollectionChanged -= FixupOrderShipmentPackages;
    					_orderShipmentPackages.CollectionChanged -= RaiseOrderShipmentPackagesChanged;
    				}
    				_orderShipmentPackages = value;
    				if (_orderShipmentPackages != null)
    				{
    					_orderShipmentPackages.CollectionChanged += FixupOrderShipmentPackages;
    					_orderShipmentPackages.CollectionChanged += RaiseOrderShipmentPackagesChanged;
    				}
    				OnNavigationPropertyChanged("OrderShipmentPackages");
    			}
    		}
    	}
    	private TrackableCollection<OrderShipmentPackage> _orderShipmentPackages;
    	partial void OrderShipmentPackagesChanged();
    	private void RaiseOrderShipmentPackagesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		OrderShipmentPackagesChanged();
    	}
    
    	public LogisticsProvider LogisticsProvider
    	{
    		get { return _logisticsProvider; }
    		set
    		{
    			if (!ReferenceEquals(_logisticsProvider, value))
    			{
    				var previousValue = _logisticsProvider;
    				_logisticsProvider = value;
    				FixupLogisticsProvider(previousValue);
    				OnNavigationPropertyChanged("LogisticsProvider");
    			}
    		}
    	}
    	private LogisticsProvider _logisticsProvider;

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
    		if (_orderShipmentPackages != null)
    		{
    			_orderShipmentPackages.CollectionChanged -= FixupOrderShipmentPackages;
    			_orderShipmentPackages.CollectionChanged -= RaiseOrderShipmentPackagesChanged;
    			_orderShipmentPackages.CollectionChanged += FixupOrderShipmentPackages;
    			_orderShipmentPackages.CollectionChanged += RaiseOrderShipmentPackagesChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		Country = null;
    		OrderCustomer = null;
    		ShippingMethod = null;
    		StateProvince = null;
    		OrderShipmentStatus = null;
    		User = null;
    		Order = null;
    		OrderShipmentPackages.Clear();
    		LogisticsProvider = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupCountry(Country previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderShipments.Contains(this))
    		{
    			previousValue.OrderShipments.Remove(this);
    		}
    
    		if (Country != null)
    		{
    			if (!Country.OrderShipments.Contains(this))
    			{
    				Country.OrderShipments.Add(this);
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
    
    	private void FixupOrderCustomer(OrderCustomer previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderShipments.Contains(this))
    		{
    			previousValue.OrderShipments.Remove(this);
    		}
    
    		if (OrderCustomer != null)
    		{
    			if (!OrderCustomer.OrderShipments.Contains(this))
    			{
    				OrderCustomer.OrderShipments.Add(this);
    			}
    
    			OrderCustomerID = OrderCustomer.OrderCustomerID;
    		}
    		else if (!skipKeys)
    		{
    			OrderCustomerID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("OrderCustomer")
    				&& (ChangeTracker.OriginalValues["OrderCustomer"] == OrderCustomer))
    			{
    				ChangeTracker.OriginalValues.Remove("OrderCustomer");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("OrderCustomer", previousValue);
    			}
    			if (OrderCustomer != null && !OrderCustomer.ChangeTracker.ChangeTrackingEnabled)
    			{
    				OrderCustomer.StartTracking();
    			}
    		}
    	}
    
    	private void FixupShippingMethod(ShippingMethod previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderShipments.Contains(this))
    		{
    			previousValue.OrderShipments.Remove(this);
    		}
    
    		if (ShippingMethod != null)
    		{
    			if (!ShippingMethod.OrderShipments.Contains(this))
    			{
    				ShippingMethod.OrderShipments.Add(this);
    			}
    
    			ShippingMethodID = ShippingMethod.ShippingMethodID;
    		}
    		else if (!skipKeys)
    		{
    			ShippingMethodID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("ShippingMethod")
    				&& (ChangeTracker.OriginalValues["ShippingMethod"] == ShippingMethod))
    			{
    				ChangeTracker.OriginalValues.Remove("ShippingMethod");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("ShippingMethod", previousValue);
    			}
    			if (ShippingMethod != null && !ShippingMethod.ChangeTracker.ChangeTrackingEnabled)
    			{
    				ShippingMethod.StartTracking();
    			}
    		}
    	}
    
    	private void FixupStateProvince(StateProvince previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderShipments.Contains(this))
    		{
    			previousValue.OrderShipments.Remove(this);
    		}
    
    		if (StateProvince != null)
    		{
    			if (!StateProvince.OrderShipments.Contains(this))
    			{
    				StateProvince.OrderShipments.Add(this);
    			}
    
    			StateProvinceID = StateProvince.StateProvinceID;
    		}
    		else if (!skipKeys)
    		{
    			StateProvinceID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("StateProvince")
    				&& (ChangeTracker.OriginalValues["StateProvince"] == StateProvince))
    			{
    				ChangeTracker.OriginalValues.Remove("StateProvince");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("StateProvince", previousValue);
    			}
    			if (StateProvince != null && !StateProvince.ChangeTracker.ChangeTrackingEnabled)
    			{
    				StateProvince.StartTracking();
    			}
    		}
    	}
    
    	private void FixupOrderShipmentStatus(OrderShipmentStatus previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderShipments.Contains(this))
    		{
    			previousValue.OrderShipments.Remove(this);
    		}
    
    		if (OrderShipmentStatus != null)
    		{
    			if (!OrderShipmentStatus.OrderShipments.Contains(this))
    			{
    				OrderShipmentStatus.OrderShipments.Add(this);
    			}
    
    			OrderShipmentStatusID = OrderShipmentStatus.OrderShipmentStatusID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("OrderShipmentStatus")
    				&& (ChangeTracker.OriginalValues["OrderShipmentStatus"] == OrderShipmentStatus))
    			{
    				ChangeTracker.OriginalValues.Remove("OrderShipmentStatus");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("OrderShipmentStatus", previousValue);
    			}
    			if (OrderShipmentStatus != null && !OrderShipmentStatus.ChangeTracker.ChangeTrackingEnabled)
    			{
    				OrderShipmentStatus.StartTracking();
    			}
    		}
    	}
    
    	private void FixupUser(User previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderShipments.Contains(this))
    		{
    			previousValue.OrderShipments.Remove(this);
    		}
    
    		if (User != null)
    		{
    			if (!User.OrderShipments.Contains(this))
    			{
    				User.OrderShipments.Add(this);
    			}
    
    			ModifiedByUserID = User.UserID;
    		}
    		else if (!skipKeys)
    		{
    			ModifiedByUserID = null;
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
    
    	private void FixupOrder(Order previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderShipments.Contains(this))
    		{
    			previousValue.OrderShipments.Remove(this);
    		}
    
    		if (Order != null)
    		{
    			if (!Order.OrderShipments.Contains(this))
    			{
    				Order.OrderShipments.Add(this);
    			}
    
    			OrderID = Order.OrderID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Order")
    				&& (ChangeTracker.OriginalValues["Order"] == Order))
    			{
    				ChangeTracker.OriginalValues.Remove("Order");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Order", previousValue);
    			}
    			if (Order != null && !Order.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Order.StartTracking();
    			}
    		}
    	}
    
    	private void FixupLogisticsProvider(LogisticsProvider previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderShipments.Contains(this))
    		{
    			previousValue.OrderShipments.Remove(this);
    		}
    
    		if (LogisticsProvider != null)
    		{
    			if (!LogisticsProvider.OrderShipments.Contains(this))
    			{
    				LogisticsProvider.OrderShipments.Add(this);
    			}
    
    			LogisticsProviderID = LogisticsProvider.LogisticsProviderID;
    		}
    		else if (!skipKeys)
    		{
    			LogisticsProviderID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("LogisticsProvider")
    				&& (ChangeTracker.OriginalValues["LogisticsProvider"] == LogisticsProvider))
    			{
    				ChangeTracker.OriginalValues.Remove("LogisticsProvider");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("LogisticsProvider", previousValue);
    			}
    			if (LogisticsProvider != null && !LogisticsProvider.ChangeTracker.ChangeTrackingEnabled)
    			{
    				LogisticsProvider.StartTracking();
    			}
    		}
    	}
    
    	private void FixupOrderShipmentPackages(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (OrderShipmentPackage item in e.NewItems)
    			{
    				item.OrderShipment = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("OrderShipmentPackages", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (OrderShipmentPackage item in e.OldItems)
    			{
    				if (ReferenceEquals(item.OrderShipment, this))
    				{
    					item.OrderShipment = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("OrderShipmentPackages", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}
