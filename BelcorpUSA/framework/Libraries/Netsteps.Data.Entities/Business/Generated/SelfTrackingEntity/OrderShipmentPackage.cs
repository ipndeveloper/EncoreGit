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
    [KnownType(typeof(OrderShipmentPackageItem))]
    [KnownType(typeof(OrderShipment))]
    [KnownType(typeof(ShippingMethod))]
    [Serializable]
    public partial class OrderShipmentPackage: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void OrderShipmentPackageIDChanged();
    	public int OrderShipmentPackageID
    	{
    		get { return _orderShipmentPackageID; }
    		set
    		{
    			if (_orderShipmentPackageID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'OrderShipmentPackageID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_orderShipmentPackageID = value;
    				OrderShipmentPackageIDChanged();
    				OnPropertyChanged("OrderShipmentPackageID");
    			}
    		}
    	}
    	private int _orderShipmentPackageID;
    	partial void OrderShipmentIDChanged();
    	public int OrderShipmentID
    	{
    		get { return _orderShipmentID; }
    		set
    		{
    			if (_orderShipmentID != value)
    			{
    				ChangeTracker.RecordOriginalValue("OrderShipmentID", _orderShipmentID);
    				if (!IsDeserializing)
    				{
    					if (OrderShipment != null && OrderShipment.OrderShipmentID != value)
    					{
    						OrderShipment = null;
    					}
    				}
    				_orderShipmentID = value;
    				OrderShipmentIDChanged();
    				OnPropertyChanged("OrderShipmentID");
    			}
    		}
    	}
    	private int _orderShipmentID;
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
    	partial void DateShippedUTCChanged();
    	public System.DateTime DateShippedUTC
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
    	private System.DateTime _dateShippedUTC;
    	partial void TrackingUrlChanged();
    	public string TrackingUrl
    	{
    		get { return _trackingUrl; }
    		set
    		{
    			if (_trackingUrl != value)
    			{
    				ChangeTracker.RecordOriginalValue("TrackingUrl", _trackingUrl);
    				_trackingUrl = value;
    				TrackingUrlChanged();
    				OnPropertyChanged("TrackingUrl");
    			}
    		}
    	}
    	private string _trackingUrl;
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

        #endregion
        #region Navigation Properties
    
    	public TrackableCollection<OrderShipmentPackageItem> OrderShipmentPackageItems
    	{
    		get
    		{
    			if (_orderShipmentPackageItems == null)
    			{
    				_orderShipmentPackageItems = new TrackableCollection<OrderShipmentPackageItem>();
    				_orderShipmentPackageItems.CollectionChanged += FixupOrderShipmentPackageItems;
    				_orderShipmentPackageItems.CollectionChanged += RaiseOrderShipmentPackageItemsChanged;
    			}
    			return _orderShipmentPackageItems;
    		}
    		set
    		{
    			if (!ReferenceEquals(_orderShipmentPackageItems, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_orderShipmentPackageItems != null)
    				{
    					_orderShipmentPackageItems.CollectionChanged -= FixupOrderShipmentPackageItems;
    					_orderShipmentPackageItems.CollectionChanged -= RaiseOrderShipmentPackageItemsChanged;
    				}
    				_orderShipmentPackageItems = value;
    				if (_orderShipmentPackageItems != null)
    				{
    					_orderShipmentPackageItems.CollectionChanged += FixupOrderShipmentPackageItems;
    					_orderShipmentPackageItems.CollectionChanged += RaiseOrderShipmentPackageItemsChanged;
    				}
    				OnNavigationPropertyChanged("OrderShipmentPackageItems");
    			}
    		}
    	}
    	private TrackableCollection<OrderShipmentPackageItem> _orderShipmentPackageItems;
    	partial void OrderShipmentPackageItemsChanged();
    	private void RaiseOrderShipmentPackageItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		OrderShipmentPackageItemsChanged();
    	}
    
    	public OrderShipment OrderShipment
    	{
    		get { return _orderShipment; }
    		set
    		{
    			if (!ReferenceEquals(_orderShipment, value))
    			{
    				var previousValue = _orderShipment;
    				_orderShipment = value;
    				FixupOrderShipment(previousValue);
    				OnNavigationPropertyChanged("OrderShipment");
    			}
    		}
    	}
    	private OrderShipment _orderShipment;
    
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
    		if (_orderShipmentPackageItems != null)
    		{
    			_orderShipmentPackageItems.CollectionChanged -= FixupOrderShipmentPackageItems;
    			_orderShipmentPackageItems.CollectionChanged -= RaiseOrderShipmentPackageItemsChanged;
    			_orderShipmentPackageItems.CollectionChanged += FixupOrderShipmentPackageItems;
    			_orderShipmentPackageItems.CollectionChanged += RaiseOrderShipmentPackageItemsChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		OrderShipmentPackageItems.Clear();
    		OrderShipment = null;
    		ShippingMethod = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupOrderShipment(OrderShipment previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderShipmentPackages.Contains(this))
    		{
    			previousValue.OrderShipmentPackages.Remove(this);
    		}
    
    		if (OrderShipment != null)
    		{
    			if (!OrderShipment.OrderShipmentPackages.Contains(this))
    			{
    				OrderShipment.OrderShipmentPackages.Add(this);
    			}
    
    			OrderShipmentID = OrderShipment.OrderShipmentID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("OrderShipment")
    				&& (ChangeTracker.OriginalValues["OrderShipment"] == OrderShipment))
    			{
    				ChangeTracker.OriginalValues.Remove("OrderShipment");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("OrderShipment", previousValue);
    			}
    			if (OrderShipment != null && !OrderShipment.ChangeTracker.ChangeTrackingEnabled)
    			{
    				OrderShipment.StartTracking();
    			}
    		}
    	}
    
    	private void FixupShippingMethod(ShippingMethod previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderShipmentPackages.Contains(this))
    		{
    			previousValue.OrderShipmentPackages.Remove(this);
    		}
    
    		if (ShippingMethod != null)
    		{
    			if (!ShippingMethod.OrderShipmentPackages.Contains(this))
    			{
    				ShippingMethod.OrderShipmentPackages.Add(this);
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
    
    	private void FixupOrderShipmentPackageItems(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (OrderShipmentPackageItem item in e.NewItems)
    			{
    				item.OrderShipmentPackage = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("OrderShipmentPackageItems", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (OrderShipmentPackageItem item in e.OldItems)
    			{
    				if (ReferenceEquals(item.OrderShipmentPackage, this))
    				{
    					item.OrderShipmentPackage = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("OrderShipmentPackageItems", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}
