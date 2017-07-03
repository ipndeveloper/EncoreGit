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
    [KnownType(typeof(ProductBase))]
    [KnownType(typeof(Product))]
    [KnownType(typeof(TaxCache))]
    [KnownType(typeof(TaxCacheOverride))]
    [Serializable]
    public partial class TaxCategory: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void TaxCategoryIDChanged();
    	public int TaxCategoryID
    	{
    		get { return _taxCategoryID; }
    		set
    		{
    			if (_taxCategoryID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'TaxCategoryID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_taxCategoryID = value;
    				TaxCategoryIDChanged();
    				OnPropertyChanged("TaxCategoryID");
    			}
    		}
    	}
    	private int _taxCategoryID;
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
    	partial void IsDefaultChanged();
    	public string IsDefault
    	{
    		get { return _isDefault; }
    		set
    		{
    			if (_isDefault != value)
    			{
    				ChangeTracker.RecordOriginalValue("IsDefault", _isDefault);
    				_isDefault = value;
    				IsDefaultChanged();
    				OnPropertyChanged("IsDefault");
    			}
    		}
    	}
    	private string _isDefault;
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
    
    	public TrackableCollection<ProductBase> ProductBases
    	{
    		get
    		{
    			if (_productBases == null)
    			{
    				_productBases = new TrackableCollection<ProductBase>();
    				_productBases.CollectionChanged += FixupProductBases;
    				_productBases.CollectionChanged += RaiseProductBasesChanged;
    			}
    			return _productBases;
    		}
    		set
    		{
    			if (!ReferenceEquals(_productBases, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_productBases != null)
    				{
    					_productBases.CollectionChanged -= FixupProductBases;
    					_productBases.CollectionChanged -= RaiseProductBasesChanged;
    				}
    				_productBases = value;
    				if (_productBases != null)
    				{
    					_productBases.CollectionChanged += FixupProductBases;
    					_productBases.CollectionChanged += RaiseProductBasesChanged;
    				}
    				OnNavigationPropertyChanged("ProductBases");
    			}
    		}
    	}
    	private TrackableCollection<ProductBase> _productBases;
    	partial void ProductBasesChanged();
    	private void RaiseProductBasesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		ProductBasesChanged();
    	}
    
    	public TrackableCollection<Product> Products
    	{
    		get
    		{
    			if (_products == null)
    			{
    				_products = new TrackableCollection<Product>();
    				_products.CollectionChanged += FixupProducts;
    				_products.CollectionChanged += RaiseProductsChanged;
    			}
    			return _products;
    		}
    		set
    		{
    			if (!ReferenceEquals(_products, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_products != null)
    				{
    					_products.CollectionChanged -= FixupProducts;
    					_products.CollectionChanged -= RaiseProductsChanged;
    				}
    				_products = value;
    				if (_products != null)
    				{
    					_products.CollectionChanged += FixupProducts;
    					_products.CollectionChanged += RaiseProductsChanged;
    				}
    				OnNavigationPropertyChanged("Products");
    			}
    		}
    	}
    	private TrackableCollection<Product> _products;
    	partial void ProductsChanged();
    	private void RaiseProductsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		ProductsChanged();
    	}
    
    	public TrackableCollection<TaxCache> TaxCaches
    	{
    		get
    		{
    			if (_taxCaches == null)
    			{
    				_taxCaches = new TrackableCollection<TaxCache>();
    				_taxCaches.CollectionChanged += FixupTaxCaches;
    				_taxCaches.CollectionChanged += RaiseTaxCachesChanged;
    			}
    			return _taxCaches;
    		}
    		set
    		{
    			if (!ReferenceEquals(_taxCaches, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_taxCaches != null)
    				{
    					_taxCaches.CollectionChanged -= FixupTaxCaches;
    					_taxCaches.CollectionChanged -= RaiseTaxCachesChanged;
    				}
    				_taxCaches = value;
    				if (_taxCaches != null)
    				{
    					_taxCaches.CollectionChanged += FixupTaxCaches;
    					_taxCaches.CollectionChanged += RaiseTaxCachesChanged;
    				}
    				OnNavigationPropertyChanged("TaxCaches");
    			}
    		}
    	}
    	private TrackableCollection<TaxCache> _taxCaches;
    	partial void TaxCachesChanged();
    	private void RaiseTaxCachesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		TaxCachesChanged();
    	}
    
    	public TrackableCollection<TaxCacheOverride> TaxCacheOverrides
    	{
    		get
    		{
    			if (_taxCacheOverrides == null)
    			{
    				_taxCacheOverrides = new TrackableCollection<TaxCacheOverride>();
    				_taxCacheOverrides.CollectionChanged += FixupTaxCacheOverrides;
    				_taxCacheOverrides.CollectionChanged += RaiseTaxCacheOverridesChanged;
    			}
    			return _taxCacheOverrides;
    		}
    		set
    		{
    			if (!ReferenceEquals(_taxCacheOverrides, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_taxCacheOverrides != null)
    				{
    					_taxCacheOverrides.CollectionChanged -= FixupTaxCacheOverrides;
    					_taxCacheOverrides.CollectionChanged -= RaiseTaxCacheOverridesChanged;
    				}
    				_taxCacheOverrides = value;
    				if (_taxCacheOverrides != null)
    				{
    					_taxCacheOverrides.CollectionChanged += FixupTaxCacheOverrides;
    					_taxCacheOverrides.CollectionChanged += RaiseTaxCacheOverridesChanged;
    				}
    				OnNavigationPropertyChanged("TaxCacheOverrides");
    			}
    		}
    	}
    	private TrackableCollection<TaxCacheOverride> _taxCacheOverrides;
    	partial void TaxCacheOverridesChanged();
    	private void RaiseTaxCacheOverridesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		TaxCacheOverridesChanged();
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
    		if (_productBases != null)
    		{
    			_productBases.CollectionChanged -= FixupProductBases;
    			_productBases.CollectionChanged -= RaiseProductBasesChanged;
    			_productBases.CollectionChanged += FixupProductBases;
    			_productBases.CollectionChanged += RaiseProductBasesChanged;
    		}
    		if (_products != null)
    		{
    			_products.CollectionChanged -= FixupProducts;
    			_products.CollectionChanged -= RaiseProductsChanged;
    			_products.CollectionChanged += FixupProducts;
    			_products.CollectionChanged += RaiseProductsChanged;
    		}
    		if (_taxCaches != null)
    		{
    			_taxCaches.CollectionChanged -= FixupTaxCaches;
    			_taxCaches.CollectionChanged -= RaiseTaxCachesChanged;
    			_taxCaches.CollectionChanged += FixupTaxCaches;
    			_taxCaches.CollectionChanged += RaiseTaxCachesChanged;
    		}
    		if (_taxCacheOverrides != null)
    		{
    			_taxCacheOverrides.CollectionChanged -= FixupTaxCacheOverrides;
    			_taxCacheOverrides.CollectionChanged -= RaiseTaxCacheOverridesChanged;
    			_taxCacheOverrides.CollectionChanged += FixupTaxCacheOverrides;
    			_taxCacheOverrides.CollectionChanged += RaiseTaxCacheOverridesChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		ProductBases.Clear();
    		Products.Clear();
    		TaxCaches.Clear();
    		TaxCacheOverrides.Clear();
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupProductBases(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (ProductBase item in e.NewItems)
    			{
    				item.TaxCategory = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("ProductBases", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (ProductBase item in e.OldItems)
    			{
    				if (ReferenceEquals(item.TaxCategory, this))
    				{
    					item.TaxCategory = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("ProductBases", item);
    				}
    			}
    		}
    	}
    
    	private void FixupProducts(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (Product item in e.NewItems)
    			{
    				if (!item.TaxCategories.Contains(this))
    				{
    					item.TaxCategories.Add(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Products", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (Product item in e.OldItems)
    			{
    				if (item.TaxCategories.Contains(this))
    				{
    					item.TaxCategories.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Products", item);
    				}
    			}
    		}
    	}
    
    	private void FixupTaxCaches(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (TaxCache item in e.NewItems)
    			{
    				item.TaxCategory = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("TaxCaches", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (TaxCache item in e.OldItems)
    			{
    				if (ReferenceEquals(item.TaxCategory, this))
    				{
    					item.TaxCategory = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("TaxCaches", item);
    				}
    			}
    		}
    	}
    
    	private void FixupTaxCacheOverrides(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (TaxCacheOverride item in e.NewItems)
    			{
    				item.TaxCategory = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("TaxCacheOverrides", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (TaxCacheOverride item in e.OldItems)
    			{
    				if (ReferenceEquals(item.TaxCategory, this))
    				{
    					item.TaxCategory = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("TaxCacheOverrides", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}