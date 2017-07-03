using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;

namespace TestMasterHelpProvider
{
	public class TextDataLoader
	{
		#region Fields

		private string _data;
		private Assembly _withEmbeddedResource;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the data loaded into this TextDataLoader.
		/// </summary>
		public string Data
		{
			get { return _data; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of TextDataLoader.
		/// </summary>
		public TextDataLoader()
		{
		}

		#endregion

		#region Methods

		/// <summary>
		/// Loads a CSV resource file.
		/// </summary>
		/// <param name="resourceLocation"></param>
		/// <param name="isEmbedded"></param>
		public void Load(string resourceLocation, bool isEmbedded = false, Assembly withEmbeddedResource = null)
		{
			_withEmbeddedResource = withEmbeddedResource;

			if (isEmbedded)
			{
				LoadEmbeddedResource(resourceLocation);
			}
			else
			{
				LoadExternalResource(resourceLocation);
			}
		}

		/// <summary>
		/// Loads an external resource.
		/// </summary>
		/// <param name="resourceLocation"></param>
		private void LoadExternalResource(string resourceLocation)
		{
			using (TextReader reader = new StreamReader(resourceLocation))
			{
				_data = reader.ReadToEnd();
			}
		}

		/// <summary>
		/// Loads an embedded resource.
		/// </summary>
		/// <param name="resourceLocation"></param>
		private void LoadEmbeddedResource(string resourceLocation)
		{
			if (_withEmbeddedResource == null)
			{
				_withEmbeddedResource = Assembly.GetExecutingAssembly();
			}

			using (TextReader reader = new StreamReader(_withEmbeddedResource.GetManifestResourceStream(resourceLocation)))
			{
				_data = reader.ReadToEnd();
			}
		}

		#endregion
	}
}
