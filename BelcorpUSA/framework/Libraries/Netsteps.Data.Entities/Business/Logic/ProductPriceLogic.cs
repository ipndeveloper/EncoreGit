namespace NetSteps.Data.Entities.Business.Logic
{
    using IRepository = NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Data.Entities.Repositories;

    public class ProductPriceLogic
    {
        #region Private

        private static ProductPriceLogic instance;

        private static IRepository.IProductPriceRepository repository;

        #endregion

        #region

        private ProductPriceLogic() { }

        public static ProductPriceLogic Intance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new ProductPriceLogic();
                    repository = new ProductPriceRepository();
                }
                return instance;
            }
        }

        #endregion

        #region Methods

        public decimal GetRetilPerItem(int ProductID)
        {
            return repository.GetRetilPerItem(ProductID);
        }

        #endregion
    }
}
