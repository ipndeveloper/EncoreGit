using System.Linq;

namespace NetSteps.Data.Entities
{
    public partial class NewsType
    {
        /// <param name="newsTypeCache">if this has a value set, it will make sure all news types in the cache have a sort index for the given language</param>
        public int GetSortIndexByLanguage(int languageID, NetSteps.Data.Entities.Cache.SmallCollectionCache.NewsTypeCache newsTypeCache = null)
        {
            if (newsTypeCache != null)
            {
                int maxSortIndex = 0;
                maxSortIndex = newsTypeCache.Max(nt => nt.NewsTypeLanguageSorts.Any(ls => ls.LanguageID == languageID)
                    ? nt.NewsTypeLanguageSorts.Where(ls => ls.LanguageID == languageID).Max(ls => ls.SortIndex) : 0);

                foreach (var newsType in newsTypeCache.Where(nt => !nt.NewsTypeLanguageSorts.Any(ls => ls.LanguageID == languageID)).OrderBy(nt => nt.NewsTypeID))
                {
                    newsType.StartEntityTracking();
                    var newLS = new NewsTypeLanguageSort()
                        {
                            NewsTypeID = newsType.NewsTypeID,
                            LanguageID = languageID,
                            SortIndex = ++maxSortIndex
                        };
                    newsType.NewsTypeLanguageSorts.Add(newLS);
                    newsType.Save();
                }
            }

            int sortIndex = 0;
            if (this.NewsTypeLanguageSorts.Any(ls => ls.LanguageID == languageID))
                sortIndex = this.NewsTypeLanguageSorts.FirstOrDefault(ls => ls.LanguageID == languageID).SortIndex;

            return sortIndex;
        }
    }
}
