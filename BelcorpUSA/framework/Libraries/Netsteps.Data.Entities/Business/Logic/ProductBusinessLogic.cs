using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Plugins.Common.PromotionKinds.Base;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class ProductBusinessLogic
	{
		public override List<string> ValidatedChildPropertiesSetByParent(Repositories.IProductRepository repository)
		{
			return new List<string>() { "ProductID", "DynamicKitID", "DynamicKitGroupID", "DynamicKitGroupRuleID", "DescriptionTranslationID" };
		}

		public virtual PaginatedList<AuditLogRow> GetAuditLog(Repositories.IProductRepository repository, Product fullyLoadedProduct, AuditLogSearchParameters param)
		{
			try
			{
				return repository.GetAuditLog(fullyLoadedProduct, param);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, null, null);
			}
		}

		public virtual bool CanBeAddedToDynamicKitGroup(Product entity, int kitProductId, int dynamicKitGroupId)
		{
			if (entity.IsDynamicKit())
				return false;

			var inventory = Create.New<InventoryBaseRepository>();

			Product kitProduct = inventory.GetProduct(kitProductId);

			if (!kitProduct.IsDynamicKit())
				return false;

			var dynamicKit = kitProduct.DynamicKits[0];
			var dynamicKitGroup = dynamicKit.DynamicKitGroups.Where(g => g.DynamicKitGroupID == dynamicKitGroupId).FirstOrDefault();

			if (dynamicKitGroup == null)
				return false;

			if (dynamicKitGroup.DynamicKitGroupRules.Any(r => (r.ProductID == entity.ProductID) && !r.Include))
			{
				return false;
			}
			if (!dynamicKitGroup.DynamicKitGroupRules.Any(r => (r.ProductTypeID == entity.ProductBase.ProductTypeID || r.ProductID == entity.ProductID) && r.Include))
			{
				return false;
			}

			return true;
		}

		public virtual decimal GetPrice(Product product, int accountTypeID, int? orderTypeID, int currencyID)
		{
			return product.GetPrice(accountTypeID, Constants.PriceRelationshipType.Products, currencyID, orderTypeID);
		}

		public virtual decimal GetAdjustedPrice(Product product, int accountTypeID, int? orderTypeID, int currencyID)
		{
			return product.GetAdjustedPrice(accountTypeID, Constants.PriceRelationshipType.Products, currencyID, orderTypeID);
		}

		public virtual decimal GetPrice(Product product, int accountTypeID, ConstantsGenerated.PriceRelationshipType priceRelationshipType, int? orderTypeID, int currencyID)
		{
			var productPriceType = AccountPriceType.GetPriceType(accountTypeID, priceRelationshipType, orderTypeID);
			return product.Prices.GetPriceByPriceType((ConstantsGenerated.ProductPriceType)productPriceType.ProductPriceTypeID, currencyID).ToDecimal();
		}

		public virtual decimal GetAdjustedPrice(Product product, int accountTypeID, ConstantsGenerated.PriceRelationshipType priceRelationshipType, int? orderTypeID, int currencyID)
		{
			var productPriceType = AccountPriceType.GetPriceType(accountTypeID, priceRelationshipType, orderTypeID);
			decimal? price = product.Prices.GetPriceByPriceType((ConstantsGenerated.ProductPriceType)productPriceType.ProductPriceTypeID, currencyID);
			if (price.HasValue)
			{
				price -= price;// / 2;  ?? doesn't this just = 0?
			}
			return price.GetValueOrDefault();
		}


		public virtual decimal GetCurrentPromotionalPrice(IOrderContext orderContext, Product product, Constants.ProductPriceType priceType, int currencyID, int marketID)
		{
			return GetCurrentPromotionalPrice(orderContext, product, (int)priceType, currencyID, marketID);
		}


		public virtual decimal GetCurrentPromotionalPrice(IOrderContext orderContext, Product product, int priceTypeID, int currencyID, int marketID)
		{
			var promotionService = Create.New<IPromotionService>();
			var promotions = promotionService.GetQualifiedPromotions<IProductPromotion>(orderContext, promo => promo.PromotedProductIDs.Any(i => i == product.ProductID));
            promotions = promotions.Where(p=>(p.StartDate==null || p.StartDate<= DateTime.Now));
            promotions = promotions.Where(p=>(p.EndDate==null || p.EndDate<=DateTime.Now));
            var price = product.Prices.GetPriceByPriceType(priceTypeID, currencyID).ToDecimal();
			var productPriceType = Create.New<IPriceTypeService>().GetPriceType(priceTypeID);
			foreach (var promo in promotions)
			{
				// technically we need a rules engine with a promotions ordering method here....
				price -= ((IProductPromotion)promo).GetPromotionAdjustmentAmount(product.ProductID, marketID, price, productPriceType);
			}
			return price;
		}

		/// <summary>
		/// Default term which will be returned. Client can override this and have its own term returned.
		/// </summary>
		/// <param name="product"></param>
		/// <param name="isCurrentlyABundle"></param>
		/// <returns></returns>
		public virtual string GetShopTerm(Product product, bool isCurrentlyABundle)
		{
			return (isCurrentlyABundle) ? "Add to Pack" : product.IsDynamicKit() ? "Create Bundle" : product.IsVariantTemplate || product.RequiresCustomization() ? "View Details" : "Add to Cart";
		}


		public virtual Product LoadWithRelations(int productID, Repositories.IProductRepository repository)
		{
			try
			{
				return repository.LoadWithRelations(productID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, null, null);
			}
		}


		#region AddChildProductRelation

		/// <summary>
		/// Adds a child product relation to the provided entity. If this new relation would create a situation where an infinite loop of
		/// products will occur, the method will throw an exception instead
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="relationTypeID"></param>
		/// <param name="childProduct"></param>
		public virtual void AddChildProductRelation(Product entity, int relationTypeID, Product childProduct)
		{
			if (entity.ProductID == childProduct.ProductID)
				throw new Exception(Translation.GetTerm("YouCannotAddAProductToItself", "You cannot add a product to itself"));

			if (relationTypeID != (int)Constants.ProductRelationsType.RelatedItem && entity.ParentProductRelations.Any(pr => pr.ProductRelationsTypeID != (int)Constants.ProductRelationsType.RelatedItem))
				throw new Exception(Translation.GetTerm("AddingThisRelationshipWouldTurnThisProductIntoAKitThisProductIsAlreadyInAKitCannotAddAKitToAKit", "Adding this relationship would turn this product into a kit. This Product is already in a kit. You cannot add a kit to a kit"));

			if ((childProduct.IsStaticKit() || childProduct.IsDynamicKit()) && relationTypeID != (int)Constants.ProductRelationsType.RelatedItem)
				throw new Exception(Translation.GetTerm("YouCannotAddAKitToAKit", "You cannot add a kit to a kit"));

			List<Product> highestLevels = GetAllRootNodesForProductRecursive(entity);
			highestLevels = FilterOutDuplicates(highestLevels);

			//Iterate through each of the top-most parent nodes associated with the product
			foreach (Product highestLevel in highestLevels)
			{
				//Create and populate the product tree from this top-most parent
				Tree<Product> productTree = new Tree<Product>(highestLevel);
				PopulateTreeRecursive(productTree.rootNode, highestLevel);

				//Find the node which contains entity in the tree
				Node<Product> NodeToAddTo = GetNodeByProductRecursive(entity, productTree.rootNode);

				CheckNodesForCircularDependencyWithNewProduct(childProduct, NodeToAddTo);
			}

			entity.ChildProductRelations.Add(new ProductRelation()
			{
				ProductRelationsTypeID = relationTypeID,
				ChildProductID = childProduct.ProductID
			});
		}

		private List<Node<Product>> GetThisAndParentNodes(Node<Product> startingNode)
		{
			List<Node<Product>> returnValue = new List<Node<Product>>();
			Node<Product> currentNode = startingNode;
			while (currentNode != null)
			{
				returnValue.Add(currentNode);
				currentNode = currentNode.Parent;
			}
			return returnValue;
		}

		/// <summary>
		/// Checks the upline of nodeToAddTo for circular references pointing at newProduct
		/// </summary>
		/// <param name="newProduct"></param>
		/// <param name="nodeToAddTo"></param>
		private void CheckNodesForCircularDependencyWithNewProduct(Product newProduct, Node<Product> nodeToAddTo)
		{
			//Grabs nodeToAddTo and upline
			List<Node<Product>> toAddToAndUpline = GetThisAndParentNodes(nodeToAddTo);

			//Populates a flat list of products from newProduct and children
			List<Product> newProducts = new List<Product>();
			PopulateFlatProductAndChildren(newProduct, newProducts);

			//Iterates through nodeToAddTo and upline, looking for instances where newProducts contains one of NodeToAddTo
			foreach (Node<Product> currentParentNode in toAddToAndUpline)
			{
				if (newProducts.Any(np => np.ProductID == currentParentNode.contents.ProductID))
					throw new Exception(Translation.GetTerm("CircularRelationship", "The product you are adding already has a relationship back to the product you are modifying. This will create a circular relationship."));
			}

		}

		/// <summary>
		/// Returns a list that has one of each product based on productID
		/// </summary>
		/// <param name="products"></param>
		/// <returns></returns>
		private List<Product> FilterOutDuplicates(List<Product> products)
		{
			List<Product> returnValue = new List<Product>();

			foreach (Product product in products)
			{
				if (!returnValue.Any(rv => rv.ProductID == product.ProductID))
					returnValue.Add(product);
			}
			return returnValue;
		}

		/// <summary>
		/// Recursive method to find all parent nodes in all product trees that involve this product
		/// </summary>
		/// <param name="currentProduct"></param>
		/// <returns></returns>
		private List<Product> GetAllRootNodesForProductRecursive(Product currentProduct)
		{
			List<Product> returnValue = new List<Product>();

			//If current node has no parents, add it to the return value and exit
			if (currentProduct.ParentProductRelations.Count == 0)
			{
				returnValue.Add(currentProduct);
				return returnValue;
			}

			//Iterate through each parent, checking their parents
			foreach (int productID in GetParentProducts(currentProduct))
			{
				Product parentProduct = LoadRelationProductFromDictionary(productID);
				returnValue.AddRange(GetAllRootNodesForProductRecursive(parentProduct));
			}

			return returnValue;
		}

		/// <summary>
		/// Uses product's productID to find node in tree
		/// </summary>
		/// <param name="p"></param>
		/// <param name="currentNode"></param>
		/// <returns></returns>
		private Node<Product> GetNodeByProductRecursive(Product p, Node<Product> currentNode)
		{
			if (currentNode.contents.ProductID == p.ProductID)
				return currentNode;

			foreach (Node<Product> p2 in currentNode.children)
			{
				Node<Product> returnProduct = GetNodeByProductRecursive(p, p2);
				if (returnProduct != null)
					return returnProduct;
			}

			return null;
		}


		private void PopulateFlatProductAndChildren(Product p, List<Product> products)
		{
			products.Add(p);

			foreach (int productID in GetChildProducts(p))
			{
				Product p2 = LoadRelationProductFromDictionary(productID);
				PopulateFlatProductAndChildren(p2, products);
			}
		}

		private IEnumerable<int> GetChildProducts(Product p)
		{
			return p.ChildProductRelations
				 .Where(cpr => cpr.ProductRelationsTypeID == (int)Constants.ProductRelationsType.Kit)
				 .Select(cpr => cpr.ChildProductID)
				 .Distinct();
		}
		private IEnumerable<int> GetParentProducts(Product p)
		{
			return p.ParentProductRelations
				 .Where(cpr => cpr.ProductRelationsTypeID == (int)Constants.ProductRelationsType.Kit)
				 .Select(cpr => cpr.ParentProductID)
				 .Distinct();
		}

		/// <summary>
		/// Load currentProduct's children into a tree
		/// </summary>
		/// <param name="parentNode"></param>
		/// <param name="currentProduct"></param>
		private void PopulateTreeRecursive(Node<Product> parentNode, Product currentProduct)
		{
			foreach (int productID in GetChildProducts(currentProduct))
			{
				Product p = LoadRelationProductFromDictionary(productID);
				Node<Product> newNode = parentNode.AddChild(p);

				PopulateTreeRecursive(newNode, p);
			}
		}

		private Dictionary<int, Product> _relationProductDictionary;
		private Product LoadRelationProductFromDictionary(int productID)
		{
			if (_relationProductDictionary == null)
				_relationProductDictionary = new Dictionary<int, Product>();

			if (_relationProductDictionary.ContainsKey(productID))
				return _relationProductDictionary[productID];

			_relationProductDictionary.Add(productID, Product.LoadWithRelations(productID));
			return _relationProductDictionary[productID];
		}

		#endregion

	}

	public class Tree<T>
	{
		public Node<T> rootNode;
		public Tree(T firstNode)
		{
			rootNode = new Node<T>(firstNode);
		}



	}

	public class Node<T>
	{
		public T contents;
		public List<Node<T>> children;
		public Node<T> Parent;

		public Node(T value)
		{
			contents = value;
			children = new List<Node<T>>();
		}

		public Node(T value, Node<T> parent)
			: this(value)
		{
			Parent = parent;
		}

		public Node<T> AddChild(T newChild)
		{
			Node<T> newNode = new Node<T>(newChild, this);
			children.Add(newNode);
			return newNode;
		}
	}
}
