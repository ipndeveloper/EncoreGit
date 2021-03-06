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
    [KnownType(typeof(Catalog))]
    [KnownType(typeof(AccountPriceType))]
    [KnownType(typeof(MarketStoreFront))]
    [Serializable]
    public partial class StoreFront: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void StoreFrontIDChanged();
    	public int StoreFrontID
    	{
    		get { return _storeFrontID; }
    		set
    		{
    			if (_storeFrontID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'StoreFrontID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_storeFrontID = value;
    				StoreFrontIDChanged();
    				OnPropertyChanged("StoreFrontID");
    			}
    		}
    	}
    	private int _storeFrontID;
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
    	partial void DateLastModifiedUTCChanged();
    	public Nullable<System.DateTime> DateLastModifiedUTC
    	{
    		get { return _dateLastModifiedUTC; }
    		set
    		{
    			if (_dateLastModifiedUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("DateLastModifiedUTC", _dateLastModifiedUTC);
    				_dateLastModifiedUTC = value;
    				DateLastModifiedUTCChanged();
    				OnPropertyChanged("DateLastModifiedUTC");
    			}
    		}
    	}
    	private Nullable<System.DateTime> _dateLastModifiedUTC;

        #endregion
        #region Navigation Properties
    
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
    
    	public TrackableCollection<AccountPriceType> AccountPriceTypes
    	{
    		get
    		{
    			if (_accountPriceTypes == null)
    			{
    				_accountPriceTypes = new TrackableCollection<AccountPriceType>();
    				_accountPriceTypes.CollectionChanged += FixupAccountPriceTypes;
    				_accountPriceTypes.CollectionChanged += RaiseAccountPriceTypesChanged;
    			}
    			return _accountPriceTypes;
    		}
    		set
    		{
    			if (!ReferenceEquals(_accountPriceTypes, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_accountPriceTypes != null)
    				{
    					_accountPriceTypes.CollectionChanged -= FixupAccountPriceTypes;
    					_accountPriceTypes.CollectionChanged -= RaiseAccountPriceTypesChanged;
    				}
    				_accountPriceTypes = value;
    				if (_accountPriceTypes != null)
    				{
    					_accountPriceTypes.CollectionChanged += FixupAccountPriceTypes;
    					_accountPriceTypes.CollectionChanged += RaiseAccountPriceTypesChanged;
    				}
    				OnNavigationPropertyChanged("AccountPriceTypes");
    			}
    		}
    	}
    	private TrackableCollection<AccountPriceType> _accountPriceTypes;
    	partial void AccountPriceTypesChanged();
    	private void RaiseAccountPriceTypesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		AccountPriceTypesChanged();
    	}
    
    	public TrackableCollection<MarketStoreFront> MarketStoreFronts
    	{
    		get
    		{
    			if (_marketStoreFronts == null)
    			{
    				_marketStoreFronts = new TrackableCollection<MarketStoreFront>();
    				_marketStoreFronts.CollectionChanged += FixupMarketStoreFronts;
    				_marketStoreFronts.CollectionChanged += RaiseMarketStoreFrontsChanged;
    			}
    			return _marketStoreFronts;
    		}
    		set
    		{
    			if (!ReferenceEquals(_marketStoreFronts, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_marketStoreFronts != null)
    				{
    					_marketStoreFronts.CollectionChanged -= FixupMarketStoreFronts;
    					_marketStoreFronts.CollectionChanged -= RaiseMarketStoreFrontsChanged;
    				}
    				_marketStoreFronts = value;
    				if (_marketStoreFronts != null)
    				{
    					_marketStoreFronts.CollectionChanged += FixupMarketStoreFronts;
    					_marketStoreFronts.CollectionChanged += RaiseMarketStoreFrontsChanged;
    				}
    				OnNavigationPropertyChanged("MarketStoreFronts");
    			}
    		}
    	}
    	private TrackableCollection<MarketStoreFront> _marketStoreFronts;
    	partial void MarketStoreFrontsChanged();
    	private void RaiseMarketStoreFrontsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		MarketStoreFrontsChanged();
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
    		if (_accountPriceTypes != null)
    		{
    			_accountPriceTypes.CollectionChanged -= FixupAccountPriceTypes;
    			_accountPriceTypes.CollectionChanged -= RaiseAccountPriceTypesChanged;
    			_accountPriceTypes.CollectionChanged += FixupAccountPriceTypes;
    			_accountPriceTypes.CollectionChanged += RaiseAccountPriceTypesChanged;
    		}
    		if (_marketStoreFronts != null)
    		{
    			_marketStoreFronts.CollectionChanged -= FixupMarketStoreFronts;
    			_marketStoreFronts.CollectionChanged -= RaiseMarketStoreFrontsChanged;
    			_marketStoreFronts.CollectionChanged += FixupMarketStoreFronts;
    			_marketStoreFronts.CollectionChanged += RaiseMarketStoreFrontsChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		Catalogs.Clear();
    		AccountPriceTypes.Clear();
    		MarketStoreFronts.Clear();
    	}

        #endregion
        #region Association Fixup
    
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
    				if (!item.StoreFronts.Contains(this))
    				{
    					item.StoreFronts.Add(this);
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
    				if (item.StoreFronts.Contains(this))
    				{
    					item.StoreFronts.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Catalogs", item);
    				}
    			}
    		}
    	}
    
    	private void FixupAccountPriceTypes(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (AccountPriceType item in e.NewItems)
    			{
    				item.StoreFront = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("AccountPriceTypes", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (AccountPriceType item in e.OldItems)
    			{
    				if (ReferenceEquals(item.StoreFront, this))
    				{
    					item.StoreFront = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("AccountPriceTypes", item);
    				}
    			}
    		}
    	}
    
    	private void FixupMarketStoreFronts(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (MarketStoreFront item in e.NewItems)
    			{
    				item.StoreFront = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("MarketStoreFronts", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (MarketStoreFront item in e.OldItems)
    			{
    				if (ReferenceEquals(item.StoreFront, this))
    				{
    					item.StoreFront = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("MarketStoreFronts", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}
