using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace NetSteps.Encore.Core.Collections
{
	/// <summary>
	/// Basic implementation of the registrar.
	/// </summary>
	/// <typeparam name="K">key type K</typeparam>
	/// <typeparam name="H">handback type H</typeparam>
	public class Registrar<K, H> : IRegistrar<K, H>
	{
		Object _lock = new Object();
		Dictionary<K, IRegistrationKey> _registrations = new Dictionary<K, IRegistrationKey>();
		int _revision = 0;

		[Serializable]
		class RegistrationKey : IRegistrationKey<K, H>
		{
			internal RegistrationKey(IRegistrar<K, H> registrar, K key, H handback)
			{
				this.Registrar = registrar;
				this.Key = key;
				this.Handback = handback;
			}

			public bool IsCanceled { get; private set; }

			public IRegistrar<K, H> Registrar { get; private set; }

			public K Key { get; private set; }

			public H Handback { get; private set; }

			public void Cancel()
			{
				if (!IsCanceled)
				{
					Registrar.CancelRegistration(this);
					this.IsCanceled = true;
				}
			}

			public Type KeyType { get { return typeof(K); } }

			public Type HandbackType { get { return typeof(H); } }

			public IRegistrar UntypedRegistrar { get { return Registrar; } }

			public object UntypedKey { get { return Key; } }

			public object UntypedHandback { get { return Handback; } }

			public event EventHandler<RegistrationEventArgs<K, H>> OnAny;

			internal void NotifyRegistrationEvent(RegistrationEventArgs<K, H> e)
			{
				if (OnAny != null)
				{
					OnAny(Registrar, e);
				}
			}
		}

		/// <summary>
		/// Determines if a key has a registration.
		/// </summary>
		/// <param name="key">the key</param>
		/// <returns><em>true</em> if the registration is present; otherwise <em>false</em></returns>
		public bool IsRegistered(K key)
		{
			lock (_lock)
			{
				return _registrations.ContainsKey(key);
			}
		}

		/// <summary>
		/// Tries to get the current registration for a key.
		/// </summary>
		/// <param name="key">the key</param>
		/// <param name="registration">reference to a variable where the registration
		/// will be returned upon success.</param>
		/// <returns><em>true</em> if the registration is present; otherwise <em>false</em></returns>
		public bool TryGetRegistration(K key, out IRegistrationKey<K, H> registration)
		{
			lock (_lock)
			{
				if (_registrations.ContainsKey(key))
				{
					registration = (IRegistrationKey<K, H>)_registrations[key];
					return true;
				}
			}
			registration = default(IRegistrationKey<K, H>);
			return false;
		}

		/// <summary>
		/// Tries to register a key and handback.
		/// </summary>
		/// <param name="key">the key</param>
		/// <param name="handback">the handback</param>
		/// <param name="registration">reference to a variable where the registration
		/// will be written upon success.</param>
		/// <returns><em>true</em> if the registration is successful; otherwise <em>false</em></returns>
		public bool TryRegister(K key, H handback, out IRegistrationKey<K, H> registration)
		{
			var result = false;
			registration = default(IRegistrationKey<K, H>);

			lock (_lock)
			{
				if (!_registrations.ContainsKey(key))
				{
					registration = new RegistrationKey(this, key, handback);
					_registrations.Add(key, registration);
					_revision++;
					result = true;
				}
			}
			if (result)
			{ // notify registration observers outside the lock...
				NotifyRegistrationEvent(new RegistrationEventArgs<K, H>(registration, RegistrationEventKind.Registration), null);
			}
			return result;
		}

		/// <summary>
		/// Tries to replace the current registration.
		/// </summary>
		/// <param name="current">the current</param>
		/// <param name="key">the key</param>
		/// <param name="handback">the handback</param>
		/// <param name="registration">reference to a variable where the new
		/// registration will be returned upon success.</param>
		/// <returns><em>true</em> if the registration is present; otherwise <em>false</em></returns>
		public bool TryReplaceRegistration(IRegistrationKey current, K key, H handback, out IRegistrationKey<K, H> registration)
		{
			var result = false;
			registration = default(IRegistrationKey<K, H>);

			RegistrationKey ours = current as RegistrationKey;
			if (ours != null && Object.ReferenceEquals(this, ours.Registrar))
			{
				lock (_lock)
				{
					if (_registrations.ContainsKey(key))
					{
						var it = _registrations[key];
						if (Object.ReferenceEquals(it, ours))
						{
							var evt = new RegistrationEventArgs<K, H>(ours, RegistrationEventKind.Replacing);
							NotifyRegistrationEvent(evt, ours);
							if (!evt.IsCanceled)
							{
								registration = new RegistrationKey(this, key, handback);
								_registrations[key] = registration;
								_revision++;
								NotifyRegistrationEvent(new RegistrationEventArgs<K, H>(ours, RegistrationEventKind.Replaced), ours);
								result = true;
							}
						}
					}
				}
			}
			if (result)
			{ // notify registration observers outside the lock...
				NotifyRegistrationEvent(new RegistrationEventArgs<K, H>(registration, RegistrationEventKind.Registration), null);
			}
			return result;
		}

		/// <summary>
		/// Cancels the registration given.
		/// </summary>
		/// <param name="registration">a registration</param>
		/// <returns><em>true</em> if the registration was canceled as a result of the call; otherwise <em>false</em>.</returns>
		public bool CancelRegistration(IRegistrationKey registration)
		{
			RegistrationKey ours = registration as RegistrationKey;
			if (ours != null && Object.ReferenceEquals(this, ours.Registrar))
			{
				var key = ours.Key;
				lock (_lock)
				{
					if (_registrations.ContainsKey(key))
					{
						var current = _registrations[key];
						if (Object.ReferenceEquals(current, registration))
						{
							var evt = new RegistrationEventArgs<K, H>(ours, RegistrationEventKind.Canceling);
							NotifyRegistrationEvent(evt, ours);
							if (!evt.IsCanceled)
							{
								_registrations.Remove(key);
								_revision++;
								NotifyRegistrationEvent(new RegistrationEventArgs<K, H>(ours, RegistrationEventKind.Canceled), ours);
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Event fired on any registration event.
		/// </summary>
		public event EventHandler<RegistrationEventArgs<K, H>> OnAny;
		/// <summary>
		/// Event fired when new registrations occur.
		/// </summary>
		public event EventHandler<RegistrationEventArgs<K, H>> OnNewRegistration;

		/// <summary>
		/// Allows subclasses to safely walk the registrations without
		/// blocking concurrent registrar operations.
		/// </summary>
		/// <param name="visitor">an action called for each regisration</param>
		protected void VisitEach(Action<IRegistrationKey<K, H>> visitor)
		{
			var snapshot = GetRegistrationSnapshot();
			foreach (var reg in snapshot)
			{
				visitor(reg);
			}
		}

		private void NotifyRegistrationEvent(RegistrationEventArgs<K, H> evt, RegistrationKey context)
		{
			if (context != null)
			{
				context.NotifyRegistrationEvent(evt);
			}
			if (evt.Kind == RegistrationEventKind.Registration
				&& OnNewRegistration != null)
			{
				OnNewRegistration(this, evt);
			}
			if (OnAny != null)
			{
				OnAny(this, evt);
			}
		}
			
		int _snapshotRevision = 0;
		IRegistrationKey<K, H>[] _snapshot = new IRegistrationKey<K, H>[0];
		private IEnumerable<IRegistrationKey<K, H>> GetRegistrationSnapshot()
		{
			IRegistrationKey<K, H>[] result;
			lock (_lock)
			{
				if (_snapshotRevision != _revision)
				{
					_snapshot = _registrations.Values.Cast<IRegistrationKey<K, H>>().ToArray();
					_snapshotRevision = _revision;
				}
				result = _snapshot;
			}
			return result;
		}
	}

}
