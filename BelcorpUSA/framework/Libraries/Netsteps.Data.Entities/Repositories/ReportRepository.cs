using System;
using System.Collections.Generic;
using System.Data;
using NetSteps.Common.Base;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Repositories
{
	[ContainerRegister(typeof(IReportRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public class ReportRepository : IReportRepository
    {

        public PaginatedList<ReportSearchData> Search(ReportSearchParameters searchParams)
        {
            throw new NotImplementedException();
        }

        public static ReportCategoryCollection LoadAllByCategory()
        {
            IDbCommand dbCommand = null;
            try
            {
                var collection = new ReportCategoryCollection();
                dbCommand = DataAccess.SetCommand("uspLoadReportCategories", "LoadReportCategories", GetReportConnectionString());
                IDataReader reader = DataAccess.ExecuteReader(dbCommand);
                while (reader.Read())
                {
                    var category = new ReportCategory()
                    {
                        CategoryName = DataAccess.GetString("CategoryName", reader),
                        ReportCategoryID = DataAccess.GetInt32("ReportCategoryID", reader),
                        IconUrl = DataAccess.GetString("IconUrl", reader),
                        Function = DataAccess.GetString("Function", reader)
                    };
                    collection.Add(category);
                }
                return collection;
            }
            catch (Exception ex)
            {
                throw new LoadDataException("Unable to load Report Categories", ex);
            }
            finally
            {
                DataAccess.Close(dbCommand);
            }
        }

        private static string _reportConnectionString = string.Empty;
        internal static string GetReportConnectionString()
        {
            if (_reportConnectionString.IsNullOrEmpty())
            {
                _reportConnectionString = EntitiesHelpers.GetAdoConnectionString<NetStepsEntities>();
            }
            return _reportConnectionString;
        }

        public static List<Report> LoadAllByCategory(ReportCategory category)
        {
        	int userID = ApplicationContext.Instance.CurrentUserID;

            IDbCommand dbCommand = null;
            try
            {
                var collection = new List<Report>();
                dbCommand = DataAccess.SetCommand("uspLoadReportsByCategory", "LoadReportsByCategory", GetReportConnectionString());
                DataAccess.AddInputParameter("ReportCategoryID", category.ReportCategoryID, dbCommand);
                IDataReader reader = DataAccess.ExecuteReader(dbCommand);
                while (reader.Read())
                {
                    var report = new Report(category)
                    {
                        Name = DataAccess.GetString("ReportName", reader),
                        Url = DataAccess.GetString("ReportUrl", reader) + "&Token=" + CorporateUser.GetSingleSignOnToken(userID),
                        Description = DataAccess.GetString("ReportDescription", reader)
                    };
                    collection.Add(report);
                }
                return collection;
            }
            catch (Exception ex)
            {
                throw new LoadDataException("Unable to load Reports", ex);
            }
            finally
            {
                DataAccess.Close(dbCommand);
            }
        }
    }
}
