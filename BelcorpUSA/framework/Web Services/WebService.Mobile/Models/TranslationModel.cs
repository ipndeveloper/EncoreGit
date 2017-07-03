using System.Collections.Generic;

namespace NetSteps.WebService.Mobile.Models
{
	public class TranslationModel
	{
		public Dictionary<string, string> at = new Dictionary<string, string>();
		public Dictionary<string, string> au = new Dictionary<string, string>();
		public Dictionary<string, string> cz = new Dictionary<string, string>();
		public Dictionary<string, string> dk = new Dictionary<string, string>();
		public Dictionary<string, string> fi = new Dictionary<string, string>();
		public Dictionary<string, string> fr = new Dictionary<string, string>();
		public Dictionary<string, string> de = new Dictionary<string, string>();
		public Dictionary<string, string> ie = new Dictionary<string, string>();
		public Dictionary<string, string> it = new Dictionary<string, string>();

		public List<string> countries = new List<string> {

		};

		public List<string> termNames = new List<string> { 
			"SignIn",
			"Username",
			"Password",
			"Disclaimer",
			"Oops",
			"Error",
			"InvalidLanguage",
			"InvalidCountry",
			"CouldntLogin",
			"UnableToConnect",
			"ServerError",
			"Yes",
			"No",
			"Ok",
			"NoRecordsFound",
			"ProblemCommunicating",
			"SignOut",
			"Home",
			"SignOutOfWorkstation",
			"SignOutConfirmation",
			"BrowserWarning",
			"Loading",
			"News",
			"Back" 
		};
	}
}