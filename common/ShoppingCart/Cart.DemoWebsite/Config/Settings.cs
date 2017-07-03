using NetSteps.Encore.Core.IoC;
using System.Web;
using System;
using System.Diagnostics.Contracts;

namespace Cart.DemoWebsite.Config
{
	public static class Settings
	{
		public static bool TestMode
		{
			get
			{
#if DEBUG
				return true;
#else
				return false;
#endif
			}
		}
	}
}