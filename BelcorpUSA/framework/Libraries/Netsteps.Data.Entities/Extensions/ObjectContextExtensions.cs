using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;

namespace NetSteps.Data.Entities.Extensions
{
	using System.Diagnostics.Contracts;

	public static class ObjectContextExtensions
    {
        public static ObjectContext SetReadUncommitted(this ObjectContext tContext)
        {
            tContext.ExecuteStoreCommand("SET NOCOUNT ON; SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            return tContext;
        }

        public static bool IsAttachedTo(this ObjectContext context, object entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            ObjectStateEntry entry;
            if (context.ObjectStateManager.TryGetObjectStateEntry(entity, out entry))
            {
                return (entry.State != EntityState.Detached);
            }

            return false;
        }

		/// <summary>
		/// Checks if all entities are attached to the given <see cref="ObjectContext"/>.
		/// </summary>
		public static bool IsAttachedToAll(this ObjectContext context, IEnumerable<object> entities)
		{
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Requires<ArgumentNullException>(entities != null);
			Contract.Requires<ArgumentException>(entities.All(x => x != null));

			return entities.All(x => context.IsAttachedTo(x));
		}
    }
}
