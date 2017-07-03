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
    [KnownType(typeof(Product))]
    [Serializable]
    public partial class PromotionProduct: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void PromotionProductIDChanged();
    	public int PromotionProductID
    	{
    		get { return _promotionProductID; }
    		set
    		{
    			if (_promotionProductID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'PromotionProductID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_promotionProductID = value;
    				PromotionProductIDChanged();
    				OnPropertyChanged("PromotionProductID");
    			}
    		}
    	}
    	private int _promotionProductID;
    	partial void PromotionIDChanged();
    	public int PromotionID
    	{
    		get { return _promotionID; }
    		set
    		{
    			if (_promotionID != value)
    			{
    				ChangeTracker.RecordOriginalValue("PromotionID", _promotionID);
    				_promotionID = value;
    				PromotionIDChanged();
    				OnPropertyChanged("PromotionID");
    			}
    		}
    	}
    	private int _promotionID;
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
    	partial void DiscountPriceChanged();
    	public Nullable<decimal> DiscountPrice
    	{
    		get { return _discountPrice; }
    		set
    		{
    			if (_discountPrice != value)
    			{
    				ChangeTracker.RecordOriginalValue("DiscountPrice", _discountPrice);
    				_discountPrice = value;
    				DiscountPriceChanged();
    				OnPropertyChanged("DiscountPrice");
    			}
    		}
    	}
    	private Nullable<decimal> _discountPrice;
    	partial void QuantityChanged();
    	public Nullable<short> Quantity
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
    	private Nullable<short> _quantity;
    	partial void PromotionProductTypeIDChanged();
    	public short PromotionProductTypeID
    	{
    		get { return _promotionProductTypeID; }
    		set
    		{
    			if (_promotionProductTypeID != value)
    			{
    				ChangeTracker.RecordOriginalValue("PromotionProductTypeID", _promotionProductTypeID);
    				_promotionProductTypeID = value;
    				PromotionProductTypeIDChanged();
    				OnPropertyChanged("PromotionProductTypeID");
    			}
    		}
    	}
    	private short _promotionProductTypeID;

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
    		Currency = null;
    		Product = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupCurrency(Currency previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.PromotionProducts.Contains(this))
    		{
    			previousValue.PromotionProducts.Remove(this);
    		}
    
    		if (Currency != null)
    		{
    			if (!Currency.PromotionProducts.Contains(this))
    			{
    				Currency.PromotionProducts.Add(this);
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
    
    	private void FixupProduct(Product previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.PromotionProducts.Contains(this))
    		{
    			previousValue.PromotionProducts.Remove(this);
    		}
    
    		if (Product != null)
    		{
    			if (!Product.PromotionProducts.Contains(this))
    			{
    				Product.PromotionProducts.Add(this);
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