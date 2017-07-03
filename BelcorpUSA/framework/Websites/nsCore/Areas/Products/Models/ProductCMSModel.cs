using System.Linq;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;

namespace nsCore.Areas.Products.Models
{
    public class ProductCMSModel
    {
        private Product _product = null;

        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public TrackableCollection<ProductFile> Files { get; set; }
        public bool IsVariant { get; set; }
        public ProductBase ProductBase { get; set; }
        public bool HasOwnDescriptionInfo { get; set; }
        public bool HasOwnImages { get; set; }
        public int ProductID { get { return this._product.ProductID; } }

        public ProductCMSModel(Product product)
            : this(product, product.IsVariant())
        {

        }

        public ProductCMSModel(Product product, bool isVariant)
        {
            this._product = product;

            IsVariant = isVariant;
            ProductBase = product.ProductBase;
            HasOwnDescriptionInfo = product.HasOwnDescriptionInfo();
            HasOwnImages = product.HasOwnImages();
            Product variantTemplate = ProductBase.Products.FirstOrDefault(p => p.IsVariantTemplate);

            if (HasOwnDescriptionInfo)
            {
                Name = product.Translations.Name();
                ShortDescription = product.Translations.ShortDescription();
                LongDescription = product.Translations.LongDescription();
            }
            else if (variantTemplate != null)
            {
                Name = !string.IsNullOrEmpty(product.Translations.Name()) ? product.Translations.Name() : variantTemplate.Translations.Name();
                ShortDescription = variantTemplate.Translations.ShortDescription();
                LongDescription = variantTemplate.Translations.LongDescription();
            }

            Files = HasOwnImages ? product.Files : new TrackableCollection<ProductFile>();
        }
    }
}