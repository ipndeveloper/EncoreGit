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
    [KnownType(typeof(ProductFile))]
    [Serializable]
    public partial class ProductFileType: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void ProductFileTypeIDChanged();
    	public int ProductFileTypeID
    	{
    		get { return _productFileTypeID; }
    		set
    		{
    			if (_productFileTypeID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'ProductFileTypeID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_productFileTypeID = value;
    				ProductFileTypeIDChanged();
    				OnPropertyChanged("ProductFileTypeID");
    			}
    		}
    	}
    	private int _productFileTypeID;
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

        #endregion
        #region Navigation Properties
    
    	public TrackableCollection<ProductFile> ProductFiles
    	{
    		get
    		{
    			if (_productFiles == null)
    			{
    				_productFiles = new TrackableCollection<ProductFile>();
    				_productFiles.CollectionChanged += FixupProductFiles;
    				_productFiles.CollectionChanged += RaiseProductFilesChanged;
    			}
    			return _productFiles;
    		}
    		set
    		{
    			if (!ReferenceEquals(_productFiles, value))
    			{
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
    				}
    				if (_productFiles != null)
    				{
    					_productFiles.CollectionChanged -= FixupProductFiles;
    					_productFiles.CollectionChanged -= RaiseProductFilesChanged;
    				}
    				_productFiles = value;
    				if (_productFiles != null)
    				{
    					_productFiles.CollectionChanged += FixupProductFiles;
    					_productFiles.CollectionChanged += RaiseProductFilesChanged;
    				}
    				OnNavigationPropertyChanged("ProductFiles");
    			}
    		}
    	}
    	private TrackableCollection<ProductFile> _productFiles;
    	partial void ProductFilesChanged();
    	private void RaiseProductFilesChanged(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		ProductFilesChanged();
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
    		if (_productFiles != null)
    		{
    			_productFiles.CollectionChanged -= FixupProductFiles;
    			_productFiles.CollectionChanged -= RaiseProductFilesChanged;
    			_productFiles.CollectionChanged += FixupProductFiles;
    			_productFiles.CollectionChanged += RaiseProductFilesChanged;
    		}
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		ProductFiles.Clear();
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupProductFiles(object sender, NotifyCollectionChangedEventArgs e)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (e.NewItems != null)
    		{
    			foreach (ProductFile item in e.NewItems)
    			{
    				item.ProductFileType = this;
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					if (!item.ChangeTracker.ChangeTrackingEnabled)
    					{
    						item.StartTracking();
    					}
    					ChangeTracker.RecordAdditionToCollectionProperties("ProductFiles", item);
    				}
    			}
    		}
    
    		if (e.OldItems != null)
    		{
    			foreach (ProductFile item in e.OldItems)
    			{
    				if (ReferenceEquals(item.ProductFileType, this))
    				{
    					item.ProductFileType = null;
    				}
    				if (ChangeTracker.ChangeTrackingEnabled)
    				{
    					ChangeTracker.RecordRemovalFromCollectionProperties("ProductFiles", item);
    				}
    			}
    		}
    	}

        #endregion
    }
}