using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;

namespace nsDistributor.Models.Shared
{
    public class MiniShopProductModel
    {
        public virtual int ProductID { get; set; }
        public virtual string Name { get; set; }
        public virtual string RetailPrice { get; set; }
        public virtual int Quantity { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual string Description { get; set; }
        public virtual string YourPrice { get; set; }
        public virtual string Volume { get; set; }
        public virtual decimal ValorPrice { get; set; }

        public virtual MiniShopProductModel LoadResources(
            Product product,
            int accountTypeID,
            int currencyID,
            int orderTypeID)
        {
            ProductID = product.ProductID;
            Name = product.Name;
            RetailPrice = product.GetPrice(
                (int)Constants.AccountType.RetailCustomer,
                currencyID,
                orderTypeID
            ).ToString(currencyID);
            ImageUrl = product.MainImage == null
                ? "~/Content/Images/Shopping/no-image.jpg".ResolveUrl()
                : product.MainImage.FilePath.ReplaceFileUploadPathToken();
            Description = product.Translations.GetByLanguageIdOrDefault(ApplicationContext.Instance.CurrentLanguageID).DetokenizeShortDescription();
            YourPrice = product.GetPrice(
                accountTypeID,
                currencyID,
                orderTypeID
            ).ToString(currencyID);
            ValorPrice = product.GetPrice(
                accountTypeID,
                currencyID,
                orderTypeID
            );
            Volume = product.GetPrice(
                accountTypeID,
                Constants.PriceRelationshipType.Commissions,
                currencyID,
                orderTypeID
            ).ToString(currencyID);

            return this;
        }
    }
}