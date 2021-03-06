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
    [KnownType(typeof(OrderItemProperty))]
    [KnownType(typeof(OrderItemPropertyValue))]
    [Serializable]
    public partial class OrderItemPropertyType: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void OrderItemPropertyTypeIDChanged();
    	public int OrderItemPropertyTypeID
    	{
    		get { return _orderItemPropertyTypeID; }
    		set
    		{
    			if (_orderItemPropertyTypeID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'OrderItemPropertyTypeID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_orderItemPropertyTypeID = value;
    				OrderItemPropertyTypeIDChanged();
    				OnPropertyChanged("OrderItemPropertyTypeID");
    			}
    		}
    	}
    	private int _orderItemPropertyTypeID;
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
    	partial void DataTypeChanged();
    	public string DataType
    	{
    		get { return _dataType; }
    		set
    		{
    			if (_dataType != value)
    			{
    				ChangeTracker.RecordOriginalValue("DataType", _dataType);
    				_dataType = value;
    				DataTypeChanged();
    				OnPropertyChanged("DataType");
    			}
    		}
    	}
    	private string _dataType;
    	partial void RequiredChanged();
    	public bool Required
    	{
    		get { return _required; }
    		set
    		{
    			if (_required != value)
    			{
    				ChangeTracker.RecordOriginalValue("Required", _required);
    				_required = value;
    				RequiredChanged();
    				OnPropertyChanged("Required");
    			}
    		}
    	}
    	private bool _required;
    	partial void SortIndexChanged();
    	public int SortIndex
    	{
    		get { return _sortIndex; }
    		set
    		{
    			if (_sortIndex != value)
    			{
    				ChangeTracker.RecordOriginalValue("SortIndex", _sortIndex);
    				_sortIndex = value;
    				SortIndexChanged();
    				OnPropertyChanged("SortIndex");
    			}
    		}
    	}
    	private int _sortIndex;
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
    	partial void DescriptionChanged();
    	public string Description
    	{
    		get { return _description; }
    		set
    		{
    			if (_description != value)
    			{
    				ChangeTracker.RecordOriginalValue("Description", _description);
    				_description = value;
    				DescriptionChanged();
    				OnPropertyChanged("Description");
    			}
    		}
    	}
    	private string _description;
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
    
    	public TrackableCollection<OrderItemProperty> OrderItemProperties
    	{
    		get
    		{
    			if (_orderItemProperties == null)
    			{
    				_orderItemProperties = new TrackableCollection<OrderItemProperty>();
    				_orderItemProperties.CollectionChanged += FixupOrderItemProperties;
    				_orderItemProperties.CollectionChanged += RaiseOrderItemPropertiesChanged;
    			}
    			return _orderItemProperties;
    		}
    		set
    		{
    			if (!ReferenceEquals(_orderItemProperties, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_orderItemProperties != null)
    				{
    					_orderItemProperties.CollectionChanged -= FixupOrderItemProperties;
    					_orderItemProperties.CollectionChanged -= RaiseOrderItemPropertiesChanged;
    				}
    				_orderItemProperties = value;
    				if (_orderItemProperties != null)
    				{
    					_orderItemProperties.CollectionChanged += FixupOrderItemProperties;
    					_orderItemProperties.CollectionChanged += RaiseOrderItemPropertiesChanged;
    				}
    				OnNavigationPropertyChanged("OrderItemProperties");
    			}
    		}
    	}
    	private TrackableCollection<OrderItemProperty> _orderItemProperties;
    	partial void OrderItemPropertiesChanged();
    	private void RaiseOrderItemPropertiesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		OrderItemPropertiesChanged();
    	}
    
    	public TrackableCollection<OrderItemPropertyValue> OrderItemPropertyValues
    	{
    		get
    		{
    			if (_orderItemPropertyValues == null)
    			{
    				_orderItemPropertyValues = new TrackableCollection<OrderItemPropertyValue>();
    				_orderItemPropertyValues.CollectionChanged += FixupOrderItemPropertyValues;
    				_orderItemPropertyValues.CollectionChanged += RaiseOrderItemPropertyValuesChanged;
    			}
    			return _orderItemPropertyValues;
    		}
    		set
    		{
    			if (!ReferenceEquals(_orderItemPropertyValues, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_orderItemPropertyValues != null)
    				{
    					_orderItemPropertyValues.CollectionChanged -= FixupOrderItemPropertyValues;
    					_orderItemPropertyValues.CollectionChanged -= RaiseOrderItemPropertyValuesChanged;
    				}
    				_orderItemPropertyValues = value;
    				if (_orderItemPropertyValues != null)
    				{
    					_orderItemPropertyValues.CollectionChanged += FixupOrderItemPropertyValues;
    					_orderItemPropertyValues.CollectionChanged += RaiseOrderItemPropertyValuesChanged;
    				}
    				OnNavigationPropertyChanged("OrderItemPropertyValues");
    			}
    		}
    	}
    	private TrackableCollection<OrderItemPropertyValue> _orderItemPropertyValues;
    	partial void OrderItemPropertyValuesChanged();
    	private void RaiseOrderItemPropertyValuesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		OrderItemPropertyValuesChanged();
    	}

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
    		if (_orderItemProperties != null)
    		{
    			_orderItemProperties.CollectionChanged -= FixupOrderItemProperties;
    			_orderItemProperties.CollectionChanged -= RaiseOrderItemPropertiesChanged;
    			_orderItemProperties.CollectionChanged += FixupOrderItemProperties;
    			_orderItemProperties.CollectionChanged += RaiseOrderItemPropertiesChanged;
    		}
    		if (_orderItemPropertyValues != null)
    		{
    			_orderItemPropertyValues.CollectionChanged -= FixupOrderItemPropertyValues;
    			_orderItemPropertyValues.CollectionChanged -= RaiseOrderItemPropertyValuesChanged;
    			_orderItemPropertyValues.CollectionChanged += FixupOrderItemPropertyValues;
    			_orderItemPropertyValues.CollectionChanged += RaiseOrderItemPropertyValuesChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		OrderItemProperties.Clear();
    		OrderItemPropertyValues.Clear();
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupOrderItemProperties(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (OrderItemProperty item in e.NewItems)
    			{
    				item.OrderItemPropertyType = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("OrderItemProperties", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (OrderItemProperty item in e.OldItems)
    			{
    				if (ReferenceEquals(item.OrderItemPropertyType, this))
    				{
    					item.OrderItemPropertyType = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("OrderItemProperties", item);
    				}
    			}
    		}
    	}
    
    	private void FixupOrderItemPropertyValues(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (OrderItemPropertyValue item in e.NewItems)
    			{
    				item.OrderItemPropertyType = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("OrderItemPropertyValues", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (OrderItemPropertyValue item in e.OldItems)
    			{
    				if (ReferenceEquals(item.OrderItemPropertyType, this))
    				{
    					item.OrderItemPropertyType = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("OrderItemPropertyValues", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}
