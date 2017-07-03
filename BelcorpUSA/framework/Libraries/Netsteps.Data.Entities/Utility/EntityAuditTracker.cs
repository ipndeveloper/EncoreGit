using System;
using System.Collections.Generic;
using System.Data.Objects;
using NetSteps.Common;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Utility
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Prototype class to log Audit changes to DB.
    /// Created: 03-12-2010
    /// </summary>
    public class EntityAuditTracker
    {
        public static void LogAuditedChanges(IObjectWithChangeTracker obj, ObjectContext context, Type entityType, PrimaryKeyInfo entityPrimaryKeyInfo)
        {
            if (!(obj is IAuditedEntity))
                return;

            // Don't track changes for Inserts - JHE
            if (Convert.ToInt32(entityPrimaryKeyInfo.PropertyInfo.GetValue(obj, null)) == 0)
                return;

            string entitySetName = EntitiesHelpers.GetEntitySetName(context, entityType);

            List<AuditLog> changes = new List<AuditLog>();
            foreach (var key in obj.ChangeTracker.OriginalValues.Keys)
            {
                changes.Add(new AuditLog()
                {
                    AuditTableID = ((Constants.AuditTable)Enum.Parse(Constants.AuditTable.NotSet.GetType(), entitySetName)).ToInt16(),
                    AuditTableName = entitySetName,
                    UserID = ApplicationContext.Instance.CurrentUser.UserID,
                    ApplicationID = ApplicationContext.Instance.ApplicationID,
                    ColumnName = key,
                    AuditChangeTypeID = Constants.AuditChangeType.Update.ToByte(),
                    DateModified = DateTime.Now,
                    OldValue = entityType.GetPropertyCached(key).GetValue(obj, null).ToString(),
                    NewValue = obj.ChangeTracker.OriginalValues[key].ToString(),
                });
            }

            foreach (var change in changes)
            {
                var repo = new AuditLogRepository();
                repo.Save(change);
            }
        }
    }
}
