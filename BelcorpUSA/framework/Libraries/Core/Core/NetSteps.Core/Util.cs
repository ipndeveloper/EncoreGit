using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Threading;
using NetSteps.Encore.Core.Properties;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace NetSteps.Encore.Core
{
	/// <summary>
	/// Utility class containing utility functions and extensions.
	/// </summary>
	public static class Util
	{
		/// <summary>
		/// Gets an SHA1 hashcode for the value given, using the default UTF8 encoding.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static byte[] GetSHA1Hash(string value)
		{
			Contract.Requires<ArgumentNullException>(value != null);
			return GetSHA1Hash(value, Encoding.UTF8);
		}

		/// <summary>
		/// Gets an SHA1 hashcode for the value given.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="enc"></param>
		/// <returns></returns>
		public static byte[] GetSHA1Hash(string value, Encoding enc)
		{
			Contract.Requires<ArgumentNullException>(value != null);
			Contract.Requires<ArgumentNullException>(enc != null);

			using (var provider = new SHA1CryptoServiceProvider())
			{
				var buffer = enc.GetBytes(value);
				var hash = provider.ComputeHash(buffer, 0, buffer.Length);
				Contract.Assert(hash != null);
				return hash;
			}
		}

		/// <summary>
		/// Gets an SHA1 hashcode for the value given and converts it to Base64, using the default UTF8 encoding.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetSHA1HashAndConvertToBase64(string value)
		{
			Contract.Requires<ArgumentNullException>(value != null);

			return Convert.ToBase64String(GetSHA1Hash(value, Encoding.UTF8));
		}

		/// <summary>
		/// Gets an SHA1 hashcode for the value given and converts it to Base64.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="enc"></param>
		/// <returns></returns>
		public static string GetSHA1HashAndConvertToBase64(string value, Encoding enc)
		{
			Contract.Requires<ArgumentNullException>(value != null);
			Contract.Requires<ArgumentNullException>(enc != null);

			return Convert.ToBase64String(GetSHA1Hash(value, enc));
		}

		/// <summary>
		/// Disposes an instance if it is disposable and sets the reference variable to null.
		/// </summary>
		/// <typeparam name="T">typeof item T</typeparam>
		/// <param name="item">reference to an item to be disposed.</param>
		/// <returns><em>true</em> if the item is disposed as a result of the call; otherwise <em>false</em>.</returns>
		public static bool Dispose<T>(ref T item)
			where T : class
		{
			Thread.MemoryBarrier();
			var disposable = item;
			Thread.MemoryBarrier();

			if (Interlocked.CompareExchange(ref item, default(T), disposable) == disposable)
			{
				if (disposable is IDisposable)
					((IDisposable)disposable).Dispose();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Interns a string if it is not already interned.
		/// </summary>
		/// <param name="value">the target string</param>
		/// <returns>the value string interned</returns>
		internal static object MakeReliableLockFromString(this string value)
		{
			Contract.Requires<ArgumentNullException>(value != null, Resources.Chk_CannotBeNull);
			return value.InternIt();
		}

		/// <summary>
		/// Interns a string if it is not already interned.
		/// </summary>
		/// <param name="value">the target string</param>
		/// <returns>the value string interned</returns>
		public static string InternIt(this string value)
		{
			Contract.Requires<ArgumentNullException>(value != null, Resources.Chk_CannotBeNull);
			return String.Intern(value);
		}

		/// <summary>
		/// Gets a lock for a type suitable for synchronizing activity on the type
		/// without blocking other activity in the VM.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static Object GetLockForType(this Type type)
		{
			Contract.Requires<ArgumentNullException>(type != null, Resources.Chk_CannotBeNull);
			return GetKeyForType(type);
		}

		/// <summary>
		/// Gets a key for a type suitable for representing the type as a hashtable
		/// or dictionary key without pinning the type and its assembly into memory.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static string GetKeyForType(this Type type)
		{
			Contract.Requires<ArgumentNullException>(type != null, Resources.Chk_CannotBeNull);
			Contract.Assume(type.AssemblyQualifiedName != null);
			return InternIt(type.AssemblyQualifiedName);
		}

		/// <summary>
		/// Gets a lock for an assembly suitable for synchronizing activity on the 
		/// assembly without blocking other activity in the VM.
		/// </summary>
		/// <param name="assembly">the assembly</param>
		/// <returns>an object suitable for locking activity against the assembly</returns>
		public static Object GetLockForAssembly(this Assembly assembly)
		{
			Contract.Requires<ArgumentNullException>(assembly != null, Resources.Chk_CannotBeNull);
			return GetKeyForAssembly(assembly);
		}

		/// <summary>
		/// Gets a key for an assembly suitable for representing the assembly as a hashtable
		/// or dictionary key without pinning the assembly into memory.
		/// </summary>
		/// <param name="assembly">the assembly</param>
		/// <returns>a key for an assembly</returns>
		public static string GetKeyForAssembly(this Assembly assembly)
		{
			Contract.Requires<ArgumentNullException>(assembly != null, Resources.Chk_CannotBeNull);
			return InternIt(assembly.FullName);
		}
				
		/// <summary>
		/// Initializes a referenced variable if it is not already initialized.
		/// </summary>
		/// <typeparam name="T">variable type T</typeparam>
		/// <param name="variable">reference to the variable being initialized</param>
		/// <param name="lck">an object used as a lock if initialization is necessary</param>
		public static T LazyInitializeWithLock<T>(ref T variable, Object lck)
			where T : class, new()
		{
			Contract.Requires<ArgumentNullException>(lck != null, Resources.Chk_CannotBeNull);

			if (variable == null)
			{
				lock (lck)
				{ // double-check the lock in case we're in a race...
					if (variable == null)
					{
						variable = new T();
					}
				}
			}
			return variable;
		}

		/// <summary>
		/// Initializes a referenced variable if it is not already initialized. Uses
		/// the <paramref name="factory"/> to create the instance if necessary.
		/// </summary>
		/// <typeparam name="T">variable type T</typeparam>
		/// <param name="variable">reference to the variable being initialized</param>
		/// <param name="lck">an object used as a lock if initialization is necessary</param>
		/// <param name="factory">factory delegate</param>
		/// <returns>the value of the variable, after the lazy initailize</returns>
		public static T LazyInitializeWithLock<T>(ref T variable, Object lck, Func<T> factory)
			where T : class
		{
			Contract.Requires<ArgumentNullException>(lck != null, Resources.Chk_CannotBeNull);
			Contract.Requires<ArgumentNullException>(factory != null, Resources.Chk_CannotBeNull);

			if (variable == null)
			{
				lock (lck)
				{ // double-check the lock in case we're in a race...
					if (variable == null)
					{
						variable = factory();
					}
				}
			}
			return variable;
		}

		/// <summary>
		/// Initializes a variable if it doesn't already have a value. This method is
		/// thread-safe and non-blocking.
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		/// <param name="value">reference to the value</param>
		/// <param name="factory">function that creates the instance if it doesn't have a value</param>
		/// <returns>the instance</returns>
		public static T NonBlockingLazyInitializeVolatile<T>(ref T value, Func<T> factory)
			where T : class
		{
			Contract.Requires(factory != null, "factory cannot be null");

			// treat the reference as volatile...
			Thread.MemoryBarrier();
			T currentValue = value;
			Thread.MemoryBarrier();

			if (currentValue != null)
			{
				return currentValue;
			}

			T instanceCreatedByOtherThread = null;
			var isDisposable = typeof(IDisposable).IsAssignableFrom(typeof(T));
			T ourNewInstance = factory();
			try
			{
				instanceCreatedByOtherThread = Interlocked.CompareExchange(ref value, ourNewInstance, null);
				return (instanceCreatedByOtherThread != null) ? instanceCreatedByOtherThread : ourNewInstance;
			}
			finally
			{
				if (instanceCreatedByOtherThread != null && isDisposable)
				{
					((IDisposable)ourNewInstance).Dispose();
				}
			}
		}

		/// <summary>
		/// Initializes a variable if it doesn't already have a value. This method is
		/// thread-safe and non-blocking.
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		public static T NonBlockingLazyInitializeVolatile<T>(ref T variable)
			where T : class, new()
		{
			// treat the reference as volatile...
			Thread.MemoryBarrier();
			T currentValue = variable;
			Thread.MemoryBarrier();

			if (currentValue != null)
			{
				return currentValue;
			}

			T instanceCreatedByOtherThread = null;
			var isDisposable = typeof(IDisposable).IsAssignableFrom(typeof(T));
			T ourNewInstance = new T();
			try
			{
				instanceCreatedByOtherThread = Interlocked.CompareExchange(ref variable, ourNewInstance, null);
				return (instanceCreatedByOtherThread != null) ? instanceCreatedByOtherThread : ourNewInstance;
			}
			finally
			{
				if (instanceCreatedByOtherThread != null && isDisposable)
				{
					((IDisposable)ourNewInstance).Dispose();
				}
			}
		}

    /// <summary>
    /// Reads the referenced value after synchronizing all processors.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="reference"></param>
    /// <returns></returns>
    public static T VolatileRead<T>(ref T reference)
    {
      Thread.MemoryBarrier();
      T result = reference;
      Thread.MemoryBarrier();

      return result;
    }

    /// <summary>
    /// Writes a value to a reference and synchronizing all processors.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="reference"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static void VolatileWrite<T>(ref T reference, T value)
    {
      Thread.MemoryBarrier();
      reference = value;
      Thread.MemoryBarrier();
    }
	}

}
