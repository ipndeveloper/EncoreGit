using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;
using WatiN.Core.Native;
using System.Xml.Linq;

namespace WatiN.Core.Extras
{
	/// <summary>
	/// This class represents CKEditor. Use Document.CkEditor to instantiate it.
	/// </summary>
	public class CkEditor
	{
		#region Fields

		private Frame _rootFrame;
		private Frame _childFrame;
		private Para _bodyPara;
		private string _paraId;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of CkEditorControl.
		/// </summary>
		/// <param name="rootFrame"></param>
		public CkEditor(Frame rootFrame)
		{
			_rootFrame = rootFrame;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Sets the inner HTML of CKEditor.
		/// </summary>
		/// <param name="html"></param>
		public void SetHtml(string html)
		{
			EnsureComponentsInitialized();
			RunScriptToSetHtml(html);
		}

		/// <summary>
		/// Sets the inner HTML of CKEditor.
		/// </summary>
		/// <param name="linqXmlDocument"></param>
		public void SetHtml(XDocument linqXmlDocument)
		{
			EnsureComponentsInitialized();
			RunScriptToSetHtml(linqXmlDocument.ToString(SaveOptions.DisableFormatting));
		}

		/// <summary>
		/// Ensures that all of the required components are initialized.
		/// </summary>
		private void EnsureComponentsInitialized()
		{
			if (_bodyPara == null || _childFrame == null)
			{
				_paraId = String.Format("para{0}", base.GetHashCode());

				_childFrame = _rootFrame.Frame(Find.ByIndex(0));

				_bodyPara = _childFrame.Para(Find.ByIndex(0));
				_bodyPara.SetAttributeValue("id", _paraId);
			}
		}

		/// <summary>
		/// Runs the script to set the HTML of CKEditor.
		/// </summary>
		/// <param name="html"></param>
		private void RunScriptToSetHtml(string html)
		{
			string script = String.Format("document.getElementById(\"{0}\").innerHTML = \"{1}\";", _paraId, html);

			_childFrame.RunScript(script);
		}

		#endregion
	}
}
