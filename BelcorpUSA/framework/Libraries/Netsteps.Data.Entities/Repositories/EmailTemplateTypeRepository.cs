using System;
using System.Data.Objects;
using System.Linq;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class EmailTemplateTypeRepository : IEmailTemplateTypeRepository
    {
        #region Members
        protected override Func<NetStepsEntities, short, IQueryable<EmailTemplateType>> loadFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, short, IQueryable<EmailTemplateType>>(
                    (context, emailTemplateTypeId) => context.EmailTemplateTypes
                                                 .Include("Tokens")
                                                 .Where(e => e.EmailTemplateTypeID == emailTemplateTypeId));
            }
        }

        #endregion
    }
}
