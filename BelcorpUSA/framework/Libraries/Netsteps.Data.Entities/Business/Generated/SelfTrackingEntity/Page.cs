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
    [KnownType(typeof(Layout))]
    [KnownType(typeof(Navigation))]
    [KnownType(typeof(Page))]
    [KnownType(typeof(Site))]
    [KnownType(typeof(User))]
    [KnownType(typeof(PageTranslation))]
    [KnownType(typeof(HtmlSection))]
    [KnownType(typeof(PageType))]
    [Serializable]
    public partial class Page: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void PageIDChanged();
    	public int PageID
    	{
    		get { return _pageID; }
    		set
    		{
    			if (_pageID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'PageID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_pageID = value;
    				PageIDChanged();
    				OnPropertyChanged("PageID");
    			}
    		}
    	}
    	private int _pageID;
    	partial void ParentIDChanged();
    	public Nullable<int> ParentID
    	{
    		get { return _parentID; }
    		set
    		{
    			if (_parentID != value)
    			{
    				ChangeTracker.RecordOriginalValue("ParentID", _parentID);
    				if (!IsDeserializing)
    				{
    					if (Page1 != null && Page1.PageID != value)
    					{
    						Page1 = null;
    					}
    				}
    				_parentID = value;
    				ParentIDChanged();
    				OnPropertyChanged("ParentID");
    			}
    		}
    	}
    	private Nullable<int> _parentID;
    	partial void SiteIDChanged();
    	public Nullable<int> SiteID
    	{
    		get { return _siteID; }
    		set
    		{
    			if (_siteID != value)
    			{
    				ChangeTracker.RecordOriginalValue("SiteID", _siteID);
    				if (!IsDeserializing)
    				{
    					if (Site != null && Site.SiteID != value)
    					{
    						Site = null;
    					}
    				}
    				_siteID = value;
    				SiteIDChanged();
    				OnPropertyChanged("SiteID");
    			}
    		}
    	}
    	private Nullable<int> _siteID;
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
    	partial void UrlChanged();
    	public string Url
    	{
    		get { return _url; }
    		set
    		{
    			if (_url != value)
    			{
    				ChangeTracker.RecordOriginalValue("Url", _url);
    				_url = value;
    				UrlChanged();
    				OnPropertyChanged("Url");
    			}
    		}
    	}
    	private string _url;
    	partial void RequiresAuthenticationChanged();
    	public Nullable<bool> RequiresAuthentication
    	{
    		get { return _requiresAuthentication; }
    		set
    		{
    			if (_requiresAuthentication != value)
    			{
    				ChangeTracker.RecordOriginalValue("RequiresAuthentication", _requiresAuthentication);
    				_requiresAuthentication = value;
    				RequiresAuthenticationChanged();
    				OnPropertyChanged("RequiresAuthentication");
    			}
    		}
    	}
    	private Nullable<bool> _requiresAuthentication;
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
    	partial void EditableChanged();
    	public bool Editable
    	{
    		get { return _editable; }
    		set
    		{
    			if (_editable != value)
    			{
    				ChangeTracker.RecordOriginalValue("Editable", _editable);
    				_editable = value;
    				EditableChanged();
    				OnPropertyChanged("Editable");
    			}
    		}
    	}
    	private bool _editable;
    	partial void UseSslChanged();
    	public Nullable<bool> UseSsl
    	{
    		get { return _useSsl; }
    		set
    		{
    			if (_useSsl != value)
    			{
    				ChangeTracker.RecordOriginalValue("UseSsl", _useSsl);
    				_useSsl = value;
    				UseSslChanged();
    				OnPropertyChanged("UseSsl");
    			}
    		}
    	}
    	private Nullable<bool> _useSsl;
    	partial void IsStartPageChanged();
    	public bool IsStartPage
    	{
    		get { return _isStartPage; }
    		set
    		{
    			if (_isStartPage != value)
    			{
    				ChangeTracker.RecordOriginalValue("IsStartPage", _isStartPage);
    				_isStartPage = value;
    				IsStartPageChanged();
    				OnPropertyChanged("IsStartPage");
    			}
    		}
    	}
    	private bool _isStartPage;
    	partial void ModifiedByUserIDChanged();
    	public Nullable<int> ModifiedByUserID
    	{
    		get { return _modifiedByUserID; }
    		set
    		{
    			if (_modifiedByUserID != value)
    			{
    				ChangeTracker.RecordOriginalValue("ModifiedByUserID", _modifiedByUserID);
    				if (!IsDeserializing)
    				{
    					if (User != null && User.UserID != value)
    					{
    						User = null;
    					}
    				}
    				_modifiedByUserID = value;
    				ModifiedByUserIDChanged();
    				OnPropertyChanged("ModifiedByUserID");
    			}
    		}
    	}
    	private Nullable<int> _modifiedByUserID;
    	partial void LayoutIDChanged();
    	public int LayoutID
    	{
    		get { return _layoutID; }
    		set
    		{
    			if (_layoutID != value)
    			{
    				ChangeTracker.RecordOriginalValue("LayoutID", _layoutID);
    				if (!IsDeserializing)
    				{
    					if (Layout != null && Layout.LayoutID != value)
    					{
    						Layout = null;
    					}
    				}
    				_layoutID = value;
    				LayoutIDChanged();
    				OnPropertyChanged("LayoutID");
    			}
    		}
    	}
    	private int _layoutID;
    	partial void PageTypeIDChanged();
    	public short PageTypeID
    	{
    		get { return _pageTypeID; }
    		set
    		{
    			if (_pageTypeID != value)
    			{
    				ChangeTracker.RecordOriginalValue("PageTypeID", _pageTypeID);
    				if (!IsDeserializing)
    				{
    					if (PageType != null && PageType.PageTypeID != value)
    					{
    						PageType = null;
    					}
    				}
    				_pageTypeID = value;
    				PageTypeIDChanged();
    				OnPropertyChanged("PageTypeID");
    			}
    		}
    	}
    	private short _pageTypeID;
    	partial void ExternalUrlChanged();
    	public string ExternalUrl
    	{
    		get { return _externalUrl; }
    		set
    		{
    			if (_externalUrl != value)
    			{
    				ChangeTracker.RecordOriginalValue("ExternalUrl", _externalUrl);
    				_externalUrl = value;
    				ExternalUrlChanged();
    				OnPropertyChanged("ExternalUrl");
    			}
    		}
    	}
    	private string _externalUrl;

        #endregion
        #region Navigation Properties
    
    	public Layout Layout
    	{
    		get { return _layout; }
    		set
    		{
    			if (!ReferenceEquals(_layout, value))
    			{
    				var previousValue = _layout;
    				_layout = value;
    				FixupLayout(previousValue);
    				OnNavigationPropertyChanged("Layout");
    			}
    		}
    	}
    	private Layout _layout;
    
    	public TrackableCollection<Navigation> Navigations
    	{
    		get
    		{
    			if (_navigations == null)
    			{
    				_navigations = new TrackableCollection<Navigation>();
    				_navigations.CollectionChanged += FixupNavigations;
    				_navigations.CollectionChanged += RaiseNavigationsChanged;
    			}
    			return _navigations;
    		}
    		set
    		{
    			if (!ReferenceEquals(_navigations, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_navigations != null)
    				{
    					_navigations.CollectionChanged -= FixupNavigations;
    					_navigations.CollectionChanged -= RaiseNavigationsChanged;
    				}
    				_navigations = value;
    				if (_navigations != null)
    				{
    					_navigations.CollectionChanged += FixupNavigations;
    					_navigations.CollectionChanged += RaiseNavigationsChanged;
    				}
    				OnNavigationPropertyChanged("Navigations");
    			}
    		}
    	}
    	private TrackableCollection<Navigation> _navigations;
    	partial void NavigationsChanged();
    	private void RaiseNavigationsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		NavigationsChanged();
    	}
    
    	public TrackableCollection<Page> Pages1
    	{
    		get
    		{
    			if (_pages1 == null)
    			{
    				_pages1 = new TrackableCollection<Page>();
    				_pages1.CollectionChanged += FixupPages1;
    				_pages1.CollectionChanged += RaisePages1Changed;
    			}
    			return _pages1;
    		}
    		set
    		{
    			if (!ReferenceEquals(_pages1, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_pages1 != null)
    				{
    					_pages1.CollectionChanged -= FixupPages1;
    					_pages1.CollectionChanged -= RaisePages1Changed;
    				}
    				_pages1 = value;
    				if (_pages1 != null)
    				{
    					_pages1.CollectionChanged += FixupPages1;
    					_pages1.CollectionChanged += RaisePages1Changed;
    				}
    				OnNavigationPropertyChanged("Pages1");
    			}
    		}
    	}
    	private TrackableCollection<Page> _pages1;
    	partial void Pages1Changed();
    	private void RaisePages1Changed(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		Pages1Changed();
    	}
    
    	public Page Page1
    	{
    		get { return _page1; }
    		set
    		{
    			if (!ReferenceEquals(_page1, value))
    			{
    				var previousValue = _page1;
    				_page1 = value;
    				FixupPage1(previousValue);
    				OnNavigationPropertyChanged("Page1");
    			}
    		}
    	}
    	private Page _page1;
    
    	public Site Site
    	{
    		get { return _site; }
    		set
    		{
    			if (!ReferenceEquals(_site, value))
    			{
    				var previousValue = _site;
    				_site = value;
    				FixupSite(previousValue);
    				OnNavigationPropertyChanged("Site");
    			}
    		}
    	}
    	private Site _site;
    
    	public User User
    	{
    		get { return _user; }
    		set
    		{
    			if (!ReferenceEquals(_user, value))
    			{
    				var previousValue = _user;
    				_user = value;
    				FixupUser(previousValue);
    				OnNavigationPropertyChanged("User");
    			}
    		}
    	}
    	private User _user;
    
    	public TrackableCollection<PageTranslation> Translations
    	{
    		get
    		{
    			if (_translations == null)
    			{
    				_translations = new TrackableCollection<PageTranslation>();
    				_translations.CollectionChanged += FixupTranslations;
    				_translations.CollectionChanged += RaiseTranslationsChanged;
    			}
    			return _translations;
    		}
    		set
    		{
    			if (!ReferenceEquals(_translations, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_translations != null)
    				{
    					_translations.CollectionChanged -= FixupTranslations;
    					_translations.CollectionChanged -= RaiseTranslationsChanged;
    				}
    				_translations = value;
    				if (_translations != null)
    				{
    					_translations.CollectionChanged += FixupTranslations;
    					_translations.CollectionChanged += RaiseTranslationsChanged;
    				}
    				OnNavigationPropertyChanged("Translations");
    			}
    		}
    	}
    	private TrackableCollection<PageTranslation> _translations;
    	partial void TranslationsChanged();
    	private void RaiseTranslationsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		TranslationsChanged();
    	}
    
    	public TrackableCollection<HtmlSection> HtmlSections
    	{
    		get
    		{
    			if (_htmlSections == null)
    			{
    				_htmlSections = new TrackableCollection<HtmlSection>();
    				_htmlSections.CollectionChanged += FixupHtmlSections;
    				_htmlSections.CollectionChanged += RaiseHtmlSectionsChanged;
    			}
    			return _htmlSections;
    		}
    		set
    		{
    			if (!ReferenceEquals(_htmlSections, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_htmlSections != null)
    				{
    					_htmlSections.CollectionChanged -= FixupHtmlSections;
    					_htmlSections.CollectionChanged -= RaiseHtmlSectionsChanged;
    				}
    				_htmlSections = value;
    				if (_htmlSections != null)
    				{
    					_htmlSections.CollectionChanged += FixupHtmlSections;
    					_htmlSections.CollectionChanged += RaiseHtmlSectionsChanged;
    				}
    				OnNavigationPropertyChanged("HtmlSections");
    			}
    		}
    	}
    	private TrackableCollection<HtmlSection> _htmlSections;
    	partial void HtmlSectionsChanged();
    	private void RaiseHtmlSectionsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		HtmlSectionsChanged();
    	}
    
    	public PageType PageType
    	{
    		get { return _pageType; }
    		set
    		{
    			if (!ReferenceEquals(_pageType, value))
    			{
    				var previousValue = _pageType;
    				_pageType = value;
    				FixupPageType(previousValue);
    				OnNavigationPropertyChanged("PageType");
    			}
    		}
    	}
    	private PageType _pageType;

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
    		if (_navigations != null)
    		{
    			_navigations.CollectionChanged -= FixupNavigations;
    			_navigations.CollectionChanged -= RaiseNavigationsChanged;
    			_navigations.CollectionChanged += FixupNavigations;
    			_navigations.CollectionChanged += RaiseNavigationsChanged;
    		}
    		if (_pages1 != null)
    		{
    			_pages1.CollectionChanged -= FixupPages1;
    			_pages1.CollectionChanged -= RaisePages1Changed;
    			_pages1.CollectionChanged += FixupPages1;
    			_pages1.CollectionChanged += RaisePages1Changed;
    		}
    		if (_translations != null)
    		{
    			_translations.CollectionChanged -= FixupTranslations;
    			_translations.CollectionChanged -= RaiseTranslationsChanged;
    			_translations.CollectionChanged += FixupTranslations;
    			_translations.CollectionChanged += RaiseTranslationsChanged;
    		}
    		if (_htmlSections != null)
    		{
    			_htmlSections.CollectionChanged -= FixupHtmlSections;
    			_htmlSections.CollectionChanged -= RaiseHtmlSectionsChanged;
    			_htmlSections.CollectionChanged += FixupHtmlSections;
    			_htmlSections.CollectionChanged += RaiseHtmlSectionsChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		Layout = null;
    		Navigations.Clear();
    		Pages1.Clear();
    		Page1 = null;
    		Site = null;
    		User = null;
    		Translations.Clear();
    		HtmlSections.Clear();
    		PageType = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupLayout(Layout previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.Pages.Contains(this))
    		{
    			previousValue.Pages.Remove(this);
    		}
    
    		if (Layout != null)
    		{
    			if (!Layout.Pages.Contains(this))
    			{
    				Layout.Pages.Add(this);
    			}
    
    			LayoutID = Layout.LayoutID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Layout")
    				&& (ChangeTracker.OriginalValues["Layout"] == Layout))
    			{
    				ChangeTracker.OriginalValues.Remove("Layout");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Layout", previousValue);
    			}
    			if (Layout != null && !Layout.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Layout.StartTracking();
    			}
    		}
    	}
    
    	private void FixupPage1(Page previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.Pages1.Contains(this))
    		{
    			previousValue.Pages1.Remove(this);
    		}
    
    		if (Page1 != null)
    		{
    			if (!Page1.Pages1.Contains(this))
    			{
    				Page1.Pages1.Add(this);
    			}
    
    			ParentID = Page1.PageID;
    		}
    		else if (!skipKeys)
    		{
    			ParentID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Page1")
    				&& (ChangeTracker.OriginalValues["Page1"] == Page1))
    			{
    				ChangeTracker.OriginalValues.Remove("Page1");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Page1", previousValue);
    			}
    			if (Page1 != null && !Page1.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Page1.StartTracking();
    			}
    		}
    	}
    
    	private void FixupSite(Site previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.Pages.Contains(this))
    		{
    			previousValue.Pages.Remove(this);
    		}
    
    		if (Site != null)
    		{
    			if (!Site.Pages.Contains(this))
    			{
    				Site.Pages.Add(this);
    			}
    
    			SiteID = Site.SiteID;
    		}
    		else if (!skipKeys)
    		{
    			SiteID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Site")
    				&& (ChangeTracker.OriginalValues["Site"] == Site))
    			{
    				ChangeTracker.OriginalValues.Remove("Site");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Site", previousValue);
    			}
    			if (Site != null && !Site.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Site.StartTracking();
    			}
    		}
    	}
    
    	private void FixupUser(User previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.Pages.Contains(this))
    		{
    			previousValue.Pages.Remove(this);
    		}
    
    		if (User != null)
    		{
    			if (!User.Pages.Contains(this))
    			{
    				User.Pages.Add(this);
    			}
    
    			ModifiedByUserID = User.UserID;
    		}
    		else if (!skipKeys)
    		{
    			ModifiedByUserID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("User")
    				&& (ChangeTracker.OriginalValues["User"] == User))
    			{
    				ChangeTracker.OriginalValues.Remove("User");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("User", previousValue);
    			}
    			if (User != null && !User.ChangeTracker.ChangeTrackingEnabled)
    			{
    				User.StartTracking();
    			}
    		}
    	}
    
    	private void FixupPageType(PageType previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.Pages.Contains(this))
    		{
    			previousValue.Pages.Remove(this);
    		}
    
    		if (PageType != null)
    		{
    			if (!PageType.Pages.Contains(this))
    			{
    				PageType.Pages.Add(this);
    			}
    
    			PageTypeID = PageType.PageTypeID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("PageType")
    				&& (ChangeTracker.OriginalValues["PageType"] == PageType))
    			{
    				ChangeTracker.OriginalValues.Remove("PageType");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("PageType", previousValue);
    			}
    			if (PageType != null && !PageType.ChangeTracker.ChangeTrackingEnabled)
    			{
    				PageType.StartTracking();
    			}
    		}
    	}
    
    	private void FixupNavigations(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (Navigation item in e.NewItems)
    			{
    				item.Page = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Navigations", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (Navigation item in e.OldItems)
    			{
    				if (ReferenceEquals(item.Page, this))
    				{
    					item.Page = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Navigations", item);
    				}
    			}
    		}
    	}
    
    	private void FixupPages1(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (Page item in e.NewItems)
    			{
    				item.Page1 = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Pages1", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (Page item in e.OldItems)
    			{
    				if (ReferenceEquals(item.Page1, this))
    				{
    					item.Page1 = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Pages1", item);
    				}
    			}
    		}
    	}
    
    	private void FixupTranslations(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (PageTranslation item in e.NewItems)
    			{
    				item.Page = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Translations", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (PageTranslation item in e.OldItems)
    			{
    				if (ReferenceEquals(item.Page, this))
    				{
    					item.Page = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Translations", item);
    				}
    			}
    		}
    	}
    
    	private void FixupHtmlSections(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (HtmlSection item in e.NewItems)
    			{
    				if (!item.Pages.Contains(this))
    				{
    					item.Pages.Add(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("HtmlSections", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (HtmlSection item in e.OldItems)
    			{
    				if (item.Pages.Contains(this))
    				{
    					item.Pages.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("HtmlSections", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}
