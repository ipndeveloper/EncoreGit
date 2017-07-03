using System;

namespace NetSteps.Encore.Core.Collections
{
	/// <summary>
	/// Base (untyped) registration key.
	/// </summary>
	public interface IRegistrationKey
	{
		/// <summary>
		/// Cancels a registration.
		/// </summary>
		void Cancel();
		/// <summary>
		/// Indicates whether the registration has been canceled.
		/// </summary>
		bool IsCanceled { get; }

		/// <summary>
		/// Gets the key's type.
		/// </summary>
		Type KeyType { get; }
		/// <summary>
		/// Gets the handback's type.
		/// </summary>
		Type HandbackType { get; }

		/// <summary>
		/// Gets a reference (untyped) to the registrar
		/// upon which this registration was made.
		/// </summary>
		IRegistrar UntypedRegistrar { get; }

		/// <summary>
		/// Gets a reference (untyped) to the key.
		/// </summary>
		object UntypedKey { get; }

		/// <summary>
		/// Gets a reference (untyped) to the handback.
		/// </summary>
		object UntypedHandback { get; }
	}

	/// <summary>
	/// Represents a strongly typed registration of key type K with
	/// a registrar.
	/// </summary>
	/// <typeparam name="K">key type K</typeparam>
	/// <typeparam name="H">handback type H</typeparam>
	public interface IRegistrationKey<K, H> : IRegistrationKey
	{
		/// <summary>
		/// Gets the strongly typed registrar upon which this
		/// registration was made.
		/// </summary>
		IRegistrar<K, H> Registrar { get; }
		/// <summary>
		/// Gets the strongly typed key.
		/// </summary>
		K Key { get; }
		/// <summary>
		/// Gets the strongly typed handback.
		/// </summary>
		H Handback { get; }

		/// <summary>
		/// Gets and sets the registration event handler.
		/// </summary>
		event EventHandler<RegistrationEventArgs<K, H>> OnAny;
	}	
}
