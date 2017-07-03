using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities.Cache;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Services
{
	[ContainerRegister(typeof(IPriceTypeService), RegistrationBehaviors.IfNoneOther)]
	public class PriceTypeService : IPriceTypeService
	{
		protected virtual IEnumerable<int> CurrencyPriceTypeIDs
		{
			get
			{
				return new int[]
				{
					(int)Constants.ProductPriceType.Retail,
					(int)Constants.ProductPriceType.Wholesale,
					(int)Constants.ProductPriceType.PreferredCustomer
				};
			}
		}

		public IEnumerable<IPriceType> GetCurrencyPriceTypes()
		{
			return SmallCollectionCache.Instance.ProductPriceTypes.Where(x => CurrencyPriceTypeIDs.Contains(x.ProductPriceTypeID)).Select(x =>
			{
				var priceType = Create.New<IPriceType>();
				priceType.Name = x.Name;
				priceType.TermName = x.TermName;
				priceType.PriceTypeID = x.ProductPriceTypeID;
				return priceType;
			});
		}

		protected virtual IEnumerable<int> VolumePriceTypeIDs
		{
			get
			{
				return new int[] 
				{
					(int)Constants.ProductPriceType.CV,
					(int)Constants.ProductPriceType.QV,
                    (int)Constants.ProductPriceType.HandlingFee
				};
			}
		}

		public IEnumerable<IPriceType> GetVolumePriceTypes()
		{
			return SmallCollectionCache.Instance.ProductPriceTypes.Where(x => VolumePriceTypeIDs.Contains(x.ProductPriceTypeID)).Select(x =>
			{
				var priceType = Create.New<IPriceType>();
				priceType.Name = x.Name;
				priceType.TermName = x.TermName;
				priceType.PriceTypeID = x.ProductPriceTypeID;

				return priceType;
			});
		}

		public IPriceType GetPriceType(string priceTypeName)
		{
			return SmallCollectionCache.Instance.ProductPriceTypes.Where(x => x.Name == priceTypeName).Select(x =>
			{
				var priceType = Create.New<IPriceType>();
				priceType.Name = x.Name;
				priceType.TermName = x.TermName;
				priceType.PriceTypeID = x.ProductPriceTypeID;

				return priceType;
			}).SingleOrDefault();
		}

		public IPriceType GetPriceType(int priceTypeID)
		{
			return SmallCollectionCache.Instance.ProductPriceTypes.Where(x => x.ProductPriceTypeID == priceTypeID).Select(x =>
			{
				var priceType = Create.New<IPriceType>();
				priceType.Name = x.Name;
				priceType.TermName = x.TermName;
				priceType.PriceTypeID = x.ProductPriceTypeID;

				return priceType;
			}).SingleOrDefault();
		}

		public virtual IPriceType GetPriceType(int accountTypeID, int priceRelationshipTypeID, int storeFrontID, int? orderTypeID = new int?())
		{
			if (orderTypeID.HasValue)
			{
				var orderType = SmallCollectionCache.Instance.OrderTypes.First(ot => ot.OrderTypeID == orderTypeID.Value);
				if (orderType.Name.Contains("Fundraiser"))
				{
					return SmallCollectionCache.Instance.ProductPriceTypes.Where(ppt => ppt.Name.Equals("Fundraiser", StringComparison.InvariantCultureIgnoreCase)).Select(x =>
					{
						var priceType = Create.New<IPriceType>();
						priceType.Name = x.Name;
						priceType.TermName = x.TermName;
						priceType.PriceTypeID = x.ProductPriceTypeID;

						return priceType;
					}).SingleOrDefault();
				}
			}

			AccountPriceType accountPriceType =
				SmallCollectionCache.Instance.AccountPriceTypes.FirstOrDefault(
					apt =>
					apt.AccountTypeID == accountTypeID && apt.PriceRelationshipTypeID == priceRelationshipTypeID
					&& apt.StoreFrontID == storeFrontID);
			if (accountPriceType == default(AccountPriceType))
			{
				return null;
			}
			else
			{
				var productPriceType = SmallCollectionCache.Instance.ProductPriceTypes.GetById(accountPriceType.ProductPriceTypeID);
				var priceType = Create.New<IPriceType>();
				priceType.Name = productPriceType.Name;
				priceType.TermName = productPriceType.TermName;
				priceType.PriceTypeID = productPriceType.ProductPriceTypeID;

				return priceType;
			}
		}
	}
}
