using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Linq;
using System.Reflection;
using NetSteps.Common.Attributes;

namespace NetSteps.Common.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: ObjectContext Extensions to extend the functionality of LINQ to Entities
	/// Created: 06-09-2010
	/// </summary>
	public static class ObjectContextExtensions
	{
		public static EdmMember GetEntityKey(this ObjectContext context, Type entityType)
		{
			return GetEntityKeys(context, entityType).FirstOrDefault();
		}
		public static List<EdmMember> GetEntityKeys(this ObjectContext context, Type entityType)
		{
            return context.MetadataWorkspace
                .GetEntityContainer(context.DefaultContainerName, DataSpace.CSpace)
                .BaseEntitySets
                .Where(e => e.ElementType.Name == entityType.Name)
                .Select(e => e.ElementType.KeyMembers.ToList())
                .FirstOrDefault()
                ?? new List<EdmMember>();
		}

		/// <summary>
		/// Will return the EntitySetName (table name in the DB) of an Entity. - JHE
		/// http://www.scip.be/index.php?Page=ArticlesNET24
		/// </summary>
		/// <param name="context"></param>
		/// <param name="entityType"></param>
		/// <returns></returns>
		public static string GetEntitySetName(this ObjectContext context, Type entityType)
		{
            var entitySetName = EntityInfoCache.EntitySetNames.GetOrAdd(entityType, x =>
            {
                return context.MetadataWorkspace
                    .GetEntityContainer(context.DefaultContainerName, DataSpace.CSpace)
                    .BaseEntitySets
                    .Where(e => e.ElementType.Name == entityType.Name)
                    .Select(e => e.Name)
                    .FirstOrDefault();
            });

            // Don't leave null values in the cache
            if (entitySetName == null)
            {
                EntityInfoCache.EntitySetNames.TryRemove(entityType, out entitySetName);
                return null;
            }

            return entitySetName;
		}

		/// <summary>
		/// Returns the Max string length in the DataBase by looking it up in Entity Framework MetaData - JHE
		/// http://stackoverflow.com/questions/748939/field-max-length-in-entity-framwork - JHE
		/// </summary>
		/// <param name="context"></param>
		/// <param name="entityType"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public static int? GetMaxLength(this ObjectContext context, Type entityType, string propertyName)
		{
			int? result = null;

            //var q = from meta in context.MetadataWorkspace.GetItems(DataSpace.CSpace)
            //                   .Where(m => m.BuiltInTypeKind == BuiltInTypeKind.EntityType)
            //        from p in (meta as EntityType).Properties
            //           .Where(p => p.DeclaringType.Name == entityType.Name
            //               && p.Name == propertyName
            //               && p.TypeUsage.EdmType.Name == "String")
            //        select p;

			var queryResult = (from meta in context.MetadataWorkspace.GetItems(DataSpace.CSpace)
							   .Where(m => m.BuiltInTypeKind == BuiltInTypeKind.EntityType)
							  from p in (meta as EntityType).Properties
								 .Where(p => p.DeclaringType.Name == entityType.Name
									 && p.Name == propertyName
									 && p.TypeUsage.EdmType.Name == "String")
							  select p.TypeUsage.Facets["MaxLength"].Value).ToList();
			if (queryResult.Count() > 0)
			{
				result = (int?)Convert.ChangeType(queryResult.First(), typeof(int?));
				//result = (Nullable<int>)queryResult.First();
			}

			return result;
		}

		#region GetPrimaryKeyInfo Methods
		/// <summary>
		/// This will first attempt to lookup the primary key by checking for an C# attribute on the Primary Key Property.
		/// Ex:     [LoadByPrimaryKey]
		///         int OrderID { get; set; }
		///         
		/// If this does not exist then the Primary key will be looked up in the Entity Framework Metadata.
		/// If more that 1 primary keys exists, null will be returned, but can be resolved by specifying which 
		///     key should be used as above ([LoadByPrimaryKey]) - JHE
		/// </summary>
		public static PrimaryKeyInfo GetPrimaryKeyInfo(this ObjectContext context, Type entityType)
		{
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (entityType == null)
            {
                throw new ArgumentNullException("entityType");
            }

            var primaryKeyInfo = EntityInfoCache.EntityPrimaryKeys.GetOrAdd(entityType, x =>
            {
                var primaryKeyProperty =
                    GetPrimaryKeyPropertyUsingAttribute(x)
                    ?? GetPrimaryKeyPropertyUsingEntity(context, x);

                return primaryKeyProperty != null
                    ? new PrimaryKeyInfo
                        {
                            ColumnName = primaryKeyProperty.Name,
                            PropertyInfo = primaryKeyProperty
                        }
                    : null;
            });

            if (primaryKeyInfo == null)
            {
                // Don't leave null values in the cache
                EntityInfoCache.EntityPrimaryKeys.TryRemove(entityType, out primaryKeyInfo);
                return null;
            }

            return primaryKeyInfo;
		}

		/// <summary>
		/// Gets Primary key using Entity Framework Metadata - JHE
		/// </summary>
		public static PropertyInfo GetPrimaryKeyPropertyUsingEntity(ObjectContext context, Type entityType)
		{
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (entityType == null)
            {
                throw new ArgumentNullException("entityType");
            }

            var keys = context.GetEntityKeys(entityType);
            return keys.Count == 1
                ? entityType.GetPropertyCached(keys[0].Name)
                : null;
		}

		/// <summary>
		/// Get Primary key using LoadByPrimaryKeyAttribute - JHE
		/// </summary>
        public static PropertyInfo GetPrimaryKeyPropertyUsingAttribute(Type entityType)
		{
            if (entityType == null)
            {
                throw new ArgumentNullException("entityType");
            }

            return entityType
                .GetPropertiesByAttribute<LoadByPrimaryKeyAttribute>()
                .FirstOrDefault()
                ??
                entityType
                .GetMetadataPropertiesByAttribute<LoadByPrimaryKeyAttribute>()
                .FirstOrDefault();
		}
		#endregion

		public static void DeleteObjects<T>(this ObjectContext context, IList<T> trackingItems)
		{
			if (trackingItems != null)
			{
				foreach (var item in trackingItems.ToList())
				{
					context.DeleteObject(item);
				}
			}
		}
	}
}
