using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.Collections;

namespace NetSteps.Core.Tests.Collections
{
	[TestClass]
	public class RegistrarTests
	{
		[TestMethod]
		public void Registrar_CanRegister()
		{
			var test = new
			{
				Key = "MyKey",
				Handback = "My Handback"
			};

			IRegistrationKey<string, string> reg, capture = null, second, third;

			var registrationEventCalled = false;
			var registrar = new Registrar<string, string>();
			registrar.OnNewRegistration += new EventHandler<RegistrationEventArgs<string, string>>((sender, args) =>
				{
					registrationEventCalled = true;
					capture = args.Key;
				});

			Assert.IsFalse(registrar.IsRegistered(test.Key));
			Assert.IsFalse(registrar.TryGetRegistration(test.Key, out reg));

			Assert.IsTrue(registrar.TryRegister(test.Key, test.Handback, out reg));
			Assert.IsTrue(registrationEventCalled);
			Assert.AreSame(reg, capture);

			Assert.AreEqual(registrar, reg.UntypedRegistrar);
			Assert.AreEqual(registrar, reg.Registrar);
			Assert.AreEqual(test.Key.GetType(), reg.KeyType);
			Assert.AreEqual(test.Key, reg.Key);
			Assert.AreEqual(test.Key, reg.UntypedKey);
			Assert.AreEqual(test.Handback.GetType(), reg.HandbackType);
			Assert.AreEqual(test.Handback, reg.Handback);
			Assert.AreEqual(test.Handback, reg.UntypedHandback);

			Assert.IsTrue(registrar.IsRegistered(test.Key));


			Assert.IsTrue(registrar.TryGetRegistration(test.Key, out second));
			Assert.AreSame(reg, second);

			Assert.IsFalse(registrar.TryRegister(test.Key, test.Handback, out third));

		}

		[TestMethod]
		public void Registrar_CanReplaceRegistration()
		{
			var test = new
			{
				Key = "MyKey",
				Handback = "My Handback",
				Replacement = "My Replacement"
			};

			IRegistrationKey<string, string> reg, capture = null, second, third;

			var registrationEventCalled = 0;
			var registrar = new Registrar<string, string>();
			registrar.OnNewRegistration += new EventHandler<RegistrationEventArgs<string, string>>((sender, args) =>
			{
				registrationEventCalled += 1;
				capture = args.Key;
			});

			Assert.IsFalse(registrar.IsRegistered(test.Key));
			Assert.IsFalse(registrar.TryGetRegistration(test.Key, out reg));

			Assert.IsTrue(registrar.TryRegister(test.Key, test.Handback, out reg));
			Assert.AreEqual(1, registrationEventCalled);
			Assert.AreSame(reg, capture);

			Assert.IsTrue(registrar.TryGetRegistration(test.Key, out second));

			Assert.IsFalse(registrar.TryRegister(test.Key, test.Replacement, out third));
			Assert.IsNull(third);
			Assert.IsTrue(registrar.TryReplaceRegistration(second, test.Key, test.Replacement, out third));
			Assert.AreEqual(2, registrationEventCalled);
			Assert.AreSame(third, capture);
		}

		[TestMethod]
		public void Registrar_PreexistingRegistrationGetsNotifiedWhenReplaced()
		{
			var test = new
			{
				Key = "MyKey",
				Handback = "My Handback",
				Replacement = "My Replacement"
			};

			IRegistrationKey<string, string> reg, capture = null, second, third;

			var registrationEventCalled = 0;
			var registrar = new Registrar<string, string>();
			registrar.OnNewRegistration += new EventHandler<RegistrationEventArgs<string, string>>((sender, args) =>
			{
				registrationEventCalled += 1;
				capture = args.Key;
			});

			Assert.IsFalse(registrar.IsRegistered(test.Key));
			Assert.IsFalse(registrar.TryGetRegistration(test.Key, out reg));

			Assert.IsTrue(registrar.TryRegister(test.Key, test.Handback, out reg));
			Assert.AreEqual(1, registrationEventCalled);
			Assert.AreSame(reg, capture);

			List<RegistrationEventKind> notificationKinds = new List<RegistrationEventKind>();
			reg.OnAny += new EventHandler<RegistrationEventArgs<string, string>>((sender, args) =>
			{
				notificationKinds.Add(args.Kind);
			});

			Assert.IsTrue(registrar.TryGetRegistration(test.Key, out second));

			Assert.IsFalse(registrar.TryRegister(test.Key, test.Replacement, out third));
			Assert.IsNull(third);
			Assert.IsTrue(registrar.TryReplaceRegistration(second, test.Key, test.Replacement, out third));
			Assert.AreEqual(2, registrationEventCalled);
			Assert.AreSame(third, capture);
			Assert.IsTrue(notificationKinds.Contains(RegistrationEventKind.Replacing));
			Assert.IsTrue(notificationKinds.Contains(RegistrationEventKind.Replaced));
		}

		[TestMethod]
		public void Registrar_RegistrationEventHandlerCanCancelReplacement()
		{
			var test = new
			{
				Key = "MyKey",
				Handback = "My Handback",
				Replacement = "My Replacement"
			};

			IRegistrationKey<string, string> reg, capture = null, second, third;

			var registrationEventCalled = 0;
			var registrar = new Registrar<string, string>();
			registrar.OnNewRegistration += new EventHandler<RegistrationEventArgs<string, string>>((sender, args) =>
			{
				registrationEventCalled += 1;
				capture = args.Key;
			});

			Assert.IsFalse(registrar.IsRegistered(test.Key));
			Assert.IsFalse(registrar.TryGetRegistration(test.Key, out reg));

			Assert.IsTrue(registrar.TryRegister(test.Key, test.Handback, out reg));
			Assert.AreEqual(1, registrationEventCalled);
			Assert.AreSame(reg, capture);

			List<RegistrationEventKind> notificationKinds = new List<RegistrationEventKind>();
			reg.OnAny += new EventHandler<RegistrationEventArgs<string, string>>((sender, args) =>
			{
				notificationKinds.Add(args.Kind);
				if (args.Kind == RegistrationEventKind.Replacing)
				{
					Assert.IsTrue(args.CanCancel);
					args.Cancel();
					Assert.IsTrue(args.IsCanceled);
				}
			});

			Assert.IsTrue(registrar.TryGetRegistration(test.Key, out second));

			Assert.IsFalse(registrar.TryRegister(test.Key, test.Replacement, out third));
			Assert.IsNull(third);

			Assert.IsFalse(registrar.TryReplaceRegistration(second, test.Key, test.Replacement, out third));
			Assert.AreEqual(1, registrationEventCalled);
			
			Assert.IsTrue(notificationKinds.Contains(RegistrationEventKind.Replacing));
			Assert.IsFalse(notificationKinds.Contains(RegistrationEventKind.Replaced));
		}
	}	
}
