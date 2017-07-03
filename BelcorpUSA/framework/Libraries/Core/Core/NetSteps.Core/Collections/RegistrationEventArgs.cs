using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Encore.Core.Collections
{
	/// <summary>
	/// Registration event kinds.
	/// </summary>
	public enum RegistrationEventKind
	{
		/// <summary>
		/// None.
		/// </summary>
		None = 0,
		/// <summary>
		/// Indicates a new registration occurred.
		/// </summary>
		Registration = 1,
		/// <summary>
		/// Indicates the registrar is replacing the registration.
		/// </summary>
		Replacing = 2,
		/// <summary>
		/// Indicates the registrar has replaced the registration.
		/// </summary>
		Replaced = 3,
		/// <summary>
		/// Indicates the registration is being canceled.
		/// </summary>
		Canceling = 4,
		/// <summary>
		/// Indicates the registration is canceled.
		/// </summary>
		Canceled = 5,
	}

	/// <summary>
	/// Base (untyped) EventArgs for registration events.
	/// </summary>
	[Serializable]
	public class RegistrationEventArgs : EventArgs
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="kind"></param>
		public RegistrationEventArgs(IRegistrationKey key, RegistrationEventKind kind)
		{
			this.UntypedKey = key;
			this.Kind = kind;
			this.CanCancel = (kind == RegistrationEventKind.Replacing || kind == RegistrationEventKind.Canceling);
		}
		/// <summary>
		/// Gets the untyped registration.
		/// </summary>
		public IRegistrationKey UntypedKey { get; private set; }
		/// <summary>
		/// Gets the kind of registration event.
		/// </summary>
		public RegistrationEventKind Kind { get; private set; }

		/// <summary>
		/// Indicates whether the event can be canceled.
		/// </summary>
		public bool CanCancel { get; private set; }

		/// <summary>
		/// Cancels the event.
		/// </summary>
		/// <returns></returns>
		public void Cancel()
		{
			if (CanCancel)
			{
				IsCanceled = true;
			}
		}

		/// <summary>
		/// Indicates whether the event has been canceled.
		/// </summary>
		public bool IsCanceled { get; private set; }
	}

	/// <summary>
	/// EventArgs for registration events.
	/// </summary>
	[Serializable]
	public class RegistrationEventArgs<K, H> : RegistrationEventArgs
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="kind"></param>
		public RegistrationEventArgs(IRegistrationKey<K, H> key, RegistrationEventKind kind)
			: base(key, kind)
		{
		}
		/// <summary>
		/// Gets the strongly typed registration key.
		/// </summary>
		public IRegistrationKey<K, H> Key { get { return (IRegistrationKey<K, H>)UntypedKey; } }
	}
	
}
