using System.Collections.Generic;
using NetSteps.Common.Exceptions;

namespace NetSteps.Data.Entities.Exceptions
{
    public class ProductShippingExcludedShippingException : NetStepsException
    {
        public ProductShippingExcludedShippingException(IEnumerable<Product> productsThatHaveExcludedShipping)
        {
            ProductsThatHaveExcludedShipping = productsThatHaveExcludedShipping;
        }

        public IEnumerable<Product> ProductsThatHaveExcludedShipping { get; set; }
    }
}
