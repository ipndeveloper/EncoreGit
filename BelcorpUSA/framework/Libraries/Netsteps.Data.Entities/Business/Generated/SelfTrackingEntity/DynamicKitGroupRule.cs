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
    [KnownType(typeof(DynamicKitGroup))]
    [KnownType(typeof(Product))]
    [KnownType(typeof(ProductType))]
    [Serializable]
    public partial class DynamicKitGroupRule: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void DynamicKitGroupRuleIDChanged();
    	public int DynamicKitGroupRuleID
    	{
    		get { return _dynamicKitGroupRuleID; }
    		set
    		{
    			if (_dynamicKitGroupRuleID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'DynamicKitGroupRuleID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_dynamicKitGroupRuleID = value;
    				DynamicKitGroupRuleIDChanged();
    				OnPropertyChanged("DynamicKitGroupRuleID");
    			}
    		}
    	}
    	private int _dynamicKitGroupRuleID;
    	partial void DynamicKitGroupIDChanged();
    	public int DynamicKitGroupID
    	{
    		get { return _dynamicKitGroupID; }
    		set
    		{
    			if (_dynamicKitGroupID != value)
    			{
    				ChangeTracker.RecordOriginalValue("DynamicKitGroupID", _dynamicKitGroupID);
    				if (!IsDeserializing)
    				{
    					if (DynamicKitGroup != null && DynamicKitGroup.DynamicKitGroupID != value)
    					{
    						DynamicKitGroup = null;
    					}
    				}
    				_dynamicKitGroupID = value;
    				DynamicKitGroupIDChanged();
    				OnPropertyChanged("DynamicKitGroupID");
    			}
    		}
    	}
    	private int _dynamicKitGroupID;
    	partial void ProductTypeIDChanged();
    	public Nullable<int> ProductTypeID
    	{
    		get { return _productTypeID; }
    		set
    		{
    			if (_productTypeID != value)
    			{
    				ChangeTracker.RecordOriginalValue("ProductTypeID", _productTypeID);
    				if (!IsDeserializing)
    				{
    					if (ProductType != null && ProductType.ProductTypeID != value)
    					{
    						ProductType = null;
    					}
    				}
    				_productTypeID = value;
    				ProductTypeIDChanged();
    				OnPropertyChanged("ProductTypeID");
    			}
    		}
    	}
    	private Nullable<int> _productTypeID;
    	partial void ProductIDChanged();
    	public Nullable<int> ProductID
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
    	private Nullable<int> _productID;
    	partial void IncludeChanged();
    	public bool Include
    	{
    		get { return _include; }
    		set
    		{
    			if (_include != value)
    			{
    				ChangeTracker.RecordOriginalValue("Include", _include);
    				_include = value;
    				IncludeChanged();
    				OnPropertyChanged("Include");
    			}
    		}
    	}
    	private bool _include;
    	partial void DefaultChanged();
    	public bool Default
    	{
    		get { return _default; }
    		set
    		{
    			if (_default != value)
    			{
    				ChangeTracker.RecordOriginalValue("Default", _default);
    				_default = value;
    				DefaultChanged();
    				OnPropertyChanged("Default");
    			}
    		}
    	}
    	private bool _default;
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
    	partial void SortOrderChanged();
    	public int SortOrder
    	{
    		get { return _sortOrder; }
    		set
    		{
    			if (_sortOrder != value)
    			{
    				ChangeTracker.RecordOriginalValue("SortOrder", _sortOrder);
    				_sortOrder = value;
    				SortOrderChanged();
    				OnPropertyChanged("SortOrder");
    			}
    		}
    	}
    	private int _sortOrder;

        #endregion
        #region Navigation Properties
    
    	public DynamicKitGroup DynamicKitGroup
    	{
    		get { return _dynamicKitGroup; }
    		set
    		{
    			if (!ReferenceEquals(_dynamicKitGroup, value))
    			{
    				var previousValue = _dynamicKitGroup;
    				_dynamicKitGroup = value;
    				FixupDynamicKitGroup(previousValue);
    				OnNavigationPropertyChanged("DynamicKitGroup");
    			}
    		}
    	}
    	private DynamicKitGroup _dynamicKitGroup;
    
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
    
    	public ProductType ProductType
    	{
    		get { return _productType; }
    		set
    		{
    			if (!ReferenceEquals(_productType, value))
    			{
    				var previousValue = _productType;
    				_productType = value;
    				FixupProductType(previousValue);
    				OnNavigationPropertyChanged("ProductType");
    			}
    		}
    	}
    	private ProductType _productType;

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
    		DynamicKitGroup = null;
    		Product = null;
    		ProductType = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupDynamicKitGroup(DynamicKitGroup previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.DynamicKitGroupRules.Contains(this))
    		{
    			previousValue.DynamicKitGroupRules.Remove(this);
    		}
    
    		if (DynamicKitGroup != null)
    		{
    			if (!DynamicKitGroup.DynamicKitGroupRules.Contains(this))
    			{
    				DynamicKitGroup.DynamicKitGroupRules.Add(this);
    			}
    
    			DynamicKitGroupID = DynamicKitGroup.DynamicKitGroupID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("DynamicKitGroup")
    				&& (ChangeTracker.OriginalValues["DynamicKitGroup"] == DynamicKitGroup))
    			{
    				ChangeTracker.OriginalValues.Remove("DynamicKitGroup");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("DynamicKitGroup", previousValue);
    			}
    			if (DynamicKitGroup != null && !DynamicKitGroup.ChangeTracker.ChangeTrackingEnabled)
    			{
    				DynamicKitGroup.StartTracking();
    			}
    		}
    	}
    
    	private void FixupProduct(Product previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.DynamicKitGroupRules.Contains(this))
    		{
    			previousValue.DynamicKitGroupRules.Remove(this);
    		}
    
    		if (Product != null)
    		{
    			if (!Product.DynamicKitGroupRules.Contains(this))
    			{
    				Product.DynamicKitGroupRules.Add(this);
    			}
    
    			ProductID = Product.ProductID;
    		}
    		else if (!skipKeys)
    		{
    			ProductID = null;
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
    
    	private void FixupProductType(ProductType previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.DynamicKitGroupRules.Contains(this))
    		{
    			previousValue.DynamicKitGroupRules.Remove(this);
    		}
    
    		if (ProductType != null)
    		{
    			if (!ProductType.DynamicKitGroupRules.Contains(this))
    			{
    				ProductType.DynamicKitGroupRules.Add(this);
    			}
    
    			ProductTypeID = ProductType.ProductTypeID;
    		}
    		else if (!skipKeys)
    		{
    			ProductTypeID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("ProductType")
    				&& (ChangeTracker.OriginalValues["ProductType"] == ProductType))
    			{
    				ChangeTracker.OriginalValues.Remove("ProductType");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("ProductType", previousValue);
    			}
    			if (ProductType != null && !ProductType.ChangeTracker.ChangeTrackingEnabled)
    			{
    				ProductType.StartTracking();
    			}
    		}
    	}

        #endregion
    }
}