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
    [KnownType(typeof(OrderType))]
    [KnownType(typeof(PaymentGateway))]
    [KnownType(typeof(PaymentType))]
    [Serializable]
    public partial class PaymentOrderType: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void PaymentOrderTypeIDChanged();
    	public int PaymentOrderTypeID
    	{
    		get { return _paymentOrderTypeID; }
    		set
    		{
    			if (_paymentOrderTypeID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'PaymentOrderTypeID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_paymentOrderTypeID = value;
    				PaymentOrderTypeIDChanged();
    				OnPropertyChanged("PaymentOrderTypeID");
    			}
    		}
    	}
    	private int _paymentOrderTypeID;
    	partial void OrderTypeIDChanged();
    	public short OrderTypeID
    	{
    		get { return _orderTypeID; }
    		set
    		{
    			if (_orderTypeID != value)
    			{
    				ChangeTracker.RecordOriginalValue("OrderTypeID", _orderTypeID);
    				if (!IsDeserializing)
    				{
    					if (OrderType != null && OrderType.OrderTypeID != value)
    					{
    						OrderType = null;
    					}
    				}
    				_orderTypeID = value;
    				OrderTypeIDChanged();
    				OnPropertyChanged("OrderTypeID");
    			}
    		}
    	}
    	private short _orderTypeID;
    	partial void PaymentTypeIDChanged();
    	public int PaymentTypeID
    	{
    		get { return _paymentTypeID; }
    		set
    		{
    			if (_paymentTypeID != value)
    			{
    				ChangeTracker.RecordOriginalValue("PaymentTypeID", _paymentTypeID);
    				if (!IsDeserializing)
    				{
    					if (PaymentType != null && PaymentType.PaymentTypeID != value)
    					{
    						PaymentType = null;
    					}
    				}
    				_paymentTypeID = value;
    				PaymentTypeIDChanged();
    				OnPropertyChanged("PaymentTypeID");
    			}
    		}
    	}
    	private int _paymentTypeID;
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
    	partial void PaymentGatewayIDChanged();
    	public short PaymentGatewayID
    	{
    		get { return _paymentGatewayID; }
    		set
    		{
    			if (_paymentGatewayID != value)
    			{
    				ChangeTracker.RecordOriginalValue("PaymentGatewayID", _paymentGatewayID);
    				if (!IsDeserializing)
    				{
    					if (PaymentGateway != null && PaymentGateway.PaymentGatewayID != value)
    					{
    						PaymentGateway = null;
    					}
    				}
    				_paymentGatewayID = value;
    				PaymentGatewayIDChanged();
    				OnPropertyChanged("PaymentGatewayID");
    			}
    		}
    	}
    	private short _paymentGatewayID;
    	partial void PaymentConfigurationIDChanged();
    	public Nullable<int> PaymentConfigurationID
    	{
    		get { return _paymentConfigurationID; }
    		set
    		{
    			if (_paymentConfigurationID != value)
    			{
    				ChangeTracker.RecordOriginalValue("PaymentConfigurationID", _paymentConfigurationID);
    				_paymentConfigurationID = value;
    				PaymentConfigurationIDChanged();
    				OnPropertyChanged("PaymentConfigurationID");
    			}
    		}
    	}
    	private Nullable<int> _paymentConfigurationID;

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
    
    	public OrderType OrderType
    	{
    		get { return _orderType; }
    		set
    		{
    			if (!ReferenceEquals(_orderType, value))
    			{
    				var previousValue = _orderType;
    				_orderType = value;
    				FixupOrderType(previousValue);
    				OnNavigationPropertyChanged("OrderType");
    			}
    		}
    	}
    	private OrderType _orderType;
    
    	public PaymentGateway PaymentGateway
    	{
    		get { return _paymentGateway; }
    		set
    		{
    			if (!ReferenceEquals(_paymentGateway, value))
    			{
    				var previousValue = _paymentGateway;
    				_paymentGateway = value;
    				FixupPaymentGateway(previousValue);
    				OnNavigationPropertyChanged("PaymentGateway");
    			}
    		}
    	}
    	private PaymentGateway _paymentGateway;
    
    	public PaymentType PaymentType
    	{
    		get { return _paymentType; }
    		set
    		{
    			if (!ReferenceEquals(_paymentType, value))
    			{
    				var previousValue = _paymentType;
    				_paymentType = value;
    				FixupPaymentType(previousValue);
    				OnNavigationPropertyChanged("PaymentType");
    			}
    		}
    	}
    	private PaymentType _paymentType;

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
    		OrderType = null;
    		PaymentGateway = null;
    		PaymentType = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupCountry(Country previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.PaymentOrderTypes.Contains(this))
    		{
    			previousValue.PaymentOrderTypes.Remove(this);
    		}
    
    		if (Country != null)
    		{
    			if (!Country.PaymentOrderTypes.Contains(this))
    			{
    				Country.PaymentOrderTypes.Add(this);
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
    
    	private void FixupOrderType(OrderType previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.PaymentOrderTypes.Contains(this))
    		{
    			previousValue.PaymentOrderTypes.Remove(this);
    		}
    
    		if (OrderType != null)
    		{
    			if (!OrderType.PaymentOrderTypes.Contains(this))
    			{
    				OrderType.PaymentOrderTypes.Add(this);
    			}
    
    			OrderTypeID = OrderType.OrderTypeID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("OrderType")
    				&& (ChangeTracker.OriginalValues["OrderType"] == OrderType))
    			{
    				ChangeTracker.OriginalValues.Remove("OrderType");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("OrderType", previousValue);
    			}
    			if (OrderType != null && !OrderType.ChangeTracker.ChangeTrackingEnabled)
    			{
    				OrderType.StartTracking();
    			}
    		}
    	}
    
    	private void FixupPaymentGateway(PaymentGateway previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.PaymentOrderTypes.Contains(this))
    		{
    			previousValue.PaymentOrderTypes.Remove(this);
    		}
    
    		if (PaymentGateway != null)
    		{
    			if (!PaymentGateway.PaymentOrderTypes.Contains(this))
    			{
    				PaymentGateway.PaymentOrderTypes.Add(this);
    			}
    
    			PaymentGatewayID = PaymentGateway.PaymentGatewayID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("PaymentGateway")
    				&& (ChangeTracker.OriginalValues["PaymentGateway"] == PaymentGateway))
    			{
    				ChangeTracker.OriginalValues.Remove("PaymentGateway");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("PaymentGateway", previousValue);
    			}
    			if (PaymentGateway != null && !PaymentGateway.ChangeTracker.ChangeTrackingEnabled)
    			{
    				PaymentGateway.StartTracking();
    			}
    		}
    	}
    
    	private void FixupPaymentType(PaymentType previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.PaymentOrderTypes.Contains(this))
    		{
    			previousValue.PaymentOrderTypes.Remove(this);
    		}
    
    		if (PaymentType != null)
    		{
    			if (!PaymentType.PaymentOrderTypes.Contains(this))
    			{
    				PaymentType.PaymentOrderTypes.Add(this);
    			}
    
    			PaymentTypeID = PaymentType.PaymentTypeID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("PaymentType")
    				&& (ChangeTracker.OriginalValues["PaymentType"] == PaymentType))
    			{
    				ChangeTracker.OriginalValues.Remove("PaymentType");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("PaymentType", previousValue);
    			}
    			if (PaymentType != null && !PaymentType.ChangeTracker.ChangeTrackingEnabled)
    			{
    				PaymentType.StartTracking();
    			}
    		}
    	}

        #endregion
    }
}
