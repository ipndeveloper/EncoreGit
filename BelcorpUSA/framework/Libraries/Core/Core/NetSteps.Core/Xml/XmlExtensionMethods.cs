using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using NetSteps.Encore.Core.Properties;

namespace NetSteps.Encore.Core.Xml
{
	/// <summary>
	/// Extensions for working with XElement and XML
	/// </summary>
	public static class XElementExtensions
	{
		/// <summary>
		/// Converts a string into a dynamic XML object.
		/// </summary>
		/// <param name="text">the source xml</param>
		/// <returns>a dynamic object shaped according to the input xml</returns>
		public static dynamic XmlToDynamic(this string text)
		{
			return XDynamic.Parse(text);
		}

		/// <summary>
		/// Converts an XElement into a dynamic XML object.
		/// </summary>
		/// <param name="xml">the source xml element</param>
		/// <returns>a dynamic object shaped according to the input xml</returns>
		public static dynamic ToDynamic(this XElement xml)
		{
			return XDynamic.ToDynamic(xml);
		}

		/// <summary>
		/// Reads a named value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static bool ReadBooleanOrDefault(this XElement element, string name)
		{
			bool value;
			ReadNamedValueOrDefault(element, name, out value);
			return value;
		}
		/// <summary>
		/// Reads a named byte value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static byte ReadByteOrDefault(this XElement element, string name)
		{
			byte value;
			ReadNamedValueOrDefault(element, name, out value);
			return value;
		}
		/// <summary>
		/// Reads a named byte array value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static byte[] ReadBytesOrDefault(this XElement element, string name)
		{
			byte[] value;
			ReadNamedValueOrDefault(element, name, out value);
			return value;
		}
		/// <summary>
		/// Reads a named char value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static char ReadCharOrDefault(this XElement element, string name)
		{
			char value;
			ReadNamedValueOrDefault(element, name, out value);
			return value;
		}
		/// <summary>
		/// Reads a named Int16 value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static short ReadInt16OrDefault(this XElement element, string name)
		{
			short value;
			ReadNamedValueOrDefault(element, name, out value);
			return value;
		}
		/// <summary>
		/// Reads a named Int32 value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static int ReadInt32OrDefault(this XElement element, string name)
		{
			int value;
			ReadNamedValueOrDefault(element, name, out value);
			return value;
		}
		/// <summary>
		/// Reads a named Int64 value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static long ReadInt64OrDefault(this XElement element, string name)
		{
			long value;
			ReadNamedValueOrDefault(element, name, out value);
			return value;
		}
		/// <summary>
		/// Reads a named decimal value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static decimal ReadDecimalOrDefault(this XElement element, string name)
		{
			decimal value;
			ReadNamedValueOrDefault(element, name, out value);
			return value;
		}
		/// <summary>
		/// Reads a named double value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static double ReadDoubleOrDefault(this XElement element, string name)
		{
			double value;
			ReadNamedValueOrDefault(element, name, out value);
			return value;
		}
		/// <summary>
		/// Reads a named single value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static float ReadSingleOrDefault(this XElement element, string name)
		{
			float value;
			ReadNamedValueOrDefault(element, name, out value);
			return value;
		}
		/// <summary>
		/// Reads a named SByte value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <returns>the value</returns>
		[CLSCompliant(false)]
		public static sbyte ReadSByteOrDefault(this XElement element, string name)
		{
			sbyte value;
			ReadNamedValueOrDefault(element, name, out value);
			return value;
		}
		/// <summary>
		/// Reads a named UInt16 value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <returns>the value</returns>
		[CLSCompliant(false)]
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static ushort ReadUInt16OrDefault(this XElement element, string name)
		{
			ushort value;
			ReadNamedValueOrDefault(element, name, out value);
			return value;
		}
		/// <summary>
		/// Reads a named UInt32 value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <returns>the value</returns>
		[CLSCompliant(false)]
		public static uint ReadUInt32OrDefault(this XElement element, string name)
		{
			uint value;
			ReadNamedValueOrDefault(element, name, out value);
			return value;
		}
		/// <summary>
		/// Reads a named UInt64 value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <returns>the value</returns>
		[CLSCompliant(false)]
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static ulong ReadUInt64OrDefault(this XElement element, string name)
		{
			ulong value;
			ReadNamedValueOrDefault(element, name, out value);
			return value;
		}
		/// <summary>
		/// Reads a named Guid value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static Guid ReadGuidOrDefault(this XElement element, string name)
		{
			Guid value;
			ReadNamedValueOrDefault(element, name, out value);
			return value;
		}
		/// <summary>
		/// Reads a named String value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static string ReadStringOrDefault(this XElement element, string name)
		{
			string value;
			ReadNamedValueOrDefault(element, name, out value);
			return value;
		}

		/// <summary>
		/// Tries to read a named value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success</param>
		/// <returns><em>true</em> if successful; otherwise <em>false</em></returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static bool TryReadNamedValue(this XElement element, string name, out bool value)
		{
			Contract.Requires(element != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name.Length > 0, Resources.Chk_CannotBeNull);

			if (element.Attribute(name) != null && element.Attribute(name).Value.Length > 0)
			{
				value = (bool)element.Attribute(name);
				return true;
			}
			if (element.Element(name) != null && element.Element(name).Value.Length > 0)
			{
				value = (bool)element.Element(name);
				return true;
			}
			value = default(bool);
			return false;
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out bool value)
		{
			TryReadNamedValue(element, name, out value);
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out bool value, bool defa)
		{
			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa;
			}
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out bool value, Func<bool> defa)
		{
			Contract.Requires(defa != null);

			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa();
			}
		}

		/// <summary>
		/// Tries to read a named value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success</param>
		/// <returns><em>true</em> if successful; otherwise <em>false</em></returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static bool TryReadNamedValue(this XElement element, string name, out byte value)
		{
			Contract.Requires(element != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name.Length > 0, Resources.Chk_CannotBeNull);

			if (element.Attribute(name) != null && element.Attribute(name).Value.Length > 0)
			{
				value = Convert.ToByte((int)element.Attribute(name));
				return true;
			}
			if (element.Element(name) != null && element.Element(name).Value.Length > 0)
			{
				value = Convert.ToByte((int)element.Element(name));
				return true;
			}
			value = default(byte);
			return false;
		}

		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out byte value)
		{
			TryReadNamedValue(element, name, out value);
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out byte value, byte defa)
		{
			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa;
			}
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out byte value, Func<byte> defa)
		{

			Contract.Requires(defa != null);

			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa();
			}
		}
		/// <summary>
		/// Tries to read a named value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success</param>
		/// <returns><em>true</em> if successful; otherwise <em>false</em></returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static bool TryReadNamedValue(this XElement element, string name, out char value)
		{
			Contract.Requires(element != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name.Length > 0, Resources.Chk_CannotBeNull);

			if (element.Attribute(name) != null && element.Attribute(name).Value.Length > 0)
			{
				value = Convert.ToChar((int)element.Attribute(name));
				return true;
			}
			if (element.Element(name) != null && element.Element(name).Value.Length > 0)
			{
				value = Convert.ToChar((int)element.Element(name));
				return true;
			}
			value = default(char);
			return false;
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out char value)
		{
			TryReadNamedValue(element, name, out value);
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out char value, char defa)
		{
			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa;
			}
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out char value, Func<char> defa)
		{
			Contract.Requires(defa != null);

			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa();
			}
		}
		/// <summary>
		/// Tries to read a named value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success</param>
		/// <returns><em>true</em> if successful; otherwise <em>false</em></returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static bool TryReadNamedValue(this XElement element, string name, out DateTime value)
		{
			Contract.Requires(element != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name.Length > 0, Resources.Chk_CannotBeNull);

			if (element.Attribute(name) != null && element.Attribute(name).Value.Length > 0)
			{
				value = (DateTime)element.Attribute(name);
				return true;
			}
			if (element.Element(name) != null && element.Element(name).Value.Length > 0)
			{
				value = (DateTime)element.Element(name);
				return true;
			}
			value = default(DateTime);
			return false;
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out DateTime value)
		{
			TryReadNamedValue(element, name, out value);
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out DateTime value, DateTime defa)
		{
			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa;
			}
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out DateTime value, Func<DateTime> defa)
		{
			Contract.Requires(defa != null);

			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa();
			}
		}
		/// <summary>
		/// Tries to read a named value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success</param>
		/// <returns><em>true</em> if successful; otherwise <em>false</em></returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static bool TryReadNamedValue(this XElement element, string name, out decimal value)
		{
			Contract.Requires(element != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name.Length > 0, Resources.Chk_CannotBeNull);

			if (element.Attribute(name) != null && element.Attribute(name).Value.Length > 0)
			{
				value = (decimal)element.Attribute(name);
				return true;
			}
			if (element.Element(name) != null && element.Element(name).Value.Length > 0)
			{
				value = (decimal)element.Element(name);
				return true;
			}
			value = default(decimal);
			return false;
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out decimal value)
		{
			TryReadNamedValue(element, name, out value);
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out decimal value, decimal defa)
		{
			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa;
			}
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out decimal value, Func<decimal> defa)
		{
			Contract.Requires(defa != null);

			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa();
			}
		}
		/// <summary>
		/// Tries to read a named value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success</param>
		/// <returns><em>true</em> if successful; otherwise <em>false</em></returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static bool TryReadNamedValue(this XElement element, string name, out double value)
		{
			Contract.Requires(element != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name.Length > 0, Resources.Chk_CannotBeNull);

			if (element.Attribute(name) != null && element.Attribute(name).Value.Length > 0)
			{
				value = (double)element.Attribute(name);
				return true;
			}
			if (element.Element(name) != null && element.Element(name).Value.Length > 0)
			{
				value = (double)element.Element(name);
				return true;
			}
			value = default(double);
			return false;
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out double value)
		{
			TryReadNamedValue(element, name, out value);
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out double value, double defa)
		{
			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa;
			}
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out double value, Func<double> defa)
		{
			Contract.Requires(defa != null);

			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa();
			}
		}
		/// <summary>
		/// Tries to read a named value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success</param>
		/// <returns><em>true</em> if successful; otherwise <em>false</em></returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static bool TryReadNamedValue(this XElement element, string name, out short value)
		{
			Contract.Requires(element != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name.Length > 0, Resources.Chk_CannotBeNull);

			if (element.Attribute(name) != null && element.Attribute(name).Value.Length > 0)
			{
				value = (short)element.Attribute(name);
				return true;
			}
			if (element.Element(name) != null && element.Element(name).Value.Length > 0)
			{
				value = (short)element.Element(name);
				return true;
			}
			value = default(short);
			return false;
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out short value)
		{
			TryReadNamedValue(element, name, out value);
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out short value, short defa)
		{
			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa;
			}
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out short value, Func<short> defa)
		{
			Contract.Requires(defa != null);

			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa();
			}
		}
		/// <summary>
		/// Tries to read a named value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success</param>
		/// <returns><em>true</em> if successful; otherwise <em>false</em></returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static bool TryReadNamedValue(this XElement element, string name, out int value)
		{
			Contract.Requires(element != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name.Length > 0, Resources.Chk_CannotBeNull);

			if (element.Attribute(name) != null && element.Attribute(name).Value.Length > 0)
			{
				value = (int)element.Attribute(name);
				return true;
			}
			if (element.Element(name) != null && element.Element(name).Value.Length > 0)
			{
				value = (int)element.Element(name);
				return true;
			}
			value = default(int);
			return false;
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out int value)
		{
			TryReadNamedValue(element, name, out value);
		}

		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out int value, int defa)
		{
			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa;
			}
		}

		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out int value, Func<int> defa)
		{
			Contract.Requires(defa != null);

			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa();
			}
		}
		/// <summary>
		/// Tries to read a named value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success</param>
		/// <returns><em>true</em> if successful; otherwise <em>false</em></returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static bool TryReadNamedValue(this XElement element, string name, out long value)
		{
			Contract.Requires(element != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name.Length > 0, Resources.Chk_CannotBeNull);

			if (element.Attribute(name) != null && element.Attribute(name).Value.Length > 0)
			{
				value = (long)element.Attribute(name);
				return true;
			}
			if (element.Element(name) != null && element.Element(name).Value.Length > 0)
			{
				value = (long)element.Element(name);
				return true;
			}
			value = default(long);
			return false;
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out long value)
		{
			TryReadNamedValue(element, name, out value);
		}

		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out long value, long defa)
		{
			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa;
			}
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out long value, Func<long> defa)
		{
			Contract.Requires(defa != null);

			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa();
			}
		}

		/// <summary>
		/// Tries to read a named value from an xml container.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success</param>
		/// <returns><em>true</em> if successful; otherwise <em>false</em></returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static bool TryReadNamedValue<T>(this XContainer element, string name, out T value)
		{
			Contract.Requires(element != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name.Length > 0, Resources.Chk_CannotBeNull);

			if (element.Element(name) != null && element.Element(name).Value.Length > 0)
			{
				//value = DataTransfer.FromXml<T>(element.Element(name));
				value = default(T);
				return true;
			}
			value = default(T);
			return false;
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault<T>(this XElement element, string name, out T value)
		{
			TryReadNamedValue(element, name, out value);
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault<T>(this XElement element, string name, out T value, T defa)
		{
			if (!TryReadNamedValue<T>(element, name, out value))
			{
				value = defa;
			}
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault<T>(this XElement element, string name, out T value, Func<T> defa)
		{
			Contract.Requires(defa != null);

			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa();
			}
		}

		/// <summary>
		/// Tries to read a named value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success</param>
		/// <returns><em>true</em> if successful; otherwise <em>false</em></returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static bool TryReadNamedValueAsEnum<T>(this XElement element, string name, out T value)
		{
			Contract.Requires(element != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name.Length > 0, Resources.Chk_CannotBeNull);
			Contract.Requires(typeof(T).IsEnum, "typeof(T) must be an enum");

			if (element.Attribute(name) != null && element.Attribute(name).Value.Length > 0)
			{
				string v = (string)element.Attribute(name);
				if (Enum.IsDefined(typeof(T), v))
				{
					value = (T)Enum.Parse(typeof(T), v);
					return true;
				}
			}
			else if (element.Element(name) != null && element.Element(name).Value.Length > 0)
			{
				string v = (string)element.Element(name);
				if (Enum.IsDefined(typeof(T), v))
				{
					value = (T)Enum.Parse(typeof(T), v);
					return true;
				}
			}
			value = default(T);
			return false;
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefaultAsEnum<T>(this XElement element, string name, out T value)
		{
			TryReadNamedValueAsEnum(element, name, out value);
		}

		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefaultAsEnum<T>(this XElement element, string name, out T value, T defa)
		{
			if (!TryReadNamedValueAsEnum<T>(element, name, out value))
			{
				value = defa;
			}
		}

		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefaultAsEnum<T>(this XElement element, string name, out T value, Func<T> defa)
		{
			Contract.Requires(defa != null);

			if (!TryReadNamedValueAsEnum(element, name, out value))
			{
				value = defa();
			}
		}

		/// <summary>
		/// Tries to read a named value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success</param>
		/// <returns><em>true</em> if successful; otherwise <em>false</em></returns>
		[CLSCompliant(false)]
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static bool TryReadNamedValue(this XElement element, string name, out sbyte value)
		{
			Contract.Requires(element != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name.Length > 0, Resources.Chk_CannotBeNull);

			if (element.Attribute(name) != null && element.Attribute(name).Value.Length > 0)
			{
				value = (sbyte)element.Attribute(name);
				return true;
			}
			if (element.Element(name) != null && element.Element(name).Value.Length > 0)
			{
				value = (sbyte)element.Element(name);
				return true;
			}
			value = default(sbyte);
			return false;
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		[CLSCompliant(false)]
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out sbyte value)
		{
			TryReadNamedValue(element, name, out value);
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[CLSCompliant(false)]
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out sbyte value, sbyte defa)
		{
			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa;
			}
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[CLSCompliant(false)]
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out sbyte value, Func<sbyte> defa)
		{
			Contract.Requires(defa != null);

			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa();
			}
		}
		/// <summary>
		/// Tries to read a named value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success</param>
		/// <returns><em>true</em> if successful; otherwise <em>false</em></returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static bool TryReadNamedValue(this XElement element, string name, out float value)
		{
			Contract.Requires(element != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name.Length > 0, Resources.Chk_CannotBeNull);

			if (element.Attribute(name) != null && element.Attribute(name).Value.Length > 0)
			{
				value = (Single)element.Attribute(name);
				return true;
			}
			if (element.Element(name) != null && element.Element(name).Value.Length > 0)
			{
				value = (Single)element.Element(name);
				return true;
			}
			value = default(Single);
			return false;
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out float value)
		{
			TryReadNamedValue(element, name, out value);
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out float value, float defa)
		{
			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa;
			}
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out float value, Func<float> defa)
		{
			Contract.Requires(defa != null);

			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa();
			}
		}
		/// <summary>
		/// Tries to read a named value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success</param>
		/// <returns><em>true</em> if successful; otherwise <em>false</em></returns>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static bool TryReadNamedValue(this XElement element, string name, out string value)
		{
			Contract.Requires(element != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name.Length > 0, Resources.Chk_CannotBeNull);

			if (element.Attribute(name) != null && element.Attribute(name).Value.Length > 0)
			{
				value = element.Attribute(name).Value;
				return true;
			}
			if (element.Element(name) != null && element.Element(name).Value.Length > 0)
			{
				value = element.Element(name).Value;
				return true;
			}
			value = default(String);
			return false;
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out string value)
		{
			TryReadNamedValue(element, name, out value);
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out string value, string defa)
		{
			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa;
			}
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out string value, Func<string> defa)
		{
			Contract.Requires(defa != null);

			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa();
			}
		}
		/// <summary>
		/// Tries to read a named value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success</param>
		/// <returns><em>true</em> if successful; otherwise <em>false</em></returns>
		[CLSCompliant(false)]
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static bool TryReadNamedValue(this XElement element, string name, out ushort value)
		{
			Contract.Requires(element != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name.Length > 0, Resources.Chk_CannotBeNull);

			if (element.Attribute(name) != null && element.Attribute(name).Value.Length > 0)
			{
				value = Convert.ToUInt16((uint)element.Attribute(name));
				return true;
			}
			if (element.Element(name) != null && element.Element(name).Value.Length > 0)
			{
				value = Convert.ToUInt16((uint)element.Element(name));
				return true;
			}
			value = default(ushort);
			return false;
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		[CLSCompliant(false)]
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out ushort value)
		{
			TryReadNamedValue(element, name, out value);
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[CLSCompliant(false)]
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out ushort value, ushort defa)
		{
			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa;
			}
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[CLSCompliant(false)]
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out ushort value, Func<ushort> defa)
		{
			Contract.Requires(defa != null);

			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa();
			}
		}
		/// <summary>
		/// Tries to read a named value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success</param>
		/// <returns><em>true</em> if successful; otherwise <em>false</em></returns>
		[CLSCompliant(false)]
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static bool TryReadNamedValue(this XElement element, string name, out uint value)
		{
			Contract.Requires(element != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name.Length > 0, Resources.Chk_CannotBeNull);

			if (element.Attribute(name) != null && element.Attribute(name).Value.Length > 0)
			{
				value = (uint)element.Attribute(name);
				return true;
			}
			if (element.Element(name) != null && element.Element(name).Value.Length > 0)
			{
				value = (uint)element.Element(name);
				return true;
			}
			value = default(uint);
			return false;
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		[CLSCompliant(false)]
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out uint value)
		{
			TryReadNamedValue(element, name, out value);
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[CLSCompliant(false)]
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out uint value, uint defa)
		{
			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa;
			}
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[CLSCompliant(false)]
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out uint value, Func<uint> defa)
		{
			Contract.Requires(defa != null);

			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa();
			}
		}
		/// <summary>
		/// Tries to read a named value from an xml element.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success</param>
		/// <returns><em>true</em> if successful; otherwise <em>false</em></returns>
		[CLSCompliant(false)]
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static bool TryReadNamedValue(this XElement element, string name, out ulong value)
		{
			Contract.Requires(element != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name != null, Resources.Chk_CannotBeNull);
			Contract.Requires(name.Length > 0, Resources.Chk_CannotBeNull);

			if (element.Attribute(name) != null && element.Attribute(name).Value.Length > 0)
			{
				value = (ulong)element.Attribute(name);
				return true;
			}
			if (element.Element(name) != null && element.Element(name).Value.Length > 0)
			{
				value = (ulong)element.Element(name);
				return true;
			}
			value = default(ulong);
			return false;
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		[CLSCompliant(false)]
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out ulong value)
		{
			TryReadNamedValue(element, name, out value);
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[CLSCompliant(false)]
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out ulong value, ulong defa)
		{
			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa;
			}
		}
		/// <summary>
		/// Read a named value from an xml element; if the value doesn't exist, value is set to
		/// the default given.
		/// </summary>
		/// <param name="element">element</param>
		/// <param name="name">name</param>
		/// <param name="value">reference to a variable that will receive the value upon success.</param>
		/// <param name="defa">default value used if no value is present on the element</param>
		[CLSCompliant(false)]
		[SuppressMessage("Microsoft.Design", "CA1011", Justification = "By design.")]
		public static void ReadNamedValueOrDefault(this XElement element, string name, out ulong value, Func<ulong> defa)
		{
			Contract.Requires(defa != null);

			if (!TryReadNamedValue(element, name, out value))
			{
				value = defa();
			}
		}


	}
}
