using System;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class FileResourceTypeRepository : BaseRepository<FileResourceType, Int32, NetStepsEntities>, IFileResourceTypeRepository, IDefaultImplementation
    {
        protected override Func<NetStepsEntities, IQueryable<FileResourceType>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<FileResourceType>>(
                   (context) => from a in context.FileResourceTypes
                                               .Include("FileResources")
                                               .Include("FileResources.FileResourceProperties")
                                select a);
            }
        }
    }
}
