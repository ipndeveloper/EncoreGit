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
    [KnownType(typeof(HtmlSectionChoice))]
    [KnownType(typeof(HtmlSectionEditType))]
    [KnownType(typeof(CalendarEvent))]
    [KnownType(typeof(News))]
    [KnownType(typeof(HtmlSectionContent))]
    [KnownType(typeof(Site))]
    [KnownType(typeof(HtmlContentEditorType))]
    [KnownType(typeof(Layout))]
    [KnownType(typeof(Page))]
    [KnownType(typeof(Policy))]
    [Serializable]
    public partial class HtmlSection: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void HtmlSectionIDChanged();
    	public int HtmlSectionID
    	{
    		get { return _htmlSectionID; }
    		set
    		{
    			if (_htmlSectionID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'HtmlSectionID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_htmlSectionID = value;
    				HtmlSectionIDChanged();
    				OnPropertyChanged("HtmlSectionID");
    			}
    		}
    	}
    	private int _htmlSectionID;
    	partial void HtmlSectionEditTypeIDChanged();
    	public short HtmlSectionEditTypeID
    	{
    		get { return _htmlSectionEditTypeID; }
    		set
    		{
    			if (_htmlSectionEditTypeID != value)
    			{
    				ChangeTracker.RecordOriginalValue("HtmlSectionEditTypeID", _htmlSectionEditTypeID);
    				if (!IsDeserializing)
    				{
    					if (HtmlSectionEditType != null && HtmlSectionEditType.HtmlSectionEditTypeID != value)
    					{
    						HtmlSectionEditType = null;
    					}
    				}
    				_htmlSectionEditTypeID = value;
    				HtmlSectionEditTypeIDChanged();
    				OnPropertyChanged("HtmlSectionEditTypeID");
    			}
    		}
    	}
    	private short _htmlSectionEditTypeID;
    	partial void SectionNameChanged();
    	public string SectionName
    	{
    		get { return _sectionName; }
    		set
    		{
    			if (_sectionName != value)
    			{
    				ChangeTracker.RecordOriginalValue("SectionName", _sectionName);
    				_sectionName = value;
    				SectionNameChanged();
    				OnPropertyChanged("SectionName");
    			}
    		}
    	}
    	private string _sectionName;
    	partial void RequiresApprovalChanged();
    	public bool RequiresApproval
    	{
    		get { return _requiresApproval; }
    		set
    		{
    			if (_requiresApproval != value)
    			{
    				ChangeTracker.RecordOriginalValue("RequiresApproval", _requiresApproval);
    				_requiresApproval = value;
    				RequiresApprovalChanged();
    				OnPropertyChanged("RequiresApproval");
    			}
    		}
    	}
    	private bool _requiresApproval;
    	partial void HtmlContentEditorTypeIDChanged();
    	public short HtmlContentEditorTypeID
    	{
    		get { return _htmlContentEditorTypeID; }
    		set
    		{
    			if (_htmlContentEditorTypeID != value)
    			{
    				ChangeTracker.RecordOriginalValue("HtmlContentEditorTypeID", _htmlContentEditorTypeID);
    				if (!IsDeserializing)
    				{
    					if (HtmlContentEditorType != null && HtmlContentEditorType.HtmlContentEditorTypeID != value)
    					{
    						HtmlContentEditorType = null;
    					}
    				}
    				_htmlContentEditorTypeID = value;
    				HtmlContentEditorTypeIDChanged();
    				OnPropertyChanged("HtmlContentEditorTypeID");
    			}
    		}
    	}
    	private short _htmlContentEditorTypeID;
    	partial void WidthChanged();
    	public Nullable<short> Width
    	{
    		get { return _width; }
    		set
    		{
    			if (_width != value)
    			{
    				ChangeTracker.RecordOriginalValue("Width", _width);
    				_width = value;
    				WidthChanged();
    				OnPropertyChanged("Width");
    			}
    		}
    	}
    	private Nullable<short> _width;
    	partial void HeightChanged();
    	public Nullable<short> Height
    	{
    		get { return _height; }
    		set
    		{
    			if (_height != value)
    			{
    				ChangeTracker.RecordOriginalValue("Height", _height);
    				_height = value;
    				HeightChanged();
    				OnPropertyChanged("Height");
    			}
    		}
    	}
    	private Nullable<short> _height;

        #endregion
        #region Navigation Properties
    
    	public TrackableCollection<HtmlSectionChoice> HtmlSectionChoices
    	{
    		get
    		{
    			if (_htmlSectionChoices == null)
    			{
    				_htmlSectionChoices = new TrackableCollection<HtmlSectionChoice>();
    				_htmlSectionChoices.CollectionChanged += FixupHtmlSectionChoices;
    				_htmlSectionChoices.CollectionChanged += RaiseHtmlSectionChoicesChanged;
    			}
    			return _htmlSectionChoices;
    		}
    		set
    		{
    			if (!ReferenceEquals(_htmlSectionChoices, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_htmlSectionChoices != null)
    				{
    					_htmlSectionChoices.CollectionChanged -= FixupHtmlSectionChoices;
    					_htmlSectionChoices.CollectionChanged -= RaiseHtmlSectionChoicesChanged;
    				}
    				_htmlSectionChoices = value;
    				if (_htmlSectionChoices != null)
    				{
    					_htmlSectionChoices.CollectionChanged += FixupHtmlSectionChoices;
    					_htmlSectionChoices.CollectionChanged += RaiseHtmlSectionChoicesChanged;
    				}
    				OnNavigationPropertyChanged("HtmlSectionChoices");
    			}
    		}
    	}
    	private TrackableCollection<HtmlSectionChoice> _htmlSectionChoices;
    	partial void HtmlSectionChoicesChanged();
    	private void RaiseHtmlSectionChoicesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		HtmlSectionChoicesChanged();
    	}
    
    	public HtmlSectionEditType HtmlSectionEditType
    	{
    		get { return _htmlSectionEditType; }
    		set
    		{
    			if (!ReferenceEquals(_htmlSectionEditType, value))
    			{
    				var previousValue = _htmlSectionEditType;
    				_htmlSectionEditType = value;
    				FixupHtmlSectionEditType(previousValue);
    				OnNavigationPropertyChanged("HtmlSectionEditType");
    			}
    		}
    	}
    	private HtmlSectionEditType _htmlSectionEditType;
    
    	public TrackableCollection<CalendarEvent> CalendarEvents
    	{
    		get
    		{
    			if (_calendarEvents == null)
    			{
    				_calendarEvents = new TrackableCollection<CalendarEvent>();
    				_calendarEvents.CollectionChanged += FixupCalendarEvents;
    				_calendarEvents.CollectionChanged += RaiseCalendarEventsChanged;
    			}
    			return _calendarEvents;
    		}
    		set
    		{
    			if (!ReferenceEquals(_calendarEvents, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_calendarEvents != null)
    				{
    					_calendarEvents.CollectionChanged -= FixupCalendarEvents;
    					_calendarEvents.CollectionChanged -= RaiseCalendarEventsChanged;
    				}
    				_calendarEvents = value;
    				if (_calendarEvents != null)
    				{
    					_calendarEvents.CollectionChanged += FixupCalendarEvents;
    					_calendarEvents.CollectionChanged += RaiseCalendarEventsChanged;
    				}
    				OnNavigationPropertyChanged("CalendarEvents");
    			}
    		}
    	}
    	private TrackableCollection<CalendarEvent> _calendarEvents;
    	partial void CalendarEventsChanged();
    	private void RaiseCalendarEventsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		CalendarEventsChanged();
    	}
    
    	public TrackableCollection<News> News
    	{
    		get
    		{
    			if (_news == null)
    			{
    				_news = new TrackableCollection<News>();
    				_news.CollectionChanged += FixupNews;
    				_news.CollectionChanged += RaiseNewsChanged;
    			}
    			return _news;
    		}
    		set
    		{
    			if (!ReferenceEquals(_news, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_news != null)
    				{
    					_news.CollectionChanged -= FixupNews;
    					_news.CollectionChanged -= RaiseNewsChanged;
    				}
    				_news = value;
    				if (_news != null)
    				{
    					_news.CollectionChanged += FixupNews;
    					_news.CollectionChanged += RaiseNewsChanged;
    				}
    				OnNavigationPropertyChanged("News");
    			}
    		}
    	}
    	private TrackableCollection<News> _news;
    	partial void NewsChanged();
    	private void RaiseNewsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		NewsChanged();
    	}
    
    	public TrackableCollection<HtmlSectionContent> HtmlSectionContents
    	{
    		get
    		{
    			if (_htmlSectionContents == null)
    			{
    				_htmlSectionContents = new TrackableCollection<HtmlSectionContent>();
    				_htmlSectionContents.CollectionChanged += FixupHtmlSectionContents;
    				_htmlSectionContents.CollectionChanged += RaiseHtmlSectionContentsChanged;
    			}
    			return _htmlSectionContents;
    		}
    		set
    		{
    			if (!ReferenceEquals(_htmlSectionContents, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_htmlSectionContents != null)
    				{
    					_htmlSectionContents.CollectionChanged -= FixupHtmlSectionContents;
    					_htmlSectionContents.CollectionChanged -= RaiseHtmlSectionContentsChanged;
    				}
    				_htmlSectionContents = value;
    				if (_htmlSectionContents != null)
    				{
    					_htmlSectionContents.CollectionChanged += FixupHtmlSectionContents;
    					_htmlSectionContents.CollectionChanged += RaiseHtmlSectionContentsChanged;
    				}
    				OnNavigationPropertyChanged("HtmlSectionContents");
    			}
    		}
    	}
    	private TrackableCollection<HtmlSectionContent> _htmlSectionContents;
    	partial void HtmlSectionContentsChanged();
    	private void RaiseHtmlSectionContentsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		HtmlSectionContentsChanged();
    	}
    
    	public TrackableCollection<Site> Sites
    	{
    		get
    		{
    			if (_sites == null)
    			{
    				_sites = new TrackableCollection<Site>();
    				_sites.CollectionChanged += FixupSites;
    				_sites.CollectionChanged += RaiseSitesChanged;
    			}
    			return _sites;
    		}
    		set
    		{
    			if (!ReferenceEquals(_sites, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_sites != null)
    				{
    					_sites.CollectionChanged -= FixupSites;
    					_sites.CollectionChanged -= RaiseSitesChanged;
    				}
    				_sites = value;
    				if (_sites != null)
    				{
    					_sites.CollectionChanged += FixupSites;
    					_sites.CollectionChanged += RaiseSitesChanged;
    				}
    				OnNavigationPropertyChanged("Sites");
    			}
    		}
    	}
    	private TrackableCollection<Site> _sites;
    	partial void SitesChanged();
    	private void RaiseSitesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		SitesChanged();
    	}
    
    	public HtmlContentEditorType HtmlContentEditorType
    	{
    		get { return _htmlContentEditorType; }
    		set
    		{
    			if (!ReferenceEquals(_htmlContentEditorType, value))
    			{
    				var previousValue = _htmlContentEditorType;
    				_htmlContentEditorType = value;
    				FixupHtmlContentEditorType(previousValue);
    				OnNavigationPropertyChanged("HtmlContentEditorType");
    			}
    		}
    	}
    	private HtmlContentEditorType _htmlContentEditorType;
    
    	public TrackableCollection<Layout> Layouts
    	{
    		get
    		{
    			if (_layouts == null)
    			{
    				_layouts = new TrackableCollection<Layout>();
    				_layouts.CollectionChanged += FixupLayouts;
    				_layouts.CollectionChanged += RaiseLayoutsChanged;
    			}
    			return _layouts;
    		}
    		set
    		{
    			if (!ReferenceEquals(_layouts, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_layouts != null)
    				{
    					_layouts.CollectionChanged -= FixupLayouts;
    					_layouts.CollectionChanged -= RaiseLayoutsChanged;
    				}
    				_layouts = value;
    				if (_layouts != null)
    				{
    					_layouts.CollectionChanged += FixupLayouts;
    					_layouts.CollectionChanged += RaiseLayoutsChanged;
    				}
    				OnNavigationPropertyChanged("Layouts");
    			}
    		}
    	}
    	private TrackableCollection<Layout> _layouts;
    	partial void LayoutsChanged();
    	private void RaiseLayoutsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		LayoutsChanged();
    	}
    
    	public TrackableCollection<Page> Pages
    	{
    		get
    		{
    			if (_pages == null)
    			{
    				_pages = new TrackableCollection<Page>();
    				_pages.CollectionChanged += FixupPages;
    				_pages.CollectionChanged += RaisePagesChanged;
    			}
    			return _pages;
    		}
    		set
    		{
    			if (!ReferenceEquals(_pages, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_pages != null)
    				{
    					_pages.CollectionChanged -= FixupPages;
    					_pages.CollectionChanged -= RaisePagesChanged;
    				}
    				_pages = value;
    				if (_pages != null)
    				{
    					_pages.CollectionChanged += FixupPages;
    					_pages.CollectionChanged += RaisePagesChanged;
    				}
    				OnNavigationPropertyChanged("Pages");
    			}
    		}
    	}
    	private TrackableCollection<Page> _pages;
    	partial void PagesChanged();
    	private void RaisePagesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		PagesChanged();
    	}
    
    	public TrackableCollection<Policy> Policies
    	{
    		get
    		{
    			if (_policies == null)
    			{
    				_policies = new TrackableCollection<Policy>();
    				_policies.CollectionChanged += FixupPolicies;
    				_policies.CollectionChanged += RaisePoliciesChanged;
    			}
    			return _policies;
    		}
    		set
    		{
    			if (!ReferenceEquals(_policies, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_policies != null)
    				{
    					_policies.CollectionChanged -= FixupPolicies;
    					_policies.CollectionChanged -= RaisePoliciesChanged;
    				}
    				_policies = value;
    				if (_policies != null)
    				{
    					_policies.CollectionChanged += FixupPolicies;
    					_policies.CollectionChanged += RaisePoliciesChanged;
    				}
    				OnNavigationPropertyChanged("Policies");
    			}
    		}
    	}
    	private TrackableCollection<Policy> _policies;
    	partial void PoliciesChanged();
    	private void RaisePoliciesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		PoliciesChanged();
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
    		if (_htmlSectionChoices != null)
    		{
    			_htmlSectionChoices.CollectionChanged -= FixupHtmlSectionChoices;
    			_htmlSectionChoices.CollectionChanged -= RaiseHtmlSectionChoicesChanged;
    			_htmlSectionChoices.CollectionChanged += FixupHtmlSectionChoices;
    			_htmlSectionChoices.CollectionChanged += RaiseHtmlSectionChoicesChanged;
    		}
    		if (_calendarEvents != null)
    		{
    			_calendarEvents.CollectionChanged -= FixupCalendarEvents;
    			_calendarEvents.CollectionChanged -= RaiseCalendarEventsChanged;
    			_calendarEvents.CollectionChanged += FixupCalendarEvents;
    			_calendarEvents.CollectionChanged += RaiseCalendarEventsChanged;
    		}
    		if (_news != null)
    		{
    			_news.CollectionChanged -= FixupNews;
    			_news.CollectionChanged -= RaiseNewsChanged;
    			_news.CollectionChanged += FixupNews;
    			_news.CollectionChanged += RaiseNewsChanged;
    		}
    		if (_htmlSectionContents != null)
    		{
    			_htmlSectionContents.CollectionChanged -= FixupHtmlSectionContents;
    			_htmlSectionContents.CollectionChanged -= RaiseHtmlSectionContentsChanged;
    			_htmlSectionContents.CollectionChanged += FixupHtmlSectionContents;
    			_htmlSectionContents.CollectionChanged += RaiseHtmlSectionContentsChanged;
    		}
    		if (_sites != null)
    		{
    			_sites.CollectionChanged -= FixupSites;
    			_sites.CollectionChanged -= RaiseSitesChanged;
    			_sites.CollectionChanged += FixupSites;
    			_sites.CollectionChanged += RaiseSitesChanged;
    		}
    		if (_layouts != null)
    		{
    			_layouts.CollectionChanged -= FixupLayouts;
    			_layouts.CollectionChanged -= RaiseLayoutsChanged;
    			_layouts.CollectionChanged += FixupLayouts;
    			_layouts.CollectionChanged += RaiseLayoutsChanged;
    		}
    		if (_pages != null)
    		{
    			_pages.CollectionChanged -= FixupPages;
    			_pages.CollectionChanged -= RaisePagesChanged;
    			_pages.CollectionChanged += FixupPages;
    			_pages.CollectionChanged += RaisePagesChanged;
    		}
    		if (_policies != null)
    		{
    			_policies.CollectionChanged -= FixupPolicies;
    			_policies.CollectionChanged -= RaisePoliciesChanged;
    			_policies.CollectionChanged += FixupPolicies;
    			_policies.CollectionChanged += RaisePoliciesChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		HtmlSectionChoices.Clear();
    		HtmlSectionEditType = null;
    		CalendarEvents.Clear();
    		News.Clear();
    		HtmlSectionContents.Clear();
    		Sites.Clear();
    		HtmlContentEditorType = null;
    		Layouts.Clear();
    		Pages.Clear();
    		Policies.Clear();
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupHtmlSectionEditType(HtmlSectionEditType previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.HtmlSections.Contains(this))
    		{
    			previousValue.HtmlSections.Remove(this);
    		}
    
    		if (HtmlSectionEditType != null)
    		{
    			if (!HtmlSectionEditType.HtmlSections.Contains(this))
    			{
    				HtmlSectionEditType.HtmlSections.Add(this);
    			}
    
    			HtmlSectionEditTypeID = HtmlSectionEditType.HtmlSectionEditTypeID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("HtmlSectionEditType")
    				&& (ChangeTracker.OriginalValues["HtmlSectionEditType"] == HtmlSectionEditType))
    			{
    				ChangeTracker.OriginalValues.Remove("HtmlSectionEditType");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("HtmlSectionEditType", previousValue);
    			}
    			if (HtmlSectionEditType != null && !HtmlSectionEditType.ChangeTracker.ChangeTrackingEnabled)
    			{
    				HtmlSectionEditType.StartTracking();
    			}
    		}
    	}
    
    	private void FixupHtmlContentEditorType(HtmlContentEditorType previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.HtmlSections.Contains(this))
    		{
    			previousValue.HtmlSections.Remove(this);
    		}
    
    		if (HtmlContentEditorType != null)
    		{
    			if (!HtmlContentEditorType.HtmlSections.Contains(this))
    			{
    				HtmlContentEditorType.HtmlSections.Add(this);
    			}
    
    			HtmlContentEditorTypeID = HtmlContentEditorType.HtmlContentEditorTypeID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("HtmlContentEditorType")
    				&& (ChangeTracker.OriginalValues["HtmlContentEditorType"] == HtmlContentEditorType))
    			{
    				ChangeTracker.OriginalValues.Remove("HtmlContentEditorType");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("HtmlContentEditorType", previousValue);
    			}
    			if (HtmlContentEditorType != null && !HtmlContentEditorType.ChangeTracker.ChangeTrackingEnabled)
    			{
    				HtmlContentEditorType.StartTracking();
    			}
    		}
    	}
    
    	private void FixupHtmlSectionChoices(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (HtmlSectionChoice item in e.NewItems)
    			{
    				item.HtmlSection = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("HtmlSectionChoices", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (HtmlSectionChoice item in e.OldItems)
    			{
    				if (ReferenceEquals(item.HtmlSection, this))
    				{
    					item.HtmlSection = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("HtmlSectionChoices", item);
    				}
    			}
    		}
    	}
    
    	private void FixupCalendarEvents(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (CalendarEvent item in e.NewItems)
    			{
    				item.HtmlSection = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("CalendarEvents", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (CalendarEvent item in e.OldItems)
    			{
    				if (ReferenceEquals(item.HtmlSection, this))
    				{
    					item.HtmlSection = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("CalendarEvents", item);
    				}
    			}
    		}
    	}
    
    	private void FixupNews(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (News item in e.NewItems)
    			{
    				item.HtmlSection = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("News", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (News item in e.OldItems)
    			{
    				if (ReferenceEquals(item.HtmlSection, this))
    				{
    					item.HtmlSection = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("News", item);
    				}
    			}
    		}
    	}
    
    	private void FixupHtmlSectionContents(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (HtmlSectionContent item in e.NewItems)
    			{
    				item.HtmlSection = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("HtmlSectionContents", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (HtmlSectionContent item in e.OldItems)
    			{
    				if (ReferenceEquals(item.HtmlSection, this))
    				{
    					item.HtmlSection = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("HtmlSectionContents", item);
    				}
    			}
    		}
    	}
    
    	private void FixupSites(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (Site item in e.NewItems)
    			{
    				if (!item.HtmlSections.Contains(this))
    				{
    					item.HtmlSections.Add(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Sites", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (Site item in e.OldItems)
    			{
    				if (item.HtmlSections.Contains(this))
    				{
    					item.HtmlSections.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Sites", item);
    				}
    			}
    		}
    	}
    
    	private void FixupLayouts(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (Layout item in e.NewItems)
    			{
    				if (!item.HtmlSections.Contains(this))
    				{
    					item.HtmlSections.Add(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Layouts", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (Layout item in e.OldItems)
    			{
    				if (item.HtmlSections.Contains(this))
    				{
    					item.HtmlSections.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Layouts", item);
    				}
    			}
    		}
    	}
    
    	private void FixupPages(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (Page item in e.NewItems)
    			{
    				if (!item.HtmlSections.Contains(this))
    				{
    					item.HtmlSections.Add(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Pages", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (Page item in e.OldItems)
    			{
    				if (item.HtmlSections.Contains(this))
    				{
    					item.HtmlSections.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Pages", item);
    				}
    			}
    		}
    	}
    
    	private void FixupPolicies(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (Policy item in e.NewItems)
    			{
    				item.HtmlSection = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Policies", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (Policy item in e.OldItems)
    			{
    				if (ReferenceEquals(item.HtmlSection, this))
    				{
    					item.HtmlSection = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Policies", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}
