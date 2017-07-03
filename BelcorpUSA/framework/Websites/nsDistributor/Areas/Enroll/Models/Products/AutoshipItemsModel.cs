using nsDistributor.Models.Shared;

namespace nsDistributor.Areas.Enroll.Models.Products
{
    public class AutoshipItemsModel : SectionModel
    {
        public virtual MiniShopModel MiniShop { get; set; }

        public AutoshipItemsModel()
        {
            this.MiniShop = new MiniShopModel();
        }

        public virtual AutoshipItemsModel LoadResources(
            dynamic modelData)
        {
            this.MiniShop.LoadResources(modelData);

            return this;
        }
    }
}