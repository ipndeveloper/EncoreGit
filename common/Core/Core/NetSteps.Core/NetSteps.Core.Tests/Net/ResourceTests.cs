using System;
using System.Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.Net;
using Newtonsoft.Json;

namespace NetSteps.Encore.Core.Tests.Net
{
	[TestClass]
	public class ResourceTests
	{
		[TestMethod]
		public void HttpGetDynamic_CanGetJsonFromWellKnownResource()
		{
			var hollywood = new Uri("http://search.twitter.com/search.json?q=hollywood");
			var supreme = new Uri("http://search.twitter.com/search.json?q=supreme%20court");
			var erlang = new Uri("http://search.twitter.com/search.json?q=erlang");

			var h = hollywood.ParallelGet(res => res.DeserializeResponseAsDynamic()).Continue((e, d) =>
			{
				foreach (var tweet in d.results)
				{
					Console.WriteLine(String.Concat(tweet.from_user_name, " says: ", tweet.text));
				}
			});
			Assert.IsFalse(h.IsCompleted);
			Assert.IsFalse(h.IsFaulted);

			var s = supreme.ParallelGet(res => res.DeserializeResponseAsDynamic()).Continue((e, d) =>
			{
				foreach (var tweet in d.results)
				{
					Console.WriteLine(String.Concat(tweet.from_user_name, " says: ", tweet.text));
				}
			});
			Assert.IsFalse(s.IsCompleted);
			Assert.IsFalse(s.IsFaulted);

			var erl = erlang.ParallelGet(res => res.DeserializeResponseAsDynamic()).Continue((e, d) =>
			{
				foreach (var tweet in d.results)
				{
					Console.WriteLine(String.Concat(tweet.from_user_name, " says: ", tweet.text));
				}
			});
			Assert.IsFalse(erl.IsCompleted);
			Assert.IsFalse(erl.IsFaulted);

			h.Wait(TimeSpan.FromSeconds(5));
			s.Wait(TimeSpan.FromSeconds(5));
			erl.Wait(TimeSpan.FromSeconds(5));

			// In MTA I'd rather:
			// 
			// var waitHandles = new WaitHandle[] { h.ToAsyncResult().AsyncWaitHandle, s.ToAsyncResult().AsyncWaitHandle, erl.ToAsyncResult().AsyncWaitHandle };
			// WaitHandle.WaitAll(waitHandles);
		}

		[TestMethod]
		public void HttpGetDynamic_Sequential()
		{
			GetTweetsAndPrint(new Uri("http://search.twitter.com/search.json?q=hollywood"));
			GetTweetsAndPrint(new Uri("http://search.twitter.com/search.json?q=supreme%20court"));
			GetTweetsAndPrint(new Uri("http://search.twitter.com/search.json?q=erlang"));
		}

		private void GetTweetsAndPrint(Uri uri)
		{
			var request = uri.MakeResourceRequest();
			request.HttpGet((e, r) => {
				var dyn = r.DeserializeResponseAsDynamic();
				foreach (var tweet in dyn.results)
				{
					Console.WriteLine(String.Concat(tweet.from_user_name, " says: ", tweet.text));
				}
			});
			
		}

		static HttpWebRequest AttachTestCredentialsUsingBasicAuth(HttpWebRequest req)
		{
			return req.WithBasicAuth("nedlyincessicturandstolu", "bnoIEkLiuSLyHobSaOxcUL4o");
		}

		[TestMethod]
		public void HttpGetDynamic_CanCallApiRequiringBasicAuth()
		{
			// This resource is a Couch DB...
			var db = "https://flitbit.cloudant.com/trubl";
			var testdb = new Uri(String.Concat(db, "/_all_docs?include_docs=true"));
			Exception unexpected = null;

			// Create request and associate creds using basic auth...
			var req = AttachTestCredentialsUsingBasicAuth(testdb.MakeResourceRequest());

			var completion = req.ParallelGet(res => res.DeserializeResponseAsDynamic())
				.Continue((e, d) =>
					{
						if ((unexpected = e) == null)
						{
							try
							{
								if (d.total_rows > 0)
								{
									foreach (var row in d.rows)
									{
										var value = row.doc;
										Console.WriteLine(String.Concat("timestamp: ", value.timestamp, ", id: ", row.id, ", info: ", value.info, ", machine_name: ", value.machine_name));
									}
								}
							}
							catch (Exception assertionFailure)
							{
								unexpected = assertionFailure;
							}
						}
					});
			completion.Wait(TimeSpan.FromSeconds(5));
			Assert.IsTrue(completion.IsCompleted);

			Assert.IsNull(unexpected);
		}

		[TestMethod]
		public void HttpGetDynamic_CanPerformHttpPutAgainstApiRequiringBasicAuth()
		{
			// This resource is a Couch DB...
			var dataGen = new DataGenerator();

			var docid = Guid.NewGuid().ToString("N");
			var data = new { timestamp = DateTime.UtcNow, info = dataGen.GetString(80), machine_name = Environment.MachineName };

			var testdb = new Uri(String.Concat("https://flitbit.cloudant.com/trubl/", docid));
			Exception unexpected = null;

			var req = AttachTestCredentialsUsingBasicAuth(testdb.MakeResourceRequest());

			var json = JsonConvert.SerializeObject(data);
			var postBody = Encoding.UTF8.GetBytes(json);

			var completion = req.ParallelPut(postBody, "application/json", res => res.DeserializeResponseAsDynamic())
				.Continue((e, d) =>
					{
						if ((unexpected = e) == null)
						{
							try
							{
								Assert.IsTrue(d.ok);
								Assert.AreEqual(docid, d.id);
							}
							catch (Exception assertionFailure)
							{
								unexpected = assertionFailure;
							}
						}
					});
			completion.Wait(TimeSpan.FromSeconds(5));
			Assert.IsTrue(completion.IsCompleted);

			Assert.IsNull(unexpected);
		}

		[TestMethod]
		public void HttpGetDynamic_CanPerformHttpDelete()
		{
			// This resource is a Couch DB...
			var db = "https://flitbit.cloudant.com/trubl";
			var testdb = new Uri(String.Concat(db, "/_all_docs"));
					
			Exception unexpected = null;

			var req = AttachTestCredentialsUsingBasicAuth(testdb.MakeResourceRequest());

			var completion = req.ParallelGet(res => res.DeserializeResponseAsDynamic())
				.Continue((e, d) =>
				{
					if ((unexpected = e) == null)
					{
						try
						{
							if (d.total_rows > 0)
							{
								foreach (var row in d.rows)
								{
									var doc = new Uri(String.Concat(db, "/", row.id, "?rev=", row.value.rev));
									var delReq = AttachTestCredentialsUsingBasicAuth(doc.MakeResourceRequest());

									var deleteResult = delReq.ParallelDelete(res => res.DeserializeResponseAsDynamic())
										.Continue((ee, dd) =>
										{
											if ((unexpected = ee) == null)
											{
												try
												{
													Assert.IsTrue(dd.ok);
												}
												catch (Exception assertionFailure)
												{
													unexpected = assertionFailure;
												}
											}
										});
									deleteResult.Wait(TimeSpan.FromSeconds(5));
									Assert.IsTrue(deleteResult.IsCompleted);
									break;
								}
							}
						}
						catch (Exception assertionFailure)
						{
							unexpected = assertionFailure;
						}
					}
				});
			completion.Wait(TimeSpan.FromSeconds(10));
			Assert.IsTrue(completion.IsCompleted);

			Assert.IsNull(unexpected);
		}

	}
}
