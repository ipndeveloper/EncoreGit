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
    [KnownType(typeof(OrderShipment))]
    [KnownType(typeof(DescriptionTranslation))]
    [KnownType(typeof(OrderShipmentPackage))]
    [KnownType(typeof(Product))]
    [Serializable]
    public partial class ShippingMethod: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void ShippingMethodIDChanged();
    	public int ShippingMethodID
    	{
    		get { return _shippingMethodID; }
    		set
    		{
    			if (_shippingMethodID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'ShippingMethodID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_shippingMethodID = value;
    				ShippingMethodIDChanged();
    				OnPropertyChanged("ShippingMethodID");
    			}
    		}
    	}
    	private int _shippingMethodID;
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
    	partial void ShortNameChanged();
    	public string ShortName
    	{
    		get { return _shortName; }
    		set
    		{
    			if (_shortName != value)
    			{
    				ChangeTracker.RecordOriginalValue("ShortName", _shortName);
    				_shortName = value;
    				ShortNameChanged();
    				OnPropertyChanged("ShortName");
    			}
    		}
    	}
    	private string _shortName;
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
    	partial void IsWillCallChanged();
    	public Nullable<bool> IsWillCall
    	{
    		get { return _isWillCall; }
    		set
    		{
    			if (_isWillCall != value)
    			{
    				ChangeTracker.RecordOriginalValue("IsWillCall", _isWillCall);
    				_isWillCall = value;
    				IsWillCallChanged();
    				OnPropertyChanged("IsWillCall");
    			}
    		}
    	}
    	private Nullable<bool> _isWillCall;
    	partial void SortIndexChanged();
    	public Nullable<byte> SortIndex
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
    	private Nullable<byte> _sortIndex;
    	partial void TrackingNumberBaseUrlChanged();
    	public string TrackingNumberBaseUrl
    	{
    		get { return _trackingNumberBaseUrl; }
    		set
    		{
    			if (_trackingNumberBaseUrl != value)
    			{
    				ChangeTracker.RecordOriginalValue("TrackingNumberBaseUrl", _trackingNumberBaseUrl);
    				_trackingNumberBaseUrl = value;
    				TrackingNumberBaseUrlChanged();
    				OnPropertyChanged("TrackingNumberBaseUrl");
    			}
    		}
    	}
    	private string _trackingNumberBaseUrl;
    	partial void AllowPoBoxChanged();
    	public bool AllowPoBox
    	{
    		get { return _allowPoBox; }
    		set
    		{
    			if (_allowPoBox != value)
    			{
    				ChangeTracker.RecordOriginalValue("AllowPoBox", _allowPoBox);
    				_allowPoBox = value;
    				AllowPoBoxChanged();
    				OnPropertyChanged("AllowPoBox");
    			}
    		}
    	}
    	private bool _allowPoBox;

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
    
    	public TrackableCollection<DescriptionTranslation> Translations
    	{
    		get
    		{
    			if (_translations == null)
    			{
    				_translations = new TrackableCollection<DescriptionTranslation>();
    				_translations.CollectionChanged += FixupTranslations;
    				_translations.CollectionChanged += RaiseTranslationsChanged;
    			}
    			return _translations;
    		}
    		set
    		{
    			if (!ReferenceEquals(_translations, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_translations != null)
    				{
    					_translations.CollectionChanged -= FixupTranslations;
    					_translations.CollectionChanged -= RaiseTranslationsChanged;
    				}
    				_translations = value;
    				if (_translations != null)
    				{
    					_translations.CollectionChanged += FixupTranslations;
    					_translations.CollectionChanged += RaiseTranslationsChanged;
    				}
    				OnNavigationPropertyChanged("Translations");
    			}
    		}
    	}
    	private TrackableCollection<DescriptionTranslation> _translations;
    	partial void TranslationsChanged();
    	private void RaiseTranslationsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		TranslationsChanged();
    	}
    
    	public TrackableCollection<OrderShipmentPackage> OrderShipmentPackages
    	{
    		get
    		{
    			if (_orderShipmentPackages == null)
    			{
    				_orderShipmentPackages = new TrackableCollection<OrderShipmentPackage>();
    				_orderShipmentPackages.CollectionChanged += FixupOrderShipmentPackages;
    				_orderShipmentPackages.CollectionChanged += RaiseOrderShipmentPackagesChanged;
    			}
    			return _orderShipmentPackages;
    		}
    		set
    		{
    			if (!ReferenceEquals(_orderShipmentPackages, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_orderShipmentPackages != null)
    				{
    					_orderShipmentPackages.CollectionChanged -= FixupOrderShipmentPackages;
    					_orderShipmentPackages.CollectionChanged -= RaiseOrderShipmentPackagesChanged;
    				}
    				_orderShipmentPackages = value;
    				if (_orderShipmentPackages != null)
    				{
    					_orderShipmentPackages.CollectionChanged += FixupOrderShipmentPackages;
    					_orderShipmentPackages.CollectionChanged += RaiseOrderShipmentPackagesChanged;
    				}
    				OnNavigationPropertyChanged("OrderShipmentPackages");
    			}
    		}
    	}
    	private TrackableCollection<OrderShipmentPackage> _orderShipmentPackages;
    	partial void OrderShipmentPackagesChanged();
    	private void RaiseOrderShipmentPackagesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		OrderShipmentPackagesChanged();
    	}
    
    	public TrackableCollection<Product> ExcludedProducts
    	{
    		get
    		{
    			if (_excludedProducts == null)
    			{
    				_excludedProducts = new TrackableCollection<Product>();
    				_excludedProducts.CollectionChanged += FixupExcludedProducts;
    				_excludedProducts.CollectionChanged += RaiseExcludedProductsChanged;
    			}
    			return _excludedProducts;
    		}
    		set
    		{
    			if (!ReferenceEquals(_excludedProducts, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_excludedProducts != null)
    				{
    					_excludedProducts.CollectionChanged -= FixupExcludedProducts;
    					_excludedProducts.CollectionChanged -= RaiseExcludedProductsChanged;
    				}
    				_excludedProducts = value;
    				if (_excludedProducts != null)
    				{
    					_excludedProducts.CollectionChanged += FixupExcludedProducts;
    					_excludedProducts.CollectionChanged += RaiseExcludedProductsChanged;
    				}
    				OnNavigationPropertyChanged("ExcludedProducts");
    			}
    		}
    	}
    	private TrackableCollection<Product> _excludedProducts;
    	partial void ExcludedProductsChanged();
    	private void RaiseExcludedProductsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		ExcludedProductsChanged();
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
    		if (_shippingOrderTypes != null)
    		{
    			_shippingOrderTypes.CollectionChanged -= FixupShippingOrderTypes;
    			_shippingOrderTypes.CollectionChanged -= RaiseShippingOrderTypesChanged;
    			_shippingOrderTypes.CollectionChanged += FixupShippingOrderTypes;
    			_shippingOrderTypes.CollectionChanged += RaiseShippingOrderTypesChanged;
    		}
    		if (_orderShipments != null)
    		{
    			_orderShipments.CollectionChanged -= FixupOrderShipments;
    			_orderShipments.CollectionChanged -= RaiseOrderShipmentsChanged;
    			_orderShipments.CollectionChanged += FixupOrderShipments;
    			_orderShipments.CollectionChanged += RaiseOrderShipmentsChanged;
    		}
    		if (_translations != null)
    		{
    			_translations.CollectionChanged -= FixupTranslations;
    			_translations.CollectionChanged -= RaiseTranslationsChanged;
    			_translations.CollectionChanged += FixupTranslations;
    			_translations.CollectionChanged += RaiseTranslationsChanged;
    		}
    		if (_orderShipmentPackages != null)
    		{
    			_orderShipmentPackages.CollectionChanged -= FixupOrderShipmentPackages;
    			_orderShipmentPackages.CollectionChanged -= RaiseOrderShipmentPackagesChanged;
    			_orderShipmentPackages.CollectionChanged += FixupOrderShipmentPackages;
    			_orderShipmentPackages.CollectionChanged += RaiseOrderShipmentPackagesChanged;
    		}
    		if (_excludedProducts != null)
    		{
    			_excludedProducts.CollectionChanged -= FixupExcludedProducts;
    			_excludedProducts.CollectionChanged -= RaiseExcludedProductsChanged;
    			_excludedProducts.CollectionChanged += FixupExcludedProducts;
    			_excludedProducts.CollectionChanged += RaiseExcludedProductsChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		ShippingOrderTypes.Clear();
    		OrderShipments.Clear();
    		Translations.Clear();
    		OrderShipmentPackages.Clear();
    		ExcludedProducts.Clear();
    	}

        #endregion
        #region Association Fixup
    
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
    				item.ShippingMethod = this;
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
    				if (ReferenceEquals(item.ShippingMethod, this))
    				{
    					item.ShippingMethod = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("ShippingOrderTypes", item);
    				}
    			}
    		}
    	}
    
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
    				item.ShippingMethod = this;
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
    				if (ReferenceEquals(item.ShippingMethod, this))
    				{
    					item.ShippingMethod = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("OrderShipments", item);
    				}
    			}
    		}
    	}
    
    	private void FixupTranslations(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (DescriptionTranslation item in e.NewItems)
    			{
    				if (!item.ShippingMethods.Contains(this))
    				{
    					item.ShippingMethods.Add(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Translations", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (DescriptionTranslation item in e.OldItems)
    			{
    				if (item.ShippingMethods.Contains(this))
    				{
    					item.ShippingMethods.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Translations", item);
    				}
    			}
    		}
    	}
    
    	private void FixupOrderShipmentPackages(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (OrderShipmentPackage item in e.NewItems)
    			{
    				item.ShippingMethod = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("OrderShipmentPackages", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (OrderShipmentPackage item in e.OldItems)
    			{
    				if (ReferenceEquals(item.ShippingMethod, this))
    				{
    					item.ShippingMethod = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("OrderShipmentPackages", item);
    				}
    			}
    		}
    	}
    
    	private void FixupExcludedProducts(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (Product item in e.NewItems)
    			{
    				if (!item.ExcludedShippingMethods.Contains(this))
    				{
    					item.ExcludedShippingMethods.Add(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("ExcludedProducts", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (Product item in e.OldItems)
    			{
    				if (item.ExcludedShippingMethods.Contains(this))
    				{
    					item.ExcludedShippingMethods.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("ExcludedProducts", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}
