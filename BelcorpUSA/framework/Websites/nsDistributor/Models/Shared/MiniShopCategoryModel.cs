using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;

namespace nsDistributor.Models.Shared
{
    public class MiniShopCategoryModel
    {
        public virtual int CategoryID { get; set; }
        public virtual string Name { get; set; }
        public virtual IEnumerable<MiniShopCategoryModel> Subcategories { get; set; }

        public virtual MiniShopCategoryModel LoadResources(
            Category category,
            IEnumerable<Category> activeCategories,
            int maxLevels = 1)
        {
            CategoryID = category.CategoryID;
            Name = category.Translations.Name();

            if (maxLevels > 0)
            {
                //Subcategories = category.ChildCategories
                //    .Where(x => activeCategories.Contains(x))
                //    .OrderBy(x => x.SortIndex)
                //    .Select(x => new MiniShopCategoryModel()
                //        .LoadResources(x, activeCategories, maxLevels - 1)
                //    );

                Subcategories = category.ChildCategories.Where(donde => donde.ParentCategoryID == CategoryID)
                    .OrderBy(x => x.SortIndex)
                    .Select(x => new MiniShopCategoryModel()
                        .LoadResources(x, activeCategories, maxLevels - 1)
                    );
            }

            return this;
        }
    }
}