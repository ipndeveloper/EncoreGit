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
    [KnownType(typeof(OrderShipment))]
    [Serializable]
    public partial class OrderShipmentStatus: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void OrderShipmentStatusIDChanged();
    	public short OrderShipmentStatusID
    	{
    		get { return _orderShipmentStatusID; }
    		set
    		{
    			if (_orderShipmentStatusID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'OrderShipmentStatusID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_orderShipmentStatusID = value;
    				OrderShipmentStatusIDChanged();
    				OnPropertyChanged("OrderShipmentStatusID");
    			}
    		}
    	}
    	private short _orderShipmentStatusID;
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
    	partial void CodeChanged();
    	public string Code
    	{
    		get { return _code; }
    		set
    		{
    			if (_code != value)
    			{
    				ChangeTracker.RecordOriginalValue("Code", _code);
    				_code = value;
    				CodeChanged();
    				OnPropertyChanged("Code");
    			}
    		}
    	}
    	private string _code;

        #endregion
        #region Navigation Properties
    
    	public TrackableCollection<OrderShipment> OrderShipments
    	{
    		get
    		{
    			if (_orderShipments == null)
    			{
    				_orderShipments = new TrackableCollection<OrderShipment>();
    				_orderShipments.CollectionChanged += FixupOrderShipments;
    				_orderShipments.CollectionChanged += RaiseOrderShipmentsChanged;
    			}
    			return _orderShipments;
    		}
    		set
    		{
    			if (!ReferenceEquals(_orderShipments, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_orderShipments != null)
    				{
    					_orderShipments.CollectionChanged -= FixupOrderShipments;
    					_orderShipments.CollectionChanged -= RaiseOrderShipmentsChanged;
    				}
    				_orderShipments = value;
    				if (_orderShipments != null)
    				{
    					_orderShipments.CollectionChanged += FixupOrderShipments;
    					_orderShipments.CollectionChanged += RaiseOrderShipmentsChanged;
    				}
    				OnNavigationPropertyChanged("OrderShipments");
    			}
    		}
    	}
    	private TrackableCollection<OrderShipment> _orderShipments;
    	partial void OrderShipmentsChanged();
    	private void RaiseOrderShipmentsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		OrderShipmentsChanged();
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
    		if (_orderShipments != null)
    		{
    			_orderShipments.CollectionChanged -= FixupOrderShipments;
    			_orderShipments.CollectionChanged -= RaiseOrderShipmentsChanged;
    			_orderShipments.CollectionChanged += FixupOrderShipments;
    			_orderShipments.CollectionChanged += RaiseOrderShipmentsChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		OrderShipments.Clear();
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupOrderShipments(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (OrderShipment item in e.NewItems)
    			{
    				item.OrderShipmentStatus = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("OrderShipments", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (OrderShipment item in e.OldItems)
    			{
    				if (ReferenceEquals(item.OrderShipmentStatus, this))
    				{
    					item.OrderShipmentStatus = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("OrderShipments", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}