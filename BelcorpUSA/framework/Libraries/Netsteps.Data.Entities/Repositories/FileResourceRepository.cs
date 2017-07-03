using System;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class FileResourceRepository : BaseRepository<FileResource, Int32, NetStepsEntities>, IFileResourceRepository, IDefaultImplementation
    {
        protected override Func<NetStepsEntities, IQueryable<FileResource>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<FileResource>>(
                   (context) => from a in context.FileResources
                                               .Include("Accounts")
                                               .Include("Accounts.Sponsor")
                                               .Include("Accounts.Orders")
                                select a);
            }
        }

        /// <summary>
        /// Get file resource properties asssciated with file resources
        /// </summary>
        /// <param name="fileResourceId"></param>
        /// <returns></returns>
        public FileResource GetFileResourceProperties(int fileResourceId)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var fileResourceProperties = context.FileResources.Include("FileResourceProperties").FirstOrDefault(ac => ac.FileResourceID == fileResourceId);
                    if (null != fileResourceProperties)
                        fileResourceProperties.StartEntityTracking();
                    return fileResourceProperties;
                }
            });
        }
    }
}
