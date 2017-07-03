using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;

namespace nsCore.Areas.Sites.Models
{
    public class DocumentCategoriesModel
    {
        #region Constructors
		public DocumentCategoriesModel() { }

        #endregion

        #region Properties
		public Category ParentCategory { get; set; }

		public IEnumerable<int> SelectedCategories { get; set; }
        #endregion

    }
}