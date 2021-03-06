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
    [KnownType(typeof(OptOut))]
    [KnownType(typeof(CampaignOptOut))]
    [Serializable]
    public partial class OptOutType: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void OptOutTypeIDChanged();
    	public short OptOutTypeID
    	{
    		get { return _optOutTypeID; }
    		set
    		{
    			if (_optOutTypeID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'OptOutTypeID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_optOutTypeID = value;
    				OptOutTypeIDChanged();
    				OnPropertyChanged("OptOutTypeID");
    			}
    		}
    	}
    	private short _optOutTypeID;
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
    
    	public TrackableCollection<OptOut> OptOuts
    	{
    		get
    		{
    			if (_optOuts == null)
    			{
    				_optOuts = new TrackableCollection<OptOut>();
    				_optOuts.CollectionChanged += FixupOptOuts;
    				_optOuts.CollectionChanged += RaiseOptOutsChanged;
    			}
    			return _optOuts;
    		}
    		set
    		{
    			if (!ReferenceEquals(_optOuts, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_optOuts != null)
    				{
    					_optOuts.CollectionChanged -= FixupOptOuts;
    					_optOuts.CollectionChanged -= RaiseOptOutsChanged;
    				}
    				_optOuts = value;
    				if (_optOuts != null)
    				{
    					_optOuts.CollectionChanged += FixupOptOuts;
    					_optOuts.CollectionChanged += RaiseOptOutsChanged;
    				}
    				OnNavigationPropertyChanged("OptOuts");
    			}
    		}
    	}
    	private TrackableCollection<OptOut> _optOuts;
    	partial void OptOutsChanged();
    	private void RaiseOptOutsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		OptOutsChanged();
    	}
    
    	public TrackableCollection<CampaignOptOut> CampaignOptOuts
    	{
    		get
    		{
    			if (_campaignOptOuts == null)
    			{
    				_campaignOptOuts = new TrackableCollection<CampaignOptOut>();
    				_campaignOptOuts.CollectionChanged += FixupCampaignOptOuts;
    				_campaignOptOuts.CollectionChanged += RaiseCampaignOptOutsChanged;
    			}
    			return _campaignOptOuts;
    		}
    		set
    		{
    			if (!ReferenceEquals(_campaignOptOuts, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_campaignOptOuts != null)
    				{
    					_campaignOptOuts.CollectionChanged -= FixupCampaignOptOuts;
    					_campaignOptOuts.CollectionChanged -= RaiseCampaignOptOutsChanged;
    				}
    				_campaignOptOuts = value;
    				if (_campaignOptOuts != null)
    				{
    					_campaignOptOuts.CollectionChanged += FixupCampaignOptOuts;
    					_campaignOptOuts.CollectionChanged += RaiseCampaignOptOutsChanged;
    				}
    				OnNavigationPropertyChanged("CampaignOptOuts");
    			}
    		}
    	}
    	private TrackableCollection<CampaignOptOut> _campaignOptOuts;
    	partial void CampaignOptOutsChanged();
    	private void RaiseCampaignOptOutsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		CampaignOptOutsChanged();
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
    		if (_optOuts != null)
    		{
    			_optOuts.CollectionChanged -= FixupOptOuts;
    			_optOuts.CollectionChanged -= RaiseOptOutsChanged;
    			_optOuts.CollectionChanged += FixupOptOuts;
    			_optOuts.CollectionChanged += RaiseOptOutsChanged;
    		}
    		if (_campaignOptOuts != null)
    		{
    			_campaignOptOuts.CollectionChanged -= FixupCampaignOptOuts;
    			_campaignOptOuts.CollectionChanged -= RaiseCampaignOptOutsChanged;
    			_campaignOptOuts.CollectionChanged += FixupCampaignOptOuts;
    			_campaignOptOuts.CollectionChanged += RaiseCampaignOptOutsChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		OptOuts.Clear();
    		CampaignOptOuts.Clear();
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupOptOuts(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (OptOut item in e.NewItems)
    			{
    				item.OptOutType = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("OptOuts", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (OptOut item in e.OldItems)
    			{
    				if (ReferenceEquals(item.OptOutType, this))
    				{
    					item.OptOutType = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("OptOuts", item);
    				}
    			}
    		}
    	}
    
    	private void FixupCampaignOptOuts(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (CampaignOptOut item in e.NewItems)
    			{
    				item.OptOutType = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("CampaignOptOuts", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (CampaignOptOut item in e.OldItems)
    			{
    				if (ReferenceEquals(item.OptOutType, this))
    				{
    					item.OptOutType = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("CampaignOptOuts", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}
