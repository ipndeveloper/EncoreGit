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
    [KnownType(typeof(ShippingRate))]
    [Serializable]
    public partial class ShippingRateType: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void ShippingRateTypeIDChanged();
    	public short ShippingRateTypeID
    	{
    		get { return _shippingRateTypeID; }
    		set
    		{
    			if (_shippingRateTypeID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'ShippingRateTypeID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_shippingRateTypeID = value;
    				ShippingRateTypeIDChanged();
    				OnPropertyChanged("ShippingRateTypeID");
    			}
    		}
    	}
    	private short _shippingRateTypeID;
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

        #endregion
        #region Navigation Properties
    
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
    		if (_shippingRates != null)
    		{
    			_shippingRates.CollectionChanged -= FixupShippingRates;
    			_shippingRates.CollectionChanged -= RaiseShippingRatesChanged;
    			_shippingRates.CollectionChanged += FixupShippingRates;
    			_shippingRates.CollectionChanged += RaiseShippingRatesChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		ShippingRates.Clear();
    	}

        #endregion
        #region Association Fixup
    
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
    				item.ShippingRateType = this;
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
    				if (ReferenceEquals(item.ShippingRateType, this))
    				{
    					item.ShippingRateType = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("ShippingRates", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}
