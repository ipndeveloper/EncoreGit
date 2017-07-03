using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TestMasterHelpProvider
{
	public static class TestMasterHelpProviderConstants
	{
		#region Fields

		#region Military Postal Codes

		public static readonly Regex MilitaryPostalCodeRegex = new Regex("^(09[0-9]{3}|96[2-6][0-9]{2}|340[0-9]{2})$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

		#endregion

		#region Configuration Settings

        public const string CoreConnectionStringSettingName = "CoreConnectionString";
		public const string CommissionsConnectionStringSettingName = "CurrentCommissionsTestConnectionString";
		public const string RunTestsInTestModeSettingName = "RunTestsInTestMode";
		public const string DefaultEnvironmentSettingName = "DefaultEnvironment";

		#endregion

		public const string DefaultEmailAddress = "netstepshelpdesk@gmail.com";
		public const int DefaultWaitTime = 1500;
		public const string FullTimestampToStringPattern = "ddd, dd MMM yyyy HH:mm:ss";

		#endregion
	}
}
