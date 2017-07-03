using NetSteps.Common.Extensions;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class ErrorLogBusinessLogic
    {
        public override void CleanDataBeforeSave(Repositories.IErrorLogRepository repository, ErrorLog entity)
        {
            // Truncate all strings to max string limits on DB to ensure that that ErrorLog can be saved. - JHE
            int? maxLength = entity.ValidationRules.GetMaxLengthRuleLength("MachineName", "StringMaxLength");
            if (maxLength.HasValue && entity.MachineName.ToCleanString().Length > maxLength.Value)
                entity.MachineName = entity.MachineName.Truncate(maxLength.Value);

            maxLength = entity.ValidationRules.GetMaxLengthRuleLength("ExceptionTypeName", "StringMaxLength");
            if (maxLength.HasValue && entity.ExceptionTypeName.ToCleanString().Length > maxLength.Value)
                entity.ExceptionTypeName = entity.ExceptionTypeName.Truncate(maxLength.Value);

            maxLength = entity.ValidationRules.GetMaxLengthRuleLength("Source", "StringMaxLength");
            if (maxLength.HasValue && entity.Source.ToCleanString().Length > maxLength.Value)
                entity.Source = entity.Source.Truncate(maxLength.Value);

            maxLength = entity.ValidationRules.GetMaxLengthRuleLength("Message", "StringMaxLength");
            if (maxLength.HasValue && entity.Message.ToCleanString().Length > maxLength.Value)
                entity.Message = entity.Message.Truncate(maxLength.Value);

            maxLength = entity.ValidationRules.GetMaxLengthRuleLength("PublicMessage", "StringMaxLength");
            if (maxLength.HasValue && entity.PublicMessage.ToCleanString().Length > maxLength.Value)
                entity.PublicMessage = entity.PublicMessage.Truncate(maxLength.Value);

            maxLength = entity.ValidationRules.GetMaxLengthRuleLength("QueryString", "StringMaxLength");
            if (maxLength.HasValue && entity.QueryString.ToCleanString().Length > maxLength.Value)
                entity.QueryString = entity.QueryString.Truncate(maxLength.Value);

            maxLength = entity.ValidationRules.GetMaxLengthRuleLength("TargetSite", "StringMaxLength");
            if (maxLength.HasValue && entity.TargetSite.ToCleanString().Length > maxLength.Value)
                entity.TargetSite = entity.TargetSite.Truncate(maxLength.Value);

            maxLength = entity.ValidationRules.GetMaxLengthRuleLength("StackTrace", "StringMaxLength");
            if (maxLength.HasValue && entity.StackTrace.ToCleanString().Length > maxLength.Value)
                entity.StackTrace = entity.StackTrace.Truncate(maxLength.Value);

            maxLength = entity.ValidationRules.GetMaxLengthRuleLength("Referrer", "StringMaxLength");
            if (maxLength.HasValue && entity.Referrer.ToCleanString().Length > maxLength.Value)
                entity.Referrer = entity.Referrer.Truncate(maxLength.Value);

            maxLength = entity.ValidationRules.GetMaxLengthRuleLength("BrowserInfo", "StringMaxLength");
            if (maxLength.HasValue && entity.BrowserInfo.ToCleanString().Length > maxLength.Value)
                entity.BrowserInfo = entity.BrowserInfo.Truncate(maxLength.Value);

            maxLength = entity.ValidationRules.GetMaxLengthRuleLength("ApplicationPoolName", "StringMaxLength");
            if (maxLength.HasValue && entity.ApplicationPoolName.ToCleanString().Length > maxLength.Value)
                entity.BrowserInfo = entity.ApplicationPoolName.Truncate(maxLength.Value);
        }
    }
}

