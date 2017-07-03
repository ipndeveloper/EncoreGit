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
    [KnownType(typeof(DomainEventQueueItem))]
    [KnownType(typeof(CampaignOptOut))]
    [KnownType(typeof(Campaign))]
    [KnownType(typeof(DomainEventTypeCategory))]
    [Serializable]
    public partial class DomainEventType: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void DomainEventTypeIDChanged();
    	public short DomainEventTypeID
    	{
    		get { return _domainEventTypeID; }
    		set
    		{
    			if (_domainEventTypeID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'DomainEventTypeID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_domainEventTypeID = value;
    				DomainEventTypeIDChanged();
    				OnPropertyChanged("DomainEventTypeID");
    			}
    		}
    	}
    	private short _domainEventTypeID;
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
    	partial void DomainEventTypeCategoryIDChanged();
    	public int DomainEventTypeCategoryID
    	{
    		get { return _domainEventTypeCategoryID; }
    		set
    		{
    			if (_domainEventTypeCategoryID != value)
    			{
    				ChangeTracker.RecordOriginalValue("DomainEventTypeCategoryID", _domainEventTypeCategoryID);
    				if (!IsDeserializing)
    				{
    					if (DomainEventTypeCategory != null && DomainEventTypeCategory.DomainEventTypeCategoryID != value)
    					{
    						DomainEventTypeCategory = null;
    					}
    				}
    				_domainEventTypeCategoryID = value;
    				DomainEventTypeCategoryIDChanged();
    				OnPropertyChanged("DomainEventTypeCategoryID");
    			}
    		}
    	}
    	private int _domainEventTypeCategoryID;

        #endregion
        #region Navigation Properties
    
    	public TrackableCollection<DomainEventQueueItem> DomainEventQueueItems
    	{
    		get
    		{
    			if (_domainEventQueueItems == null)
    			{
    				_domainEventQueueItems = new TrackableCollection<DomainEventQueueItem>();
    				_domainEventQueueItems.CollectionChanged += FixupDomainEventQueueItems;
    				_domainEventQueueItems.CollectionChanged += RaiseDomainEventQueueItemsChanged;
    			}
    			return _domainEventQueueItems;
    		}
    		set
    		{
    			if (!ReferenceEquals(_domainEventQueueItems, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_domainEventQueueItems != null)
    				{
    					_domainEventQueueItems.CollectionChanged -= FixupDomainEventQueueItems;
    					_domainEventQueueItems.CollectionChanged -= RaiseDomainEventQueueItemsChanged;
    				}
    				_domainEventQueueItems = value;
    				if (_domainEventQueueItems != null)
    				{
    					_domainEventQueueItems.CollectionChanged += FixupDomainEventQueueItems;
    					_domainEventQueueItems.CollectionChanged += RaiseDomainEventQueueItemsChanged;
    				}
    				OnNavigationPropertyChanged("DomainEventQueueItems");
    			}
    		}
    	}
    	private TrackableCollection<DomainEventQueueItem> _domainEventQueueItems;
    	partial void DomainEventQueueItemsChanged();
    	private void RaiseDomainEventQueueItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		DomainEventQueueItemsChanged();
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
    
    	public TrackableCollection<Campaign> Campaigns
    	{
    		get
    		{
    			if (_campaigns == null)
    			{
    				_campaigns = new TrackableCollection<Campaign>();
    				_campaigns.CollectionChanged += FixupCampaigns;
    				_campaigns.CollectionChanged += RaiseCampaignsChanged;
    			}
    			return _campaigns;
    		}
    		set
    		{
    			if (!ReferenceEquals(_campaigns, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_campaigns != null)
    				{
    					_campaigns.CollectionChanged -= FixupCampaigns;
    					_campaigns.CollectionChanged -= RaiseCampaignsChanged;
    				}
    				_campaigns = value;
    				if (_campaigns != null)
    				{
    					_campaigns.CollectionChanged += FixupCampaigns;
    					_campaigns.CollectionChanged += RaiseCampaignsChanged;
    				}
    				OnNavigationPropertyChanged("Campaigns");
    			}
    		}
    	}
    	private TrackableCollection<Campaign> _campaigns;
    	partial void CampaignsChanged();
    	private void RaiseCampaignsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		CampaignsChanged();
    	}
    
    	public DomainEventTypeCategory DomainEventTypeCategory
    	{
    		get { return _domainEventTypeCategory; }
    		set
    		{
    			if (!ReferenceEquals(_domainEventTypeCategory, value))
    			{
    				var previousValue = _domainEventTypeCategory;
    				_domainEventTypeCategory = value;
    				FixupDomainEventTypeCategory(previousValue);
    				OnNavigationPropertyChanged("DomainEventTypeCategory");
    			}
    		}
    	}
    	private DomainEventTypeCategory _domainEventTypeCategory;

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
    		if (_domainEventQueueItems != null)
    		{
    			_domainEventQueueItems.CollectionChanged -= FixupDomainEventQueueItems;
    			_domainEventQueueItems.CollectionChanged -= RaiseDomainEventQueueItemsChanged;
    			_domainEventQueueItems.CollectionChanged += FixupDomainEventQueueItems;
    			_domainEventQueueItems.CollectionChanged += RaiseDomainEventQueueItemsChanged;
    		}
    		if (_campaignOptOuts != null)
    		{
    			_campaignOptOuts.CollectionChanged -= FixupCampaignOptOuts;
    			_campaignOptOuts.CollectionChanged -= RaiseCampaignOptOutsChanged;
    			_campaignOptOuts.CollectionChanged += FixupCampaignOptOuts;
    			_campaignOptOuts.CollectionChanged += RaiseCampaignOptOutsChanged;
    		}
    		if (_campaigns != null)
    		{
    			_campaigns.CollectionChanged -= FixupCampaigns;
    			_campaigns.CollectionChanged -= RaiseCampaignsChanged;
    			_campaigns.CollectionChanged += FixupCampaigns;
    			_campaigns.CollectionChanged += RaiseCampaignsChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		DomainEventQueueItems.Clear();
    		CampaignOptOuts.Clear();
    		Campaigns.Clear();
    		DomainEventTypeCategory = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupDomainEventTypeCategory(DomainEventTypeCategory previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.DomainEventTypes.Contains(this))
    		{
    			previousValue.DomainEventTypes.Remove(this);
    		}
    
    		if (DomainEventTypeCategory != null)
    		{
    			if (!DomainEventTypeCategory.DomainEventTypes.Contains(this))
    			{
    				DomainEventTypeCategory.DomainEventTypes.Add(this);
    			}
    
    			DomainEventTypeCategoryID = DomainEventTypeCategory.DomainEventTypeCategoryID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("DomainEventTypeCategory")
    				&& (ChangeTracker.OriginalValues["DomainEventTypeCategory"] == DomainEventTypeCategory))
    			{
    				ChangeTracker.OriginalValues.Remove("DomainEventTypeCategory");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("DomainEventTypeCategory", previousValue);
    			}
    			if (DomainEventTypeCategory != null && !DomainEventTypeCategory.ChangeTracker.ChangeTrackingEnabled)
    			{
    				DomainEventTypeCategory.StartTracking();
    			}
    		}
    	}
    
    	private void FixupDomainEventQueueItems(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (DomainEventQueueItem item in e.NewItems)
    			{
    				item.DomainEventType = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("DomainEventQueueItems", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (DomainEventQueueItem item in e.OldItems)
    			{
    				if (ReferenceEquals(item.DomainEventType, this))
    				{
    					item.DomainEventType = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("DomainEventQueueItems", item);
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
    				item.DomainEventType = this;
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
    				if (ReferenceEquals(item.DomainEventType, this))
    				{
    					item.DomainEventType = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("CampaignOptOuts", item);
    				}
    			}
    		}
    	}
    
    	private void FixupCampaigns(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (Campaign item in e.NewItems)
    			{
    				item.DomainEventType = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Campaigns", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (Campaign item in e.OldItems)
    			{
    				if (ReferenceEquals(item.DomainEventType, this))
    				{
    					item.DomainEventType = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Campaigns", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}
