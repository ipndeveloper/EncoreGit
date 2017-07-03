using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Couchbase.AspNet.SessionState;

namespace NetSteps.Web.Session.Couchbase
{
	public class NSCouchbaseSessionStateStoreProvider : NSSessionStateStoreProviderBase<CouchbaseSessionStateProvider>
	{
		#region Fields

		private CouchbaseSessionStateProvider _delegateProvider;

		#endregion

		#region Properties

		public override CouchbaseSessionStateProvider DelegateProvider
		{
			get
			{
				if (_delegateProvider == null)
				{
					_delegateProvider = new CouchbaseSessionStateProvider();
				}
				return _delegateProvider;
			}
		}

		#endregion
	}
}
