using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using System.Web;
using System.Configuration;
using System.Web.Configuration;
using System.IO;
using System.Security.Permissions;
using System.Runtime.Serialization;
using NetSteps.Diagnostics.Utilities;
using System.Xml;

namespace NetSteps.Web.Session
{
	public abstract class NSSessionStateStoreProviderBase<T> : SessionStateStoreProviderBase
		where T : SessionStateStoreProviderBase, new()
	{
		#region Fields

		private const string __ignoreConfigKey = "ignoredExtensions";
		private static string[] __defaultIgnore = new string[] { "js", "jpg", "gif", "png", "jpeg", "css" };
		private const string __traceOutputKey = "traceOutput";

		#endregion

		#region Properties

		public abstract T DelegateProvider
		{
			get;
		}

		private string[] IgnoredExtensions { get; set; }
		private int Timeout { get; set; }
		private string TraceOutput { get; set; }

		#endregion

		#region Methods

		public override SessionStateStoreData CreateNewStoreData(HttpContext context, int timeout)
		{
			return DelegateProvider.CreateNewStoreData(context, timeout);
		}

		public override void CreateUninitializedItem(HttpContext context, string id, int timeout)
		{
			DelegateProvider.CreateUninitializedItem(context, id, timeout);
		}

		public override void Dispose()
		{
			DelegateProvider.Dispose();
		}

		public override void EndRequest(HttpContext context)
		{
			if (ShouldShortCircuit(context)) return;
			DelegateProvider.EndRequest(context);
		}

		public override SessionStateStoreData GetItem(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
		{
			if (ShouldShortCircuit(context))
			{
				locked = false;
				lockAge = TimeSpan.MinValue;
				lockId = new InternalLockId();
				actions = SessionStateActions.None;

				return new SessionStateStoreData(new SessionStateItemCollection(), SessionStateUtility.GetSessionStaticObjects(context), Timeout);
			}

			return DelegateProvider.GetItem(context, id, out locked, out lockAge, out lockId, out actions);
		}

		public override SessionStateStoreData GetItemExclusive(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
		{
			if (ShouldShortCircuit(context))
			{
				locked = true;
				lockAge = TimeSpan.MinValue;
				lockId = new InternalLockId();
				actions = SessionStateActions.None;

				return new SessionStateStoreData(new SessionStateItemCollection(), SessionStateUtility.GetSessionStaticObjects(context), Timeout);
			}

			return DelegateProvider.GetItemExclusive(context, id, out locked, out lockAge, out lockId, out actions);
		}

		public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
		{
			base.Initialize(name, config);

			SessionStateSection sessionConfig = ConfigurationManager.GetSection("system.web/sessionState") as SessionStateSection;
			if (sessionConfig != null)
			{
				Timeout = (int)Math.Floor(sessionConfig.Timeout.TotalMinutes);
			}
			else
			{
				Timeout = 20;
			}

			if (config != null)
			{
				string configIgnored = config[__ignoreConfigKey];
				if (configIgnored != null)
				{
					config.Remove(__ignoreConfigKey);
					string[] ignoredConfig = configIgnored.Split(",;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
					IgnoredExtensions = ignoredConfig;
				}
				else
				{
					IgnoredExtensions = __defaultIgnore;
				}
				string configTraceOutput = config[__traceOutputKey];
				if (configTraceOutput != null && Directory.Exists(configTraceOutput)) TraceOutput = configTraceOutput;
				this.TraceInformation(String.Format("Initializing with:\r\nIgnore: {0}\r\nTraceOutput: {1}", configIgnored, configTraceOutput));
			}

			DelegateProvider.Initialize(name, config);
		}

		public override void InitializeRequest(HttpContext context)
		{
			if (ShouldShortCircuit(context)) return;

			DelegateProvider.InitializeRequest(context);
		}

		public override void ReleaseItemExclusive(HttpContext context, string id, object lockId)
		{
			if (lockId is InternalLockId) return;

			DelegateProvider.ReleaseItemExclusive(context, id, lockId);
		}

		public override void RemoveItem(HttpContext context, string id, object lockId, SessionStateStoreData item)
		{
			DelegateProvider.RemoveItem(context, id, lockId, item);
		}

		public override void ResetItemTimeout(HttpContext context, string id)
		{
			if (ShouldShortCircuit(context)) return;
			DelegateProvider.ResetItemTimeout(context, id);
		}

		public override void SetAndReleaseItemExclusive(HttpContext context, string id, SessionStateStoreData item, object lockId, bool newItem)
		{
			if (lockId is InternalLockId) return;
			if (TraceOutput != null)
			{
				try
				{
					using (FileStream stream = File.Create(Path.Combine(TraceOutput, String.Concat(id, ".set.xml"))))
					{
						XmlTextWriter writer = new XmlTextWriter(stream,Encoding.UTF8);
						writer.Formatting = Formatting.Indented;
						writer.Indentation = 1;
						writer.IndentChar = '\t';
						StreamingContext ctx = new StreamingContext(StreamingContextStates.All);				
						foreach (string key in item.Items.Keys) new NetDataContractSerializer(ctx).WriteObject(writer, item.Items[key]);
						writer.Flush();
						stream.Flush();
						stream.Close();
					}
				}
				catch (Exception exception)
				{
					this.TraceException(exception);
				}
			}
			DelegateProvider.SetAndReleaseItemExclusive(context, id, item, lockId, newItem);
		}

		public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback)
		{
			return DelegateProvider.SetItemExpireCallback(expireCallback);
		}

		private bool ShouldShortCircuit(HttpContext context)
		{
			string currentExt = context.Request.CurrentExecutionFilePathExtension;
			if (!String.IsNullOrWhiteSpace(currentExt) && IgnoredExtensions.Any())
			{
				currentExt = currentExt.TrimStart('.');
				return IgnoredExtensions.Contains(currentExt);
			}
			return false;
		}

		#endregion

		#region Internals

		struct InternalLockId
		{
			int Id;
		}

		#endregion
	}
}
