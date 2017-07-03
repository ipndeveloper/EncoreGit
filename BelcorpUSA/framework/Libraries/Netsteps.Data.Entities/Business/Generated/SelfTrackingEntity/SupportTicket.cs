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
    [KnownType(typeof(SupportTicketCategory))]
    [KnownType(typeof(SupportTicketPriority))]
    [KnownType(typeof(User))]
    [KnownType(typeof(SupportTicketStatus))]
    [KnownType(typeof(Note))]
    [KnownType(typeof(Order))]
    [Serializable]
    public partial class SupportTicket: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void SupportTicketIDChanged();
    	public int SupportTicketID
    	{
    		get { return _supportTicketID; }
    		set
    		{
    			if (_supportTicketID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'SupportTicketID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_supportTicketID = value;
    				SupportTicketIDChanged();
    				OnPropertyChanged("SupportTicketID");
    			}
    		}
    	}
    	private int _supportTicketID;
    	partial void SupportTicketNumberChanged();
    	public string SupportTicketNumber
    	{
    		get { return _supportTicketNumber; }
    		set
    		{
    			if (_supportTicketNumber != value)
    			{
    				ChangeTracker.RecordOriginalValue("SupportTicketNumber", _supportTicketNumber);
    				_supportTicketNumber = value;
    				SupportTicketNumberChanged();
    				OnPropertyChanged("SupportTicketNumber");
    			}
    		}
    	}
    	private string _supportTicketNumber;
    	partial void AccountIDChanged();
    	public int AccountID
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
    	private int _accountID;
    	partial void AssignedUserIDChanged();
    	public Nullable<int> AssignedUserID
    	{
    		get { return _assignedUserID; }
    		set
    		{
    			if (_assignedUserID != value)
    			{
    				ChangeTracker.RecordOriginalValue("AssignedUserID", _assignedUserID);
    				if (!IsDeserializing)
    				{
    					if (AssignedUser != null && AssignedUser.UserID != value)
    					{
    						AssignedUser = null;
    					}
    				}
    				_assignedUserID = value;
    				AssignedUserIDChanged();
    				OnPropertyChanged("AssignedUserID");
    			}
    		}
    	}
    	private Nullable<int> _assignedUserID;
    	partial void TitleChanged();
    	public string Title
    	{
    		get { return _title; }
    		set
    		{
    			if (_title != value)
    			{
    				ChangeTracker.RecordOriginalValue("Title", _title);
    				_title = value;
    				TitleChanged();
    				OnPropertyChanged("Title");
    			}
    		}
    	}
    	private string _title;
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
    	partial void SupportTicketCategoryIDChanged();
    	public Nullable<short> SupportTicketCategoryID
    	{
    		get { return _supportTicketCategoryID; }
    		set
    		{
    			if (_supportTicketCategoryID != value)
    			{
    				ChangeTracker.RecordOriginalValue("SupportTicketCategoryID", _supportTicketCategoryID);
    				if (!IsDeserializing)
    				{
    					if (SupportTicketCategory != null && SupportTicketCategory.SupportTicketCategoryID != value)
    					{
    						SupportTicketCategory = null;
    					}
    				}
    				_supportTicketCategoryID = value;
    				SupportTicketCategoryIDChanged();
    				OnPropertyChanged("SupportTicketCategoryID");
    			}
    		}
    	}
    	private Nullable<short> _supportTicketCategoryID;
    	partial void SupportTicketPriorityIDChanged();
    	public short SupportTicketPriorityID
    	{
    		get { return _supportTicketPriorityID; }
    		set
    		{
    			if (_supportTicketPriorityID != value)
    			{
    				ChangeTracker.RecordOriginalValue("SupportTicketPriorityID", _supportTicketPriorityID);
    				if (!IsDeserializing)
    				{
    					if (SupportTicketPriority != null && SupportTicketPriority.SupportTicketPriorityID != value)
    					{
    						SupportTicketPriority = null;
    					}
    				}
    				_supportTicketPriorityID = value;
    				SupportTicketPriorityIDChanged();
    				OnPropertyChanged("SupportTicketPriorityID");
    			}
    		}
    	}
    	private short _supportTicketPriorityID;
    	partial void SupportTicketStatusIDChanged();
    	public short SupportTicketStatusID
    	{
    		get { return _supportTicketStatusID; }
    		set
    		{
    			if (_supportTicketStatusID != value)
    			{
    				ChangeTracker.RecordOriginalValue("SupportTicketStatusID", _supportTicketStatusID);
    				if (!IsDeserializing)
    				{
    					if (SupportTicketStatus != null && SupportTicketStatus.SupportTicketStatusID != value)
    					{
    						SupportTicketStatus = null;
    					}
    				}
    				_supportTicketStatusID = value;
    				SupportTicketStatusIDChanged();
    				OnPropertyChanged("SupportTicketStatusID");
    			}
    		}
    	}
    	private short _supportTicketStatusID;
    	partial void IsVisibleToOwnerChanged();
    	public bool IsVisibleToOwner
    	{
    		get { return _isVisibleToOwner; }
    		set
    		{
    			if (_isVisibleToOwner != value)
    			{
    				ChangeTracker.RecordOriginalValue("IsVisibleToOwner", _isVisibleToOwner);
    				_isVisibleToOwner = value;
    				IsVisibleToOwnerChanged();
    				OnPropertyChanged("IsVisibleToOwner");
    			}
    		}
    	}
    	private bool _isVisibleToOwner;
    	partial void CreatedByUserIDChanged();
    	public Nullable<int> CreatedByUserID
    	{
    		get { return _createdByUserID; }
    		set
    		{
    			if (_createdByUserID != value)
    			{
    				ChangeTracker.RecordOriginalValue("CreatedByUserID", _createdByUserID);
    				if (!IsDeserializing)
    				{
    					if (CreatedByUser != null && CreatedByUser.UserID != value)
    					{
    						CreatedByUser = null;
    					}
    				}
    				_createdByUserID = value;
    				CreatedByUserIDChanged();
    				OnPropertyChanged("CreatedByUserID");
    			}
    		}
    	}
    	private Nullable<int> _createdByUserID;
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
    					if (ModifiedByUser != null && ModifiedByUser.UserID != value)
    					{
    						ModifiedByUser = null;
    					}
    				}
    				_modifiedByUserID = value;
    				ModifiedByUserIDChanged();
    				OnPropertyChanged("ModifiedByUserID");
    			}
    		}
    	}
    	private Nullable<int> _modifiedByUserID;
    	partial void DateCreatedUTCChanged();
    	public System.DateTime DateCreatedUTC
    	{
    		get { return _dateCreatedUTC; }
    		set
    		{
    			if (_dateCreatedUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("DateCreatedUTC", _dateCreatedUTC);
    				_dateCreatedUTC = value;
    				DateCreatedUTCChanged();
    				OnPropertyChanged("DateCreatedUTC");
    			}
    		}
    	}
    	private System.DateTime _dateCreatedUTC;
    	partial void DateLastModifiedUTCChanged();
    	public System.DateTime DateLastModifiedUTC
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
    	private System.DateTime _dateLastModifiedUTC;
    	partial void SupportLevelIDChanged();
    	public Nullable<int> SupportLevelID
    	{
    		get { return _supportLevelID; }
    		set
    		{
    			if (_supportLevelID != value)
    			{
    				ChangeTracker.RecordOriginalValue("SupportLevelID", _supportLevelID);
    				_supportLevelID = value;
    				SupportLevelIDChanged();
    				OnPropertyChanged("SupportLevelID");
    			}
    		}
    	}
    	private Nullable<int> _supportLevelID;
    	partial void DateCloseUTCChanged();
    	public Nullable<System.DateTime> DateCloseUTC
    	{
    		get { return _dateCloseUTC; }
    		set
    		{
    			if (_dateCloseUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("DateCloseUTC", _dateCloseUTC);
    				_dateCloseUTC = value;
    				DateCloseUTCChanged();
    				OnPropertyChanged("DateCloseUTC");
    			}
    		}
    	}
    	private Nullable<System.DateTime> _dateCloseUTC;
    	partial void SupportMotiveIDChanged();
    	public Nullable<int> SupportMotiveID
    	{
    		get { return _supportMotiveID; }
    		set
    		{
    			if (_supportMotiveID != value)
    			{
    				ChangeTracker.RecordOriginalValue("SupportMotiveID", _supportMotiveID);
    				_supportMotiveID = value;
    				SupportMotiveIDChanged();
    				OnPropertyChanged("SupportMotiveID");
    			}
    		}
    	}
    	private Nullable<int> _supportMotiveID;
    	partial void ScalingUserIDChanged();
    	public Nullable<int> ScalingUserID
    	{
    		get { return _scalingUserID; }
    		set
    		{
    			if (_scalingUserID != value)
    			{
    				ChangeTracker.RecordOriginalValue("ScalingUserID", _scalingUserID);
    				if (!IsDeserializing)
    				{
    					if (User2 != null && User2.UserID != value)
    					{
    						User2 = null;
    					}
    				}
    				_scalingUserID = value;
    				ScalingUserIDChanged();
    				OnPropertyChanged("ScalingUserID");
    			}
    		}
    	}
    	private Nullable<int> _scalingUserID;
    	partial void IsConfirmChanged();
    	public bool IsConfirm
    	{
    		get { return _isConfirm; }
    		set
    		{
    			if (_isConfirm != value)
    			{
    				ChangeTracker.RecordOriginalValue("IsConfirm", _isConfirm);
    				_isConfirm = value;
    				IsConfirmChanged();
    				OnPropertyChanged("IsConfirm");
    			}
    		}
    	}
    	private bool _isConfirm;
    	partial void BlockUserIDChanged();
    	public int BlockUserID
    	{
    		get { return _blockUserID; }
    		set
    		{
    			if (_blockUserID != value)
    			{
    				ChangeTracker.RecordOriginalValue("BlockUserID", _blockUserID);
    				_blockUserID = value;
    				BlockUserIDChanged();
    				OnPropertyChanged("BlockUserID");
    			}
    		}
    	}
    	private int _blockUserID;
    	partial void IsSiteDWSChanged();
    	public Nullable<bool> IsSiteDWS
    	{
    		get { return _isSiteDWS; }
    		set
    		{
    			if (_isSiteDWS != value)
    			{
    				ChangeTracker.RecordOriginalValue("IsSiteDWS", _isSiteDWS);
    				_isSiteDWS = value;
    				IsSiteDWSChanged();
    				OnPropertyChanged("IsSiteDWS");
    			}
    		}
    	}
    	private Nullable<bool> _isSiteDWS;

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
    
    	public SupportTicketCategory SupportTicketCategory
    	{
    		get { return _supportTicketCategory; }
    		set
    		{
    			if (!ReferenceEquals(_supportTicketCategory, value))
    			{
    				var previousValue = _supportTicketCategory;
    				_supportTicketCategory = value;
    				FixupSupportTicketCategory(previousValue);
    				OnNavigationPropertyChanged("SupportTicketCategory");
    			}
    		}
    	}
    	private SupportTicketCategory _supportTicketCategory;
    
    	public SupportTicketPriority SupportTicketPriority
    	{
    		get { return _supportTicketPriority; }
    		set
    		{
    			if (!ReferenceEquals(_supportTicketPriority, value))
    			{
    				var previousValue = _supportTicketPriority;
    				_supportTicketPriority = value;
    				FixupSupportTicketPriority(previousValue);
    				OnNavigationPropertyChanged("SupportTicketPriority");
    			}
    		}
    	}
    	private SupportTicketPriority _supportTicketPriority;
    
    	public User CreatedByUser
    	{
    		get { return _createdByUser; }
    		set
    		{
    			if (!ReferenceEquals(_createdByUser, value))
    			{
    				var previousValue = _createdByUser;
    				_createdByUser = value;
    				FixupCreatedByUser(previousValue);
    				OnNavigationPropertyChanged("CreatedByUser");
    			}
    		}
    	}
    	private User _createdByUser;
    
    	public User ModifiedByUser
    	{
    		get { return _modifiedByUser; }
    		set
    		{
    			if (!ReferenceEquals(_modifiedByUser, value))
    			{
    				var previousValue = _modifiedByUser;
    				_modifiedByUser = value;
    				FixupModifiedByUser(previousValue);
    				OnNavigationPropertyChanged("ModifiedByUser");
    			}
    		}
    	}
    	private User _modifiedByUser;
    
    	public SupportTicketStatus SupportTicketStatus
    	{
    		get { return _supportTicketStatus; }
    		set
    		{
    			if (!ReferenceEquals(_supportTicketStatus, value))
    			{
    				var previousValue = _supportTicketStatus;
    				_supportTicketStatus = value;
    				FixupSupportTicketStatus(previousValue);
    				OnNavigationPropertyChanged("SupportTicketStatus");
    			}
    		}
    	}
    	private SupportTicketStatus _supportTicketStatus;
    
    	public User AssignedUser
    	{
    		get { return _assignedUser; }
    		set
    		{
    			if (!ReferenceEquals(_assignedUser, value))
    			{
    				var previousValue = _assignedUser;
    				_assignedUser = value;
    				FixupAssignedUser(previousValue);
    				OnNavigationPropertyChanged("AssignedUser");
    			}
    		}
    	}
    	private User _assignedUser;
    
    	public TrackableCollection<Note> Notes
    	{
    		get
    		{
    			if (_notes == null)
    			{
    				_notes = new TrackableCollection<Note>();
    				_notes.CollectionChanged += FixupNotes;
    				_notes.CollectionChanged += RaiseNotesChanged;
    			}
    			return _notes;
    		}
    		set
    		{
    			if (!ReferenceEquals(_notes, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_notes != null)
    				{
    					_notes.CollectionChanged -= FixupNotes;
    					_notes.CollectionChanged -= RaiseNotesChanged;
    				}
    				_notes = value;
    				if (_notes != null)
    				{
    					_notes.CollectionChanged += FixupNotes;
    					_notes.CollectionChanged += RaiseNotesChanged;
    				}
    				OnNavigationPropertyChanged("Notes");
    			}
    		}
    	}
    	private TrackableCollection<Note> _notes;
    	partial void NotesChanged();
    	private void RaiseNotesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		NotesChanged();
    	}
    
    	public TrackableCollection<Order> Orders
    	{
    		get
    		{
    			if (_orders == null)
    			{
    				_orders = new TrackableCollection<Order>();
    				_orders.CollectionChanged += FixupOrders;
    				_orders.CollectionChanged += RaiseOrdersChanged;
    			}
    			return _orders;
    		}
    		set
    		{
    			if (!ReferenceEquals(_orders, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_orders != null)
    				{
    					_orders.CollectionChanged -= FixupOrders;
    					_orders.CollectionChanged -= RaiseOrdersChanged;
    				}
    				_orders = value;
    				if (_orders != null)
    				{
    					_orders.CollectionChanged += FixupOrders;
    					_orders.CollectionChanged += RaiseOrdersChanged;
    				}
    				OnNavigationPropertyChanged("Orders");
    			}
    		}
    	}
    	private TrackableCollection<Order> _orders;
    	partial void OrdersChanged();
    	private void RaiseOrdersChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		OrdersChanged();
    	}
    
    	public User User2
    	{
    		get { return _user2; }
    		set
    		{
    			if (!ReferenceEquals(_user2, value))
    			{
    				var previousValue = _user2;
    				_user2 = value;
    				FixupUser2(previousValue);
    				OnNavigationPropertyChanged("User2");
    			}
    		}
    	}
    	private User _user2;

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
    		if (_notes != null)
    		{
    			_notes.CollectionChanged -= FixupNotes;
    			_notes.CollectionChanged -= RaiseNotesChanged;
    			_notes.CollectionChanged += FixupNotes;
    			_notes.CollectionChanged += RaiseNotesChanged;
    		}
    		if (_orders != null)
    		{
    			_orders.CollectionChanged -= FixupOrders;
    			_orders.CollectionChanged -= RaiseOrdersChanged;
    			_orders.CollectionChanged += FixupOrders;
    			_orders.CollectionChanged += RaiseOrdersChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		Account = null;
    		SupportTicketCategory = null;
    		SupportTicketPriority = null;
    		CreatedByUser = null;
    		ModifiedByUser = null;
    		SupportTicketStatus = null;
    		AssignedUser = null;
    		Notes.Clear();
    		Orders.Clear();
    		User2 = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupAccount(Account previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.SupportTickets.Contains(this))
    		{
    			previousValue.SupportTickets.Remove(this);
    		}
    
    		if (Account != null)
    		{
    			if (!Account.SupportTickets.Contains(this))
    			{
    				Account.SupportTickets.Add(this);
    			}
    
    			AccountID = Account.AccountID;
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
    
    	private void FixupSupportTicketCategory(SupportTicketCategory previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.SupportTickets.Contains(this))
    		{
    			previousValue.SupportTickets.Remove(this);
    		}
    
    		if (SupportTicketCategory != null)
    		{
    			if (!SupportTicketCategory.SupportTickets.Contains(this))
    			{
    				SupportTicketCategory.SupportTickets.Add(this);
    			}
    
    			SupportTicketCategoryID = SupportTicketCategory.SupportTicketCategoryID;
    		}
    		else if (!skipKeys)
    		{
    			SupportTicketCategoryID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("SupportTicketCategory")
    				&& (ChangeTracker.OriginalValues["SupportTicketCategory"] == SupportTicketCategory))
    			{
    				ChangeTracker.OriginalValues.Remove("SupportTicketCategory");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("SupportTicketCategory", previousValue);
    			}
    			if (SupportTicketCategory != null && !SupportTicketCategory.ChangeTracker.ChangeTrackingEnabled)
    			{
    				SupportTicketCategory.StartTracking();
    			}
    		}
    	}
    
    	private void FixupSupportTicketPriority(SupportTicketPriority previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.SupportTickets.Contains(this))
    		{
    			previousValue.SupportTickets.Remove(this);
    		}
    
    		if (SupportTicketPriority != null)
    		{
    			if (!SupportTicketPriority.SupportTickets.Contains(this))
    			{
    				SupportTicketPriority.SupportTickets.Add(this);
    			}
    
    			SupportTicketPriorityID = SupportTicketPriority.SupportTicketPriorityID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("SupportTicketPriority")
    				&& (ChangeTracker.OriginalValues["SupportTicketPriority"] == SupportTicketPriority))
    			{
    				ChangeTracker.OriginalValues.Remove("SupportTicketPriority");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("SupportTicketPriority", previousValue);
    			}
    			if (SupportTicketPriority != null && !SupportTicketPriority.ChangeTracker.ChangeTrackingEnabled)
    			{
    				SupportTicketPriority.StartTracking();
    			}
    		}
    	}
    
    	private void FixupCreatedByUser(User previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.SupportTickets.Contains(this))
    		{
    			previousValue.SupportTickets.Remove(this);
    		}
    
    		if (CreatedByUser != null)
    		{
    			if (!CreatedByUser.SupportTickets.Contains(this))
    			{
    				CreatedByUser.SupportTickets.Add(this);
    			}
    
    			CreatedByUserID = CreatedByUser.UserID;
    		}
    		else if (!skipKeys)
    		{
    			CreatedByUserID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("CreatedByUser")
    				&& (ChangeTracker.OriginalValues["CreatedByUser"] == CreatedByUser))
    			{
    				ChangeTracker.OriginalValues.Remove("CreatedByUser");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("CreatedByUser", previousValue);
    			}
    			if (CreatedByUser != null && !CreatedByUser.ChangeTracker.ChangeTrackingEnabled)
    			{
    				CreatedByUser.StartTracking();
    			}
    		}
    	}
    
    	private void FixupModifiedByUser(User previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.SupportTickets1.Contains(this))
    		{
    			previousValue.SupportTickets1.Remove(this);
    		}
    
    		if (ModifiedByUser != null)
    		{
    			if (!ModifiedByUser.SupportTickets1.Contains(this))
    			{
    				ModifiedByUser.SupportTickets1.Add(this);
    			}
    
    			ModifiedByUserID = ModifiedByUser.UserID;
    		}
    		else if (!skipKeys)
    		{
    			ModifiedByUserID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("ModifiedByUser")
    				&& (ChangeTracker.OriginalValues["ModifiedByUser"] == ModifiedByUser))
    			{
    				ChangeTracker.OriginalValues.Remove("ModifiedByUser");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("ModifiedByUser", previousValue);
    			}
    			if (ModifiedByUser != null && !ModifiedByUser.ChangeTracker.ChangeTrackingEnabled)
    			{
    				ModifiedByUser.StartTracking();
    			}
    		}
    	}
    
    	private void FixupSupportTicketStatus(SupportTicketStatus previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.SupportTickets.Contains(this))
    		{
    			previousValue.SupportTickets.Remove(this);
    		}
    
    		if (SupportTicketStatus != null)
    		{
    			if (!SupportTicketStatus.SupportTickets.Contains(this))
    			{
    				SupportTicketStatus.SupportTickets.Add(this);
    			}
    
    			SupportTicketStatusID = SupportTicketStatus.SupportTicketStatusID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("SupportTicketStatus")
    				&& (ChangeTracker.OriginalValues["SupportTicketStatus"] == SupportTicketStatus))
    			{
    				ChangeTracker.OriginalValues.Remove("SupportTicketStatus");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("SupportTicketStatus", previousValue);
    			}
    			if (SupportTicketStatus != null && !SupportTicketStatus.ChangeTracker.ChangeTrackingEnabled)
    			{
    				SupportTicketStatus.StartTracking();
    			}
    		}
    	}
    
    	private void FixupAssignedUser(User previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.SupportTickets2.Contains(this))
    		{
    			previousValue.SupportTickets2.Remove(this);
    		}
    
    		if (AssignedUser != null)
    		{
    			if (!AssignedUser.SupportTickets2.Contains(this))
    			{
    				AssignedUser.SupportTickets2.Add(this);
    			}
    
    			AssignedUserID = AssignedUser.UserID;
    		}
    		else if (!skipKeys)
    		{
    			AssignedUserID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("AssignedUser")
    				&& (ChangeTracker.OriginalValues["AssignedUser"] == AssignedUser))
    			{
    				ChangeTracker.OriginalValues.Remove("AssignedUser");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("AssignedUser", previousValue);
    			}
    			if (AssignedUser != null && !AssignedUser.ChangeTracker.ChangeTrackingEnabled)
    			{
    				AssignedUser.StartTracking();
    			}
    		}
    	}
    
    	private void FixupUser2(User previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.SupportTickets2_1.Contains(this))
    		{
    			previousValue.SupportTickets2_1.Remove(this);
    		}
    
    		if (User2 != null)
    		{
    			if (!User2.SupportTickets2_1.Contains(this))
    			{
    				User2.SupportTickets2_1.Add(this);
    			}
    
    			ScalingUserID = User2.UserID;
    		}
    		else if (!skipKeys)
    		{
    			ScalingUserID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("User2")
    				&& (ChangeTracker.OriginalValues["User2"] == User2))
    			{
    				ChangeTracker.OriginalValues.Remove("User2");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("User2", previousValue);
    			}
    			if (User2 != null && !User2.ChangeTracker.ChangeTrackingEnabled)
    			{
    				User2.StartTracking();
    			}
    		}
    	}
    
    	private void FixupNotes(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (Note item in e.NewItems)
    			{
    				if (!item.SupportTickets.Contains(this))
    				{
    					item.SupportTickets.Add(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Notes", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (Note item in e.OldItems)
    			{
    				if (item.SupportTickets.Contains(this))
    				{
    					item.SupportTickets.Remove(this);
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Notes", item);
    				}
    			}
    		}
    	}
    
    	private void FixupOrders(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (Order item in e.NewItems)
    			{
    				item.SupportTicket = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("Orders", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (Order item in e.OldItems)
    			{
    				if (ReferenceEquals(item.SupportTicket, this))
    				{
    					item.SupportTicket = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("Orders", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}
