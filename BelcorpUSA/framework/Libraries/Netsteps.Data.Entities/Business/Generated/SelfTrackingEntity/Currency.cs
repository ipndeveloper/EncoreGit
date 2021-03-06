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
    [KnownType(typeof(Country))]
    [KnownType(typeof(ProductPrice))]
    [KnownType(typeof(OrderPayment))]
    [KnownType(typeof(Order))]
    [KnownType(typeof(ShippingRate))]
    [KnownType(typeof(OrderPaymentResult))]
    [KnownType(typeof(PromotionProduct))]
    [Serializable]
    public partial class Currency: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void CurrencyIDChanged();
    	public int CurrencyID
    	{
    		get { return _currencyID; }
    		set
    		{
    			if (_currencyID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'CurrencyID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_currencyID = value;
    				CurrencyIDChanged();
    				OnPropertyChanged("CurrencyID");
    			}
    		}
    	}
    	private int _currencyID;
    	partial void CurrencyCodeChanged();
    	public string CurrencyCode
    	{
    		get { return _currencyCode; }
    		set
    		{
    			if (_currencyCode != value)
    			{
    				ChangeTracker.RecordOriginalValue("CurrencyCode", _currencyCode);
    				_currencyCode = value;
    				CurrencyCodeChanged();
    				OnPropertyChanged("CurrencyCode");
    			}
    		}
    	}
    	private string _currencyCode;
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
    	partial void CurrencySymbolChanged();
    	public string CurrencySymbol
    	{
    		get { return _currencySymbol; }
    		set
    		{
    			if (_currencySymbol != value)
    			{
    				ChangeTracker.RecordOriginalValue("CurrencySymbol", _currencySymbol);
    				_currencySymbol = value;
    				CurrencySymbolChanged();
    				OnPropertyChanged("CurrencySymbol");
    			}
    		}
    	}
    	private string _currencySymbol;
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
    	partial void CultureInfoChanged();
    	public string CultureInfo
    	{
    		get { return _cultureInfo; }
    		set
    		{
    			if (_cultureInfo != value)
    			{
    				ChangeTracker.RecordOriginalValue("CultureInfo", _cultureInfo);
    				_cultureInfo = value;
    				CultureInfoChanged();
    				OnPropertyChanged("CultureInfo");
    			}
    		}
    	}
    	private string _cultureInfo;

        #endregion
        #region Navigation Properties
    
    	public TrackableCollection<Country> Countries
    	{
    		get
    		{
    			if (_countries == null)
    			{
    				_countries = new TrackableCollection<Country>();
    				_countries.CollectionChanged += FixupCountries;
    				_countries.CollectionChanged += RaiseCountriesChanged;
    			}
    			return _countries;
    		}
    		set
    		{
    			if (!ReferenceEquals(_countries, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_countries != null)
    				{
    					_countries.CollectionChanged -= FixupCountries;
    					_countries.CollectionChanged -= RaiseCountriesChanged;
    				}
    				_countries = value;
    				if (_countries != null)
    				{
    					_countries.CollectionChanged += FixupCountries;
    					_countries.CollectionChanged += RaiseCountriesChanged;
    				}
    				OnNavigationPropertyChanged("Countries");
    			}
    		}
    	}
    	private TrackableCollection<Country> _countries;
    	partial void CountriesChanged();
    	private void RaiseCountriesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		CountriesChanged();
    	}
    
    	public TrackableCollection<ProductPrice> ProductPrices
    	{
    		get
    		{
    			if (_productPrices == null)
    			{
    				_productPrices = new TrackableCollection<ProductPrice>();
    				_productPrices.CollectionChanged += FixupProductPrices;
    				_productPrices.CollectionChanged += RaiseProductPricesChanged;
    			}
    			return _productPrices;
    		}
    		set
    		{
    			if (!ReferenceEquals(_productPrices, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_productPrices != null)
    				{
    					_productPrices.CollectionChanged -= FixupProductPrices;
    					_productPrices.CollectionChanged -= RaiseProductPricesChanged;
    				}
    				_productPrices = value;
    				if (_productPrices != null)
    				{
    					_productPrices.CollectionChanged += FixupProductPrices;
    					_productPrices.CollectionChanged += RaiseProductPricesChanged;
    				}
    				OnNavigationPropertyChanged("ProductPrices");
    			}
    		}
    	}
    	private TrackableCollection<ProductPrice> _productPrices;
    	partial void ProductPricesChanged();
    	private void RaiseProductPricesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		ProductPricesChanged();
    	}
    
    	public TrackableCollection<OrderPayment> OrderPayments
    	{
    		get
    		{
    			if (_orderPayments == null)
    			{
    				_orderPayments = new TrackableCollection<OrderPayment>();
    				_orderPayments.CollectionChanged += FixupOrderPayments;
    				_orderPayments.CollectionChanged += RaiseOrderPaymentsChanged;
    			}
    			return _orderPayments;
    		}
    		set
    		{
    			if (!ReferenceEquals(_orderPayments, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_orderPayments != null)
    				{
    					_orderPayments.CollectionChanged -= FixupOrderPayments;
    					_orderPayments.CollectionChanged -= RaiseOrderPaymentsChanged;
    				}
    				_orderPayments = value;
    				if (_orderPayments != null)
    				{
    					_orderPayments.CollectionChanged += FixupOrderPayments;
    					_orderPayments.CollectionChanged += RaiseOrderPaymentsChanged;
    				}
    				OnNavigationPropertyChanged("OrderPayments");
    			}
    		}
    	}
    	private TrackableCollection<OrderPayment> _orderPayments;
    	partial void OrderPaymentsChanged();
    	private void RaiseOrderPaymentsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		OrderPaymentsChanged();
    	}
    
    	public TrackableCollection<Country> Countries1
    	{
    		get
    		{
    			if (_countries1 == null)
    			{
    				_countries1 = new TrackableCollection<Country>();
    				_countries1.CollectionChanged += FixupCountries1;
    				_countries1.CollectionChanged += RaiseCountries1Changed;
    			}
    			return _countries1;
    		}
    		set
    		{
    			if (!ReferenceEquals(_countries1, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_countries1 != null)
    				{
    					_countries1.CollectionChanged -= FixupCountries1;
    					_countries1.CollectionChanged -= RaiseCountries1Changed;
    				}
    				_countries1 = value;
    				if (_countries1 != null)
    				{
    					_countries1.CollectionChanged += FixupCountries1;
    					_countries1.CollectionChanged += RaiseCountries1Changed;
    				}
    				OnNavigationPropertyChanged("Countries1");
    			}
    		}
    	}
    	private TrackableCollection<Country> _countries1;
    	partial void Countries1Changed();
    	private void RaiseCountries1Changed(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		Countries1Changed();
    	}
    
    	public TrackableCollection<Order> Orders
    	{
    		get
    		{
    			if (_orders == null)
    			{
    				_orders = new TrackableCollection<Order>();
    				_orders.CollectionChanged += FixupOrders;
    				_orders.CollectionChanged += RaiseOrdersChanged;
    			}
    			return _orders;
    		}
    		set
    		{
    			if (!ReferenceEquals(_orders, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_orders != null)
    				{
    					_orders.CollectionChanged -= FixupOrders;
    					_orders.CollectionChanged -= RaiseOrdersChanged;
    				}
    				_orders = value;
    				if (_orders != null)
    				{
    					_orders.CollectionChanged += FixupOrders;
    					_orders.CollectionChanged += RaiseOrdersChanged;
    				}
    				OnNavigationPropertyChanged("Orders");
    			}
    		}
    	}
    	private TrackableCollection<Order> _orders;
    	partial void OrdersChanged();
    	private void RaiseOrdersChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		OrdersChanged();
    	}
    
    	public TrackableCollection<ShippingRate> ShippingRates
    	{
    		get
    		{
    			if (_shippingRates == null)
    			{
    				_shippingRates = new TrackableCollection<ShippingRate>();
    				_shippingRates.CollectionChanged += FixupShippingRates;
    				_shippingRates.CollectionChanged += RaiseShippingRatesChanged;
    			}
    			return _shippingRates;
    		}
    		set
    		{
    			if (!ReferenceEquals(_shippingRates, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_shippingRates != null)
    				{
    					_shippingRates.CollectionChanged -= FixupShippingRates;
    					_shippingRates.CollectionChanged -= RaiseShippingRatesChanged;
    				}
    				_shippingRates = value;
    				if (_shippingRates != null)
    				{
    					_shippingRates.CollectionChanged += FixupShippingRates;
    					_shippingRates.CollectionChanged += RaiseShippingRatesChanged;
    				}
    				OnNavigationPropertyChanged("ShippingRates");
    			}
    		}
    	}
    	private TrackableCollection<ShippingRate> _shippingRates;
    	partial void ShippingRatesChanged();
    	private void RaiseShippingRatesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		ShippingRatesChanged();
    	}
    
    	public TrackableCollection<OrderPaymentResult> OrderPaymentResults
    	{
    		get
    		{
    			if (_orderPaymentResults == null)
    			{
    				_orderPaymentResults = new TrackableCollection<OrderPaymentResult>();
    				_orderPaymentResults.CollectionChanged += FixupOrderPaymentResults;
    				_orderPaymentResults.CollectionChanged += RaiseOrderPaymentResultsChanged;
    			}
    			return _orderPaymentResults;
    		}
    		set
    		{
    			if (!ReferenceEquals(_orderPaymentResults, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_orderPaymentResults != null)
    				{
    					_orderPaymentResults.CollectionChanged -= FixupOrderPaymentResults;
    					_orderPaymentResults.CollectionChanged -= RaiseOrderPaymentResultsChanged;
    				}
    				_orderPaymentResults = value;
    				if (_orderPaymentResults != null)
    				{
    					_orderPaymentResults.CollectionChanged += FixupOrderPaymentResults;
    					_orderPaymentResults.CollectionChanged += RaiseOrderPaymentResultsChanged;
    				}
    				OnNavigationPropertyChanged("OrderPaymentResults");
    			}
    		}
    	}
    	private TrackableCollection<OrderPaymentResult> _orderPaymentResults;
    	partial void OrderPaymentResultsChanged();
    	private void RaiseOrderPaymentResultsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		OrderPaymentResultsChanged();
    	}
    
    	public TrackableCollection<PromotionProduct> PromotionProducts
    	{
    		get
    		{
    			if (_promotionProducts == null)
    			{
    				_promotionProducts = new TrackableCollection<PromotionProduct>();
    				_promotionProducts.CollectionChanged += FixupPromotionProducts;
    				_promotionProducts.CollectionChanged += RaisePromotionProductsChanged;
    			}
    			return _promotionProducts;
    		}
    		set
    		{
    			if (!ReferenceEquals(_promotionProducts, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_promotionProducts != null)
    				{
    					_promotionProducts.CollectionChanged -= FixupPromotionProducts;
    					_promotionProducts.CollectionChanged -= RaisePromotionProductsChanged;
    				}
    				_promotionProducts = value;
    				if (_promotionProducts != null)
    				{
    					_promotionProducts.CollectionChanged += FixupPromotionProducts;
    					_promotionProducts.CollectionChanged += RaisePromotionProductsChanged;
    				}
    				OnNavigationPropertyChanged("PromotionProducts");
    			}
    		}
    	}
    	private TrackableCollection<PromotionProduct> _promotionProducts;
    	partial void PromotionProductsChanged();
    	private void RaisePromotionProductsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		PromotionProductsChanged();
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
    		if (_countries != null)
    		{
    			_countries.CollectionChanged -= FixupCountries;
    			_countries.CollectionChanged -= RaiseCountriesChanged;
    			_countries.CollectionChanged += FixupCountries;
    			_countries.CollectionChanged += RaiseCountriesChanged;
    		}
    		if (_productPrices != null)
    		{
    			_productPrices.CollectionChanged -= FixupProductPrices;
    			_productPrices.CollectionChanged -= RaiseProductPricesChanged;
    			_productPrices.CollectionChanged += FixupProductPrices;
    			_productPrices.CollectionChanged += RaiseProductPricesChanged;
    		}
    		if (_orderPayments != null)
    		{
    			_orderPayments.CollectionChanged -= FixupOrderPayments;
    			_orderPayments.CollectionChanged -= RaiseOrderPaymentsChanged;
    			_orderPayments.CollectionChanged += FixupOrderPayments;
    			_orderPayments.CollectionChanged += RaiseOrderPaymentsChanged;
    		}
    		if (_countries1 != null)
    		{
    			_countries1.CollectionChanged -= FixupCountries1;
    			_countries1.CollectionChanged -= RaiseCountries1Changed;
    			_countries1.CollectionChanged += FixupCountries1;
    			_countries1.CollectionChanged += RaiseCountries1Changed;
    		}
    		if (_orders != null)
    		{
    			_orders.CollectionChanged -= FixupOrders;
    			_orders.CollectionChanged -= RaiseOrdersChanged;
    			_orders.CollectionChanged += FixupOrders;
    			_orders.CollectionChanged += RaiseOrdersChanged;
    		}
    		if (_shippingRates != null)
    		{
    			_shippingRates.CollectionChanged -= FixupShippingRates;
    			_shippingRates.CollectionChanged -= RaiseShippingRatesChanged;
    			_shippingRates.CollectionChanged += FixupShippingRates;
    			_shippingRates.CollectionChanged += RaiseShippingRatesChanged;
    		}
    		if (_orderPaymentResults != null)
    		{
    			_orderPaymentResults.CollectionChanged -= FixupOrderPaymentResults;
    			_orderPaymentResults.CollectionChanged -= RaiseOrderPaymentResultsChanged;
    			_orderPaymentResults.CollectionChanged += FixupOrderPaymentResults;
    			_orderPaymentResults.CollectionChanged += RaiseOrderPaymentResultsChanged;
    		}
    		if (_promotionProducts != null)
    		{
    			_promotionProducts.CollectionChanged -= FixupPromotionProducts;
    			_promotionProducts.CollectionChanged -= RaisePromotionProductsChanged;
    			_promotionProducts.CollectionChanged += FixupPromotionProducts;
    			_promotionProducts.CollectionChanged += RaisePromotionProductsChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		Countries.Clear();
    		ProductPrices.Clear();
    		OrderPayments.Clear();
    		Countries1.Clear();
    		Orders.Clear();
    		ShippingRates.Clear();
    		OrderPaymentResults.Clear();
    		PromotionProducts.Clear();
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupCountries(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (Country item in e.NewItems)
    			{
    				item.Currency = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Countries", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (Country item in e.OldItems)
    			{
    				if (ReferenceEquals(item.Currency, this))
    				{
    					item.Currency = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Countries", item);
    				}
    			}
    		}
    	}
    
    	private void FixupProductPrices(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (ProductPrice item in e.NewItems)
    			{
    				item.Currency = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("ProductPrices", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (ProductPrice item in e.OldItems)
    			{
    				if (ReferenceEquals(item.Currency, this))
    				{
    					item.Currency = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("ProductPrices", item);
    				}
    			}
    		}
    	}
    
    	private void FixupOrderPayments(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (OrderPayment item in e.NewItems)
    			{
    				item.Currency = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("OrderPayments", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (OrderPayment item in e.OldItems)
    			{
    				if (ReferenceEquals(item.Currency, this))
    				{
    					item.Currency = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("OrderPayments", item);
    				}
    			}
    		}
    	}
    
    	private void FixupCountries1(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (Country item in e.NewItems)
    			{
    				if (!item.Currencies.Contains(this))
    				{
    					item.Currencies.Add(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Countries1", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (Country item in e.OldItems)
    			{
    				if (item.Currencies.Contains(this))
    				{
    					item.Currencies.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Countries1", item);
    				}
    			}
    		}
    	}
    
    	private void FixupOrders(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (Order item in e.NewItems)
    			{
    				item.Currency = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Orders", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (Order item in e.OldItems)
    			{
    				if (ReferenceEquals(item.Currency, this))
    				{
    					item.Currency = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Orders", item);
    				}
    			}
    		}
    	}
    
    	private void FixupShippingRates(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (ShippingRate item in e.NewItems)
    			{
    				item.Currency = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("ShippingRates", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (ShippingRate item in e.OldItems)
    			{
    				if (ReferenceEquals(item.Currency, this))
    				{
    					item.Currency = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("ShippingRates", item);
    				}
    			}
    		}
    	}
    
    	private void FixupOrderPaymentResults(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (OrderPaymentResult item in e.NewItems)
    			{
    				item.Currency = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("OrderPaymentResults", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (OrderPaymentResult item in e.OldItems)
    			{
    				if (ReferenceEquals(item.Currency, this))
    				{
    					item.Currency = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("OrderPaymentResults", item);
    				}
    			}
    		}
    	}
    
    	private void FixupPromotionProducts(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (PromotionProduct item in e.NewItems)
    			{
    				item.Currency = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("PromotionProducts", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (PromotionProduct item in e.OldItems)
    			{
    				if (ReferenceEquals(item.Currency, this))
    				{
    					item.Currency = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("PromotionProducts", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}
