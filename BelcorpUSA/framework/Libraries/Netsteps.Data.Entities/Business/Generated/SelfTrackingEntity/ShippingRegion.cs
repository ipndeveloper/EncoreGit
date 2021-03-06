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
    [KnownType(typeof(ShippingOrderType))]
    [KnownType(typeof(StateProvince))]
    [KnownType(typeof(Warehouse))]
    [Serializable]
    public partial class ShippingRegion: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void ShippingRegionIDChanged();
    	public int ShippingRegionID
    	{
    		get { return _shippingRegionID; }
    		set
    		{
    			if (_shippingRegionID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'ShippingRegionID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_shippingRegionID = value;
    				ShippingRegionIDChanged();
    				OnPropertyChanged("ShippingRegionID");
    			}
    		}
    	}
    	private int _shippingRegionID;
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
    	partial void EditableChanged();
    	public bool Editable
    	{
    		get { return _editable; }
    		set
    		{
    			if (_editable != value)
    			{
    				ChangeTracker.RecordOriginalValue("Editable", _editable);
    				_editable = value;
    				EditableChanged();
    				OnPropertyChanged("Editable");
    			}
    		}
    	}
    	private bool _editable;
    	partial void WarehouseIDChanged();
    	public Nullable<int> WarehouseID
    	{
    		get { return _warehouseID; }
    		set
    		{
    			if (_warehouseID != value)
    			{
    				ChangeTracker.RecordOriginalValue("WarehouseID", _warehouseID);
    				if (!IsDeserializing)
    				{
    					if (Warehouse != null && Warehouse.WarehouseID != value)
    					{
    						Warehouse = null;
    					}
    				}
    				_warehouseID = value;
    				WarehouseIDChanged();
    				OnPropertyChanged("WarehouseID");
    			}
    		}
    	}
    	private Nullable<int> _warehouseID;

        #endregion
        #region Navigation Properties
    
    	public TrackableCollection<ShippingOrderType> ShippingOrderTypes
    	{
    		get
    		{
    			if (_shippingOrderTypes == null)
    			{
    				_shippingOrderTypes = new TrackableCollection<ShippingOrderType>();
    				_shippingOrderTypes.CollectionChanged += FixupShippingOrderTypes;
    				_shippingOrderTypes.CollectionChanged += RaiseShippingOrderTypesChanged;
    			}
    			return _shippingOrderTypes;
    		}
    		set
    		{
    			if (!ReferenceEquals(_shippingOrderTypes, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_shippingOrderTypes != null)
    				{
    					_shippingOrderTypes.CollectionChanged -= FixupShippingOrderTypes;
    					_shippingOrderTypes.CollectionChanged -= RaiseShippingOrderTypesChanged;
    				}
    				_shippingOrderTypes = value;
    				if (_shippingOrderTypes != null)
    				{
    					_shippingOrderTypes.CollectionChanged += FixupShippingOrderTypes;
    					_shippingOrderTypes.CollectionChanged += RaiseShippingOrderTypesChanged;
    				}
    				OnNavigationPropertyChanged("ShippingOrderTypes");
    			}
    		}
    	}
    	private TrackableCollection<ShippingOrderType> _shippingOrderTypes;
    	partial void ShippingOrderTypesChanged();
    	private void RaiseShippingOrderTypesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		ShippingOrderTypesChanged();
    	}
    
    	public TrackableCollection<StateProvince> StateProvinces
    	{
    		get
    		{
    			if (_stateProvinces == null)
    			{
    				_stateProvinces = new TrackableCollection<StateProvince>();
    				_stateProvinces.CollectionChanged += FixupStateProvinces;
    				_stateProvinces.CollectionChanged += RaiseStateProvincesChanged;
    			}
    			return _stateProvinces;
    		}
    		set
    		{
    			if (!ReferenceEquals(_stateProvinces, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_stateProvinces != null)
    				{
    					_stateProvinces.CollectionChanged -= FixupStateProvinces;
    					_stateProvinces.CollectionChanged -= RaiseStateProvincesChanged;
    				}
    				_stateProvinces = value;
    				if (_stateProvinces != null)
    				{
    					_stateProvinces.CollectionChanged += FixupStateProvinces;
    					_stateProvinces.CollectionChanged += RaiseStateProvincesChanged;
    				}
    				OnNavigationPropertyChanged("StateProvinces");
    			}
    		}
    	}
    	private TrackableCollection<StateProvince> _stateProvinces;
    	partial void StateProvincesChanged();
    	private void RaiseStateProvincesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		StateProvincesChanged();
    	}
    
    	public Warehouse Warehouse
    	{
    		get { return _warehouse; }
    		set
    		{
    			if (!ReferenceEquals(_warehouse, value))
    			{
    				var previousValue = _warehouse;
    				_warehouse = value;
    				FixupWarehouse(previousValue);
    				OnNavigationPropertyChanged("Warehouse");
    			}
    		}
    	}
    	private Warehouse _warehouse;

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
    		if (_shippingOrderTypes != null)
    		{
    			_shippingOrderTypes.CollectionChanged -= FixupShippingOrderTypes;
    			_shippingOrderTypes.CollectionChanged -= RaiseShippingOrderTypesChanged;
    			_shippingOrderTypes.CollectionChanged += FixupShippingOrderTypes;
    			_shippingOrderTypes.CollectionChanged += RaiseShippingOrderTypesChanged;
    		}
    		if (_stateProvinces != null)
    		{
    			_stateProvinces.CollectionChanged -= FixupStateProvinces;
    			_stateProvinces.CollectionChanged -= RaiseStateProvincesChanged;
    			_stateProvinces.CollectionChanged += FixupStateProvinces;
    			_stateProvinces.CollectionChanged += RaiseStateProvincesChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		ShippingOrderTypes.Clear();
    		StateProvinces.Clear();
    		Warehouse = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupWarehouse(Warehouse previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.ShippingRegions.Contains(this))
    		{
    			previousValue.ShippingRegions.Remove(this);
    		}
    
    		if (Warehouse != null)
    		{
    			if (!Warehouse.ShippingRegions.Contains(this))
    			{
    				Warehouse.ShippingRegions.Add(this);
    			}
    
    			WarehouseID = Warehouse.WarehouseID;
    		}
    		else if (!skipKeys)
    		{
    			WarehouseID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Warehouse")
    				&& (ChangeTracker.OriginalValues["Warehouse"] == Warehouse))
    			{
    				ChangeTracker.OriginalValues.Remove("Warehouse");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Warehouse", previousValue);
    			}
    			if (Warehouse != null && !Warehouse.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Warehouse.StartTracking();
    			}
    		}
    	}
    
    	private void FixupShippingOrderTypes(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (ShippingOrderType item in e.NewItems)
    			{
    				item.ShippingRegion = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("ShippingOrderTypes", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (ShippingOrderType item in e.OldItems)
    			{
    				if (ReferenceEquals(item.ShippingRegion, this))
    				{
    					item.ShippingRegion = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("ShippingOrderTypes", item);
    				}
    			}
    		}
    	}
    
    	private void FixupStateProvinces(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (StateProvince item in e.NewItems)
    			{
    				item.ShippingRegion = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("StateProvinces", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (StateProvince item in e.OldItems)
    			{
    				if (ReferenceEquals(item.ShippingRegion, this))
    				{
    					item.ShippingRegion = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("StateProvinces", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}
