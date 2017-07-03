using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class ProductMatrixBusinessLogic
    {
        static IProductMatrixRepository _IProductMatrixRepository;
        static ProductMatrixBusinessLogic instance;
        public static ProductMatrixBusinessLogic Instance
        {

            get
            {
                if (instance == null)
                {
                    instance = new ProductMatrixBusinessLogic();
                    _IProductMatrixRepository = new ProductMatrixRepository();
                }
                return instance;
            }
        }
        public static PaginatedList<ProductMatrix> GetMatrixErrorLog(ProductMatrixParameters searchParams)
        {
            _IProductMatrixRepository = new ProductMatrixRepository();
            return _IProductMatrixRepository.GetMatrixErrorLog(searchParams);
        }
    }
}
