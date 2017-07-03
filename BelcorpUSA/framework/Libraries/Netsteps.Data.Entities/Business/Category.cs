using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.Logic.Interfaces;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities
{
    public partial class Category
    {
        #region Properties
        private int _categoryTreeID = 0;
        public int CategoryTreeID
        {
            get
            {
                //if (ParentCategoryID.HasValue)
                //{
                //    Category parentCategory = ParentCategory ?? Category.Load(ParentCategoryID.Value);
                //    while (parentCategory.ParentCategoryID.HasValue)
                //        parentCategory = parentCategory.ParentCategory ?? Category.Load(parentCategory.ParentCategoryID.Value);
                //    return parentCategory.CategoryID;
                //}
                //return CategoryID;
                if (CategoryID > 0 && _categoryTreeID == 0)
                    _categoryTreeID = Repository.GetCategoryTreeID(CategoryID);
                return _categoryTreeID;
            }
        }

        partial void ParentCategoryIDChanged()
        {
            _categoryTreeID = 0;
        }

        public long TotalProducts
        {
            get
            {
                return AddProductTotals(this);
            }
        }

        /// <summary>
        /// This property is purely an in-memory property to determine whether to show a category on the UI - DES
        /// </summary>
        public bool Display
        {
            get;
            set;
        }

        private int AddProductTotals(Category category)
        {
            int total = 0;
            if (category != null)
            {
                total += category.ProductBases.Count;
                if (category.ChildCategories.Count > 0)
                {
                    foreach (Category cat in category.ChildCategories)
                    {
                        total += AddProductTotals(cat);
                    }
                }
            }
            return total;
        }
        #endregion

        #region Methods
        public Category FindCategory(int categoryID)
        {
            if (CategoryID == categoryID)
                return this;

            Category category;

            if (ChildCategories != null && ChildCategories.Count > 0)
            {
                category = ChildCategories.FirstOrDefault(c => c.CategoryID == categoryID);
                if (category != default(Category))
                    return category;

                foreach (Category cat in ChildCategories)
                {
                    if (cat.ChildCategories != null && cat.ChildCategories.Count > 0)
                    {
                        category = cat.ChildCategories.FindCategory(categoryID);
                        if (category != default(Category))
                            return category;
                    }
                }
            }

            return null;
        }

        public static List<Category> LoadAllFullByCategoryTypeId(int categoryTypeId)
        {
            try
            {
                var categories = Repository.LoadAllFullByCategoryTypeId(categoryTypeId);
                foreach (var item in categories)
                {
                    item.StartTracking();
                    item.IsLazyLoadingEnabled = true;
                }
                return categories;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<Category> LoadFullTopLevelByCategoryTypeId(int categoryTypeId)
        {
            try
            {
                var categories = Repository.LoadFullTopLevelByCategoryTypeId(categoryTypeId);
                foreach (var item in categories)
                {
                    item.StartTracking();
                    item.IsLazyLoadingEnabled = true;
                }
                return categories;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static TrackableCollection<Category> LoadFullByParent(int parentCategoryID)
        {
            try
            {
                return Repository.LoadFullByParent(parentCategoryID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static PaginatedList<CategorySearchData> Search(FilterPaginatedListParameters<Category> searchParams)
        {
            try
            {
                return Repository.Search(searchParams);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static new Category LoadFull(int primaryKey)
        {
            Category cat = EntityBusinessBase<Category, Int32, ICategoryRepository, ICategoryBusinessLogic>.LoadFull(primaryKey);
            cat.StopTracking();
            cat.ChildCategories = null;
            cat.StartTracking();
            return cat;
        }

        public static Category LoadByNumber(string categoryNumber)
        {
            return Repository.LoadCategoryByNumber(categoryNumber);
        }

        public static Category LoadFullTree(int categoryTreeID)
        {
            try
            {
                return Repository.LoadFullTree(categoryTreeID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static CategoryTranslation LoadTranslation(int categoryID, int languageID)
        {
            try
            {
                return Repository.LoadTranslation(categoryID, languageID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<ArchiveCategorySearchData> LoadAllArchiveCategories()
        {
            try
            {
                return Repository.LoadAllArchiveCategories();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void DeleteTree(int categoryTreeID)
        {
            try
            {
                Repository.DeleteTree(categoryTreeID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        new public static void Delete(int primaryKey)
        {
            Category.DeleteTree(primaryKey);
        }
        #endregion

	    public static List<Category> LoadCategoriesByStoreFrontId(int storeFrontId)
	    {
	        return Repository.LoadCategoriesByStoreFrontId(storeFrontId);
        }
    }
}
