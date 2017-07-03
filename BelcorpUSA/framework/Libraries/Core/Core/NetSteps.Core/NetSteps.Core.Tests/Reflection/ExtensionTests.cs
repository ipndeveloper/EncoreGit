using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetSteps.Encore.Core.Reflection.Tests
{
	[TestClass]
	public class ExtensionTests
	{
		[TestMethod]
		public void IsNumber_correctly_identifies_number_types()
		{			
			Assert.IsTrue(typeof(Byte).IsNumber());
			Assert.IsTrue(typeof(SByte).IsNumber());
			Assert.IsTrue(typeof(Int16).IsNumber());
			Assert.IsTrue(typeof(Int32).IsNumber());
			Assert.IsTrue(typeof(Int64).IsNumber());
			Assert.IsTrue(typeof(UInt16).IsNumber());
			Assert.IsTrue(typeof(UInt32).IsNumber());
			Assert.IsTrue(typeof(UInt64).IsNumber());
			Assert.IsTrue(typeof(Single).IsNumber());
			Assert.IsTrue(typeof(Double).IsNumber());
			Assert.IsTrue(typeof(Decimal).IsNumber());
			Assert.IsFalse(typeof(String).IsNumber());
			Assert.IsFalse(typeof(Guid).IsNumber());
			Assert.IsFalse(typeof(object).IsNumber());
		}

		interface ILevelOne : IEquatable<object> { }
		interface ILevelTwo : ILevelOne { }
		interface ILevelThree : IEquatable<object>, ILevelTwo { }

		[TestMethod]
		public void GetTypeHierarchyInDeclarationOrder_correctly_calculates_declaration_order_of_interfaces_and_removes_duplicates()
		{
			var hierarchy = typeof(ILevelOne).GetTypeHierarchyInDeclarationOrder();
			Assert.AreEqual(2, hierarchy.Count());
			Assert.AreSame(typeof(IEquatable<object>), hierarchy.First());
			Assert.AreSame(typeof(ILevelOne), hierarchy.Skip(1).First());

			hierarchy = typeof(ILevelTwo).GetTypeHierarchyInDeclarationOrder();
			Assert.AreEqual(3, hierarchy.Count());
			Assert.AreSame(typeof(IEquatable<object>), hierarchy.First());
			Assert.AreSame(typeof(ILevelOne), hierarchy.Skip(1).First());
			Assert.AreSame(typeof(ILevelTwo), hierarchy.Skip(2).First());

			// This hierarchy includes a duplicate declaration for IEquatable<object>, ensure duplicate is removed.
			hierarchy = typeof(ILevelThree).GetTypeHierarchyInDeclarationOrder();
			Assert.AreEqual(4, hierarchy.Count());
			Assert.AreSame(typeof(IEquatable<object>), hierarchy.First());
			Assert.AreSame(typeof(ILevelOne), hierarchy.Skip(1).First());
			Assert.AreSame(typeof(ILevelTwo), hierarchy.Skip(2).First());
			Assert.AreSame(typeof(ILevelThree), hierarchy.Skip(3).First());
		}
	}
}
