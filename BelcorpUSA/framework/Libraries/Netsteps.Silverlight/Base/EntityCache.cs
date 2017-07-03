using System;
using NetSteps.Silverlight.Threading;

namespace NetSteps.Silverlight.Base
{
	/// <summary>
	/// Author: John Egbert
	/// For client side caching of data that is cached in memory and Isolated storage.
	/// Created: 12/10/2009
	/// </summary>
	public abstract class EntityCache<T> where T : class
	{
		protected readonly object _lock = new object();
		protected T _cachedInstance = null;

		protected string _fileName = string.Empty;
		protected string FileName
		{
			get
			{
				if (string.IsNullOrEmpty(_fileName))
					_fileName = string.Format("{0}-cache.dat", typeof(T).Name);
				return _fileName;
			}
			set
			{
				_fileName = value;
			}
		}

		protected bool _updateInstanceOnAccess = true;
		public bool UpdateInstanceOnAccess
		{
			get
			{
				return _updateInstanceOnAccess;
			}
			set
			{
				_updateInstanceOnAccess = value;
			}
		}

		public virtual void GetInstance(Action<T> callback)
		{
			if (_cachedInstance == null)
			{
				if (IsolatedStorage.ContainsFile(FileName))
				{
					_cachedInstance = IsolatedStorage.LoadData<T>(FileName);
					if (callback != null)
						callback(_cachedInstance);

					if (UpdateInstanceOnAccess)
					{
						BackgroundAction.DoWork(() =>
						{
							UpdateInstanceFromLiveSource();  // Update data from LIVE - JHE
						});
					}
				}
				else
					LoadInstance(callback);
			}
			else if (callback != null)
				callback(_cachedInstance);
		}

		protected abstract void LoadInstance(Action<T> callback);

		public virtual void UpdateInstanceFromLiveSource()
		{

		}

		protected virtual void SaveInstanceToIsolatedStorage()
		{
			// Do this in another thread; not UI thread (for performance) - JHE
			BackgroundAction.DoWork(() =>
			{
				if (_cachedInstance == null)
				{
					//IsolatedStorage.DeleteFile(FileName);
				}
				else
					IsolatedStorage.SaveData<T>(_cachedInstance, FileName);
			});
		}
	}
}
