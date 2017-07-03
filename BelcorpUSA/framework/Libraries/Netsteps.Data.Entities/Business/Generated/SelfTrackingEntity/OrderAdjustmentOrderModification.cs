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
    [KnownType(typeof(OrderAdjustment))]
    [KnownType(typeof(OrderCustomer))]
    [Serializable]
    public partial class OrderAdjustmentOrderModification: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void OrderAdjustmentOrderModificationIDChanged();
    	public int OrderAdjustmentOrderModificationID
    	{
    		get { return _orderAdjustmentOrderModificationID; }
    		set
    		{
    			if (_orderAdjustmentOrderModificationID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'OrderAdjustmentOrderModificationID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_orderAdjustmentOrderModificationID = value;
    				OrderAdjustmentOrderModificationIDChanged();
    				OnPropertyChanged("OrderAdjustmentOrderModificationID");
    			}
    		}
    	}
    	private int _orderAdjustmentOrderModificationID;
    	partial void OrderAdjustmentIDChanged();
    	public int OrderAdjustmentID
    	{
    		get { return _orderAdjustmentID; }
    		set
    		{
    			if (_orderAdjustmentID != value)
    			{
    				ChangeTracker.RecordOriginalValue("OrderAdjustmentID", _orderAdjustmentID);
    				if (!IsDeserializing)
    				{
    					if (OrderAdjustment != null && OrderAdjustment.OrderAdjustmentID != value)
    					{
    						OrderAdjustment = null;
    					}
    				}
    				_orderAdjustmentID = value;
    				OrderAdjustmentIDChanged();
    				OnPropertyChanged("OrderAdjustmentID");
    			}
    		}
    	}
    	private int _orderAdjustmentID;
    	partial void PropertyNameChanged();
    	public string PropertyName
    	{
    		get { return _propertyName; }
    		set
    		{
    			if (_propertyName != value)
    			{
    				ChangeTracker.RecordOriginalValue("PropertyName", _propertyName);
    				_propertyName = value;
    				PropertyNameChanged();
    				OnPropertyChanged("PropertyName");
    			}
    		}
    	}
    	private string _propertyName;
    	partial void ModificationOperationIDChanged();
    	public int ModificationOperationID
    	{
    		get { return _modificationOperationID; }
    		set
    		{
    			if (_modificationOperationID != value)
    			{
    				ChangeTracker.RecordOriginalValue("ModificationOperationID", _modificationOperationID);
    				_modificationOperationID = value;
    				ModificationOperationIDChanged();
    				OnPropertyChanged("ModificationOperationID");
    			}
    		}
    	}
    	private int _modificationOperationID;
    	partial void ModificationDecimalValueChanged();
    	public Nullable<decimal> ModificationDecimalValue
    	{
    		get { return _modificationDecimalValue; }
    		set
    		{
    			if (_modificationDecimalValue != value)
    			{
    				ChangeTracker.RecordOriginalValue("ModificationDecimalValue", _modificationDecimalValue);
    				_modificationDecimalValue = value;
    				ModificationDecimalValueChanged();
    				OnPropertyChanged("ModificationDecimalValue");
    			}
    		}
    	}
    	private Nullable<decimal> _modificationDecimalValue;
    	partial void ModificationDescriptionChanged();
    	public string ModificationDescription
    	{
    		get { return _modificationDescription; }
    		set
    		{
    			if (_modificationDescription != value)
    			{
    				ChangeTracker.RecordOriginalValue("ModificationDescription", _modificationDescription);
    				_modificationDescription = value;
    				ModificationDescriptionChanged();
    				OnPropertyChanged("ModificationDescription");
    			}
    		}
    	}
    	private string _modificationDescription;
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

        #endregion
        #region Navigation Properties
    
    	public OrderAdjustment OrderAdjustment
    	{
    		get { return _orderAdjustment; }
    		set
    		{
    			if (!ReferenceEquals(_orderAdjustment, value))
    			{
    				var previousValue = _orderAdjustment;
    				_orderAdjustment = value;
    				FixupOrderAdjustment(previousValue);
    				OnNavigationPropertyChanged("OrderAdjustment");
    			}
    		}
    	}
    	private OrderAdjustment _orderAdjustment;
    
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
    		OrderAdjustment = null;
    		OrderCustomer = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupOrderAdjustment(OrderAdjustment previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderAdjustmentOrderModifications.Contains(this))
    		{
    			previousValue.OrderAdjustmentOrderModifications.Remove(this);
    		}
    
    		if (OrderAdjustment != null)
    		{
    			if (!OrderAdjustment.OrderAdjustmentOrderModifications.Contains(this))
    			{
    				OrderAdjustment.OrderAdjustmentOrderModifications.Add(this);
    			}
    
    			OrderAdjustmentID = OrderAdjustment.OrderAdjustmentID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("OrderAdjustment")
    				&& (ChangeTracker.OriginalValues["OrderAdjustment"] == OrderAdjustment))
    			{
    				ChangeTracker.OriginalValues.Remove("OrderAdjustment");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("OrderAdjustment", previousValue);
    			}
    			if (OrderAdjustment != null && !OrderAdjustment.ChangeTracker.ChangeTrackingEnabled)
    			{
    				OrderAdjustment.StartTracking();
    			}
    		}
    	}
    
    	private void FixupOrderCustomer(OrderCustomer previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderAdjustmentOrderModifications.Contains(this))
    		{
    			previousValue.OrderAdjustmentOrderModifications.Remove(this);
    		}
    
    		if (OrderCustomer != null)
    		{
    			if (!OrderCustomer.OrderAdjustmentOrderModifications.Contains(this))
    			{
    				OrderCustomer.OrderAdjustmentOrderModifications.Add(this);
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

        #endregion
    }
}
