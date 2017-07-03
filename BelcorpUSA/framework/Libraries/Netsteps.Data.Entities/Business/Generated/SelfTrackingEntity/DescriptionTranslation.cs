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
    [KnownType(typeof(Language))]
    [KnownType(typeof(Catalog))]
    [KnownType(typeof(ProductBase))]
    [KnownType(typeof(Product))]
    [KnownType(typeof(Archive))]
    [KnownType(typeof(ShippingMethod))]
    [KnownType(typeof(DynamicKitGroup))]
    [KnownType(typeof(Brand))]
    [Serializable]
    public partial class DescriptionTranslation: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void DescriptionTranslationIDChanged();
    	public int DescriptionTranslationID
    	{
    		get { return _descriptionTranslationID; }
    		set
    		{
    			if (_descriptionTranslationID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'DescriptionTranslationID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_descriptionTranslationID = value;
    				DescriptionTranslationIDChanged();
    				OnPropertyChanged("DescriptionTranslationID");
    			}
    		}
    	}
    	private int _descriptionTranslationID;
    	partial void LanguageIDChanged();
    	public int LanguageID
    	{
    		get { return _languageID; }
    		set
    		{
    			if (_languageID != value)
    			{
    				ChangeTracker.RecordOriginalValue("LanguageID", _languageID);
    				if (!IsDeserializing)
    				{
    					if (Language != null && Language.LanguageID != value)
    					{
    						Language = null;
    					}
    				}
    				_languageID = value;
    				LanguageIDChanged();
    				OnPropertyChanged("LanguageID");
    			}
    		}
    	}
    	private int _languageID;
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
    	partial void ShortDescriptionChanged();
    	public string ShortDescription
    	{
    		get { return _shortDescription; }
    		set
    		{
    			if (_shortDescription != value)
    			{
    				ChangeTracker.RecordOriginalValue("ShortDescription", _shortDescription);
    				_shortDescription = value;
    				ShortDescriptionChanged();
    				OnPropertyChanged("ShortDescription");
    			}
    		}
    	}
    	private string _shortDescription;
    	partial void LongDescriptionChanged();
    	public string LongDescription
    	{
    		get { return _longDescription; }
    		set
    		{
    			if (_longDescription != value)
    			{
    				ChangeTracker.RecordOriginalValue("LongDescription", _longDescription);
    				_longDescription = value;
    				LongDescriptionChanged();
    				OnPropertyChanged("LongDescription");
    			}
    		}
    	}
    	private string _longDescription;
    	partial void ImagePathChanged();
    	public string ImagePath
    	{
    		get { return _imagePath; }
    		set
    		{
    			if (_imagePath != value)
    			{
    				ChangeTracker.RecordOriginalValue("ImagePath", _imagePath);
    				_imagePath = value;
    				ImagePathChanged();
    				OnPropertyChanged("ImagePath");
    			}
    		}
    	}
    	private string _imagePath;
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
    
    	public Language Language
    	{
    		get { return _language; }
    		set
    		{
    			if (!ReferenceEquals(_language, value))
    			{
    				var previousValue = _language;
    				_language = value;
    				FixupLanguage(previousValue);
    				OnNavigationPropertyChanged("Language");
    			}
    		}
    	}
    	private Language _language;
    
    	public TrackableCollection<Catalog> Catalogs
    	{
    		get
    		{
    			if (_catalogs == null)
    			{
    				_catalogs = new TrackableCollection<Catalog>();
    				_catalogs.CollectionChanged += FixupCatalogs;
    				_catalogs.CollectionChanged += RaiseCatalogsChanged;
    			}
    			return _catalogs;
    		}
    		set
    		{
    			if (!ReferenceEquals(_catalogs, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_catalogs != null)
    				{
    					_catalogs.CollectionChanged -= FixupCatalogs;
    					_catalogs.CollectionChanged -= RaiseCatalogsChanged;
    				}
    				_catalogs = value;
    				if (_catalogs != null)
    				{
    					_catalogs.CollectionChanged += FixupCatalogs;
    					_catalogs.CollectionChanged += RaiseCatalogsChanged;
    				}
    				OnNavigationPropertyChanged("Catalogs");
    			}
    		}
    	}
    	private TrackableCollection<Catalog> _catalogs;
    	partial void CatalogsChanged();
    	private void RaiseCatalogsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		CatalogsChanged();
    	}
    
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
    
    	public TrackableCollection<Archive> Archives
    	{
    		get
    		{
    			if (_archives == null)
    			{
    				_archives = new TrackableCollection<Archive>();
    				_archives.CollectionChanged += FixupArchives;
    				_archives.CollectionChanged += RaiseArchivesChanged;
    			}
    			return _archives;
    		}
    		set
    		{
    			if (!ReferenceEquals(_archives, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_archives != null)
    				{
    					_archives.CollectionChanged -= FixupArchives;
    					_archives.CollectionChanged -= RaiseArchivesChanged;
    				}
    				_archives = value;
    				if (_archives != null)
    				{
    					_archives.CollectionChanged += FixupArchives;
    					_archives.CollectionChanged += RaiseArchivesChanged;
    				}
    				OnNavigationPropertyChanged("Archives");
    			}
    		}
    	}
    	private TrackableCollection<Archive> _archives;
    	partial void ArchivesChanged();
    	private void RaiseArchivesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		ArchivesChanged();
    	}
    
    	public TrackableCollection<ShippingMethod> ShippingMethods
    	{
    		get
    		{
    			if (_shippingMethods == null)
    			{
    				_shippingMethods = new TrackableCollection<ShippingMethod>();
    				_shippingMethods.CollectionChanged += FixupShippingMethods;
    				_shippingMethods.CollectionChanged += RaiseShippingMethodsChanged;
    			}
    			return _shippingMethods;
    		}
    		set
    		{
    			if (!ReferenceEquals(_shippingMethods, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_shippingMethods != null)
    				{
    					_shippingMethods.CollectionChanged -= FixupShippingMethods;
    					_shippingMethods.CollectionChanged -= RaiseShippingMethodsChanged;
    				}
    				_shippingMethods = value;
    				if (_shippingMethods != null)
    				{
    					_shippingMethods.CollectionChanged += FixupShippingMethods;
    					_shippingMethods.CollectionChanged += RaiseShippingMethodsChanged;
    				}
    				OnNavigationPropertyChanged("ShippingMethods");
    			}
    		}
    	}
    	private TrackableCollection<ShippingMethod> _shippingMethods;
    	partial void ShippingMethodsChanged();
    	private void RaiseShippingMethodsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		ShippingMethodsChanged();
    	}
    
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
    
    	public TrackableCollection<Brand> Brands
    	{
    		get
    		{
    			if (_brands == null)
    			{
    				_brands = new TrackableCollection<Brand>();
    				_brands.CollectionChanged += FixupBrands;
    				_brands.CollectionChanged += RaiseBrandsChanged;
    			}
    			return _brands;
    		}
    		set
    		{
    			if (!ReferenceEquals(_brands, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_brands != null)
    				{
    					_brands.CollectionChanged -= FixupBrands;
    					_brands.CollectionChanged -= RaiseBrandsChanged;
    				}
    				_brands = value;
    				if (_brands != null)
    				{
    					_brands.CollectionChanged += FixupBrands;
    					_brands.CollectionChanged += RaiseBrandsChanged;
    				}
    				OnNavigationPropertyChanged("Brands");
    			}
    		}
    	}
    	private TrackableCollection<Brand> _brands;
    	partial void BrandsChanged();
    	private void RaiseBrandsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		BrandsChanged();
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
    		if (_catalogs != null)
    		{
    			_catalogs.CollectionChanged -= FixupCatalogs;
    			_catalogs.CollectionChanged -= RaiseCatalogsChanged;
    			_catalogs.CollectionChanged += FixupCatalogs;
    			_catalogs.CollectionChanged += RaiseCatalogsChanged;
    		}
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
    		if (_archives != null)
    		{
    			_archives.CollectionChanged -= FixupArchives;
    			_archives.CollectionChanged -= RaiseArchivesChanged;
    			_archives.CollectionChanged += FixupArchives;
    			_archives.CollectionChanged += RaiseArchivesChanged;
    		}
    		if (_shippingMethods != null)
    		{
    			_shippingMethods.CollectionChanged -= FixupShippingMethods;
    			_shippingMethods.CollectionChanged -= RaiseShippingMethodsChanged;
    			_shippingMethods.CollectionChanged += FixupShippingMethods;
    			_shippingMethods.CollectionChanged += RaiseShippingMethodsChanged;
    		}
    		if (_dynamicKitGroups != null)
    		{
    			_dynamicKitGroups.CollectionChanged -= FixupDynamicKitGroups;
    			_dynamicKitGroups.CollectionChanged -= RaiseDynamicKitGroupsChanged;
    			_dynamicKitGroups.CollectionChanged += FixupDynamicKitGroups;
    			_dynamicKitGroups.CollectionChanged += RaiseDynamicKitGroupsChanged;
    		}
    		if (_brands != null)
    		{
    			_brands.CollectionChanged -= FixupBrands;
    			_brands.CollectionChanged -= RaiseBrandsChanged;
    			_brands.CollectionChanged += FixupBrands;
    			_brands.CollectionChanged += RaiseBrandsChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		Language = null;
    		Catalogs.Clear();
    		ProductBases.Clear();
    		Products.Clear();
    		Archives.Clear();
    		ShippingMethods.Clear();
    		DynamicKitGroups.Clear();
    		Brands.Clear();
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupLanguage(Language previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.DescriptionTranslations.Contains(this))
    		{
    			previousValue.DescriptionTranslations.Remove(this);
    		}
    
    		if (Language != null)
    		{
    			if (!Language.DescriptionTranslations.Contains(this))
    			{
    				Language.DescriptionTranslations.Add(this);
    			}
    
    			LanguageID = Language.LanguageID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Language")
    				&& (ChangeTracker.OriginalValues["Language"] == Language))
    			{
    				ChangeTracker.OriginalValues.Remove("Language");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Language", previousValue);
    			}
    			if (Language != null && !Language.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Language.StartTracking();
    			}
    		}
    	}
    
    	private void FixupCatalogs(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (Catalog item in e.NewItems)
    			{
    				if (!item.Translations.Contains(this))
    				{
    					item.Translations.Add(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Catalogs", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (Catalog item in e.OldItems)
    			{
    				if (item.Translations.Contains(this))
    				{
    					item.Translations.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Catalogs", item);
    				}
    			}
    		}
    	}
    
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
    				if (!item.Translations.Contains(this))
    				{
    					item.Translations.Add(this);
    				}
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
    				if (item.Translations.Contains(this))
    				{
    					item.Translations.Remove(this);
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
    				if (!item.Translations.Contains(this))
    				{
    					item.Translations.Add(this);
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
    				if (item.Translations.Contains(this))
    				{
    					item.Translations.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Products", item);
    				}
    			}
    		}
    	}
    
    	private void FixupArchives(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (Archive item in e.NewItems)
    			{
    				if (!item.Translations.Contains(this))
    				{
    					item.Translations.Add(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Archives", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (Archive item in e.OldItems)
    			{
    				if (item.Translations.Contains(this))
    				{
    					item.Translations.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Archives", item);
    				}
    			}
    		}
    	}
    
    	private void FixupShippingMethods(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (ShippingMethod item in e.NewItems)
    			{
    				if (!item.Translations.Contains(this))
    				{
    					item.Translations.Add(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("ShippingMethods", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (ShippingMethod item in e.OldItems)
    			{
    				if (item.Translations.Contains(this))
    				{
    					item.Translations.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("ShippingMethods", item);
    				}
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
    				if (!item.Translations.Contains(this))
    				{
    					item.Translations.Add(this);
    				}
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
    				if (item.Translations.Contains(this))
    				{
    					item.Translations.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("DynamicKitGroups", item);
    				}
    			}
    		}
    	}
    
    	private void FixupBrands(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (Brand item in e.NewItems)
    			{
    				if (!item.DescriptionTranslations.Contains(this))
    				{
    					item.DescriptionTranslations.Add(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Brands", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (Brand item in e.OldItems)
    			{
    				if (item.DescriptionTranslations.Contains(this))
    				{
    					item.DescriptionTranslations.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Brands", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}
