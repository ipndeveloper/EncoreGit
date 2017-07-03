using nsDistributor.Models.Shared;

namespace nsDistributor.Areas.Enroll.Models.Products
{
    public class EnrollmentItemsModel : SectionModel
    {
        public virtual MiniShopModel MiniShop { get; set; }

        public EnrollmentItemsModel()
        {
            this.MiniShop = new MiniShopModel();
        }

        public virtual EnrollmentItemsModel LoadResources(
            dynamic modelData)
        {
            this.MiniShop.LoadResources(modelData);

            return this;
        }
    }
}