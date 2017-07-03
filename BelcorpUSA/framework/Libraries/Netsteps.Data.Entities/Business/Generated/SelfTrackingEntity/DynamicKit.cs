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
    [KnownType(typeof(DynamicKitPricingType))]
    [Serializable]
    public partial class DynamicKit: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void DynamicKitIDChanged();
    	public int DynamicKitID
    	{
    		get { return _dynamicKitID; }
    		set
    		{
    			if (_dynamicKitID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'DynamicKitID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_dynamicKitID = value;
    				DynamicKitIDChanged();
    				OnPropertyChanged("DynamicKitID");
    			}
    		}
    	}
    	private int _dynamicKitID;
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
    	partial void DynamicKitPricingTypeIDChanged();
    	public Nullable<int> DynamicKitPricingTypeID
    	{
    		get { return _dynamicKitPricingTypeID; }
    		set
    		{
    			if (_dynamicKitPricingTypeID != value)
    			{
    				ChangeTracker.RecordOriginalValue("DynamicKitPricingTypeID", _dynamicKitPricingTypeID);
    				if (!IsDeserializing)
    				{
    					if (DynamicKitPricingType != null && DynamicKitPricingType.DynamicKitPricingTypeID != value)
    					{
    						DynamicKitPricingType = null;
    					}
    				}
    				_dynamicKitPricingTypeID = value;
    				DynamicKitPricingTypeIDChanged();
    				OnPropertyChanged("DynamicKitPricingTypeID");
    			}
    		}
    	}
    	private Nullable<int> _dynamicKitPricingTypeID;

        #endregion
        #region Navigation Properties
    
    	public TrackableCollection<DynamicKitGroup> DynamicKitGroups
    	{
    		get
    		{
    			if (_dynamicKitGroups == null)
    			{
    				_dynamicKitGroups = new TrackableCollection<DynamicKitGroup>();
    				_dynamicKitGroups.CollectionChanged += FixupDynamicKitGroups;
    				_dynamicKitGroups.CollectionChanged += RaiseDynamicKitGroupsChanged;
    			}
    			return _dynamicKitGroups;
    		}
    		set
    		{
    			if (!ReferenceEquals(_dynamicKitGroups, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_dynamicKitGroups != null)
    				{
    					_dynamicKitGroups.CollectionChanged -= FixupDynamicKitGroups;
    					_dynamicKitGroups.CollectionChanged -= RaiseDynamicKitGroupsChanged;
    				}
    				_dynamicKitGroups = value;
    				if (_dynamicKitGroups != null)
    				{
    					_dynamicKitGroups.CollectionChanged += FixupDynamicKitGroups;
    					_dynamicKitGroups.CollectionChanged += RaiseDynamicKitGroupsChanged;
    				}
    				OnNavigationPropertyChanged("DynamicKitGroups");
    			}
    		}
    	}
    	private TrackableCollection<DynamicKitGroup> _dynamicKitGroups;
    	partial void DynamicKitGroupsChanged();
    	private void RaiseDynamicKitGroupsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		DynamicKitGroupsChanged();
    	}
    
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
    
    	public DynamicKitPricingType DynamicKitPricingType
    	{
    		get { return _dynamicKitPricingType; }
    		set
    		{
    			if (!ReferenceEquals(_dynamicKitPricingType, value))
    			{
    				var previousValue = _dynamicKitPricingType;
    				_dynamicKitPricingType = value;
    				FixupDynamicKitPricingType(previousValue);
    				OnNavigationPropertyChanged("DynamicKitPricingType");
    			}
    		}
    	}
    	private DynamicKitPricingType _dynamicKitPricingType;

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
    		if (_dynamicKitGroups != null)
    		{
    			_dynamicKitGroups.CollectionChanged -= FixupDynamicKitGroups;
    			_dynamicKitGroups.CollectionChanged -= RaiseDynamicKitGroupsChanged;
    			_dynamicKitGroups.CollectionChanged += FixupDynamicKitGroups;
    			_dynamicKitGroups.CollectionChanged += RaiseDynamicKitGroupsChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		DynamicKitGroups.Clear();
    		Product = null;
    		DynamicKitPricingType = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupProduct(Product previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.DynamicKits.Contains(this))
    		{
    			previousValue.DynamicKits.Remove(this);
    		}
    
    		if (Product != null)
    		{
    			if (!Product.DynamicKits.Contains(this))
    			{
    				Product.DynamicKits.Add(this);
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
    
    	private void FixupDynamicKitPricingType(DynamicKitPricingType previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.DynamicKits.Contains(this))
    		{
    			previousValue.DynamicKits.Remove(this);
    		}
    
    		if (DynamicKitPricingType != null)
    		{
    			if (!DynamicKitPricingType.DynamicKits.Contains(this))
    			{
    				DynamicKitPricingType.DynamicKits.Add(this);
    			}
    
    			DynamicKitPricingTypeID = DynamicKitPricingType.DynamicKitPricingTypeID;
    		}
    		else if (!skipKeys)
    		{
    			DynamicKitPricingTypeID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("DynamicKitPricingType")
    				&& (ChangeTracker.OriginalValues["DynamicKitPricingType"] == DynamicKitPricingType))
    			{
    				ChangeTracker.OriginalValues.Remove("DynamicKitPricingType");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("DynamicKitPricingType", previousValue);
    			}
    			if (DynamicKitPricingType != null && !DynamicKitPricingType.ChangeTracker.ChangeTrackingEnabled)
    			{
    				DynamicKitPricingType.StartTracking();
    			}
    		}
    	}
    
    	private void FixupDynamicKitGroups(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (DynamicKitGroup item in e.NewItems)
    			{
    				item.DynamicKit = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("DynamicKitGroups", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (DynamicKitGroup item in e.OldItems)
    			{
    				if (ReferenceEquals(item.DynamicKit, this))
    				{
    					item.DynamicKit = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("DynamicKitGroups", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}