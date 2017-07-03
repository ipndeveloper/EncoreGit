using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Moq;

namespace NetSteps.Testing.Core
{
	public static class MockHelpers
	{
		private static Random random = new Random();
		public static int RandomID()
		{
			return RandomNumber(1000000);
		}

		public static int RandomNumber(int i)
		{
			return random.Next(i);
		}

		public static HttpContext FakeHttpContext(string url = "http://mySomething/")
		{
			var httpRequest = new HttpRequest("", url, "");
			var stringWriter = new StringWriter();
			var httpResponce = new HttpResponse(stringWriter);
			var httpContext = new HttpContext(httpRequest, httpResponce);

			var sessionContainer = new HttpSessionStateContainer(
				"id",
				new SessionStateItemCollection(),
				new HttpStaticObjectsCollection(),
				10,
				true,
				HttpCookieMode.AutoDetect,
				SessionStateMode.InProc,
				false
			);

			httpContext.Items["AspSession"] = typeof(HttpSessionState).GetConstructor(
				BindingFlags.NonPublic | BindingFlags.Instance,
				null,
				CallingConventions.Standard,
				new[] { typeof(HttpSessionStateContainer) },
				null
			).Invoke(new object[] { sessionContainer });

			return httpContext;
		}

		public static HttpContextBase FakeHttpContextBase()
		{
			var context = new Mock<HttpContextBase>();
			var request = new Mock<HttpRequestBase>();
			var response = new Mock<HttpResponseBase>();
			var session = new FakeHttpSession();
			var server = new Mock<HttpServerUtilityBase>();

			context.Setup(ctx => ctx.Request).Returns(request.Object);
			context.Setup(ctx => ctx.Response).Returns(response.Object);
			context.Setup(ctx => ctx.Session).Returns(session);
			context.Setup(ctx => ctx.Server).Returns(server.Object);

			return context.Object;
		}

		public static HttpContextBase FakeHttpContextBase(string url)
		{
			HttpContextBase context = FakeHttpContextBase();
			context.Request.SetupRequestUrl(url);
			return context;
		}

		public class FakeHttpSession : HttpSessionStateBase
		{
			private Dictionary<string, object> _sessionStorage = new Dictionary<string, object>();

			public override object this[string name]
			{
				get
				{
					return _sessionStorage.ContainsKey(name) ? _sessionStorage[name] : null;
				}
				set
				{
					_sessionStorage[name] = value;
				}
			}
		}

		public static void SetFakeControllerContext(this Controller controller)
		{
			var httpContext = FakeHttpContextBase();
			ControllerContext context = new ControllerContext(new RequestContext(httpContext, new RouteData()), controller);
			controller.ControllerContext = context;
		}

		static string GetUrlFileName(string url)
		{
			if (url.Contains("?"))
				return url.Substring(0, url.IndexOf("?"));
			else
				return url;
		}

		static NameValueCollection GetQueryStringParameters(string url)
		{
			if (url.Contains("?"))
			{
				NameValueCollection parameters = new NameValueCollection();

				string[] parts = url.Split("?".ToCharArray());
				string[] keys = parts[1].Split("&".ToCharArray());

				foreach (string key in keys)
				{
					string[] part = key.Split("=".ToCharArray());
					parameters.Add(part[0], part[1]);
				}

				return parameters;
			}
			else
			{
				return null;
			}
		}

		public static void SetHttpMethodResult(this HttpRequestBase request, string httpMethod)
		{
			Mock.Get(request)
				.Setup(req => req.HttpMethod)
				.Returns(httpMethod);
		}

		public static void SetupRequestUrl(this HttpRequestBase request, string url)
		{
			if (url == null)
				throw new ArgumentNullException("url");

			if (!url.StartsWith("~/"))
				throw new ArgumentException("Sorry, we expect a virtual url starting with \"~/\".");

			var mock = Mock.Get(request);

			mock.Setup(req => req.QueryString)
				.Returns(GetQueryStringParameters(url));
			mock.Setup(req => req.AppRelativeCurrentExecutionFilePath)
				.Returns(GetUrlFileName(url));
			mock.Setup(req => req.PathInfo)
				.Returns(string.Empty);
		}
	}
}
