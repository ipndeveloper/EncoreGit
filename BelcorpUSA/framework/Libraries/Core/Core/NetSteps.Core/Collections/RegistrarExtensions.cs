using System;
using System.Diagnostics.Contracts;

namespace NetSteps.Encore.Core.Collections
{
	/// <summary>
	/// Contains utility methods for registrars.
	/// </summary>
	public static class RegistrarExtensions
	{
		/// <summary>
		/// Adds a registration for the key and handback given.
		/// </summary>
		/// <typeparam name="K">key type K</typeparam>
		/// <typeparam name="H">handback type H</typeparam>
		/// <param name="registrar">the registrar</param>
		/// <param name="key">the key</param>
		/// <param name="handback">the handback</param>
		public static void Register<K, H>(this IRegistrar<K, H> registrar, K key, H handback)
		{
			Contract.Requires<ArgumentNullException>(registrar != null);

			IRegistrationKey<K, H> reg;
			if (!registrar.TryRegister(key, handback, out reg))
				throw new RegistrationException(String.Concat("Already registered: ", Convert.ToString(key)));
		}

		/// <summary>
		/// Unregisters the current registration associated with the key given.
		/// </summary>
		/// <typeparam name="K">key type K</typeparam>
		/// <typeparam name="H">handback type H</typeparam>
		/// <param name="registrar">the registrar</param>
		/// <param name="key">the key</param>
		public static void Register<K, H>(this IRegistrar<K, H> registrar, K key)
		{
			Contract.Requires<ArgumentNullException>(registrar != null);

			IRegistrationKey<K, H> reg;
			if (registrar.TryGetRegistration(key, out reg))
			{
				if (!registrar.CancelRegistration(reg))
					throw new RegistrationException(String.Concat("Unregister was canceled: ", Convert.ToString(key)));
			}
		}

	}
}
