using nsDistributor.Models.Shared;

namespace nsDistributor.Models.Autoship
{
    public class EditModel
    {
        public virtual MiniShopModel MiniShop { get; set; }

        public EditModel()
        {
            this.MiniShop = new MiniShopModel();
        }

        public virtual EditModel LoadResources(
            dynamic modelData)
        {
            this.MiniShop.LoadResources(modelData);

            return this;
        }
    }
}
