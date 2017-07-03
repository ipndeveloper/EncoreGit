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
    [KnownType(typeof(HtmlContentStatus))]
    [KnownType(typeof(User))]
    [KnownType(typeof(HtmlContent))]
    [Serializable]
    public partial class HtmlContentHistory: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void HtmlContentHistoryIDChanged();
    	public int HtmlContentHistoryID
    	{
    		get { return _htmlContentHistoryID; }
    		set
    		{
    			if (_htmlContentHistoryID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'HtmlContentHistoryID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_htmlContentHistoryID = value;
    				HtmlContentHistoryIDChanged();
    				OnPropertyChanged("HtmlContentHistoryID");
    			}
    		}
    	}
    	private int _htmlContentHistoryID;
    	partial void HtmlContentIDChanged();
    	public int HtmlContentID
    	{
    		get { return _htmlContentID; }
    		set
    		{
    			if (_htmlContentID != value)
    			{
    				ChangeTracker.RecordOriginalValue("HtmlContentID", _htmlContentID);
    				if (!IsDeserializing)
    				{
    					if (HtmlContent != null && HtmlContent.HtmlContentID != value)
    					{
    						HtmlContent = null;
    					}
    				}
    				_htmlContentID = value;
    				HtmlContentIDChanged();
    				OnPropertyChanged("HtmlContentID");
    			}
    		}
    	}
    	private int _htmlContentID;
    	partial void HtmlContentStatusIDChanged();
    	public int HtmlContentStatusID
    	{
    		get { return _htmlContentStatusID; }
    		set
    		{
    			if (_htmlContentStatusID != value)
    			{
    				ChangeTracker.RecordOriginalValue("HtmlContentStatusID", _htmlContentStatusID);
    				if (!IsDeserializing)
    				{
    					if (HtmlContentStatus != null && HtmlContentStatus.HtmlContentStatusID != value)
    					{
    						HtmlContentStatus = null;
    					}
    				}
    				_htmlContentStatusID = value;
    				HtmlContentStatusIDChanged();
    				OnPropertyChanged("HtmlContentStatusID");
    			}
    		}
    	}
    	private int _htmlContentStatusID;
    	partial void UserIDChanged();
    	public Nullable<int> UserID
    	{
    		get { return _userID; }
    		set
    		{
    			if (_userID != value)
    			{
    				ChangeTracker.RecordOriginalValue("UserID", _userID);
    				if (!IsDeserializing)
    				{
    					if (User != null && User.UserID != value)
    					{
    						User = null;
    					}
    				}
    				_userID = value;
    				UserIDChanged();
    				OnPropertyChanged("UserID");
    			}
    		}
    	}
    	private Nullable<int> _userID;
    	partial void HistoryDateUTCChanged();
    	public Nullable<System.DateTime> HistoryDateUTC
    	{
    		get { return _historyDateUTC; }
    		set
    		{
    			if (_historyDateUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("HistoryDateUTC", _historyDateUTC);
    				_historyDateUTC = value;
    				HistoryDateUTCChanged();
    				OnPropertyChanged("HistoryDateUTC");
    			}
    		}
    	}
    	private Nullable<System.DateTime> _historyDateUTC;
    	partial void CommentsChanged();
    	public string Comments
    	{
    		get { return _comments; }
    		set
    		{
    			if (_comments != value)
    			{
    				ChangeTracker.RecordOriginalValue("Comments", _comments);
    				_comments = value;
    				CommentsChanged();
    				OnPropertyChanged("Comments");
    			}
    		}
    	}
    	private string _comments;
    	partial void MessageSeenChanged();
    	public bool MessageSeen
    	{
    		get { return _messageSeen; }
    		set
    		{
    			if (_messageSeen != value)
    			{
    				ChangeTracker.RecordOriginalValue("MessageSeen", _messageSeen);
    				_messageSeen = value;
    				MessageSeenChanged();
    				OnPropertyChanged("MessageSeen");
    			}
    		}
    	}
    	private bool _messageSeen;

        #endregion
        #region Navigation Properties
    
    	public HtmlContentStatus HtmlContentStatus
    	{
    		get { return _htmlContentStatus; }
    		set
    		{
    			if (!ReferenceEquals(_htmlContentStatus, value))
    			{
    				var previousValue = _htmlContentStatus;
    				_htmlContentStatus = value;
    				FixupHtmlContentStatus(previousValue);
    				OnNavigationPropertyChanged("HtmlContentStatus");
    			}
    		}
    	}
    	private HtmlContentStatus _htmlContentStatus;
    
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
    
    	public HtmlContent HtmlContent
    	{
    		get { return _htmlContent; }
    		set
    		{
    			if (!ReferenceEquals(_htmlContent, value))
    			{
    				var previousValue = _htmlContent;
    				_htmlContent = value;
    				FixupHtmlContent(previousValue);
    				OnNavigationPropertyChanged("HtmlContent");
    			}
    		}
    	}
    	private HtmlContent _htmlContent;

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
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		HtmlContentStatus = null;
    		User = null;
    		HtmlContent = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupHtmlContentStatus(HtmlContentStatus previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.HtmlContentHistories.Contains(this))
    		{
    			previousValue.HtmlContentHistories.Remove(this);
    		}
    
    		if (HtmlContentStatus != null)
    		{
    			if (!HtmlContentStatus.HtmlContentHistories.Contains(this))
    			{
    				HtmlContentStatus.HtmlContentHistories.Add(this);
    			}
    
    			HtmlContentStatusID = HtmlContentStatus.HtmlContentStatusID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("HtmlContentStatus")
    				&& (ChangeTracker.OriginalValues["HtmlContentStatus"] == HtmlContentStatus))
    			{
    				ChangeTracker.OriginalValues.Remove("HtmlContentStatus");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("HtmlContentStatus", previousValue);
    			}
    			if (HtmlContentStatus != null && !HtmlContentStatus.ChangeTracker.ChangeTrackingEnabled)
    			{
    				HtmlContentStatus.StartTracking();
    			}
    		}
    	}
    
    	private void FixupUser(User previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.HtmlContentHistories.Contains(this))
    		{
    			previousValue.HtmlContentHistories.Remove(this);
    		}
    
    		if (User != null)
    		{
    			if (!User.HtmlContentHistories.Contains(this))
    			{
    				User.HtmlContentHistories.Add(this);
    			}
    
    			UserID = User.UserID;
    		}
    		else if (!skipKeys)
    		{
    			UserID = null;
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
    
    	private void FixupHtmlContent(HtmlContent previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.HtmlContentHistories.Contains(this))
    		{
    			previousValue.HtmlContentHistories.Remove(this);
    		}
    
    		if (HtmlContent != null)
    		{
    			if (!HtmlContent.HtmlContentHistories.Contains(this))
    			{
    				HtmlContent.HtmlContentHistories.Add(this);
    			}
    
    			HtmlContentID = HtmlContent.HtmlContentID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("HtmlContent")
    				&& (ChangeTracker.OriginalValues["HtmlContent"] == HtmlContent))
    			{
    				ChangeTracker.OriginalValues.Remove("HtmlContent");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("HtmlContent", previousValue);
    			}
    			if (HtmlContent != null && !HtmlContent.ChangeTracker.ChangeTrackingEnabled)
    			{
    				HtmlContent.StartTracking();
    			}
    		}
    	}

        #endregion
    }
}