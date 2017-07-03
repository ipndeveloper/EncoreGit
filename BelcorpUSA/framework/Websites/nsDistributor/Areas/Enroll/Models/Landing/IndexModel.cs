using System.Collections.Generic;
using System.Linq;
using nsDistributor.Areas.Enroll.Models.Shared;

namespace nsDistributor.Areas.Enroll.Models.Landing
{
    public class IndexModel
    {
        #region Models
        public virtual IEnumerable<EnrollmentTypeModel> EnrollmentTypes { get; set; }
        #endregion

        #region Infrastructure
        public IndexModel()
        {
            EnrollmentTypes = Enumerable.Empty<EnrollmentTypeModel>();
        }

        public IndexModel LoadResources(
            IEnumerable<EnrollmentTypeModel> enrollmentTypes)
        {
            EnrollmentTypes = enrollmentTypes;

            return this;
        }
        #endregion
    }
}
