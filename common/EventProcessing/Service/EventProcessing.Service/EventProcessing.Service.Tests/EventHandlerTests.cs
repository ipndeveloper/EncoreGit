using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetSteps.EventProcessing.Service.Tests
{
	[TestClass]
	public class EventHandlerTests
	{
		[TestMethod]
		public void AddAssemblies_AllRetrievable()
		{
			var projectRoot = Directory.GetParent(Assembly.GetExecutingAssembly().Location).Parent.Parent.FullName;
			var assembly = Assembly.LoadFrom(Path.Combine(projectRoot, "FailureEventHandler.dll"));
			var eventHandler = new EventTypeRegistry();

			eventHandler.AddEventHandlerTypesFromAssembly(assembly);
			eventHandler.AddEventHandlerTypesFromAssembly(Assembly.LoadFrom(Path.Combine(projectRoot, "TestArea.dll")));

			Type handler1 = eventHandler.GetType("FailureEventHandler.FailureClass");
			Type handler2 = eventHandler.GetType("FailureEventHandler.ExceptionClass");
			Type handler3 = eventHandler.GetType("TestArea.TestClass");

			Assert.IsTrue(handler1 != null && handler2 != null && handler3 != null);
		}

		[TestMethod]
		public void GetNonExistentType_Fails()
		{
			var eventHandler = new EventTypeRegistry();
			var result = eventHandler.GetType("Faker.Fakey");
			Assert.IsNull(result);
		}
	}
}
