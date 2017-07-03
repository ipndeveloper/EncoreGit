using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Core.Cache;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Common.Services;
using NetSteps.Encore.Core.IoC;

namespace DependentClass
{

    public class TitleService : ITitleService
    {
        public IAccountTitle GetAccountTitle(int accountID, int titleTypeID, int? periodID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAccountTitle> GetAccountTitles(int accountID, int? periodID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ITitle> GetTitles()
        {
            throw new NotImplementedException();
        }
    }

    //public class TitleService : ITitleService
    //{
    //    private static SmallCollectionCache _cache = SmallCollectionCache.Instance;
    //    private static ICache<Tuple<int, int>, IEnumerable<IAccountTitle>> accountTitlesCache = new ActiveMruLocalMemoryCache<Tuple<int, int>, IEnumerable<IAccountTitle>>("accountTitles", new DelegatedDemuxCacheItemResolver<Tuple<int, int>, IEnumerable<IAccountTitle>>(ResolveAccountTitleList));

    //    public IEnumerable<ITitle> GetTitles()
    //    {
    //        return getTitles();
    //    }

    //    private static IEnumerable<ITitle> getTitles()
    //    {
    //        return _cache.Titles.Select(t =>
    //        {
    //            var title = Create.New<ITitle>();
    //            title.TitleID = t.TitleID;
    //            title.SortOrder = t.SortOrder;
    //            title.TermName = t.TermName != null ? t.TermName : t.TitleCode;
    //            title.TitleCode = t.TitleCode;
    //            title.Active = t.Active;
    //            return title;
    //        });
    //    }

    //    public IAccountTitle GetAccountTitle(int accountID, int titleTypeID, int? periodID)
    //    {
    //        if (!periodID.HasValue)
    //        {
    //            periodID = _cache.Periods.GetCurrentPeriod().PeriodID;
    //        }

    //        IEnumerable<IAccountTitle> titles = null;
    //        if (accountTitlesCache.TryGet(new Tuple<int, int>(accountID, periodID ?? _cache.Periods.GetCurrentPeriod().PeriodID), out titles))
    //        {
    //            return titles.FirstOrDefault(title => title.TitleTypeID == titleTypeID);
    //        }
    //        else
    //        {
    //            throw new TitleNotFoundException(accountID, titleTypeID, periodID.Value);
    //        }
    //    }

    //    public class TitleNotFoundException : Exception
    //    {
    //        public int AccountID { get; set; }
    //        public int? TitleTypeID { get; set; }
    //        public int PeriodID { get; set; }

    //        public TitleNotFoundException(int accountID, int titleTypeID, int periodID)
    //        {
    //            AccountID = accountID;
    //            TitleTypeID = titleTypeID;
    //            PeriodID = periodID;
    //        }

    //        public TitleNotFoundException(int accountID, int periodID)
    //        {
    //            AccountID = accountID;
    //            PeriodID = periodID;
    //        }
    //    }

    //    public IEnumerable<IAccountTitle> GetAccountTitles(int accountID, int? periodID)
    //    {
    //        IEnumerable<IAccountTitle> titles = null;
    //        if (accountTitlesCache.TryGet(new Tuple<int, int>(accountID, periodID ?? _cache.Periods.GetCurrentPeriod().PeriodID), out titles))
    //        {
    //            return titles;
    //        }
    //        else
    //        {
    //            throw new TitleNotFoundException(accountID, accountID, periodID ?? _cache.Periods.GetCurrentPeriod().PeriodID);
    //        }
    //    }

    //    private static bool ResolveAccountTitleList(Tuple<int, int> key, out IEnumerable<IAccountTitle> value)
    //    {
    //        var accountID = key.Item1;
    //        var periodID = key.Item2;

    //        List<Commissions_AccountTitles_View> matches;
    //        using (var context = new NetStepsEntities())
    //        {
    //            matches = context.Commissions_AccountTitles_View.Where(a => a.PeriodID == periodID && a.AccountID == accountID).ToList();
    //        }
    //        if (matches == null)
    //        {
    //            throw new TitleService.TitleNotFoundException(accountID, periodID);
    //        }
    //        var accountTitles = new List<IAccountTitle>();
    //        foreach (var match in matches)
    //        {
    //            var accountTitle = Create.New<IAccountTitle>();
    //            accountTitle.AccountID = match.AccountID;
    //            accountTitle.TitleTypeID = match.TitleTypeID;
    //            accountTitle.PeriodID = match.PeriodID;
    //            accountTitle.Title = getTitles().Single(t => t.TitleID == match.TitleID);
    //            accountTitles.Add(accountTitle);
    //        }
    //        value = accountTitles;
    //        if (value == null)
    //        {
    //            return false;
    //        }
    //        else
    //        {
    //            return true;
    //        }
    //    }
    //}

    //public class DefaultTitleService : ITitleService
    //{
    //    public IAccountTitle GetAccountTitle(int accountID, int titleTypeID, int? periodID)
    //    {
    //        return Create.New<IAccountTitle>();
    //    }

    //    public IEnumerable<IAccountTitle> GetAccountTitles(int accountID, int? periodID)
    //    {
    //        return Enumerable.Empty<IAccountTitle>();
    //    }

    //    public IEnumerable<ITitle> GetTitles()
    //    {
    //        return Enumerable.Empty<ITitle>();
    //    }
    //}

    //[ContainerRegister(typeof(IAccountTitle), RegistrationBehaviors.Default)]
    //public class AccountTitle : IAccountTitle
    //{
    //    public int AccountID { get; set; }
    //    public int PeriodID { get; set; }
    //    public ITitle Title { get; set; }
    //    public int TitleTypeID { get; set; }
    //}

    //[ContainerRegister(typeof(ITitle), RegistrationBehaviors.Default)]
    //public class Title : ITitle
    //{
    //    public int TitleID { get; set; }
    //    public int SortOrder { get; set; }
    //    public string TermName { get; set; }
    //    public string TitleCode { get; set; }
    //    public bool Active { get; set; }
    //}
}
