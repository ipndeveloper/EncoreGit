using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Common.TokenReplacement;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class AlertTemplateRepository : BaseRepository<AlertTemplate, int, NetStepsEntities>, IDefaultImplementation, IAlertTemplateRepository
    {
        protected override Func<NetStepsEntities, int, IQueryable<AlertTemplate>> loadFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, int, IQueryable<AlertTemplate>>(
                    (context, alertTemplateId) => context.AlertTemplates
                                                 .Include("AlertTemplateTranslations")
                                                 .Include("Tokens")
                                                 .Where(a => a.AlertTemplateID == alertTemplateId));
            }
        }

        public List<Alert> GetAlertsForAccount(int accountID, int languageID)
        {
            var alerts = new List<Alert>();
            //Don't use the commissions db as per Lundy
            IDbCommand dbCommand = null;

            try
            {
                dbCommand = DataAccess.SetCommand("usp_alerts_get_by_account_id", connectionString: GetConnectionString());
                DataAccess.AddInputParameter("AccountID", accountID, dbCommand);
                IDataReader reader = DataAccess.ExecuteReader(dbCommand);

                while (reader.Read())
                {
                    Alert alert = GetNextAlert(reader, languageID);
                    alerts.Add(alert);
                    reader.NextResult();
                }

                return alerts;
            }
            finally
            {
                DataAccess.Close(dbCommand);
            }
        }

        protected Alert GetNextAlert(IDataReader reader, int languageID)
        {
            var alert = new Alert();
            alert.AccountAlertID = reader.GetInt32Safe("AccountAlertID");
            alert.AlertTemplateID = reader.GetInt32("AlertTemplateID");
            alert.Priority = (Constants.AlertPriority)reader.GetInt16("AlertPriorityID");
            alert.ActionLinkUrl = reader.GetString("ActionLinkUrl");
            alert.OpenActionLinkInNewWindow = reader.GetBoolean("OpenActionLinkInNewWindow");
            alert.CanBeDismissed = reader.GetBooleanSafe("CanBeDismissed");
            reader.NextResult();

            Dictionary<string, string> tokensAndValues = new Dictionary<string, string>();

            // Get tokens and values for this alert
            while (reader.Read())
            {
                string placeholder = (string)reader["Placeholder"];
                string value = (string)reader["Value"];
                tokensAndValues.Add(placeholder, value);
            }

            alert.Message = GetTokenReplacedAndLocalizedAlertMessage(languageID, alert.AlertTemplateID, tokensAndValues);

            return alert;
        }

        protected string GetTokenReplacedAndLocalizedAlertMessage(int languageID, int alertTemplateID, Dictionary<string, string> tokensAndValues)
        {
            ITokenValueProvider tokenValueProvider = new TokenValueProvider(tokensAndValues);
            TokenReplacer replacer = new TokenReplacer(tokenValueProvider, Constants.BEGIN_TOKEN_DELIMITER, Constants.END_TOKEN_DELIMITER);

            var alertTemplateTranslation = GetAlertTemplateTranslation(alertTemplateID, languageID);


            return replacer.ReplaceTokens(alertTemplateTranslation.Message);
        }

        protected AlertTemplateTranslation GetAlertTemplateTranslation(int alertTemplateID, int preferredLanguageID)
        {
            var alertTemplate = AlertTemplate.LoadFull(alertTemplateID);
            var alertTemplateTranslation = alertTemplate.AlertTemplateTranslations.Where(x => x.LanguageID == preferredLanguageID).FirstOrDefault();

            // if we don't have a translation for the preferred language, grab the first translation in any language
            if (alertTemplateTranslation == null)
            {
                alertTemplateTranslation = alertTemplate.AlertTemplateTranslations.FirstOrDefault();
            }

            if (alertTemplateTranslation == null)
            {
                string exceptionMessage = "Could not find any translations for AlertTemplate \"" + alertTemplate.Name + "\"";
                throw EntityExceptionHelper.GetAndLogNetStepsException(exceptionMessage);
            }

            return alertTemplateTranslation;
        }

        private static string _connectionString = string.Empty;
        internal static string GetConnectionString()
        {
            if (_connectionString.IsNullOrEmpty())
            {
                using (NetStepsEntities context = CreateContext())
                {
                    IDbConnection conn = (context.Connection as EntityConnection).StoreConnection;
                    _connectionString = conn.ConnectionString;
                }
            }

            return _connectionString;
        }


        public PaginatedList<AlertTemplate> Search(AlertTemplateSearchParameters searchParameters)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
                {
                    using (NetStepsEntities context = CreateContext())
                    {
                        PaginatedList<AlertTemplate> results = new PaginatedList<AlertTemplate>(searchParameters);

                        IQueryable<AlertTemplate> alertTemplates = context.AlertTemplates; // this.loadAllFullQuery(context);

                        if (searchParameters.AlertTemplateID.HasValue)
                        {
                            alertTemplates = alertTemplates.Where(x => x.AlertTemplateID == searchParameters.AlertTemplateID.Value);
                        }

                        if (!String.IsNullOrEmpty(searchParameters.StoredProcedureName))
                        {
                            alertTemplates = alertTemplates.Where(x => x.StoredProcedureName.Contains(searchParameters.StoredProcedureName));
                        }

                        if (!String.IsNullOrEmpty(searchParameters.Name))
                        {
                            alertTemplates = alertTemplates.Where(x => x.Name.Contains(searchParameters.Name));
                        }

                        if (searchParameters.Active.HasValue)
                        {
                            alertTemplates = alertTemplates.Where(x => x.Active == searchParameters.Active);
                        }

                        if (searchParameters.AlertPriorityID.HasValue)
                        {
                            alertTemplates = alertTemplates.Where(x => x.AlertPriorityID == searchParameters.AlertPriorityID.Value);
                        }

                        if (searchParameters.WhereClause != null)
                        {
                            alertTemplates = alertTemplates.Where(searchParameters.WhereClause);
                        }

                        if (!String.IsNullOrEmpty(searchParameters.OrderBy))
                        {
                            alertTemplates = alertTemplates.ApplyOrderByFilter(searchParameters, context);
                        }
                        else
                        {
                            alertTemplates = alertTemplates.OrderBy(x => x.AlertTemplateID);
                        }

                        results.TotalCount = alertTemplates.Count();

                        alertTemplates = alertTemplates.ApplyPagination(searchParameters);

                        results.AddRange(alertTemplates);

                        return results;
                    }
                });
        }
    }
}
