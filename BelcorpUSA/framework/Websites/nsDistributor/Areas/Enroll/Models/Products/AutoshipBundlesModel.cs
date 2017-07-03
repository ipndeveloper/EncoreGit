using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NetSteps.Data.Entities;
using NetSteps.Enrollment.Common.Models.Context;

namespace nsDistributor.Areas.Enroll.Models.Products
{
    public class AutoshipBundlesModel : SectionModel
    {
        #region Values
        public bool Skippable { get; set; }

        private int? _selectedProductID;

        [NSRequired(TermName = "ErrorAutoshipBundleRequired", ErrorMessage = "Please select an autoship bundle.")]
        public virtual int? SelectedProductID
        {
            // This is confusing to prevent double validation error messages, as SelectedProductIds is also required.
            // Clients not using SelectedProductIds should be unaffected, while those using it can still require that one is selected.
            get
            {
                return _selectedProductID ?? SelectedProductIds.FirstOrDefault();
            }
            set
            {
                _selectedProductID = value;
                SelectedProductIds = value == null
                    ? Enumerable.Empty<int>()
                    : new[] { value.Value };
            }
        }

        // Required - see comment in SelectedProductID
        public virtual IEnumerable<int> SelectedProductIds
        {
            get { return this.AutoshipBundles.Where(x => x.Selected).Select(x => x.ProductID); }
            set
            {
                foreach (var autoship in this.AutoshipBundles)
                    autoship.Selected = value.Any(selectedId => autoship.ProductID == selectedId);
            }
        }
        #endregion

        #region Resources
        public virtual IFormatProvider FormatProvider { get; set; }
        #endregion

        #region Models
        public virtual List<AutoshipBundleModel> AutoshipBundles { get; set; }
        #endregion

        #region Infrastructure
        public AutoshipBundlesModel()
        {
            this.AutoshipBundles = new List<AutoshipBundleModel>();
        }

        public virtual AutoshipBundlesModel LoadValues(
            int? selectedProductID,
            bool skippable)
        {
            this.SelectedProductID = selectedProductID;
            this.Skippable = skippable;

            return this;
        }

        public virtual AutoshipBundlesModel LoadValues(
            List<int> selectedProductIds,
            bool skippable)
        {
            this.SelectedProductIds = selectedProductIds;
            this.Skippable = skippable;

            return this;
        }

        public virtual AutoshipBundlesModel LoadResources(
            IFormatProvider formatProvider,
            IEnumerable<Product> autoshipBundleProducts,
            IEnrollmentContext enrollmentContext)
        {
            this.FormatProvider = formatProvider;
            
            foreach (var product in autoshipBundleProducts)
            {
                    var model = this.AutoshipBundles.FirstOrDefault(x => x.ProductID == product.ProductID) ?? new AutoshipBundleModel();
                    model.LoadResources(product, enrollmentContext);

                    if (!this.AutoshipBundles.Contains(model))
                        this.AutoshipBundles.Add(model);
            }

            return this;
        }
        #endregion
    }
}