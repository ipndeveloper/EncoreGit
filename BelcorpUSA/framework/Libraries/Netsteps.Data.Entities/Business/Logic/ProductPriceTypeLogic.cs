namespace NetSteps.Data.Entities.Business.Logic
{
    using NetSteps.Data.Entities.Repositories;
    using System.Collections.Generic;
    using System.Linq;

    public class ProductPriceTypeLogic
    {
        #region Private

        private static ProductPriceTypeLogic instance;

        private static IProductPriceTypeRepository repository;

        private ProductPriceType DtoToBo(NetSteps.Data.Entities.Dto.ProductPriceTypeDto dto)
        {
            return new ProductPriceType()
            {
                ProductPriceTypeID = dto.ProductPriceTypeID,
                Name = dto.Name,
                TermName = dto.TermName,
                Description = dto.Description,
                Active = dto.Active,
                Editable = dto.Editable,
                Mandatory = dto.Mandatory
            };
        }
        #endregion

        #region Singleton

        private ProductPriceTypeLogic() { }

        public static ProductPriceTypeLogic Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new ProductPriceTypeLogic();
                    repository = new ProductPriceTypeRepository();
                }
                return instance;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns all product price types active
        /// </summary>
        /// <returns></returns>
        public List<ProductPriceType> ListProductPriceTypes()
        {
            var data = repository.ListProductPriceTypes();
            return (from r in data select DtoToBo(r)).ToList();
        }

        #endregion
    }
}
