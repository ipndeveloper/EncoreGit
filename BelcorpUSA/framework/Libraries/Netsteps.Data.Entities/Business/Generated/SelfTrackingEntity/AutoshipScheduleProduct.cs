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
    [KnownType(typeof(AutoshipSchedule))]
    [KnownType(typeof(Product))]
    [Serializable]
    public partial class AutoshipScheduleProduct: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void AutoshipScheduleProductIDChanged();
    	public int AutoshipScheduleProductID
    	{
    		get { return _autoshipScheduleProductID; }
    		set
    		{
    			if (_autoshipScheduleProductID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'AutoshipScheduleProductID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_autoshipScheduleProductID = value;
    				AutoshipScheduleProductIDChanged();
    				OnPropertyChanged("AutoshipScheduleProductID");
    			}
    		}
    	}
    	private int _autoshipScheduleProductID;
    	partial void AutoshipScheduleIDChanged();
    	public int AutoshipScheduleID
    	{
    		get { return _autoshipScheduleID; }
    		set
    		{
    			if (_autoshipScheduleID != value)
    			{
    				ChangeTracker.RecordOriginalValue("AutoshipScheduleID", _autoshipScheduleID);
    				if (!IsDeserializing)
    				{
    					if (AutoshipSchedule != null && AutoshipSchedule.AutoshipScheduleID != value)
    					{
    						AutoshipSchedule = null;
    					}
    				}
    				_autoshipScheduleID = value;
    				AutoshipScheduleIDChanged();
    				OnPropertyChanged("AutoshipScheduleID");
    			}
    		}
    	}
    	private int _autoshipScheduleID;
    	partial void ProductIDChanged();
    	public int ProductID
    	{
    		get { return _productID; }
    		set
    		{
    			if (_productID != value)
    			{
    				ChangeTracker.RecordOriginalValue("ProductID", _productID);
    				if (!IsDeserializing)
    				{
    					if (Product != null && Product.ProductID != value)
    					{
    						Product = null;
    					}
    				}
    				_productID = value;
    				ProductIDChanged();
    				OnPropertyChanged("ProductID");
    			}
    		}
    	}
    	private int _productID;
    	partial void QuantityChanged();
    	public int Quantity
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
    	private int _quantity;

        #endregion
        #region Navigation Properties
    
    	public AutoshipSchedule AutoshipSchedule
    	{
    		get { return _autoshipSchedule; }
    		set
    		{
    			if (!ReferenceEquals(_autoshipSchedule, value))
    			{
    				var previousValue = _autoshipSchedule;
    				_autoshipSchedule = value;
    				FixupAutoshipSchedule(previousValue);
    				OnNavigationPropertyChanged("AutoshipSchedule");
    			}
    		}
    	}
    	private AutoshipSchedule _autoshipSchedule;
    
    	public Product Product
    	{
    		get { return _product; }
    		set
    		{
    			if (!ReferenceEquals(_product, value))
    			{
    				var previousValue = _product;
    				_product = value;
    				FixupProduct(previousValue);
    				OnNavigationPropertyChanged("Product");
    			}
    		}
    	}
    	private Product _product;

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
    		AutoshipSchedule = null;
    		Product = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupAutoshipSchedule(AutoshipSchedule previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.AutoshipScheduleProducts.Contains(this))
    		{
    			previousValue.AutoshipScheduleProducts.Remove(this);
    		}
    
    		if (AutoshipSchedule != null)
    		{
    			if (!AutoshipSchedule.AutoshipScheduleProducts.Contains(this))
    			{
    				AutoshipSchedule.AutoshipScheduleProducts.Add(this);
    			}
    
    			AutoshipScheduleID = AutoshipSchedule.AutoshipScheduleID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("AutoshipSchedule")
    				&& (ChangeTracker.OriginalValues["AutoshipSchedule"] == AutoshipSchedule))
    			{
    				ChangeTracker.OriginalValues.Remove("AutoshipSchedule");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("AutoshipSchedule", previousValue);
    			}
    			if (AutoshipSchedule != null && !AutoshipSchedule.ChangeTracker.ChangeTrackingEnabled)
    			{
    				AutoshipSchedule.StartTracking();
    			}
    		}
    	}
    
    	private void FixupProduct(Product previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.AutoshipScheduleProducts.Contains(this))
    		{
    			previousValue.AutoshipScheduleProducts.Remove(this);
    		}
    
    		if (Product != null)
    		{
    			if (!Product.AutoshipScheduleProducts.Contains(this))
    			{
    				Product.AutoshipScheduleProducts.Add(this);
    			}
    
    			ProductID = Product.ProductID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Product")
    				&& (ChangeTracker.OriginalValues["Product"] == Product))
    			{
    				ChangeTracker.OriginalValues.Remove("Product");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Product", previousValue);
    			}
    			if (Product != null && !Product.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Product.StartTracking();
    			}
    		}
    	}

        #endregion
    }
}