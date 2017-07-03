
using NetSteps.Commissions.Common.Models;
namespace nsCore.Areas.Commissions.Models
{
    public class JobDetailsModel
    {

        #region Constructors

		public JobDetailsModel()
        {
        }

        #endregion

        #region Properties

		public int CommissionRunTypeID { get; set; }
		public CommissionRunKind CommissionRunType { get; set; }
		public string JobDisplayName { get; set; }
		public string JobName { get; set; }
		public bool IsJobRunnable { get; set; }
		public JobOutputModel JobOutput { get; set; }

        #endregion

        #region Methods

        #endregion

    }
}