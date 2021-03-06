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
    [KnownType(typeof(OrderItemPropertyType))]
    [KnownType(typeof(OrderItemPropertyValue))]
    [KnownType(typeof(OrderItem))]
    [Serializable]
    public partial class OrderItemProperty: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void OrderItemPropertyIDChanged();
    	public int OrderItemPropertyID
    	{
    		get { return _orderItemPropertyID; }
    		set
    		{
    			if (_orderItemPropertyID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'OrderItemPropertyID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_orderItemPropertyID = value;
    				OrderItemPropertyIDChanged();
    				OnPropertyChanged("OrderItemPropertyID");
    			}
    		}
    	}
    	private int _orderItemPropertyID;
    	partial void OrderItemIDChanged();
    	public int OrderItemID
    	{
    		get { return _orderItemID; }
    		set
    		{
    			if (_orderItemID != value)
    			{
    				ChangeTracker.RecordOriginalValue("OrderItemID", _orderItemID);
    				if (!IsDeserializing)
    				{
    					if (OrderItem != null && OrderItem.OrderItemID != value)
    					{
    						OrderItem = null;
    					}
    				}
    				_orderItemID = value;
    				OrderItemIDChanged();
    				OnPropertyChanged("OrderItemID");
    			}
    		}
    	}
    	private int _orderItemID;
    	partial void OrderItemPropertyTypeIDChanged();
    	public int OrderItemPropertyTypeID
    	{
    		get { return _orderItemPropertyTypeID; }
    		set
    		{
    			if (_orderItemPropertyTypeID != value)
    			{
    				ChangeTracker.RecordOriginalValue("OrderItemPropertyTypeID", _orderItemPropertyTypeID);
    				if (!IsDeserializing)
    				{
    					if (OrderItemPropertyType != null && OrderItemPropertyType.OrderItemPropertyTypeID != value)
    					{
    						OrderItemPropertyType = null;
    					}
    				}
    				_orderItemPropertyTypeID = value;
    				OrderItemPropertyTypeIDChanged();
    				OnPropertyChanged("OrderItemPropertyTypeID");
    			}
    		}
    	}
    	private int _orderItemPropertyTypeID;
    	partial void OrderItemPropertyValueIDChanged();
    	public Nullable<int> OrderItemPropertyValueID
    	{
    		get { return _orderItemPropertyValueID; }
    		set
    		{
    			if (_orderItemPropertyValueID != value)
    			{
    				ChangeTracker.RecordOriginalValue("OrderItemPropertyValueID", _orderItemPropertyValueID);
    				if (!IsDeserializing)
    				{
    					if (OrderItemPropertyValue != null && OrderItemPropertyValue.OrderItemPropertyValueID != value)
    					{
    						OrderItemPropertyValue = null;
    					}
    				}
    				_orderItemPropertyValueID = value;
    				OrderItemPropertyValueIDChanged();
    				OnPropertyChanged("OrderItemPropertyValueID");
    			}
    		}
    	}
    	private Nullable<int> _orderItemPropertyValueID;
    	partial void PropertyValueChanged();
    	public string PropertyValue
    	{
    		get { return _propertyValue; }
    		set
    		{
    			if (_propertyValue != value)
    			{
    				ChangeTracker.RecordOriginalValue("PropertyValue", _propertyValue);
    				_propertyValue = value;
    				PropertyValueChanged();
    				OnPropertyChanged("PropertyValue");
    			}
    		}
    	}
    	private string _propertyValue;
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
    
    	public OrderItemPropertyType OrderItemPropertyType
    	{
    		get { return _orderItemPropertyType; }
    		set
    		{
    			if (!ReferenceEquals(_orderItemPropertyType, value))
    			{
    				var previousValue = _orderItemPropertyType;
    				_orderItemPropertyType = value;
    				FixupOrderItemPropertyType(previousValue);
    				OnNavigationPropertyChanged("OrderItemPropertyType");
    			}
    		}
    	}
    	private OrderItemPropertyType _orderItemPropertyType;
    
    	public OrderItemPropertyValue OrderItemPropertyValue
    	{
    		get { return _orderItemPropertyValue; }
    		set
    		{
    			if (!ReferenceEquals(_orderItemPropertyValue, value))
    			{
    				var previousValue = _orderItemPropertyValue;
    				_orderItemPropertyValue = value;
    				FixupOrderItemPropertyValue(previousValue);
    				OnNavigationPropertyChanged("OrderItemPropertyValue");
    			}
    		}
    	}
    	private OrderItemPropertyValue _orderItemPropertyValue;
    
    	public OrderItem OrderItem
    	{
    		get { return _orderItem; }
    		set
    		{
    			if (!ReferenceEquals(_orderItem, value))
    			{
    				var previousValue = _orderItem;
    				_orderItem = value;
    				FixupOrderItem(previousValue);
    				OnNavigationPropertyChanged("OrderItem");
    			}
    		}
    	}
    	private OrderItem _orderItem;

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
    		OrderItemPropertyType = null;
    		OrderItemPropertyValue = null;
    		OrderItem = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupOrderItemPropertyType(OrderItemPropertyType previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderItemProperties.Contains(this))
    		{
    			previousValue.OrderItemProperties.Remove(this);
    		}
    
    		if (OrderItemPropertyType != null)
    		{
    			if (!OrderItemPropertyType.OrderItemProperties.Contains(this))
    			{
    				OrderItemPropertyType.OrderItemProperties.Add(this);
    			}
    
    			OrderItemPropertyTypeID = OrderItemPropertyType.OrderItemPropertyTypeID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("OrderItemPropertyType")
    				&& (ChangeTracker.OriginalValues["OrderItemPropertyType"] == OrderItemPropertyType))
    			{
    				ChangeTracker.OriginalValues.Remove("OrderItemPropertyType");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("OrderItemPropertyType", previousValue);
    			}
    			if (OrderItemPropertyType != null && !OrderItemPropertyType.ChangeTracker.ChangeTrackingEnabled)
    			{
    				OrderItemPropertyType.StartTracking();
    			}
    		}
    	}
    
    	private void FixupOrderItemPropertyValue(OrderItemPropertyValue previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderItemProperties.Contains(this))
    		{
    			previousValue.OrderItemProperties.Remove(this);
    		}
    
    		if (OrderItemPropertyValue != null)
    		{
    			if (!OrderItemPropertyValue.OrderItemProperties.Contains(this))
    			{
    				OrderItemPropertyValue.OrderItemProperties.Add(this);
    			}
    
    			OrderItemPropertyValueID = OrderItemPropertyValue.OrderItemPropertyValueID;
    		}
    		else if (!skipKeys)
    		{
    			OrderItemPropertyValueID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("OrderItemPropertyValue")
    				&& (ChangeTracker.OriginalValues["OrderItemPropertyValue"] == OrderItemPropertyValue))
    			{
    				ChangeTracker.OriginalValues.Remove("OrderItemPropertyValue");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("OrderItemPropertyValue", previousValue);
    			}
    			if (OrderItemPropertyValue != null && !OrderItemPropertyValue.ChangeTracker.ChangeTrackingEnabled)
    			{
    				OrderItemPropertyValue.StartTracking();
    			}
    		}
    	}
    
    	private void FixupOrderItem(OrderItem previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderItemProperties.Contains(this))
    		{
    			previousValue.OrderItemProperties.Remove(this);
    		}
    
    		if (OrderItem != null)
    		{
    			if (!OrderItem.OrderItemProperties.Contains(this))
    			{
    				OrderItem.OrderItemProperties.Add(this);
    			}
    
    			OrderItemID = OrderItem.OrderItemID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("OrderItem")
    				&& (ChangeTracker.OriginalValues["OrderItem"] == OrderItem))
    			{
    				ChangeTracker.OriginalValues.Remove("OrderItem");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("OrderItem", previousValue);
    			}
    			if (OrderItem != null && !OrderItem.ChangeTracker.ChangeTrackingEnabled)
    			{
    				OrderItem.StartTracking();
    			}
    		}
    	}

        #endregion
    }
}
