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
    [KnownType(typeof(HtmlContentWorkflow))]
    [Serializable]
    public partial class HtmlContentWorkflowType: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void HtmlContentWorkflowTypeIDChanged();
    	public short HtmlContentWorkflowTypeID
    	{
    		get { return _htmlContentWorkflowTypeID; }
    		set
    		{
    			if (_htmlContentWorkflowTypeID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'HtmlContentWorkflowTypeID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_htmlContentWorkflowTypeID = value;
    				HtmlContentWorkflowTypeIDChanged();
    				OnPropertyChanged("HtmlContentWorkflowTypeID");
    			}
    		}
    	}
    	private short _htmlContentWorkflowTypeID;
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
    
    	public TrackableCollection<HtmlContentWorkflow> HtmlContentWorkflows
    	{
    		get
    		{
    			if (_htmlContentWorkflows == null)
    			{
    				_htmlContentWorkflows = new TrackableCollection<HtmlContentWorkflow>();
    				_htmlContentWorkflows.CollectionChanged += FixupHtmlContentWorkflows;
    				_htmlContentWorkflows.CollectionChanged += RaiseHtmlContentWorkflowsChanged;
    			}
    			return _htmlContentWorkflows;
    		}
    		set
    		{
    			if (!ReferenceEquals(_htmlContentWorkflows, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_htmlContentWorkflows != null)
    				{
    					_htmlContentWorkflows.CollectionChanged -= FixupHtmlContentWorkflows;
    					_htmlContentWorkflows.CollectionChanged -= RaiseHtmlContentWorkflowsChanged;
    				}
    				_htmlContentWorkflows = value;
    				if (_htmlContentWorkflows != null)
    				{
    					_htmlContentWorkflows.CollectionChanged += FixupHtmlContentWorkflows;
    					_htmlContentWorkflows.CollectionChanged += RaiseHtmlContentWorkflowsChanged;
    				}
    				OnNavigationPropertyChanged("HtmlContentWorkflows");
    			}
    		}
    	}
    	private TrackableCollection<HtmlContentWorkflow> _htmlContentWorkflows;
    	partial void HtmlContentWorkflowsChanged();
    	private void RaiseHtmlContentWorkflowsChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		HtmlContentWorkflowsChanged();
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
    		if (_htmlContentWorkflows != null)
    		{
    			_htmlContentWorkflows.CollectionChanged -= FixupHtmlContentWorkflows;
    			_htmlContentWorkflows.CollectionChanged -= RaiseHtmlContentWorkflowsChanged;
    			_htmlContentWorkflows.CollectionChanged += FixupHtmlContentWorkflows;
    			_htmlContentWorkflows.CollectionChanged += RaiseHtmlContentWorkflowsChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		HtmlContentWorkflows.Clear();
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupHtmlContentWorkflows(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (HtmlContentWorkflow item in e.NewItems)
    			{
    				item.HtmlContentWorkflowType = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("HtmlContentWorkflows", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (HtmlContentWorkflow item in e.OldItems)
    			{
    				if (ReferenceEquals(item.HtmlContentWorkflowType, this))
    				{
    					item.HtmlContentWorkflowType = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("HtmlContentWorkflows", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}