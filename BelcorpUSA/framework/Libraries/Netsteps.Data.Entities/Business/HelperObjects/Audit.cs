using System;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business.Interfaces;

namespace NetSteps.Data.Entities
{
	public class Audit
	{
		public static void UpdateAuditFields(object auditedEntity)
		{
			UpdateCreatedFields(auditedEntity);
			UpdateModifiedFields(auditedEntity);
		}

		public static void UpdateCreatedFields(object auditedEntity)
		{
			if (auditedEntity != null && auditedEntity is IDateCreated)
			{
				IDateCreated entity = auditedEntity as IDateCreated;
				if (entity.DateCreated.IsNullOrEmpty())
					entity.DateCreated = DateTime.Now;
			}
			if (auditedEntity != null && auditedEntity is ICreatedByUserID)
			{
				ICreatedByUserID entity = auditedEntity as ICreatedByUserID;
				if (!entity.CreatedByUserID.HasValue())
					entity.CreatedByUserID = ApplicationContext.Instance.CurrentUserID.ToIntNullable();
			}
		}

		public static void UpdateModifiedFields(object auditedEntity)
        {
            if (auditedEntity != null && auditedEntity is IObjectWithChangeTracker 
                && (auditedEntity as IObjectWithChangeTracker).ChangeTracker.State != ObjectState.Unchanged)
            {
                if (auditedEntity is IDateLastModifiedNullable)
                {
                    IDateLastModifiedNullable entity = auditedEntity as IDateLastModifiedNullable;
                    entity.DateLastModified = DateTime.Now;
                }

				if(auditedEntity is IDateLastModified)
				{
					IDateLastModified entity = auditedEntity as IDateLastModified;
					entity.DateLastModified = DateTime.Now;
				}

                if (auditedEntity is IModifiedByUserID)
                {
                    IModifiedByUserID entity = auditedEntity as IModifiedByUserID;
                    entity.ModifiedByUserID = ApplicationContext.Instance.CurrentUserID.ToIntNullable();
                }
            }
		}
	}
}
