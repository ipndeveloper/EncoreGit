using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NetSteps.Core.Cache;
using NetSteps.Encore.Core.Dto;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Cache.AppFabric.Tests
{
	[TestClass]
	public class JsonDistributedCacheTests
	{
		[TestMethod]
		public void Can_Add_Update_And_Remove_Items_In_JsonCache()
		{
			ICache<string, IMyTestClass> myCache = new JsonDistributedCacheAside<string, IMyTestClass>("MyTestCache");

			var thing1 = new MyTestClass() { IntProp = 1, StringProp = "thing 1" };

			Assert.IsTrue(myCache.TryAdd(thing1.StringProp, thing1));

			var thing1Again = myCache.Get(thing1.StringProp);

			Assert.AreEqual(thing1.StringProp, thing1Again.StringProp);
			Assert.AreEqual(thing1.IntProp, thing1Again.IntProp);

			thing1Again.IntProp = 2;

			Assert.IsTrue(myCache.TryUpdate(thing1Again.StringProp, thing1Again, thing1));

			var thing1Also = myCache.Get(thing1.StringProp);

			Assert.AreEqual(thing1Again.StringProp, thing1Also.StringProp);
			Assert.AreEqual(thing1Again.IntProp, thing1Also.IntProp);

			Assert.AreNotEqual(thing1.IntProp, thing1Also.IntProp);

			IMyTestClass thing1Removed;
			Assert.IsTrue(myCache.TryRemove(thing1.StringProp, out thing1Removed));
		}

		[TestMethod]
		public void Can_Add_Update_And_Remove_DTO_Items_In_JsonCache()
		{
			ICache<string, IMyTestClass> myCache = new JsonDistributedCacheAside<string, IMyTestClass>("MyTestCache");

			var thing1 = Create.New<IMyTestClass>();
			thing1.IntProp = 1;
			thing1.StringProp = "thing 1";

			Assert.IsTrue(myCache.TryAdd(thing1.StringProp, thing1));

			var thing1Again = myCache.Get(thing1.StringProp);

			Assert.AreEqual(thing1.StringProp, thing1Again.StringProp);
			Assert.AreEqual(thing1.IntProp, thing1Again.IntProp);

			thing1Again.IntProp = 2;

			Assert.IsTrue(myCache.TryUpdate(thing1Again.StringProp, thing1Again, thing1));

			var thing1Also = myCache.Get(thing1.StringProp);

			Assert.AreEqual(thing1Again.StringProp, thing1Also.StringProp);
			Assert.AreEqual(thing1Again.IntProp, thing1Also.IntProp);

			Assert.AreNotEqual(thing1.IntProp, thing1Also.IntProp);

			IMyTestClass thing1Removed;
			Assert.IsTrue(myCache.TryRemove(thing1.StringProp, out thing1Removed));
		}

		[TestMethod]
		public void Can_Add_Update_And_Remove_Items_In_JsonCache_With_ClassType_Key()
		{
			ICache<ClassTypeKey, IMyTestClass> myCache = new JsonDistributedCacheAside<ClassTypeKey, IMyTestClass>("MyTestCache");

			var thing1 = Create.New<IMyTestClass>();
			thing1.IntProp = 1;
			thing1.StringProp = "thing 1";

			ClassTypeKey key1 = new ClassTypeKey() { Param1 = "thing 1", Param2 = 1, Param3 = true };

			Assert.IsTrue(myCache.TryAdd(key1, thing1));

			//Test retrieve from new instance of key Class...
			key1 = new ClassTypeKey() { Param1 = "thing 1", Param2 = 1, Param3 = true };
			var thing1Again = myCache.Get(key1);
			Assert.IsNotNull(thing1Again);
			Assert.AreEqual(thing1.StringProp, thing1Again.StringProp);
			Assert.AreEqual(thing1.IntProp, thing1Again.IntProp);

			thing1Again.IntProp = 2;

			Assert.IsTrue(myCache.TryUpdate(key1, thing1Again, thing1));

			var thing1Also = myCache.Get(key1);

			Assert.AreEqual(thing1Again.StringProp, thing1Also.StringProp);
			Assert.AreEqual(thing1Again.IntProp, thing1Also.IntProp);

			Assert.AreNotEqual(thing1.IntProp, thing1Also.IntProp);

			var thing2 = Create.New<IMyTestClass>();
			thing2.IntProp = 1;
			thing2.StringProp = "thing 2";

			ClassTypeKey key2 = new ClassTypeKey() { Param1 = "thing 2", Param2 = 2, Param3 = true };

			Assert.IsTrue(myCache.TryAdd(key2, thing2));

			var thing2Again = myCache.Get(key2);

			Assert.AreEqual(thing2.StringProp, thing2Again.StringProp);
			Assert.AreEqual(thing2.IntProp, thing2Again.IntProp);

			thing2Again.IntProp = 3;

			Assert.IsTrue(myCache.TryUpdate(key2, thing2Again, thing2));

			var thing2Also = myCache.Get(key2);

			Assert.AreEqual(thing2Again.StringProp, thing2Also.StringProp);
			Assert.AreEqual(thing2Again.IntProp, thing2Also.IntProp);

			Assert.AreNotEqual(thing2.IntProp, thing2Also.IntProp);

			IMyTestClass thing1Removed;
			Assert.IsTrue(myCache.TryRemove(key1, out thing1Removed));

			IMyTestClass thing2Removed;
			Assert.IsTrue(myCache.TryRemove(key2, out thing2Removed));
		}
	}
}
