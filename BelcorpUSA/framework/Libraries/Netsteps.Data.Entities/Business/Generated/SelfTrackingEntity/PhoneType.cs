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
    [KnownType(typeof(AccountPhone))]
    [KnownType(typeof(Address))]
    [Serializable]
    public partial class PhoneType: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void PhoneTypeIDChanged();
    	public int PhoneTypeID
    	{
    		get { return _phoneTypeID; }
    		set
    		{
    			if (_phoneTypeID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'PhoneTypeID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_phoneTypeID = value;
    				PhoneTypeIDChanged();
    				OnPropertyChanged("PhoneTypeID");
    			}
    		}
    	}
    	private int _phoneTypeID;
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
    
    	public TrackableCollection<AccountPhone> AccountPhones
    	{
    		get
    		{
    			if (_accountPhones == null)
    			{
    				_accountPhones = new TrackableCollection<AccountPhone>();
    				_accountPhones.CollectionChanged += FixupAccountPhones;
    				_accountPhones.CollectionChanged += RaiseAccountPhonesChanged;
    			}
    			return _accountPhones;
    		}
    		set
    		{
    			if (!ReferenceEquals(_accountPhones, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_accountPhones != null)
    				{
    					_accountPhones.CollectionChanged -= FixupAccountPhones;
    					_accountPhones.CollectionChanged -= RaiseAccountPhonesChanged;
    				}
    				_accountPhones = value;
    				if (_accountPhones != null)
    				{
    					_accountPhones.CollectionChanged += FixupAccountPhones;
    					_accountPhones.CollectionChanged += RaiseAccountPhonesChanged;
    				}
    				OnNavigationPropertyChanged("AccountPhones");
    			}
    		}
    	}
    	private TrackableCollection<AccountPhone> _accountPhones;
    	partial void AccountPhonesChanged();
    	private void RaiseAccountPhonesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		AccountPhonesChanged();
    	}
    
    	public TrackableCollection<Address> Addresses
    	{
    		get
    		{
    			if (_addresses == null)
    			{
    				_addresses = new TrackableCollection<Address>();
    				_addresses.CollectionChanged += FixupAddresses;
    				_addresses.CollectionChanged += RaiseAddressesChanged;
    			}
    			return _addresses;
    		}
    		set
    		{
    			if (!ReferenceEquals(_addresses, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_addresses != null)
    				{
    					_addresses.CollectionChanged -= FixupAddresses;
    					_addresses.CollectionChanged -= RaiseAddressesChanged;
    				}
    				_addresses = value;
    				if (_addresses != null)
    				{
    					_addresses.CollectionChanged += FixupAddresses;
    					_addresses.CollectionChanged += RaiseAddressesChanged;
    				}
    				OnNavigationPropertyChanged("Addresses");
    			}
    		}
    	}
    	private TrackableCollection<Address> _addresses;
    	partial void AddressesChanged();
    	private void RaiseAddressesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		AddressesChanged();
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
    		if (_accountPhones != null)
    		{
    			_accountPhones.CollectionChanged -= FixupAccountPhones;
    			_accountPhones.CollectionChanged -= RaiseAccountPhonesChanged;
    			_accountPhones.CollectionChanged += FixupAccountPhones;
    			_accountPhones.CollectionChanged += RaiseAccountPhonesChanged;
    		}
    		if (_addresses != null)
    		{
    			_addresses.CollectionChanged -= FixupAddresses;
    			_addresses.CollectionChanged -= RaiseAddressesChanged;
    			_addresses.CollectionChanged += FixupAddresses;
    			_addresses.CollectionChanged += RaiseAddressesChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		AccountPhones.Clear();
    		Addresses.Clear();
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupAccountPhones(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (AccountPhone item in e.NewItems)
    			{
    				item.PhoneType = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("AccountPhones", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (AccountPhone item in e.OldItems)
    			{
    				if (ReferenceEquals(item.PhoneType, this))
    				{
    					item.PhoneType = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("AccountPhones", item);
    				}
    			}
    		}
    	}
    
    	private void FixupAddresses(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (Address item in e.NewItems)
    			{
    				item.PhoneType = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Addresses", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (Address item in e.OldItems)
    			{
    				if (ReferenceEquals(item.PhoneType, this))
    				{
    					item.PhoneType = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Addresses", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}