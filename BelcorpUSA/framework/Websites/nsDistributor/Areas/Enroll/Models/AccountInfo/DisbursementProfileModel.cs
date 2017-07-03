// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisbursementProfileModel.cs" company="NetSteps">
//   Copyright 2012, NetSteps, LLC
// </copyright>
// <summary>
//   Model holds all the disbursement profiles
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using NetSteps.Web.Mvc.Controls.Models;

namespace nsDistributor.Areas.Enroll.Models.AccountInfo
{
    /// <summary>
    /// Model holds all the disbursement profiles
    /// </summary>
    public class DisbursementProfileModel : SectionModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisbursementProfileModel"/> class.
        /// </summary>
        public DisbursementProfileModel()
        {
            ViewModel = new DisbursementProfileViewModel();
        }

        /// <summary>
        /// Gets or sets ViewModel.
        /// </summary>
        public DisbursementProfileViewModel ViewModel { get; set; }
    }
}