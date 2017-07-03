using System;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class NewsTypeRepository
    {
        #region Members
        protected override Func<NetStepsEntities, IQueryable<NewsType>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<NewsType>>(
                 (context) => context.NewsTypes
                                        .Include("NewsTypeLanguageSorts"));
            }
        }

        public override SqlUpdatableList<NewsType> LoadAllFullWithSqlDependency()
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var newsTypes = context.NewsTypes
                        .Include("NewsTypeLanguageSorts")
                        .ToList();

                    SqlUpdatableList<NewsType> list = new SqlUpdatableList<NewsType>();

                    list.AddRange(newsTypes);

                    return list;
                }
            });
        }
        #endregion

        public void Delete(int newsTypeID)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    if (context.News.Any(n => n.NewsTypeID == newsTypeID))
                        throw new DeleteDataException("Some news items still use this news type. Please assign these news items to a different news type before proceeding.");
                    
                    var obj = context.NewsTypes
                        .Include("NewsTypeLanguageSorts")
                        .FirstOrDefault(nt => nt.NewsTypeID == newsTypeID);
                    if (obj == null)
                        return;

                    obj.StartEntityTracking();

                    context.DeleteObjects(obj.NewsTypeLanguageSorts);
                    context.DeleteObject(obj);

                    Save(obj, context);
                }
            });
        }
    }
}
