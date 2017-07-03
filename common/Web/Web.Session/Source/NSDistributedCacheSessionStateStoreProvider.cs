using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Microsoft.Web.DistributedCache;
using System.Web.Configuration;

namespace NetSteps.Web.Session
{
	public class NSDistributedCacheSessionStateStoreProvider : NSSessionStateStoreProviderBase<DistributedCacheSessionStateStoreProvider>
	{
		#region Fields

		private DistributedCacheSessionStateStoreProvider _delegateProvider;

		#endregion

		#region Properties

		public override DistributedCacheSessionStateStoreProvider DelegateProvider
		{
			get
			{
				if (_delegateProvider == null)
				{
					_delegateProvider = new DistributedCacheSessionStateStoreProvider();
				}
				return _delegateProvider;
			}
		}

		#endregion
	}
}
