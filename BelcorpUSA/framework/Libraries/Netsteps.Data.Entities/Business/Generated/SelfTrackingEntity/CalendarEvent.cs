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
    [KnownType(typeof(Account))]
    [KnownType(typeof(CalendarEventAttribute))]
    [KnownType(typeof(CalendarEvent))]
    [KnownType(typeof(AccountListValue))]
    [KnownType(typeof(User))]
    [KnownType(typeof(Market))]
    [KnownType(typeof(HtmlSection))]
    [KnownType(typeof(Address))]
    [KnownType(typeof(Site))]
    [Serializable]
    public partial class CalendarEvent: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void CalendarEventIDChanged();
    	public int CalendarEventID
    	{
    		get { return _calendarEventID; }
    		set
    		{
    			if (_calendarEventID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'CalendarEventID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_calendarEventID = value;
    				CalendarEventIDChanged();
    				OnPropertyChanged("CalendarEventID");
    			}
    		}
    	}
    	private int _calendarEventID;
    	partial void CalendarEventTypeIDChanged();
    	public Nullable<int> CalendarEventTypeID
    	{
    		get { return _calendarEventTypeID; }
    		set
    		{
    			if (_calendarEventTypeID != value)
    			{
    				ChangeTracker.RecordOriginalValue("CalendarEventTypeID", _calendarEventTypeID);
    				if (!IsDeserializing)
    				{
    					if (AccountListValue2 != null && AccountListValue2.AccountListValueID != value)
    					{
    						AccountListValue2 = null;
    					}
    				}
    				_calendarEventTypeID = value;
    				CalendarEventTypeIDChanged();
    				OnPropertyChanged("CalendarEventTypeID");
    			}
    		}
    	}
    	private Nullable<int> _calendarEventTypeID;
    	partial void CalendarCategoryIDChanged();
    	public Nullable<int> CalendarCategoryID
    	{
    		get { return _calendarCategoryID; }
    		set
    		{
    			if (_calendarCategoryID != value)
    			{
    				ChangeTracker.RecordOriginalValue("CalendarCategoryID", _calendarCategoryID);
    				if (!IsDeserializing)
    				{
    					if (AccountListValue != null && AccountListValue.AccountListValueID != value)
    					{
    						AccountListValue = null;
    					}
    				}
    				_calendarCategoryID = value;
    				CalendarCategoryIDChanged();
    				OnPropertyChanged("CalendarCategoryID");
    			}
    		}
    	}
    	private Nullable<int> _calendarCategoryID;
    	partial void CalendarPriorityIDChanged();
    	public Nullable<int> CalendarPriorityID
    	{
    		get { return _calendarPriorityID; }
    		set
    		{
    			if (_calendarPriorityID != value)
    			{
    				ChangeTracker.RecordOriginalValue("CalendarPriorityID", _calendarPriorityID);
    				if (!IsDeserializing)
    				{
    					if (AccountListValue3 != null && AccountListValue3.AccountListValueID != value)
    					{
    						AccountListValue3 = null;
    					}
    				}
    				_calendarPriorityID = value;
    				CalendarPriorityIDChanged();
    				OnPropertyChanged("CalendarPriorityID");
    			}
    		}
    	}
    	private Nullable<int> _calendarPriorityID;
    	partial void CalendarStatusIDChanged();
    	public Nullable<int> CalendarStatusID
    	{
    		get { return _calendarStatusID; }
    		set
    		{
    			if (_calendarStatusID != value)
    			{
    				ChangeTracker.RecordOriginalValue("CalendarStatusID", _calendarStatusID);
    				if (!IsDeserializing)
    				{
    					if (AccountListValue4 != null && AccountListValue4.AccountListValueID != value)
    					{
    						AccountListValue4 = null;
    					}
    				}
    				_calendarStatusID = value;
    				CalendarStatusIDChanged();
    				OnPropertyChanged("CalendarStatusID");
    			}
    		}
    	}
    	private Nullable<int> _calendarStatusID;
    	partial void CalendarColorCodingIDChanged();
    	public Nullable<int> CalendarColorCodingID
    	{
    		get { return _calendarColorCodingID; }
    		set
    		{
    			if (_calendarColorCodingID != value)
    			{
    				ChangeTracker.RecordOriginalValue("CalendarColorCodingID", _calendarColorCodingID);
    				if (!IsDeserializing)
    				{
    					if (AccountListValue1 != null && AccountListValue1.AccountListValueID != value)
    					{
    						AccountListValue1 = null;
    					}
    				}
    				_calendarColorCodingID = value;
    				CalendarColorCodingIDChanged();
    				OnPropertyChanged("CalendarColorCodingID");
    			}
    		}
    	}
    	private Nullable<int> _calendarColorCodingID;
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
    					if (CalendarEvent1 != null && CalendarEvent1.CalendarEventID != value)
    					{
    						CalendarEvent1 = null;
    					}
    				}
    				_parentID = value;
    				ParentIDChanged();
    				OnPropertyChanged("ParentID");
    			}
    		}
    	}
    	private Nullable<int> _parentID;
    	partial void AccountIDChanged();
    	public Nullable<int> AccountID
    	{
    		get { return _accountID; }
    		set
    		{
    			if (_accountID != value)
    			{
    				ChangeTracker.RecordOriginalValue("AccountID", _accountID);
    				if (!IsDeserializing)
    				{
    					if (Account != null && Account.AccountID != value)
    					{
    						Account = null;
    					}
    				}
    				_accountID = value;
    				AccountIDChanged();
    				OnPropertyChanged("AccountID");
    			}
    		}
    	}
    	private Nullable<int> _accountID;
    	partial void StartDateUTCChanged();
    	public System.DateTime StartDateUTC
    	{
    		get { return _startDateUTC; }
    		set
    		{
    			if (_startDateUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("StartDateUTC", _startDateUTC);
    				_startDateUTC = value;
    				StartDateUTCChanged();
    				OnPropertyChanged("StartDateUTC");
    			}
    		}
    	}
    	private System.DateTime _startDateUTC;
    	partial void EndDateUTCChanged();
    	public Nullable<System.DateTime> EndDateUTC
    	{
    		get { return _endDateUTC; }
    		set
    		{
    			if (_endDateUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("EndDateUTC", _endDateUTC);
    				_endDateUTC = value;
    				EndDateUTCChanged();
    				OnPropertyChanged("EndDateUTC");
    			}
    		}
    	}
    	private Nullable<System.DateTime> _endDateUTC;
    	partial void ReminderDateUTCChanged();
    	public Nullable<System.DateTime> ReminderDateUTC
    	{
    		get { return _reminderDateUTC; }
    		set
    		{
    			if (_reminderDateUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("ReminderDateUTC", _reminderDateUTC);
    				_reminderDateUTC = value;
    				ReminderDateUTCChanged();
    				OnPropertyChanged("ReminderDateUTC");
    			}
    		}
    	}
    	private Nullable<System.DateTime> _reminderDateUTC;
    	partial void RecurringScheduleIDChanged();
    	public Nullable<int> RecurringScheduleID
    	{
    		get { return _recurringScheduleID; }
    		set
    		{
    			if (_recurringScheduleID != value)
    			{
    				ChangeTracker.RecordOriginalValue("RecurringScheduleID", _recurringScheduleID);
    				_recurringScheduleID = value;
    				RecurringScheduleIDChanged();
    				OnPropertyChanged("RecurringScheduleID");
    			}
    		}
    	}
    	private Nullable<int> _recurringScheduleID;
    	partial void AddressIDChanged();
    	public Nullable<int> AddressID
    	{
    		get { return _addressID; }
    		set
    		{
    			if (_addressID != value)
    			{
    				ChangeTracker.RecordOriginalValue("AddressID", _addressID);
    				if (!IsDeserializing)
    				{
    					if (Address != null && Address.AddressID != value)
    					{
    						Address = null;
    					}
    				}
    				_addressID = value;
    				AddressIDChanged();
    				OnPropertyChanged("AddressID");
    			}
    		}
    	}
    	private Nullable<int> _addressID;
    	partial void IsCorporateChanged();
    	public bool IsCorporate
    	{
    		get { return _isCorporate; }
    		set
    		{
    			if (_isCorporate != value)
    			{
    				ChangeTracker.RecordOriginalValue("IsCorporate", _isCorporate);
    				_isCorporate = value;
    				IsCorporateChanged();
    				OnPropertyChanged("IsCorporate");
    			}
    		}
    	}
    	private bool _isCorporate;
    	partial void IsPublicChanged();
    	public bool IsPublic
    	{
    		get { return _isPublic; }
    		set
    		{
    			if (_isPublic != value)
    			{
    				ChangeTracker.RecordOriginalValue("IsPublic", _isPublic);
    				_isPublic = value;
    				IsPublicChanged();
    				OnPropertyChanged("IsPublic");
    			}
    		}
    	}
    	private bool _isPublic;
    	partial void IsAllDayEventChanged();
    	public bool IsAllDayEvent
    	{
    		get { return _isAllDayEvent; }
    		set
    		{
    			if (_isAllDayEvent != value)
    			{
    				ChangeTracker.RecordOriginalValue("IsAllDayEvent", _isAllDayEvent);
    				_isAllDayEvent = value;
    				IsAllDayEventChanged();
    				OnPropertyChanged("IsAllDayEvent");
    			}
    		}
    	}
    	private bool _isAllDayEvent;
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
    	partial void MarketIDChanged();
    	public Nullable<int> MarketID
    	{
    		get { return _marketID; }
    		set
    		{
    			if (_marketID != value)
    			{
    				ChangeTracker.RecordOriginalValue("MarketID", _marketID);
    				if (!IsDeserializing)
    				{
    					if (Market != null && Market.MarketID != value)
    					{
    						Market = null;
    					}
    				}
    				_marketID = value;
    				MarketIDChanged();
    				OnPropertyChanged("MarketID");
    			}
    		}
    	}
    	private Nullable<int> _marketID;
    	partial void HtmlSectionIDChanged();
    	public Nullable<int> HtmlSectionID
    	{
    		get { return _htmlSectionID; }
    		set
    		{
    			if (_htmlSectionID != value)
    			{
    				ChangeTracker.RecordOriginalValue("HtmlSectionID", _htmlSectionID);
    				if (!IsDeserializing)
    				{
    					if (HtmlSection != null && HtmlSection.HtmlSectionID != value)
    					{
    						HtmlSection = null;
    					}
    				}
    				_htmlSectionID = value;
    				HtmlSectionIDChanged();
    				OnPropertyChanged("HtmlSectionID");
    			}
    		}
    	}
    	private Nullable<int> _htmlSectionID;

        #endregion
        #region Navigation Properties
    
    	public Account Account
    	{
    		get { return _account; }
    		set
    		{
    			if (!ReferenceEquals(_account, value))
    			{
    				var previousValue = _account;
    				_account = value;
    				FixupAccount(previousValue);
    				OnNavigationPropertyChanged("Account");
    			}
    		}
    	}
    	private Account _account;
    
    	public TrackableCollection<CalendarEventAttribute> CalendarEventAttributes
    	{
    		get
    		{
    			if (_calendarEventAttributes == null)
    			{
    				_calendarEventAttributes = new TrackableCollection<CalendarEventAttribute>();
    				_calendarEventAttributes.CollectionChanged += FixupCalendarEventAttributes;
    				_calendarEventAttributes.CollectionChanged += RaiseCalendarEventAttributesChanged;
    			}
    			return _calendarEventAttributes;
    		}
    		set
    		{
    			if (!ReferenceEquals(_calendarEventAttributes, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_calendarEventAttributes != null)
    				{
    					_calendarEventAttributes.CollectionChanged -= FixupCalendarEventAttributes;
    					_calendarEventAttributes.CollectionChanged -= RaiseCalendarEventAttributesChanged;
    				}
    				_calendarEventAttributes = value;
    				if (_calendarEventAttributes != null)
    				{
    					_calendarEventAttributes.CollectionChanged += FixupCalendarEventAttributes;
    					_calendarEventAttributes.CollectionChanged += RaiseCalendarEventAttributesChanged;
    				}
    				OnNavigationPropertyChanged("CalendarEventAttributes");
    			}
    		}
    	}
    	private TrackableCollection<CalendarEventAttribute> _calendarEventAttributes;
    	partial void CalendarEventAttributesChanged();
    	private void RaiseCalendarEventAttributesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		CalendarEventAttributesChanged();
    	}
    
    	public TrackableCollection<CalendarEvent> CalendarEvents1
    	{
    		get
    		{
    			if (_calendarEvents1 == null)
    			{
    				_calendarEvents1 = new TrackableCollection<CalendarEvent>();
    				_calendarEvents1.CollectionChanged += FixupCalendarEvents1;
    				_calendarEvents1.CollectionChanged += RaiseCalendarEvents1Changed;
    			}
    			return _calendarEvents1;
    		}
    		set
    		{
    			if (!ReferenceEquals(_calendarEvents1, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_calendarEvents1 != null)
    				{
    					_calendarEvents1.CollectionChanged -= FixupCalendarEvents1;
    					_calendarEvents1.CollectionChanged -= RaiseCalendarEvents1Changed;
    				}
    				_calendarEvents1 = value;
    				if (_calendarEvents1 != null)
    				{
    					_calendarEvents1.CollectionChanged += FixupCalendarEvents1;
    					_calendarEvents1.CollectionChanged += RaiseCalendarEvents1Changed;
    				}
    				OnNavigationPropertyChanged("CalendarEvents1");
    			}
    		}
    	}
    	private TrackableCollection<CalendarEvent> _calendarEvents1;
    	partial void CalendarEvents1Changed();
    	private void RaiseCalendarEvents1Changed(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		CalendarEvents1Changed();
    	}
    
    	public CalendarEvent CalendarEvent1
    	{
    		get { return _calendarEvent1; }
    		set
    		{
    			if (!ReferenceEquals(_calendarEvent1, value))
    			{
    				var previousValue = _calendarEvent1;
    				_calendarEvent1 = value;
    				FixupCalendarEvent1(previousValue);
    				OnNavigationPropertyChanged("CalendarEvent1");
    			}
    		}
    	}
    	private CalendarEvent _calendarEvent1;
    
    	public AccountListValue AccountListValue
    	{
    		get { return _accountListValue; }
    		set
    		{
    			if (!ReferenceEquals(_accountListValue, value))
    			{
    				var previousValue = _accountListValue;
    				_accountListValue = value;
    				FixupAccountListValue(previousValue);
    				OnNavigationPropertyChanged("AccountListValue");
    			}
    		}
    	}
    	private AccountListValue _accountListValue;
    
    	public AccountListValue AccountListValue1
    	{
    		get { return _accountListValue1; }
    		set
    		{
    			if (!ReferenceEquals(_accountListValue1, value))
    			{
    				var previousValue = _accountListValue1;
    				_accountListValue1 = value;
    				FixupAccountListValue1(previousValue);
    				OnNavigationPropertyChanged("AccountListValue1");
    			}
    		}
    	}
    	private AccountListValue _accountListValue1;
    
    	public AccountListValue AccountListValue2
    	{
    		get { return _accountListValue2; }
    		set
    		{
    			if (!ReferenceEquals(_accountListValue2, value))
    			{
    				var previousValue = _accountListValue2;
    				_accountListValue2 = value;
    				FixupAccountListValue2(previousValue);
    				OnNavigationPropertyChanged("AccountListValue2");
    			}
    		}
    	}
    	private AccountListValue _accountListValue2;
    
    	public AccountListValue AccountListValue3
    	{
    		get { return _accountListValue3; }
    		set
    		{
    			if (!ReferenceEquals(_accountListValue3, value))
    			{
    				var previousValue = _accountListValue3;
    				_accountListValue3 = value;
    				FixupAccountListValue3(previousValue);
    				OnNavigationPropertyChanged("AccountListValue3");
    			}
    		}
    	}
    	private AccountListValue _accountListValue3;
    
    	public AccountListValue AccountListValue4
    	{
    		get { return _accountListValue4; }
    		set
    		{
    			if (!ReferenceEquals(_accountListValue4, value))
    			{
    				var previousValue = _accountListValue4;
    				_accountListValue4 = value;
    				FixupAccountListValue4(previousValue);
    				OnNavigationPropertyChanged("AccountListValue4");
    			}
    		}
    	}
    	private AccountListValue _accountListValue4;
    
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
    
    	public Market Market
    	{
    		get { return _market; }
    		set
    		{
    			if (!ReferenceEquals(_market, value))
    			{
    				var previousValue = _market;
    				_market = value;
    				FixupMarket(previousValue);
    				OnNavigationPropertyChanged("Market");
    			}
    		}
    	}
    	private Market _market;
    
    	public HtmlSection HtmlSection
    	{
    		get { return _htmlSection; }
    		set
    		{
    			if (!ReferenceEquals(_htmlSection, value))
    			{
    				var previousValue = _htmlSection;
    				_htmlSection = value;
    				FixupHtmlSection(previousValue);
    				OnNavigationPropertyChanged("HtmlSection");
    			}
    		}
    	}
    	private HtmlSection _htmlSection;
    
    	public Address Address
    	{
    		get { return _address; }
    		set
    		{
    			if (!ReferenceEquals(_address, value))
    			{
    				var previousValue = _address;
    				_address = value;
    				FixupAddress(previousValue);
    				OnNavigationPropertyChanged("Address");
    			}
    		}
    	}
    	private Address _address;
    
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
    		if (_calendarEventAttributes != null)
    		{
    			_calendarEventAttributes.CollectionChanged -= FixupCalendarEventAttributes;
    			_calendarEventAttributes.CollectionChanged -= RaiseCalendarEventAttributesChanged;
    			_calendarEventAttributes.CollectionChanged += FixupCalendarEventAttributes;
    			_calendarEventAttributes.CollectionChanged += RaiseCalendarEventAttributesChanged;
    		}
    		if (_calendarEvents1 != null)
    		{
    			_calendarEvents1.CollectionChanged -= FixupCalendarEvents1;
    			_calendarEvents1.CollectionChanged -= RaiseCalendarEvents1Changed;
    			_calendarEvents1.CollectionChanged += FixupCalendarEvents1;
    			_calendarEvents1.CollectionChanged += RaiseCalendarEvents1Changed;
    		}
    		if (_sites != null)
    		{
    			_sites.CollectionChanged -= FixupSites;
    			_sites.CollectionChanged -= RaiseSitesChanged;
    			_sites.CollectionChanged += FixupSites;
    			_sites.CollectionChanged += RaiseSitesChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		Account = null;
    		CalendarEventAttributes.Clear();
    		CalendarEvents1.Clear();
    		CalendarEvent1 = null;
    		AccountListValue = null;
    		AccountListValue1 = null;
    		AccountListValue2 = null;
    		AccountListValue3 = null;
    		AccountListValue4 = null;
    		User = null;
    		Market = null;
    		HtmlSection = null;
    		Address = null;
    		Sites.Clear();
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupAccount(Account previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.CalendarEvents.Contains(this))
    		{
    			previousValue.CalendarEvents.Remove(this);
    		}
    
    		if (Account != null)
    		{
    			if (!Account.CalendarEvents.Contains(this))
    			{
    				Account.CalendarEvents.Add(this);
    			}
    
    			AccountID = Account.AccountID;
    		}
    		else if (!skipKeys)
    		{
    			AccountID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Account")
    				&& (ChangeTracker.OriginalValues["Account"] == Account))
    			{
    				ChangeTracker.OriginalValues.Remove("Account");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Account", previousValue);
    			}
    			if (Account != null && !Account.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Account.StartTracking();
    			}
    		}
    	}
    
    	private void FixupCalendarEvent1(CalendarEvent previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.CalendarEvents1.Contains(this))
    		{
    			previousValue.CalendarEvents1.Remove(this);
    		}
    
    		if (CalendarEvent1 != null)
    		{
    			if (!CalendarEvent1.CalendarEvents1.Contains(this))
    			{
    				CalendarEvent1.CalendarEvents1.Add(this);
    			}
    
    			ParentID = CalendarEvent1.CalendarEventID;
    		}
    		else if (!skipKeys)
    		{
    			ParentID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("CalendarEvent1")
    				&& (ChangeTracker.OriginalValues["CalendarEvent1"] == CalendarEvent1))
    			{
    				ChangeTracker.OriginalValues.Remove("CalendarEvent1");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("CalendarEvent1", previousValue);
    			}
    			if (CalendarEvent1 != null && !CalendarEvent1.ChangeTracker.ChangeTrackingEnabled)
    			{
    				CalendarEvent1.StartTracking();
    			}
    		}
    	}
    
    	private void FixupAccountListValue(AccountListValue previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.CalendarEvents.Contains(this))
    		{
    			previousValue.CalendarEvents.Remove(this);
    		}
    
    		if (AccountListValue != null)
    		{
    			if (!AccountListValue.CalendarEvents.Contains(this))
    			{
    				AccountListValue.CalendarEvents.Add(this);
    			}
    
    			CalendarCategoryID = AccountListValue.AccountListValueID;
    		}
    		else if (!skipKeys)
    		{
    			CalendarCategoryID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("AccountListValue")
    				&& (ChangeTracker.OriginalValues["AccountListValue"] == AccountListValue))
    			{
    				ChangeTracker.OriginalValues.Remove("AccountListValue");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("AccountListValue", previousValue);
    			}
    			if (AccountListValue != null && !AccountListValue.ChangeTracker.ChangeTrackingEnabled)
    			{
    				AccountListValue.StartTracking();
    			}
    		}
    	}
    
    	private void FixupAccountListValue1(AccountListValue previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.CalendarEvents1.Contains(this))
    		{
    			previousValue.CalendarEvents1.Remove(this);
    		}
    
    		if (AccountListValue1 != null)
    		{
    			if (!AccountListValue1.CalendarEvents1.Contains(this))
    			{
    				AccountListValue1.CalendarEvents1.Add(this);
    			}
    
    			CalendarColorCodingID = AccountListValue1.AccountListValueID;
    		}
    		else if (!skipKeys)
    		{
    			CalendarColorCodingID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("AccountListValue1")
    				&& (ChangeTracker.OriginalValues["AccountListValue1"] == AccountListValue1))
    			{
    				ChangeTracker.OriginalValues.Remove("AccountListValue1");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("AccountListValue1", previousValue);
    			}
    			if (AccountListValue1 != null && !AccountListValue1.ChangeTracker.ChangeTrackingEnabled)
    			{
    				AccountListValue1.StartTracking();
    			}
    		}
    	}
    
    	private void FixupAccountListValue2(AccountListValue previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.CalendarEvents2.Contains(this))
    		{
    			previousValue.CalendarEvents2.Remove(this);
    		}
    
    		if (AccountListValue2 != null)
    		{
    			if (!AccountListValue2.CalendarEvents2.Contains(this))
    			{
    				AccountListValue2.CalendarEvents2.Add(this);
    			}
    
    			CalendarEventTypeID = AccountListValue2.AccountListValueID;
    		}
    		else if (!skipKeys)
    		{
    			CalendarEventTypeID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("AccountListValue2")
    				&& (ChangeTracker.OriginalValues["AccountListValue2"] == AccountListValue2))
    			{
    				ChangeTracker.OriginalValues.Remove("AccountListValue2");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("AccountListValue2", previousValue);
    			}
    			if (AccountListValue2 != null && !AccountListValue2.ChangeTracker.ChangeTrackingEnabled)
    			{
    				AccountListValue2.StartTracking();
    			}
    		}
    	}
    
    	private void FixupAccountListValue3(AccountListValue previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.CalendarEvents3.Contains(this))
    		{
    			previousValue.CalendarEvents3.Remove(this);
    		}
    
    		if (AccountListValue3 != null)
    		{
    			if (!AccountListValue3.CalendarEvents3.Contains(this))
    			{
    				AccountListValue3.CalendarEvents3.Add(this);
    			}
    
    			CalendarPriorityID = AccountListValue3.AccountListValueID;
    		}
    		else if (!skipKeys)
    		{
    			CalendarPriorityID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("AccountListValue3")
    				&& (ChangeTracker.OriginalValues["AccountListValue3"] == AccountListValue3))
    			{
    				ChangeTracker.OriginalValues.Remove("AccountListValue3");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("AccountListValue3", previousValue);
    			}
    			if (AccountListValue3 != null && !AccountListValue3.ChangeTracker.ChangeTrackingEnabled)
    			{
    				AccountListValue3.StartTracking();
    			}
    		}
    	}
    
    	private void FixupAccountListValue4(AccountListValue previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.CalendarEvents4.Contains(this))
    		{
    			previousValue.CalendarEvents4.Remove(this);
    		}
    
    		if (AccountListValue4 != null)
    		{
    			if (!AccountListValue4.CalendarEvents4.Contains(this))
    			{
    				AccountListValue4.CalendarEvents4.Add(this);
    			}
    
    			CalendarStatusID = AccountListValue4.AccountListValueID;
    		}
    		else if (!skipKeys)
    		{
    			CalendarStatusID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("AccountListValue4")
    				&& (ChangeTracker.OriginalValues["AccountListValue4"] == AccountListValue4))
    			{
    				ChangeTracker.OriginalValues.Remove("AccountListValue4");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("AccountListValue4", previousValue);
    			}
    			if (AccountListValue4 != null && !AccountListValue4.ChangeTracker.ChangeTrackingEnabled)
    			{
    				AccountListValue4.StartTracking();
    			}
    		}
    	}
    
    	private void FixupUser(User previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.CalendarEvents.Contains(this))
    		{
    			previousValue.CalendarEvents.Remove(this);
    		}
    
    		if (User != null)
    		{
    			if (!User.CalendarEvents.Contains(this))
    			{
    				User.CalendarEvents.Add(this);
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
    
    	private void FixupMarket(Market previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.CalendarEvents.Contains(this))
    		{
    			previousValue.CalendarEvents.Remove(this);
    		}
    
    		if (Market != null)
    		{
    			if (!Market.CalendarEvents.Contains(this))
    			{
    				Market.CalendarEvents.Add(this);
    			}
    
    			MarketID = Market.MarketID;
    		}
    		else if (!skipKeys)
    		{
    			MarketID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Market")
    				&& (ChangeTracker.OriginalValues["Market"] == Market))
    			{
    				ChangeTracker.OriginalValues.Remove("Market");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Market", previousValue);
    			}
    			if (Market != null && !Market.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Market.StartTracking();
    			}
    		}
    	}
    
    	private void FixupHtmlSection(HtmlSection previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.CalendarEvents.Contains(this))
    		{
    			previousValue.CalendarEvents.Remove(this);
    		}
    
    		if (HtmlSection != null)
    		{
    			if (!HtmlSection.CalendarEvents.Contains(this))
    			{
    				HtmlSection.CalendarEvents.Add(this);
    			}
    
    			HtmlSectionID = HtmlSection.HtmlSectionID;
    		}
    		else if (!skipKeys)
    		{
    			HtmlSectionID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("HtmlSection")
    				&& (ChangeTracker.OriginalValues["HtmlSection"] == HtmlSection))
    			{
    				ChangeTracker.OriginalValues.Remove("HtmlSection");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("HtmlSection", previousValue);
    			}
    			if (HtmlSection != null && !HtmlSection.ChangeTracker.ChangeTrackingEnabled)
    			{
    				HtmlSection.StartTracking();
    			}
    		}
    	}
    
    	private void FixupAddress(Address previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.CalendarEvents.Contains(this))
    		{
    			previousValue.CalendarEvents.Remove(this);
    		}
    
    		if (Address != null)
    		{
    			if (!Address.CalendarEvents.Contains(this))
    			{
    				Address.CalendarEvents.Add(this);
    			}
    
    			AddressID = Address.AddressID;
    		}
    		else if (!skipKeys)
    		{
    			AddressID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Address")
    				&& (ChangeTracker.OriginalValues["Address"] == Address))
    			{
    				ChangeTracker.OriginalValues.Remove("Address");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Address", previousValue);
    			}
    			if (Address != null && !Address.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Address.StartTracking();
    			}
    		}
    	}
    
    	private void FixupCalendarEventAttributes(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (CalendarEventAttribute item in e.NewItems)
    			{
    				item.CalendarEvent = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("CalendarEventAttributes", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (CalendarEventAttribute item in e.OldItems)
    			{
    				if (ReferenceEquals(item.CalendarEvent, this))
    				{
    					item.CalendarEvent = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("CalendarEventAttributes", item);
    				}
    			}
    		}
    	}
    
    	private void FixupCalendarEvents1(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (CalendarEvent item in e.NewItems)
    			{
    				item.CalendarEvent1 = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("CalendarEvents1", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (CalendarEvent item in e.OldItems)
    			{
    				if (ReferenceEquals(item.CalendarEvent1, this))
    				{
    					item.CalendarEvent1 = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("CalendarEvents1", item);
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
    				if (!item.CalendarEvents.Contains(this))
    				{
    					item.CalendarEvents.Add(this);
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
    				if (item.CalendarEvents.Contains(this))
    				{
    					item.CalendarEvents.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Sites", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}
