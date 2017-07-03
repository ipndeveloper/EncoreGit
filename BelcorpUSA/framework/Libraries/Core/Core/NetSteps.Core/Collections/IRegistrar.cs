using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetSteps.Encore.Core.Collections
{
	/// <summary>
	/// Base (untyped) registrar.
	/// </summary>
	public interface IRegistrar
	{
		/// <summary>
		/// Cancels the registration given.
		/// </summary>
		/// <param name="key">a registration</param>
		/// <returns><em>true</em> if the registration was canceled as a result of the call; otherwise <em>false</em>.</returns>
		bool CancelRegistration(IRegistrationKey key);
	}

	/// <summary>
	/// Strongly typed registrar; maintains registrations.
	/// </summary>
	/// <typeparam name="K">key type K</typeparam>
	/// <typeparam name="H">handback type H</typeparam>
	public interface IRegistrar<K, H> : IRegistrar
	{
		/// <summary>
		/// Determines if a key has a registration.
		/// </summary>
		/// <param name="key">the key</param>
		/// <returns><em>true</em> if the registration is present; otherwise <em>false</em></returns>
		bool IsRegistered(K key);

		/// <summary>
		/// Tries to get the current registration for a key.
		/// </summary>
		/// <param name="key">the key</param>
		/// <param name="registration">reference to a variable where the registration
		/// will be returned upon success.</param>
		/// <returns><em>true</em> if the registration is present; otherwise <em>false</em></returns>
		bool TryGetRegistration(K key, out IRegistrationKey<K, H> registration);

		/// <summary>
		/// Tries to register a key and handback.
		/// </summary>
		/// <param name="key">the key</param>
		/// <param name="handback">the handback</param>
		/// <param name="registration">reference to a variable where the registration
		/// will be returned upon success.</param>
		/// <returns><em>true</em> if the registration is successful; otherwise <em>false</em></returns>
		bool TryRegister(K key, H handback, out IRegistrationKey<K, H> registration);

		/// <summary>
		/// Tries to replace the current registration.
		/// </summary>
		/// <param name="current">the current</param>
		/// <param name="key">the key</param>
		/// <param name="handback">the handback</param>
		/// <param name="registration">reference to a variable where the new
		/// registration will be returned upon success.</param>
		/// <returns><em>true</em> if the registration is present; otherwise <em>false</em></returns>
		bool TryReplaceRegistration(IRegistrationKey current, K key, H handback, out IRegistrationKey<K, H> registration);

		/// <summary>
		/// Event fired on any registration event.
		/// </summary>
		event EventHandler<RegistrationEventArgs<K, H>> OnAny;
		/// <summary>
		/// Event fired when new registrations occur.
		/// </summary>
		event EventHandler<RegistrationEventArgs<K, H>> OnNewRegistration;
	}
}