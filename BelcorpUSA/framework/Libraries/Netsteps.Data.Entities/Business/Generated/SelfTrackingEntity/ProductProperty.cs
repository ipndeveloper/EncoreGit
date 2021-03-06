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
    [KnownType(typeof(ProductPropertyType))]
    [KnownType(typeof(ProductPropertyValue))]
    [KnownType(typeof(Product))]
    [Serializable]
    public partial class ProductProperty: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void ProductPropertyIDChanged();
    	public int ProductPropertyID
    	{
    		get { return _productPropertyID; }
    		set
    		{
    			if (_productPropertyID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'ProductPropertyID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_productPropertyID = value;
    				ProductPropertyIDChanged();
    				OnPropertyChanged("ProductPropertyID");
    			}
    		}
    	}
    	private int _productPropertyID;
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
    	partial void ProductPropertyTypeIDChanged();
    	public int ProductPropertyTypeID
    	{
    		get { return _productPropertyTypeID; }
    		set
    		{
    			if (_productPropertyTypeID != value)
    			{
    				ChangeTracker.RecordOriginalValue("ProductPropertyTypeID", _productPropertyTypeID);
    				if (!IsDeserializing)
    				{
    					if (ProductPropertyType != null && ProductPropertyType.ProductPropertyTypeID != value)
    					{
    						ProductPropertyType = null;
    					}
    				}
    				_productPropertyTypeID = value;
    				ProductPropertyTypeIDChanged();
    				OnPropertyChanged("ProductPropertyTypeID");
    			}
    		}
    	}
    	private int _productPropertyTypeID;
    	partial void ProductPropertyValueIDChanged();
    	public Nullable<int> ProductPropertyValueID
    	{
    		get { return _productPropertyValueID; }
    		set
    		{
    			if (_productPropertyValueID != value)
    			{
    				ChangeTracker.RecordOriginalValue("ProductPropertyValueID", _productPropertyValueID);
    				if (!IsDeserializing)
    				{
    					if (ProductPropertyValue != null && ProductPropertyValue.ProductPropertyValueID != value)
    					{
    						ProductPropertyValue = null;
    					}
    				}
    				_productPropertyValueID = value;
    				ProductPropertyValueIDChanged();
    				OnPropertyChanged("ProductPropertyValueID");
    			}
    		}
    	}
    	private Nullable<int> _productPropertyValueID;
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
    
    	public ProductPropertyType ProductPropertyType
    	{
    		get { return _productPropertyType; }
    		set
    		{
    			if (!ReferenceEquals(_productPropertyType, value))
    			{
    				var previousValue = _productPropertyType;
    				_productPropertyType = value;
    				FixupProductPropertyType(previousValue);
    				OnNavigationPropertyChanged("ProductPropertyType");
    			}
    		}
    	}
    	private ProductPropertyType _productPropertyType;
    
    	public ProductPropertyValue ProductPropertyValue
    	{
    		get { return _productPropertyValue; }
    		set
    		{
    			if (!ReferenceEquals(_productPropertyValue, value))
    			{
    				var previousValue = _productPropertyValue;
    				_productPropertyValue = value;
    				FixupProductPropertyValue(previousValue);
    				OnNavigationPropertyChanged("ProductPropertyValue");
    			}
    		}
    	}
    	private ProductPropertyValue _productPropertyValue;
    
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
    		ProductPropertyType = null;
    		ProductPropertyValue = null;
    		Product = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupProductPropertyType(ProductPropertyType previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.ProductProperties.Contains(this))
    		{
    			previousValue.ProductProperties.Remove(this);
    		}
    
    		if (ProductPropertyType != null)
    		{
    			if (!ProductPropertyType.ProductProperties.Contains(this))
    			{
    				ProductPropertyType.ProductProperties.Add(this);
    			}
    
    			ProductPropertyTypeID = ProductPropertyType.ProductPropertyTypeID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("ProductPropertyType")
    				&& (ChangeTracker.OriginalValues["ProductPropertyType"] == ProductPropertyType))
    			{
    				ChangeTracker.OriginalValues.Remove("ProductPropertyType");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("ProductPropertyType", previousValue);
    			}
    			if (ProductPropertyType != null && !ProductPropertyType.ChangeTracker.ChangeTrackingEnabled)
    			{
    				ProductPropertyType.StartTracking();
    			}
    		}
    	}
    
    	private void FixupProductPropertyValue(ProductPropertyValue previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.ProductProperties.Contains(this))
    		{
    			previousValue.ProductProperties.Remove(this);
    		}
    
    		if (ProductPropertyValue != null)
    		{
    			if (!ProductPropertyValue.ProductProperties.Contains(this))
    			{
    				ProductPropertyValue.ProductProperties.Add(this);
    			}
    
    			ProductPropertyValueID = ProductPropertyValue.ProductPropertyValueID;
    		}
    		else if (!skipKeys)
    		{
    			ProductPropertyValueID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("ProductPropertyValue")
    				&& (ChangeTracker.OriginalValues["ProductPropertyValue"] == ProductPropertyValue))
    			{
    				ChangeTracker.OriginalValues.Remove("ProductPropertyValue");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("ProductPropertyValue", previousValue);
    			}
    			if (ProductPropertyValue != null && !ProductPropertyValue.ChangeTracker.ChangeTrackingEnabled)
    			{
    				ProductPropertyValue.StartTracking();
    			}
    		}
    	}
    
    	private void FixupProduct(Product previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.Properties.Contains(this))
    		{
    			previousValue.Properties.Remove(this);
    		}
    
    		if (Product != null)
    		{
    			if (!Product.Properties.Contains(this))
    			{
    				Product.Properties.Add(this);
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
