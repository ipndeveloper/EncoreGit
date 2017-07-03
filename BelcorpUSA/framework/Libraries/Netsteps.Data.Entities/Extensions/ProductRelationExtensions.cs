using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Extensions;

namespace NetSteps.Data.Entities.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: ProductRelation Extensions
    /// Created: 06-23-2010
    /// </summary>
    public static class ProductRelationExtensions
    {
        public static bool ContainsRelation(this IEnumerable<ProductRelation> productRelations, int productRelationsTypeID, int childProductID)
        {
            return !productRelations.Where(c => c.ProductRelationsTypeID == productRelationsTypeID && c.ChildProductID == childProductID).ToList().IsNullOrEmpty();
        }
    }
}
