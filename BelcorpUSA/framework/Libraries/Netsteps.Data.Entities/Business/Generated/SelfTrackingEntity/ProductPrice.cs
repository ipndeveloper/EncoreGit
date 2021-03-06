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
    [KnownType(typeof(Currency))]
    [KnownType(typeof(ProductPriceType))]
    [KnownType(typeof(Product))]
    [KnownType(typeof(User))]
    [KnownType(typeof(Catalog))]
    [Serializable]
    public partial class ProductPrice: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void ProductPriceIDChanged();
    	public int ProductPriceID
    	{
    		get { return _productPriceID; }
    		set
    		{
    			if (_productPriceID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'ProductPriceID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_productPriceID = value;
    				ProductPriceIDChanged();
    				OnPropertyChanged("ProductPriceID");
    			}
    		}
    	}
    	private int _productPriceID;
    	partial void ProductPriceTypeIDChanged();
    	public int ProductPriceTypeID
    	{
    		get { return _productPriceTypeID; }
    		set
    		{
    			if (_productPriceTypeID != value)
    			{
    				ChangeTracker.RecordOriginalValue("ProductPriceTypeID", _productPriceTypeID);
    				if (!IsDeserializing)
    				{
    					if (ProductPriceType != null && ProductPriceType.ProductPriceTypeID != value)
    					{
    						ProductPriceType = null;
    					}
    				}
    				_productPriceTypeID = value;
    				ProductPriceTypeIDChanged();
    				OnPropertyChanged("ProductPriceTypeID");
    			}
    		}
    	}
    	private int _productPriceTypeID;
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
    	partial void CurrencyIDChanged();
    	public int CurrencyID
    	{
    		get { return _currencyID; }
    		set
    		{
    			if (_currencyID != value)
    			{
    				ChangeTracker.RecordOriginalValue("CurrencyID", _currencyID);
    				if (!IsDeserializing)
    				{
    					if (Currency != null && Currency.CurrencyID != value)
    					{
    						Currency = null;
    					}
    				}
    				_currencyID = value;
    				CurrencyIDChanged();
    				OnPropertyChanged("CurrencyID");
    			}
    		}
    	}
    	private int _currencyID;
    	partial void PriceChanged();
    	public decimal Price
    	{
    		get { return _price; }
    		set
    		{
    			if (_price != value)
    			{
    				ChangeTracker.RecordOriginalValue("Price", _price);
    				_price = value;
    				PriceChanged();
    				OnPropertyChanged("Price");
    			}
    		}
    	}
    	private decimal _price;
    	partial void ModifiedByUserIDChanged();
    	public Nullable<int> ModifiedByUserID
    	{
    		get { return _modifiedByUserID; }
    		set
    		{
    			if (_modifiedByUserID != value)
    			{
    				ChangeTracker.RecordOriginalValue("ModifiedByUserID", _modifiedByUserID);
    				if (!IsDeserializing)
    				{
    					if (User != null && User.UserID != value)
    					{
    						User = null;
    					}
    				}
    				_modifiedByUserID = value;
    				ModifiedByUserIDChanged();
    				OnPropertyChanged("ModifiedByUserID");
    			}
    		}
    	}
    	private Nullable<int> _modifiedByUserID;
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
    	partial void CatalogIDChanged();
    	public int CatalogID
    	{
    		get { return _catalogID; }
    		set
    		{
    			if (_catalogID != value)
    			{
    				ChangeTracker.RecordOriginalValue("CatalogID", _catalogID);
    				if (!IsDeserializing)
    				{
    					if (Catalog != null && Catalog.CatalogID != value)
    					{
    						Catalog = null;
    					}
    				}
    				_catalogID = value;
    				CatalogIDChanged();
    				OnPropertyChanged("CatalogID");
    			}
    		}
    	}
    	private int _catalogID;

        #endregion
        #region Navigation Properties
    
    	public Currency Currency
    	{
    		get { return _currency; }
    		set
    		{
    			if (!ReferenceEquals(_currency, value))
    			{
    				var previousValue = _currency;
    				_currency = value;
    				FixupCurrency(previousValue);
    				OnNavigationPropertyChanged("Currency");
    			}
    		}
    	}
    	private Currency _currency;
    
    	public ProductPriceType ProductPriceType
    	{
    		get { return _productPriceType; }
    		set
    		{
    			if (!ReferenceEquals(_productPriceType, value))
    			{
    				var previousValue = _productPriceType;
    				_productPriceType = value;
    				FixupProductPriceType(previousValue);
    				OnNavigationPropertyChanged("ProductPriceType");
    			}
    		}
    	}
    	private ProductPriceType _productPriceType;
    
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
    
    	public User User
    	{
    		get { return _user; }
    		set
    		{
    			if (!ReferenceEquals(_user, value))
    			{
    				var previousValue = _user;
    				_user = value;
    				FixupUser(previousValue);
    				OnNavigationPropertyChanged("User");
    			}
    		}
    	}
    	private User _user;
    
    	public Catalog Catalog
    	{
    		get { return _catalog; }
    		set
    		{
    			if (!ReferenceEquals(_catalog, value))
    			{
    				var previousValue = _catalog;
    				_catalog = value;
    				FixupCatalog(previousValue);
    				OnNavigationPropertyChanged("Catalog");
    			}
    		}
    	}
    	private Catalog _catalog;

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
    		Currency = null;
    		ProductPriceType = null;
    		Product = null;
    		User = null;
    		Catalog = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupCurrency(Currency previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.ProductPrices.Contains(this))
    		{
    			previousValue.ProductPrices.Remove(this);
    		}
    
    		if (Currency != null)
    		{
    			if (!Currency.ProductPrices.Contains(this))
    			{
    				Currency.ProductPrices.Add(this);
    			}
    
    			CurrencyID = Currency.CurrencyID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Currency")
    				&& (ChangeTracker.OriginalValues["Currency"] == Currency))
    			{
    				ChangeTracker.OriginalValues.Remove("Currency");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Currency", previousValue);
    			}
    			if (Currency != null && !Currency.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Currency.StartTracking();
    			}
    		}
    	}
    
    	private void FixupProductPriceType(ProductPriceType previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.ProductPrices.Contains(this))
    		{
    			previousValue.ProductPrices.Remove(this);
    		}
    
    		if (ProductPriceType != null)
    		{
    			if (!ProductPriceType.ProductPrices.Contains(this))
    			{
    				ProductPriceType.ProductPrices.Add(this);
    			}
    
    			ProductPriceTypeID = ProductPriceType.ProductPriceTypeID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("ProductPriceType")
    				&& (ChangeTracker.OriginalValues["ProductPriceType"] == ProductPriceType))
    			{
    				ChangeTracker.OriginalValues.Remove("ProductPriceType");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("ProductPriceType", previousValue);
    			}
    			if (ProductPriceType != null && !ProductPriceType.ChangeTracker.ChangeTrackingEnabled)
    			{
    				ProductPriceType.StartTracking();
    			}
    		}
    	}
    
    	private void FixupProduct(Product previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.Prices.Contains(this))
    		{
    			previousValue.Prices.Remove(this);
    		}
    
    		if (Product != null)
    		{
    			if (!Product.Prices.Contains(this))
    			{
    				Product.Prices.Add(this);
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
    
    	private void FixupUser(User previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.ProductPrices.Contains(this))
    		{
    			previousValue.ProductPrices.Remove(this);
    		}
    
    		if (User != null)
    		{
    			if (!User.ProductPrices.Contains(this))
    			{
    				User.ProductPrices.Add(this);
    			}
    
    			ModifiedByUserID = User.UserID;
    		}
    		else if (!skipKeys)
    		{
    			ModifiedByUserID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("User")
    				&& (ChangeTracker.OriginalValues["User"] == User))
    			{
    				ChangeTracker.OriginalValues.Remove("User");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("User", previousValue);
    			}
    			if (User != null && !User.ChangeTracker.ChangeTrackingEnabled)
    			{
    				User.StartTracking();
    			}
    		}
    	}
    
    	private void FixupCatalog(Catalog previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.ProductPrices.Contains(this))
    		{
    			previousValue.ProductPrices.Remove(this);
    		}
    
    		if (Catalog != null)
    		{
    			if (!Catalog.ProductPrices.Contains(this))
    			{
    				Catalog.ProductPrices.Add(this);
    			}
    
    			CatalogID = Catalog.CatalogID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Catalog")
    				&& (ChangeTracker.OriginalValues["Catalog"] == Catalog))
    			{
    				ChangeTracker.OriginalValues.Remove("Catalog");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Catalog", previousValue);
    			}
    			if (Catalog != null && !Catalog.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Catalog.StartTracking();
    			}
    		}
    	}

        #endregion
    }
}
