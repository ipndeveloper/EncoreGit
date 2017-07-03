using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Common.Services;
using NetSteps.Encore.Core.IoC;

namespace DependentClass
{
    public class JewelKadePriceTypeService : IPriceTypeService
    {
        protected IEnumerable<int> CurrencyPriceTypeIDs
        {
            get
            {
                var lqvProductPriceType = definedPriceTypes.First(x => x.Name == "LQVW");
                return new int[] 
											{
												(int)ProductPriceType.Retail,
												(int)ProductPriceType.Wholesale,
												(int)ProductPriceType.PreferredCustomer,
                                                lqvProductPriceType.PriceTypeID
											};
            }
        }

        public virtual IPriceType GetPriceType(int accountTypeID, int priceRelationshipTypeID, int storeFrontID, int? orderTypeID = new int?())
        {
            throw new NotImplementedException();
            //if (orderTypeID.HasValue)
            //{
            //    var orderType = SmallCollectionCache.Instance.OrderTypes.First(ot => ot.OrderTypeID == orderTypeID.Value);
            //    if (orderType.Name.Contains("Fundraiser"))
            //        return SmallCollectionCache.Instance.ProductPriceTypes.Where(ppt => ppt.Name.Equals("Fundraiser", StringComparison.InvariantCultureIgnoreCase)).Select(x =>
            //        {
            //            var priceType = Create.New<IPriceType>();
            //            priceType.Name = x.Name;
            //            priceType.TermName = x.TermName;
            //            priceType.PriceTypeID = x.ProductPriceTypeID;
            //            return priceType;
            //        }).SingleOrDefault();
            //}

            //AccountPriceType accountPriceType =
            //    SmallCollectionCache.Instance.AccountPriceTypes.FirstOrDefault(
            //        apt =>
            //        apt.AccountTypeID == accountTypeID && apt.PriceRelationshipTypeID == priceRelationshipTypeID
            //        && apt.StoreFrontID == storeFrontID);
            //if (accountPriceType == default(AccountPriceType))
            //{
            //    return null;
            //}
            //else
            //{
            //    var ppt = SmallCollectionCache.Instance.ProductPriceTypes.GetById(accountPriceType.ProductPriceTypeID);
            //    var priceType = Create.New<IPriceType>();
            //    priceType.Name = ppt.Name;
            //    priceType.TermName = ppt.TermName;
            //    priceType.PriceTypeID = ppt.ProductPriceTypeID;
            //    return priceType;
            //}
        }



        public IEnumerable<IPriceType> GetCurrencyPriceTypes()
        {
            return definedPriceTypes.Where(x => CurrencyPriceTypeIDs.Contains(x.PriceTypeID)).Select(x =>
            {
                var priceType = Create.New<IPriceType>();
                priceType.Name = x.Name;
                priceType.TermName = x.TermName;
                priceType.PriceTypeID = x.PriceTypeID;
                return priceType;
            });

        }

        protected virtual IEnumerable<int> VolumePriceTypeIDs
        {
            get
            {
                return new int[] 
											{
												(int)ProductPriceType.CV,
												(int)ProductPriceType.QV
											};
            }
        }

        public IEnumerable<IPriceType> GetVolumePriceTypes()
        {
            return definedPriceTypes.Where(x => VolumePriceTypeIDs.Contains(x.PriceTypeID)).Select(x =>
            {
                var priceType = Create.New<IPriceType>();
                priceType.Name = x.Name;
                priceType.TermName = x.TermName;
                priceType.PriceTypeID = x.PriceTypeID;
                return priceType;
            });
        }


        public IPriceType GetPriceType(string priceTypeName)
        {
            return definedPriceTypes.Where(x => x.Name == priceTypeName).Select(x =>
            {
                var priceType = Create.New<IPriceType>();
                priceType.Name = x.Name;
                priceType.TermName = x.TermName;
                priceType.PriceTypeID = x.PriceTypeID;
                return priceType;
            }).SingleOrDefault();
        }


        public IPriceType GetPriceType(int priceTypeID)
        {
            return definedPriceTypes.Where(x => x.PriceTypeID == priceTypeID).Select(x =>
            {
                var priceType = Create.New<IPriceType>();
                priceType.Name = x.Name;
                priceType.TermName = x.TermName;
                priceType.PriceTypeID = x.PriceTypeID;
                return priceType;
            }).SingleOrDefault();
        }


        public JewelKadePriceTypeService()
        {
            definedPriceTypes = new List<IPriceType>()
            {
                new PriceType() { Name = "Retail", PriceTypeID = 1, TermName = "Retail" },
                new PriceType() { Name = "Preferred Customer", PriceTypeID = 2, TermName = "PreferredCustomer" },
                new PriceType() { Name = "Shipping Fee", PriceTypeID = 10, TermName = "ShippingFee" },
                new PriceType() { Name = "Handling Fee", PriceTypeID = 11, TermName = "HandlingFee" },
                new PriceType() { Name = "CV", PriceTypeID = 18, TermName = "CV" },
                new PriceType() { Name = "Host Base", PriceTypeID = 20, TermName = "HostBase" },
                new PriceType() { Name = "QV", PriceTypeID = 21, TermName = "QV" },
                new PriceType() { Name = "Wholesale", PriceTypeID = 22, TermName = "Wholesale" },
                new PriceType() { Name = "Award", PriceTypeID = 23, TermName = "Award" },
                new PriceType() { Name = "MinimumPurchase", PriceTypeID = 24, TermName = "MinimumPurchase" },
                new PriceType() { Name = "MaximumAward", PriceTypeID = 25, TermName = "MaximumAward" },
                new PriceType() { Name = "Fundraiser", PriceTypeID = 26, TermName = "Fundraiser" },
                new PriceType() { Name = "Taxable Price", PriceTypeID = 1000, TermName = "Taxable Price" },
                new PriceType() { Name = "RPVDiscount", PriceTypeID = 1001, TermName = "RPVDiscount" },
                new PriceType() { Name = "LQVW", PriceTypeID = 1002, TermName = "LQVW" }
            };
        }
        private List<IPriceType> definedPriceTypes { get; set; }

        private class PriceType : IPriceType
        {
            public string Name { get; set; }

            public int PriceTypeID { get; set; }

            public string TermName { get; set; }
        }
    }
}
