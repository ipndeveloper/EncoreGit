using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetSteps.Bundles.Component.Tests
{
	
	using NetSteps.Bundles.Common;
	using NetSteps.Bundles.Common.Models;
	using NetSteps.Encore.Core.IoC;

	[TestClass]
	public class BundleComponentTests
	{
		private BundleComponent _bundleComponent;
		public BundleComponent Component
		{
			get
			{
				if (_bundleComponent == null)
				{
					_bundleComponent = new BundleComponent();
				}
				return _bundleComponent;
			}
		}

		[TestMethod]
		public void ValidateGroup_HasAllItems_ReturnsTrue()
		{
			// Arrange
			InitializeMockTwoGroup();
			var dynamicKitGroup = MockTwoGroupBundleRepository.CreateDynamicKitGroup(2, MockTwoGroupBundleRepository.CreateDynamicKitGroupRule(2, true));
			var items = CreateListOfBundleItems(CreateBundleItem(2, 2));
			bool expected = true;

			// Act
			bool result = Component.ValidateGroup(dynamicKitGroup, items);

			// Assert
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void ValidateGroup_HasLessItems_ReturnsFalse()
		{
			// Arrange
			InitializeMockTwoGroup();
			var dynamicKitGroup = MockTwoGroupBundleRepository.CreateDynamicKitGroup(2, MockTwoGroupBundleRepository.CreateDynamicKitGroupRule(2, true));
			var items = CreateListOfBundleItems(CreateBundleItem(2, 1));
			bool expected = false;

			// Act
			bool result = Component.ValidateGroup(dynamicKitGroup, items);

			// Assert
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void ApplyItemsToGroup_StartWithZeroGiveTwo_ReturnTwo()
		{
			// Arrange
			InitializeMockTwoGroup();
			int requiredItemsOnGroup = 2;
			var dynamicKitGroup = MockTwoGroupBundleRepository.CreateDynamicKitGroup(2, MockTwoGroupBundleRepository.CreateDynamicKitGroupRule(requiredItemsOnGroup, true));
			int quantityOnItem = 2;
			var item = CreateBundleItem(2, quantityOnItem);
			int startingQuantity = 0;
			int expected = 2;

			// Act
			int result = Component.ApplyItemsToGroup(dynamicKitGroup, item, startingQuantity);

			// Assert
			Assert.AreEqual(expected, result);
			Assert.AreEqual(0, item.Quantity);
		}

		[TestMethod]
		public void ApplyItemsToGroup_StartWithZeroGiveThree_ReturnTwo()
		{
			// Arrange
			InitializeMockTwoGroup();
			int requiredItemsOnGroup = 2;
			var dynamicKitGroup = MockTwoGroupBundleRepository.CreateDynamicKitGroup(2, MockTwoGroupBundleRepository.CreateDynamicKitGroupRule(requiredItemsOnGroup, true));
			int quantityOnItem = 3;
			var item = CreateBundleItem(2, quantityOnItem);
			int startingQuantity = 0;
			int expected = 2;

			// Act
			int result = Component.ApplyItemsToGroup(dynamicKitGroup, item, startingQuantity);

			// Assert
			Assert.AreEqual(expected, result);
			Assert.AreEqual(1, item.Quantity);
		}

		[TestMethod]
		public void ApplyItemstoGroup_StartWithOneGiveTwo_ReturnsOne()
		{
			// Arrange
			InitializeMockTwoGroup();
			int requiredItemsOnGroup = 2;
			var dynamicKitGroup = MockTwoGroupBundleRepository.CreateDynamicKitGroup(2, MockTwoGroupBundleRepository.CreateDynamicKitGroupRule(requiredItemsOnGroup, true));
			int quantityOnItem = 2;
			var item = CreateBundleItem(2, quantityOnItem);
			int startingQuantity = 1;
			int expected = 1;

			// Act
			int result = Component.ApplyItemsToGroup(dynamicKitGroup, item, startingQuantity);

			// Assert
			Assert.AreEqual(expected, result);
			Assert.AreEqual(1, item.Quantity);
		}

		[TestMethod]
		public void CanBeAddedToDynamicKitGroup_ValidItem_ReturnsTrue()
		{
			// Arrange
			InitializeMockTwoGroup();
			var item = CreateBundleItem(1, 1);
			var dynamicKitGroup = MockTwoGroupBundleRepository.CreateDynamicKitGroup(2, MockTwoGroupBundleRepository.CreateDynamicKitGroupRule(1, true));
			bool expected = true;

			// Act
			bool result = Component.CanBeAddedToDynamicKitGroup(item, dynamicKitGroup);

			// Assert
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void CanBeAddedToDynamicKitGroup_StaticKit_ReturnsFalse()
		{
			// Arrange
			InitializeMockTwoGroup();
			var item = CreateBundleItem(1, 1, false, true);
			var dynamicKitGroup = MockTwoGroupBundleRepository.CreateDynamicKitGroup(2, MockTwoGroupBundleRepository.CreateDynamicKitGroupRule(1, true));
			bool expected = false;

			// Act
			bool result = Component.CanBeAddedToDynamicKitGroup(item, dynamicKitGroup);

			// Assert
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void CanBeAddedToDynamicKitGroup_DynamicKit_ReturnsFalse()
		{
			// Arrange
			InitializeMockTwoGroup();
			var item = CreateBundleItem(1, 1, false, false, true);
			var dynamicKitGroup = MockTwoGroupBundleRepository.CreateDynamicKitGroup(2, MockTwoGroupBundleRepository.CreateDynamicKitGroupRule(1, true));
			bool expected = false;

			// Act
			bool result = Component.CanBeAddedToDynamicKitGroup(item, dynamicKitGroup);

			// Assert
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void CanBeAddedToDynamicKitGroup_HostReward_ReturnsFalse()
		{
			// Arrange
			InitializeMockTwoGroup();
			var item = CreateBundleItem(1, 1, true);
			var dynamicKitGroup = MockTwoGroupBundleRepository.CreateDynamicKitGroup(2, MockTwoGroupBundleRepository.CreateDynamicKitGroupRule(1, true));
			bool expected = false;

			// Act
			bool result = Component.CanBeAddedToDynamicKitGroup(item, dynamicKitGroup);

			// Assert
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void IsValidKit_GivenEnoughItems_ReturnsTrue()
		{
			// Arrange
			InitializeMockTwoGroup();
			var bundleItems = CreateListOfBundleItems(CreateBundleItem(1, 1), CreateBundleItem(2, 2));
			bool expected = true;

			// Act
			bool result = Component.IsValidKit(bundleItems, 1);


			// Assert
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void IsValidKit_NotEnoughItems_ReturnsFalse()
		{
			// Arrange
			InitializeMockTwoGroup();
			var bundleItems = CreateListOfBundleItems(CreateBundleItem(3, 3));
			bool expected = false;

			// Act
			bool result = Component.IsValidKit(bundleItems, 1);

			// Assert
			Assert.AreEqual(expected, result);
		}

		private void InitializeMockTwoGroup()
		{
			var root = Container.Root;
			root.Registry
				.ForType<IBundleRepository>()
				.Register<MockTwoGroupBundleRepository>()
				.ResolveAsSingleton()
				.End();
		}

		public static List<IBundleItem> CreateListOfBundleItems(params IBundleItem[] items)
		{
			return items.ToList();
		}

		public static IBundleItem CreateBundleItem(int productTypeID, int quantity, bool isParentStaticKit = false, bool isStaticKit = false, bool isDynamicKit = false)
		{
			var returnValue = Create.New<IBundleItem>();

			returnValue.ProductID = 99;
			returnValue.ProductTypeID = productTypeID;
			returnValue.IsParentStaticKit = isParentStaticKit;
			returnValue.IsStaticKit = isStaticKit;
			returnValue.Quantity = quantity;
			returnValue.IsDynamicKit = isDynamicKit;

			return returnValue;
		}

		public class MockTwoGroupBundleRepository : IBundleRepository
		{
			public static IDynamicKitGroupRule CreateDynamicKitGroupRule(int productTypeID, bool required)
			{
				var returnValue = Create.New<IDynamicKitGroupRule>();

				returnValue.ProductTypeID = productTypeID;
				returnValue.Required = required;
				returnValue.Include = true;

				return returnValue;
			}

			public static IDynamicKitGroup CreateDynamicKitGroup(int minProductCount, params IDynamicKitGroupRule[] rules)
			{
				var returnValue = Create.New<IDynamicKitGroup>();

				returnValue.MinimumProductCount = minProductCount;
				returnValue.DynamicKitGroupRules = rules.ToList();

				return returnValue;
			}

			public IKit GetDynamicKitProductByID(int dynamicKitProductID)
			{
				var returnValue = Create.New<IKit>();
				returnValue.ProductID = dynamicKitProductID;

				var dynamicKit = Create.New<IDynamicKit>();
				returnValue.DynamicKits = new List<IDynamicKit> { dynamicKit };

				var firstDynamicKitGroup = CreateDynamicKitGroup(1, CreateDynamicKitGroupRule(1, true));
				var secondDynamicKitGroup = CreateDynamicKitGroup(2, CreateDynamicKitGroupRule(2, true));

				dynamicKit.DynamicKitGroups = new List<IDynamicKitGroup> { firstDynamicKitGroup, secondDynamicKitGroup };

				return returnValue;
			}

			public List<IKit> GetDynamicKitProducts(int storeFrontID, bool sort = false, bool sortDescending = false)
			{
				throw new NotImplementedException();
			}
		}
	}


}
