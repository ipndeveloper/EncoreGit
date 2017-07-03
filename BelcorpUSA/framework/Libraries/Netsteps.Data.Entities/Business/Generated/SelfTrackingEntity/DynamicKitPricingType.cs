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
    [KnownType(typeof(DynamicKit))]
    [Serializable]
    public partial class DynamicKitPricingType: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void DynamicKitPricingTypeIDChanged();
    	public int DynamicKitPricingTypeID
    	{
    		get { return _dynamicKitPricingTypeID; }
    		set
    		{
    			if (_dynamicKitPricingTypeID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'DynamicKitPricingTypeID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_dynamicKitPricingTypeID = value;
    				DynamicKitPricingTypeIDChanged();
    				OnPropertyChanged("DynamicKitPricingTypeID");
    			}
    		}
    	}
    	private int _dynamicKitPricingTypeID;
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
    
    	public TrackableCollection<DynamicKit> DynamicKits
    	{
    		get
    		{
    			if (_dynamicKits == null)
    			{
    				_dynamicKits = new TrackableCollection<DynamicKit>();
    				_dynamicKits.CollectionChanged += FixupDynamicKits;
    				_dynamicKits.CollectionChanged += RaiseDynamicKitsChanged;
    			}
    			return _dynamicKits;
    		}
    		set
    		{
    			if (!ReferenceEquals(_dynamicKits, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_dynamicKits != null)
    				{
    					_dynamicKits.CollectionChanged -= FixupDynamicKits;
    					_dynamicKits.CollectionChanged -= RaiseDynamicKitsChanged;
    				}
    				_dynamicKits = value;
    				if (_dynamicKits != null)
    				{
    					_dynamicKits.CollectionChanged += FixupDynamicKits;
    					_dynamicKits.CollectionChanged += RaiseDynamicKitsChanged;
    				}
    				OnNavigationPropertyChanged("DynamicKits");
    			}
    		}
    	}
    	private TrackableCollection<DynamicKit> _dynamicKits;
    	partial void DynamicKitsChanged();
    	private void RaiseDynamicKitsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		DynamicKitsChanged();
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
    		if (_dynamicKits != null)
    		{
    			_dynamicKits.CollectionChanged -= FixupDynamicKits;
    			_dynamicKits.CollectionChanged -= RaiseDynamicKitsChanged;
    			_dynamicKits.CollectionChanged += FixupDynamicKits;
    			_dynamicKits.CollectionChanged += RaiseDynamicKitsChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		DynamicKits.Clear();
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupDynamicKits(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (DynamicKit item in e.NewItems)
    			{
    				item.DynamicKitPricingType = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("DynamicKits", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (DynamicKit item in e.OldItems)
    			{
    				if (ReferenceEquals(item.DynamicKitPricingType, this))
    				{
    					item.DynamicKitPricingType = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("DynamicKits", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}
