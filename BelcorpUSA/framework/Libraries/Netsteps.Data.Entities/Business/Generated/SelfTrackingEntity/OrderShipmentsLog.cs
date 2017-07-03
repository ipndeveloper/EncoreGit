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
    [KnownType(typeof(Order))]
    [Serializable]
    public partial class OrderShipmentsLog: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void OrderShipmentLogIDChanged();
    	public int OrderShipmentLogID
    	{
    		get { return _orderShipmentLogID; }
    		set
    		{
    			if (_orderShipmentLogID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'OrderShipmentLogID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_orderShipmentLogID = value;
    				OrderShipmentLogIDChanged();
    				OnPropertyChanged("OrderShipmentLogID");
    			}
    		}
    	}
    	private int _orderShipmentLogID;
    	partial void LogDateUTCChanged();
    	public System.DateTime LogDateUTC
    	{
    		get { return _logDateUTC; }
    		set
    		{
    			if (_logDateUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("LogDateUTC", _logDateUTC);
    				_logDateUTC = value;
    				LogDateUTCChanged();
    				OnPropertyChanged("LogDateUTC");
    			}
    		}
    	}
    	private System.DateTime _logDateUTC;
    	partial void SucceededChanged();
    	public Nullable<bool> Succeeded
    	{
    		get { return _succeeded; }
    		set
    		{
    			if (_succeeded != value)
    			{
    				ChangeTracker.RecordOriginalValue("Succeeded", _succeeded);
    				_succeeded = value;
    				SucceededChanged();
    				OnPropertyChanged("Succeeded");
    			}
    		}
    	}
    	private Nullable<bool> _succeeded;
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
    	partial void ShipNoChanged();
    	public int ShipNo
    	{
    		get { return _shipNo; }
    		set
    		{
    			if (_shipNo != value)
    			{
    				ChangeTracker.RecordOriginalValue("ShipNo", _shipNo);
    				_shipNo = value;
    				ShipNoChanged();
    				OnPropertyChanged("ShipNo");
    			}
    		}
    	}
    	private int _shipNo;
    	partial void SkuChanged();
    	public string Sku
    	{
    		get { return _sku; }
    		set
    		{
    			if (_sku != value)
    			{
    				ChangeTracker.RecordOriginalValue("Sku", _sku);
    				_sku = value;
    				SkuChanged();
    				OnPropertyChanged("Sku");
    			}
    		}
    	}
    	private string _sku;
    	partial void StatusChanged();
    	public string Status
    	{
    		get { return _status; }
    		set
    		{
    			if (_status != value)
    			{
    				ChangeTracker.RecordOriginalValue("Status", _status);
    				_status = value;
    				StatusChanged();
    				OnPropertyChanged("Status");
    			}
    		}
    	}
    	private string _status;
    	partial void ShippingMethodChanged();
    	public string ShippingMethod
    	{
    		get { return _shippingMethod; }
    		set
    		{
    			if (_shippingMethod != value)
    			{
    				ChangeTracker.RecordOriginalValue("ShippingMethod", _shippingMethod);
    				_shippingMethod = value;
    				ShippingMethodChanged();
    				OnPropertyChanged("ShippingMethod");
    			}
    		}
    	}
    	private string _shippingMethod;
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
    	partial void QuantityChanged();
    	public Nullable<int> Quantity
    	{
    		get { return _quantity; }
    		set
    		{
    			if (_quantity != value)
    			{
    				ChangeTracker.RecordOriginalValue("Quantity", _quantity);
    				_quantity = value;
    				QuantityChanged();
    				OnPropertyChanged("Quantity");
    			}
    		}
    	}
    	private Nullable<int> _quantity;

        #endregion
        #region Navigation Properties
    
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
    		Order = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupOrder(Order previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderShipmentsLogs.Contains(this))
    		{
    			previousValue.OrderShipmentsLogs.Remove(this);
    		}
    
    		if (Order != null)
    		{
    			if (!Order.OrderShipmentsLogs.Contains(this))
    			{
    				Order.OrderShipmentsLogs.Add(this);
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

        #endregion
    }
}