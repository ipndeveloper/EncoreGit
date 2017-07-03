using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class ArchiveRepository : BaseRepository<Archive, int, NetStepsEntities>, IDefaultImplementation
    {
        #region Members
        protected override Func<NetStepsEntities, IQueryable<Archive>> loadAllFullQuery
        {
            get
            {
                // Compiled Query prototype - JHE
                // http://msdn.microsoft.com/en-us/library/bb896297(VS.100).aspx
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<Archive>>(
                    (context) => context.Archives.Include("Categories").Include("Translations"));
            }
        }
        #endregion

        public List<Archive> GetRecent100()
        {
            using (NetStepsEntities context = CreateContext())
            {
                return (from o in context.Archives.Take(100)
                        orderby o.ArchiveID descending
                        select o).ToList();
            }
        }

        internal static IQueryable<Archive> FilterByFileType(IQueryable<Archive> matchingItems, ArchiveSearchParameters searchParameters)
        {
            var predicate = PredicateBuilder.False(matchingItems);
            int count = 0;
            foreach (Constants.FileType fileType in searchParameters.FileTypes)
            {
                string[] fileTypeStrings = IO.GetFileTypeExtenstions(fileType);
                foreach (string s in fileTypeStrings)
                {
                    string extension = s; //this looks stupid/redundant at first glance, but prevents the loop from closing over the same 's' variable every time when the predicate is executed.
                    var exp = matchingItems.GetExpression(a => a.ArchivePath.EndsWith(extension));
	                if(count == 0)
	                {
		                predicate = exp;
	                }
                    else
                    {
                        var newExp = exp.Or(predicate);
                        predicate = newExp;
                    }
                    count++;
                }
            }
            return matchingItems.Where(predicate);
        }

        public PaginatedList<ArchiveSearchData> Search(ArchiveSearchParameters searchParameters)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var results = new PaginatedList<ArchiveSearchData>(searchParameters);

                    int languageID = (searchParameters.LanguageID != null) ? searchParameters.LanguageID.ToInt() : ApplicationContext.Instance.CurrentLanguageID;

                    IQueryable<Archive> matchingItems = context.Archives.Include("Translations").Where(a => a.Sites.Select(s => s.SiteID).Contains(searchParameters.SiteID));

                    if (!searchParameters.Query.IsNullOrEmpty())
                        matchingItems = matchingItems.Where(a => a.Translations.Any(t => t.Name.Contains(searchParameters.Query) || t.ShortDescription.Contains(searchParameters.Query)));
                    if (searchParameters.Active.HasValue)
                        matchingItems = matchingItems.Where(a => a.Active == searchParameters.Active.Value);

                    if (searchParameters.IsDownloadable.HasValue)
                        matchingItems = matchingItems.Where(a => a.IsDownloadable == searchParameters.IsDownloadable.Value);

                    if (searchParameters.StartDate.HasValue)
                    {
                        var startDateUTC = searchParameters.StartDate.Value.LocalToUTC();
                        matchingItems = matchingItems.Where(a => !a.StartDateUTC.HasValue || a.StartDateUTC.Value <= startDateUTC);
                    }
                    if (searchParameters.EndDate.HasValue)
                    {
                        var endDateUTC = searchParameters.EndDate.Value.LocalToUTC();
                        matchingItems = matchingItems.Where(a => !a.EndDateUTC.HasValue || a.EndDateUTC.Value >= endDateUTC);
                    }

                    if (searchParameters.WhereClause != null)
                        matchingItems = matchingItems.Where(searchParameters.WhereClause);

                    if (searchParameters.FileTypes != null && searchParameters.FileTypes.Count > 0)
                        matchingItems = FilterByFileType(matchingItems, searchParameters);

                    if (searchParameters.CategoryID.HasValue)
                    {
                        matchingItems = matchingItems.Where(a => a.Categories.Any(c => c.CategoryID == searchParameters.CategoryID)).AsQueryable();
                    }

	                if(!searchParameters.OrderBy.IsNullOrEmpty())
	                {
		                switch(searchParameters.OrderBy)
		                {
			                case "Name":
				                matchingItems = matchingItems.ApplyOrderByFilter(searchParameters.OrderByDirection,
					                a => a.Translations.FirstOrDefault(t => t.LanguageID == languageID).Name);
				                break;
			                case "FileName":
				                matchingItems = matchingItems.ApplyOrderByFilter(searchParameters.OrderByDirection,
					                a => a.ArchivePath);
				                break;
			                default:
				                matchingItems = matchingItems.ApplyOrderByFilter(searchParameters, context);
				                break;
		                }
	                }
	                else
	                {
		                matchingItems = matchingItems.OrderBy(a => a.StartDateUTC);
	                }

                    // TotalCount must be set before applying Pagination - JHE
                    results.TotalCount = matchingItems.Count();

                    matchingItems = matchingItems.ApplyPagination(searchParameters);

                    var archiveInfos = matchingItems.Select(a => new
                                       {
                                           Name = !a.Translations.Any(t => t.LanguageID == ApplicationContext.Instance.CurrentLanguageID) ? a.Translations.Any() ? a.Translations.FirstOrDefault().Name : "" : a.Translations.FirstOrDefault(t => t.LanguageID == ApplicationContext.Instance.CurrentLanguageID).Name,
                                           a.StartDateUTC,
                                           a.EndDateUTC,
                                           a.ArchiveID,
                                           a.Active,
                                           a.ArchivePath,
                                           a.ArchiveImage,
                                           a.IsDownloadable,
                                           a.IsEmailable
                                       }).ToList();

                    results.AddRange(archiveInfos.Select(a => new ArchiveSearchData
                    {
                        Name = a.Name,
                        StartDate = a.StartDateUTC.HasValue ? a.StartDateUTC.ToDateTime().UTCToLocal() : (DateTime?)null,
                        EndDate = a.EndDateUTC.HasValue ? a.EndDateUTC.ToDateTime().UTCToLocal() : (DateTime?)null,
                        ArchiveID = a.ArchiveID,
                        Active = a.Active,
                        ArchivePath = a.ArchivePath,
                        ArchiveImage = a.ArchiveImage,
                        IsDownloadable = a.IsDownloadable,
                        IsEmailable = a.IsEmailable
                    }));

                    return results;
                }
            });
        }

        public List<Archive> LoadAllFullBySiteID(int siteID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return loadAllFullQuery(context).Where(a => a.Sites.Select(s => s.SiteID).Contains(siteID)).ToList();
                }
            });
        }
    }
}