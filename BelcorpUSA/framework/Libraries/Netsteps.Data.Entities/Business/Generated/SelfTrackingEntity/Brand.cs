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
    [KnownType(typeof(Merchant))]
    [KnownType(typeof(DescriptionTranslation))]
    [Serializable]
    public partial class Brand: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void BrandIDChanged();
    	public int BrandID
    	{
    		get { return _brandID; }
    		set
    		{
    			if (_brandID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'BrandID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_brandID = value;
    				BrandIDChanged();
    				OnPropertyChanged("BrandID");
    			}
    		}
    	}
    	private int _brandID;
    	partial void BrandNumberChanged();
    	public string BrandNumber
    	{
    		get { return _brandNumber; }
    		set
    		{
    			if (_brandNumber != value)
    			{
    				ChangeTracker.RecordOriginalValue("BrandNumber", _brandNumber);
    				_brandNumber = value;
    				BrandNumberChanged();
    				OnPropertyChanged("BrandNumber");
    			}
    		}
    	}
    	private string _brandNumber;

        #endregion
        #region Navigation Properties
    
    	public TrackableCollection<Merchant> Merchants
    	{
    		get
    		{
    			if (_merchants == null)
    			{
    				_merchants = new TrackableCollection<Merchant>();
    				_merchants.CollectionChanged += FixupMerchants;
    				_merchants.CollectionChanged += RaiseMerchantsChanged;
    			}
    			return _merchants;
    		}
    		set
    		{
    			if (!ReferenceEquals(_merchants, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_merchants != null)
    				{
    					_merchants.CollectionChanged -= FixupMerchants;
    					_merchants.CollectionChanged -= RaiseMerchantsChanged;
    				}
    				_merchants = value;
    				if (_merchants != null)
    				{
    					_merchants.CollectionChanged += FixupMerchants;
    					_merchants.CollectionChanged += RaiseMerchantsChanged;
    				}
    				OnNavigationPropertyChanged("Merchants");
    			}
    		}
    	}
    	private TrackableCollection<Merchant> _merchants;
    	partial void MerchantsChanged();
    	private void RaiseMerchantsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		MerchantsChanged();
    	}
    
    	public TrackableCollection<DescriptionTranslation> DescriptionTranslations
    	{
    		get
    		{
    			if (_descriptionTranslations == null)
    			{
    				_descriptionTranslations = new TrackableCollection<DescriptionTranslation>();
    				_descriptionTranslations.CollectionChanged += FixupDescriptionTranslations;
    				_descriptionTranslations.CollectionChanged += RaiseDescriptionTranslationsChanged;
    			}
    			return _descriptionTranslations;
    		}
    		set
    		{
    			if (!ReferenceEquals(_descriptionTranslations, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_descriptionTranslations != null)
    				{
    					_descriptionTranslations.CollectionChanged -= FixupDescriptionTranslations;
    					_descriptionTranslations.CollectionChanged -= RaiseDescriptionTranslationsChanged;
    				}
    				_descriptionTranslations = value;
    				if (_descriptionTranslations != null)
    				{
    					_descriptionTranslations.CollectionChanged += FixupDescriptionTranslations;
    					_descriptionTranslations.CollectionChanged += RaiseDescriptionTranslationsChanged;
    				}
    				OnNavigationPropertyChanged("DescriptionTranslations");
    			}
    		}
    	}
    	private TrackableCollection<DescriptionTranslation> _descriptionTranslations;
    	partial void DescriptionTranslationsChanged();
    	private void RaiseDescriptionTranslationsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		DescriptionTranslationsChanged();
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
    		if (_merchants != null)
    		{
    			_merchants.CollectionChanged -= FixupMerchants;
    			_merchants.CollectionChanged -= RaiseMerchantsChanged;
    			_merchants.CollectionChanged += FixupMerchants;
    			_merchants.CollectionChanged += RaiseMerchantsChanged;
    		}
    		if (_descriptionTranslations != null)
    		{
    			_descriptionTranslations.CollectionChanged -= FixupDescriptionTranslations;
    			_descriptionTranslations.CollectionChanged -= RaiseDescriptionTranslationsChanged;
    			_descriptionTranslations.CollectionChanged += FixupDescriptionTranslations;
    			_descriptionTranslations.CollectionChanged += RaiseDescriptionTranslationsChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		Merchants.Clear();
    		DescriptionTranslations.Clear();
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupMerchants(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (Merchant item in e.NewItems)
    			{
    				item.Brand = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Merchants", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (Merchant item in e.OldItems)
    			{
    				if (ReferenceEquals(item.Brand, this))
    				{
    					item.Brand = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Merchants", item);
    				}
    			}
    		}
    	}
    
    	private void FixupDescriptionTranslations(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (DescriptionTranslation item in e.NewItems)
    			{
    				if (!item.Brands.Contains(this))
    				{
    					item.Brands.Add(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("DescriptionTranslations", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (DescriptionTranslation item in e.OldItems)
    			{
    				if (item.Brands.Contains(this))
    				{
    					item.Brands.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("DescriptionTranslations", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}