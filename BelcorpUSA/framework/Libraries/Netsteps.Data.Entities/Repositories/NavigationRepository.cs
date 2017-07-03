using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class NavigationRepository
    {
        #region Members
        #endregion

        // Port of usp_navigation_select_single_level - JHE
        public List<Navigation> LoadSingleLevelNav(int siteId, int navigationTypeId, int? parentId, bool showAll)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    Site site = context.Sites.FirstOrDefault(s => s.SiteID == siteId);

                    if (site.SiteTypeID == Constants.SiteType.Replicated.ToInt() && site.BaseSiteID.ToInt() > 0)
                        siteId = site.BaseSiteID.ToInt();

					//Null cannot equal null for the parent IDs.  Added logic to set null to be 0 when comparing.
					var nav = context.Navigations.Where(n => n.SiteID == siteId && (n.ParentID ?? 0) == (parentId ?? 0) && ((!showAll && n.Active) || showAll)).OrderBy(n => n.SortIndex);

                    return nav.ToList();
                }
            });
        }



        public override void Delete(Navigation obj)
        {
            Delete(obj.NavigationID);
        }
        public override void Delete(int primaryKey)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var obj = context.Navigations
                                            .Include("Translations")
                                            .FirstOrDefault(pb => pb.NavigationID == primaryKey);
                    if (obj == null)
                        return;

                    obj.StartEntityTracking();

                    context.DeleteObjects(obj.Translations);
                    context.DeleteObject(obj);

                    Save(obj, context);
                }
            });
        }

    }
}
