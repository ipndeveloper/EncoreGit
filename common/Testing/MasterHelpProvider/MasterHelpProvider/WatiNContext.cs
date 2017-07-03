using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;

namespace TestMasterHelpProvider
{
	public sealed class WatiNContext
	{
		#region Fields

		private static WatiNContext __watinContext;

		private IDictionary<string, object> _context;
		private Browser _browser;

		#endregion

		#region Properties

		/// <summary>
		/// Gets all of the keys stored in the context.
		/// </summary>
		public ICollection<string> Keys
		{
			get { return _context.Keys; }
		}

		/// <summary>
		/// Gets all of the values stored in the context,
		/// </summary>
		public ICollection<object> Values
		{
			get { return _context.Values; }
		}

		/// <summary>
		/// Gets an object from the context by its key-name. If the object does not exists per the key-name then null is returned.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public object this[string key]
		{
			get
			{
				object retVal = null;

				if (_context.ContainsKey(key))
				{
					retVal = _context[key];
				}

				return retVal;
			}
		}

		/// <summary>
		/// Gets or sets the Browser instance for the context.
		/// </summary>
		public Browser Browser
		{
			get { return _browser; }
			set { _browser = value; }
		}

		/// <summary>
		/// Gets a reference to the BrowserFactory object.
		/// </summary>
		public AbstractBrowserFactory BrowserFactory
		{
			get { return WatiNContext.AbstractBrowserFactory.GetInstance(); }
		}

		#endregion

		#region Constructors

		private WatiNContext()
		{
			_context = new Dictionary<string, object>();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets a reference to the context.
		/// </summary>
		/// <returns></returns>
		public static WatiNContext GetContext()
		{
			if (WatiNContext.__watinContext == null)
			{
				WatiNContext.__watinContext = new WatiNContext();
			}

			return WatiNContext.__watinContext;
		}

		/// <summary>
		/// Determines whether the context contains a certain key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool ContainsKey(string key)
		{
			return _context.ContainsKey(key);
		}

		/// <summary>
		/// Clears all items out of the context.
		/// </summary>
		public void Clear()
		{
			_context.Clear();
		}

		/// <summary>
		/// Adds a value to the context by key and value.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Add(string key, object value)
		{
			_context.Add(key, value);
		}

		/// <summary>
		/// Removes a an object by its key-name from the context.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Remove(string key)
		{
			return _context.Remove(key);
		}

		#endregion

		#region Browser Factory

		public sealed class AbstractBrowserFactory
		{
			#region Fields

			private static AbstractBrowserFactory __instance;
			private static Utils.BrowserType __browserType;

			#endregion

			#region Properties

			/// <summary>
			/// Gets or sets the type of browser that the BrowserFactory is currently producing.
			/// </summary>
			public static Utils.BrowserType BrowserType
			{
				get { return __browserType; }
				set { __browserType = value; }
			}

			#endregion

			#region Constructors

			/// <summary>
			/// Creates an instance of AbstractBrowserFactory and sets the browser type to Internet Explorer.
			/// </summary>
			private AbstractBrowserFactory()
			{
				__browserType = Utils.BrowserType.InternetExplorer;
			}

			#endregion

			#region Methods

			/// <summary>
			/// Gets a reference to AbstractBrowserFactory's instance.
			/// </summary>
			/// <returns></returns>
			public static AbstractBrowserFactory GetInstance()
			{
				if (AbstractBrowserFactory.__instance == null)
				{
					AbstractBrowserFactory.__instance = new AbstractBrowserFactory();
				}

				return AbstractBrowserFactory.__instance;
			}

			/// <summary>
			/// Creates a new instance of a browser as defined by the BrowserType property.
			/// Chrome and Undefined are supported as IE currently.
			/// </summary>
			/// <returns></returns>
			public Browser CreateBrowser()
			{
				Browser browser;

				if (AbstractBrowserFactory.__browserType == Utils.BrowserType.FireFox)
				{
					browser = new FireFox();
				}
				else
				{
					browser = new IE();
				}

				return browser;
			}

			/// <summary>
			/// Creates a borwser and browses to the URL defined.
			/// </summary>
			/// <param name="url"></param>
			/// <returns></returns>
			public Browser CreateBrowser(string url)
			{
				Browser browser = CreateBrowser();
				browser.GoTo(url);

				return browser;
			}

			/// <summary>
			/// Creates a borwser and browses to the URL defined.
			/// </summary>
			/// <param name="uri"></param>
			/// <returns></returns>
			public Browser CreateBrowser(Uri uri)
			{
				Browser browser = CreateBrowser();
				browser.GoTo(uri);

				return browser;
			}

			#endregion
		}

		#endregion
	}
}
