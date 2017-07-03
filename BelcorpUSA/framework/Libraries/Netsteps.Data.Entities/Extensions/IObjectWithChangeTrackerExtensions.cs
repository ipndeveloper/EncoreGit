using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Validation.NetTiers;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Interfaces;

namespace NetSteps.Data.Entities.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: IObjectWithChangeTracker Extensions
	/// Created: 06-09-2010
	/// </summary>
	public static class IObjectWithChangeTrackerExtensions
	{
		#region IObjectWithChangeTracker Property Cache
		// Creating a list of Property Names per Type that need to be called recursively to improve performance. - JHE
		private static readonly ConcurrentDictionary<Type, List<PropertyInfo>> changeTrackerPropertyCache = new ConcurrentDictionary<Type, List<PropertyInfo>>();
		private static List<PropertyInfo> GetChangeTrackerPropertiesCached(IObjectWithChangeTracker trackingItem)
		{
			Type entityType = trackingItem.GetType();
			return changeTrackerPropertyCache.GetOrAdd(entityType,
			 key =>
			 {
				 List<PropertyInfo> changeTrackerProperties = new List<PropertyInfo>();

				 var props = key.GetPropertiesCached();
				 //var ignoreProps = entityType.GetPropertiesByAttribute(typeof(IgnoreOnValidationAttribute));
				 //props.RemoveWhere(p => ignoreProps.Contains(p, new LambdaComparer<PropertyInfo>((x, y) => x.Name == y.Name)));
				 foreach (PropertyInfo p in props)
				 {
					 if (p.PropertyType.Name == "TrackableCollection`1" && p.CanRead)
						 changeTrackerProperties.Add(p);
					 else if (p.CanRead && p.PropertyType.Name.ToLower() != "String".ToLower() && p.PropertyType.GetInterface("IObjectWithChangeTracker") != null)
						 changeTrackerProperties.Add(p);
				 }

				 return changeTrackerProperties;
			 });
		}
		#endregion

		/// <summary>
		/// Author: John Egbert
		/// Description: Method to recursively call AcceptChanges() on child objects modified - JHE
		/// Created: 02-18-2010
		/// </summary>
		/// 
		public static void AcceptAllChangesRecursive(this IObjectWithChangeTracker trackingItem, List<IObjectWithChangeTracker> allTrackerItems = null)
		{
			ForEachTrackingItemRecursiveNew(trackingItem, (ti) => { ti.AcceptChanges(); }, true, true, allTrackerItems);
		}

		public static void AcceptAllChanges(this IObjectWithChangeTracker trackingItem)
		{
			AcceptAllChanges(trackingItem, new List<IObjectWithChangeTracker>());
		}

		public static void AcceptAllChanges(this IObjectWithChangeTracker trackingItem, List<IObjectWithChangeTracker> processedObjects)
		{
			foreach (var list in trackingItem.ChangeTracker.ObjectsAddedToCollectionProperties.Values.ToList())
			{
				foreach (var item in list)
				{
					if (item is IObjectWithChangeTracker && item as IObjectWithChangeTracker != null)
					{
						IObjectWithChangeTracker i = item as IObjectWithChangeTracker;
						if (!processedObjects.Contains(i))
						{
							processedObjects.Add(i);
							AcceptAllChanges(i, processedObjects);
						}
					}
				}
			}

			foreach (var list in trackingItem.ChangeTracker.ObjectsRemovedFromCollectionProperties.Values.ToList())
			{
				foreach (var item in list)
				{
					if (item is IObjectWithChangeTracker && item as IObjectWithChangeTracker != null)
					{
						IObjectWithChangeTracker i = item as IObjectWithChangeTracker;
						if (!processedObjects.Contains(i))
						{
							processedObjects.Add(i);
							AcceptAllChanges(i, processedObjects);
						}
					}
				}
			}

			trackingItem.AcceptChanges();
		}

		public static void StartTrackingRecursive(this IObjectWithChangeTracker trackingItem)
		{
			ForEachTrackingItemRecursiveNew(trackingItem, (ti) => { ti.StartTracking(); }, true, true);
		}

		public static void StopTrackingRecursive(this IObjectWithChangeTracker trackingItem)
		{
			ForEachTrackingItemRecursiveNew(trackingItem, (ti) => { ti.StopTracking(); }, true, true);
		}

		public static void StartTrackingAndEnableLazyLoadingRecursive(this IObjectWithChangeTracker trackingItem)
		{
			Action<IObjectWithChangeTracker> func = (ti) =>
			{
				ti.StartTracking();
				if (ti is IIsLazyLoadingEnabled)
				{
					IIsLazyLoadingEnabled entity = (ti as IIsLazyLoadingEnabled);
					entity.IsLazyLoadingEnabled = true;
				}
			};

			ForEachTrackingItemRecursiveNew(trackingItem, func, true, true);
		}

		public static void EnableLazyLoadingRecursive(this IObjectWithChangeTracker trackingItem)
		{
			Action<IObjectWithChangeTracker> func = (ti) =>
			{
				if (ti is IIsLazyLoadingEnabled)
				{
					IIsLazyLoadingEnabled entity = (ti as IIsLazyLoadingEnabled);
					entity.IsLazyLoadingEnabled = true;
				}
			};

			ForEachTrackingItemRecursiveNew(trackingItem, func, true, true);

		}

		public static void DisableLazyLoadingRecursive(this IObjectWithChangeTracker trackingItem, List<IObjectWithChangeTracker> allTrackerItems = null)
		{
			Action<IObjectWithChangeTracker> func = (ti) =>
			{
				if (ti is IIsLazyLoadingEnabled)
				{
					IIsLazyLoadingEnabled entity = (ti as IIsLazyLoadingEnabled);
					entity.IsLazyLoadingEnabled = false;
				}
			};

			ForEachTrackingItemRecursiveNew(trackingItem, func, true, true, allTrackerItems);
		}

		/// <summary>
		/// Method to recursively check that an Entity is valid according to Business rules of the Entity
		/// and each child entity. - JHE
		/// </summary>
		/// <param name="trackingItem"></param>
		/// <returns></returns>
		public static ValidationResult IsValidRecursive(this IObjectWithChangeTracker trackingItem)
		{
			ValidationResult result = new ValidationResult();
			List<IObjectWithChangeTracker> processedObjects = new List<IObjectWithChangeTracker>();
			List<IValidation> invalidObjects = new List<IValidation>();

			Action<IObjectWithChangeTracker> func = (ti) =>
			{
				if (ti == null)
				{ return; }

				if (!(ti is IValidation))
				{ return; }

				IValidation entity = (ti as IValidation);
				entity.Validate();
				if (entity.IsValid)
				{ return; }

				if (invalidObjects.Contains(entity))
				{ return; }

				invalidObjects.Add(entity);

				string entityName = entity.GetType().Name;
				foreach (var item in entity.BrokenRulesList)
					item.EntityName = entityName;


				// Remove all broken rules for properties that the parent object should set - JHE
				//if (trackingItem != entity && trackingItem is IValidation)
				//{
				IValidation parentEntity = trackingItem as IValidation;
				entity.BrokenRulesList.RemoveWhere(br => parentEntity.ValidatedChildPropertiesSetByParent().ContainsIgnoreCase(br.Property));
				//}

				result.BrokenRulesList.AddRange(entity.BrokenRulesList);
			};

			ForEachTrackingItemRecursiveNew(trackingItem, func, true, true);
			return result;
		}

		/// <summary>
		/// This methods recursively checks the object and all child object to see if it or a child entity
		/// has changed (obj.ChangeTracker.State != ObjectState.Unchanged). - JHE
		/// </summary>
		/// <param name="trackingItem"></param>
		/// <returns></returns>
		public static bool IsModifiedRecursive(this IObjectWithChangeTracker trackingItem, List<IObjectWithChangeTracker> allTrackerItems = null)
		{
			bool isModified = false;

			Action<IObjectWithChangeTracker> func = (ti) =>
			{
				if (ti.ChangeTracker.State != ObjectState.Unchanged)
					isModified = true;
			};

			ForEachTrackingItemRecursiveNew(trackingItem, func, true, true, allTrackerItems);
			return isModified;
		}


		public static void CleanDataBeforeSaveRecursive(this IObjectWithChangeTracker trackingItem)
		{
			Action<IObjectWithChangeTracker> func = (ti) =>
			{
				if (ti is ICleanDataBeforeSave)
					(ti as ICleanDataBeforeSave).CleanDataBeforeSave();
			};

			ForEachTrackingItemRecursiveNew(trackingItem, func, true, true);
		}

		public static void UpdateAuditFieldsRecursive(this IObjectWithChangeTracker trackingItem, List<IObjectWithChangeTracker> allTrackerItems = null)
		{
			Action<IObjectWithChangeTracker> func = (ti) =>
			{
				if (ti is IObjectWithChangeTracker)
					Audit.UpdateAuditFields(ti);
			};

			ForEachTrackingItemRecursiveNew(trackingItem, func, true, true, allTrackerItems);
		}

		/// <summary>
		/// Author: John Egbert
		/// Description: Method to Reflect on the properties of a IObjectWithChangeTracker and find collections of
		/// IObjectWithChangeTracker objects and recursively perform an action on each object to essentially
		/// perform an action on all child IObjectWithChangeTracker of an object graph if called with callRecursively = true.
		/// http://blogs.msdn.com/adonet/pages/feature-ctp-walkthrough-self-tracking-entities-for-the-entity-framework.aspx
		/// Created: 02-19-2010
		/// </summary>
		public static void ForEachTrackingItemRecursive(this IObjectWithChangeTracker trackingItem, Action<IObjectWithChangeTracker> action, List<IObjectWithChangeTracker> processedObjects, bool callRecursively, Func<bool> returnIfTrue = null, bool resetLazyLoading = true, bool processMainEntityFirst = false)
		{
			if (trackingItem == null)
				throw new ArgumentNullException("trackingItem");

			bool lazyLoad = (trackingItem as IIsLazyLoadingEnabled).IsLazyLoadingEnabled;
			if (resetLazyLoading)
				(trackingItem as IIsLazyLoadingEnabled).IsLazyLoadingEnabled = false;

			if (returnIfTrue != null && returnIfTrue())
			{
				if (resetLazyLoading)
					(trackingItem as IIsLazyLoadingEnabled).IsLazyLoadingEnabled = lazyLoad;
				return;
			}

			if (processMainEntityFirst)
			{
				if (!processedObjects.Contains(trackingItem))
				{
					processedObjects.Add(trackingItem);
					action(trackingItem);
				}
			}

			var trackerProps = GetChangeTrackerPropertiesCached(trackingItem);

			foreach (PropertyInfo p in trackerProps)
			{
				if (returnIfTrue != null && returnIfTrue())
				{
					if (resetLazyLoading)
						(trackingItem as IIsLazyLoadingEnabled).IsLazyLoadingEnabled = lazyLoad;
					return;
				}

				if (p.PropertyType.Name == "TrackableCollection`1")
				{
					IList list = (IList)p.GetValue(trackingItem, null);
					if (list != null)
					{
						foreach (IObjectWithChangeTracker b in list)
						{
							if (returnIfTrue != null && returnIfTrue())
							{
								if (resetLazyLoading)
									(trackingItem as IIsLazyLoadingEnabled).IsLazyLoadingEnabled = lazyLoad;
								return;
							}

							if (list != null && callRecursively && !processedObjects.Contains(b))
							{
								if (!processMainEntityFirst)
									processedObjects.Add(b);
								b.ForEachTrackingItemRecursive(action, processedObjects, callRecursively, returnIfTrue, resetLazyLoading, processMainEntityFirst);
							}
							action(b);
							if (returnIfTrue != null && returnIfTrue())
							{
								if (resetLazyLoading)
									(trackingItem as IIsLazyLoadingEnabled).IsLazyLoadingEnabled = lazyLoad;
								return;
							}
						}
					}
				}
				else
				{
					try
					{
						IObjectWithChangeTracker item = (IObjectWithChangeTracker)p.GetValue(trackingItem, null);
						if (item != null && callRecursively && !processedObjects.Contains(item))
						{
							if (!processMainEntityFirst)
								processedObjects.Add(item);
							item.ForEachTrackingItemRecursive(action, processedObjects, callRecursively, returnIfTrue, resetLazyLoading, processMainEntityFirst);
						}
					}
					catch (Exception ex)
					{
						throw ex;
					}
				}
			}

			if (!processMainEntityFirst)
			{
				processedObjects.Add(trackingItem);
				action(trackingItem);
			}

			if (returnIfTrue != null && returnIfTrue())
			{
				if (resetLazyLoading)
					(trackingItem as IIsLazyLoadingEnabled).IsLazyLoadingEnabled = lazyLoad;
				return;
			}

			if (resetLazyLoading)
				(trackingItem as IIsLazyLoadingEnabled).IsLazyLoadingEnabled = lazyLoad;
		}

		/// <summary>
		/// New Method using the other new method GetAllChangeTrackerItems to more efficiently process an object graph of IObjectWithChangeTracker(s). - JHE
		/// </summary>
		/// <param name="trackingItem"></param>
		/// <param name="action"></param>
		/// <param name="callRecursively"></param>
		/// <param name="resetLazyLoading"></param>
		public static void ForEachTrackingItemRecursiveNew(this IObjectWithChangeTracker trackingItem, Action<IObjectWithChangeTracker> action, bool callRecursively = true, bool resetLazyLoading = true, List<IObjectWithChangeTracker> allTrackerItems = null)
		{
			if (allTrackerItems == null)
			{
				allTrackerItems = new List<IObjectWithChangeTracker>();
				GetAllChangeTrackerItems(trackingItem, allTrackerItems, callRecursively, resetLazyLoading);
			}

			foreach (var trackerItem in allTrackerItems)
				action(trackerItem);
		}

		public static IList<IObjectWithChangeTracker> GetAllChangeTrackerItems(this IObjectWithChangeTracker trackingItem, IList<IObjectWithChangeTracker> allTrackerItems = null, bool callRecursively = true, bool resetLazyLoading = true)
		{
			if (trackingItem == null)
				throw new ArgumentNullException("trackingItem");

			if (allTrackerItems == null)
				allTrackerItems = new List<IObjectWithChangeTracker>();

			bool lazyLoad = (trackingItem as IIsLazyLoadingEnabled).IsLazyLoadingEnabled;
			if (resetLazyLoading)
				(trackingItem as IIsLazyLoadingEnabled).IsLazyLoadingEnabled = false;

			var trackerProps = GetChangeTrackerPropertiesCached(trackingItem);

			foreach (PropertyInfo p in trackerProps)
			{
				if (p.PropertyType.Name == "TrackableCollection`1")
				{
					IList list = (IList)p.GetValue(trackingItem, null);

					if (list != null)
					{
						foreach (IObjectWithChangeTracker b in list)
						{
							if (list != null && callRecursively && !allTrackerItems.Contains(b))
							{
								if (!allTrackerItems.Contains(b))
									allTrackerItems.Add(b);
								b.GetAllChangeTrackerItems(allTrackerItems, callRecursively, resetLazyLoading);
							}
						}
					}
				}
				else
				{
					IObjectWithChangeTracker item = (IObjectWithChangeTracker)p.GetValue(trackingItem, null);

					if (item != null && callRecursively && !allTrackerItems.Contains(item))
					{
						if (!allTrackerItems.Contains(item))
							allTrackerItems.Add(item);
						item.GetAllChangeTrackerItems(allTrackerItems, callRecursively, resetLazyLoading);
					}
				}
			}

			if (!allTrackerItems.Contains(trackingItem))
				allTrackerItems.Add(trackingItem);

			if (resetLazyLoading)
				(trackingItem as IIsLazyLoadingEnabled).IsLazyLoadingEnabled = lazyLoad;

			return allTrackerItems;
		}


		// New prototype method - JHE
		public static void GetAllTrackableCollections<T>(this IObjectWithChangeTracker trackingItem, IList<IList<T>> allTrackerItems, bool callRecursively = true, bool resetLazyLoading = true)
		{
			if (trackingItem == null)
				throw new ArgumentNullException("trackingItem");

			List<IObjectWithChangeTracker> allTrackerItems2 = new List<IObjectWithChangeTracker>();
			GetAllChangeTrackerItems(trackingItem, allTrackerItems2, callRecursively, resetLazyLoading);

			Type type = typeof(T);
			foreach (var item in allTrackerItems2)
			{
				bool lazyLoad = (item as IIsLazyLoadingEnabled).IsLazyLoadingEnabled;
				if (resetLazyLoading)
					(item as IIsLazyLoadingEnabled).IsLazyLoadingEnabled = false;

				var trackerProps = GetChangeTrackerPropertiesCached(item);

				foreach (PropertyInfo p in trackerProps)
				{
					if (p.PropertyType.Name == "TrackableCollection`1" && p.PropertyType.GetGenericArguments()[0] == type)
					{
						IList list = (IList)p.GetValue(item, null);
						if (list != null)
						{
							if (!allTrackerItems.Contains(list as IList<T>))
								allTrackerItems.Add(list as IList<T>);
						}
					}
				}

				if (resetLazyLoading)
					(item as IIsLazyLoadingEnabled).IsLazyLoadingEnabled = lazyLoad;
			}
		}



		public static List<IObjectWithChangeTracker> FindDuplicateEntitiesInObjectGraph(this IObjectWithChangeTracker trackingItem)
		{
			List<IObjectWithChangeTracker> duplicateItems = new List<IObjectWithChangeTracker>();
			Dictionary<Type, List<IObjectWithChangeTracker>> allTrackerItemsDict = new Dictionary<Type, List<IObjectWithChangeTracker>>();

			List<IObjectWithChangeTracker> allTrackerItems = new List<IObjectWithChangeTracker>();
			GetAllChangeTrackerItems(trackingItem, allTrackerItems, true, true);

			foreach (var trackerItem in allTrackerItems)
			{
				Type type = trackerItem.GetType();

				if (!allTrackerItemsDict.ContainsKey(type))
					allTrackerItemsDict.Add(type, new List<IObjectWithChangeTracker>());

				allTrackerItemsDict[type].Add(trackerItem);
			}

			foreach (var key in allTrackerItemsDict.Keys)
			{
				foreach (var firstItem in allTrackerItemsDict[key])
				{
					foreach (var secondItem in allTrackerItemsDict[key])
					{
						if ((firstItem as IListValue).ID == (secondItem as IListValue).ID && !System.Object.ReferenceEquals(firstItem, secondItem))
						{
							if (!duplicateItems.Contains(firstItem))
								duplicateItems.Add(firstItem);
							if (!duplicateItems.Contains(secondItem))
								duplicateItems.Add(secondItem);
							break;
						}
					}
				}
			}

			return duplicateItems;
		}

		public static void RemoveUnModifiedDuplicateEntitiesInObjectGraph(this IObjectWithChangeTracker trackingItem)
		{
			List<IObjectWithChangeTracker> duplicateEntities = trackingItem.FindDuplicateEntitiesInObjectGraph();

			List<IObjectWithChangeTracker> allTrackerItems = new List<IObjectWithChangeTracker>();
			GetAllChangeTrackerItems(trackingItem, allTrackerItems, true, true);


			foreach (var type in duplicateEntities.Select(t => t.GetType()))
			{
				var typeObjs = duplicateEntities.Select(t => t.GetType() == type);

				foreach (var item in allTrackerItems)
				{
					Type eachType = item.GetType();
					var props = eachType.GetPropertiesCached().Where(p => p.PropertyType == type);
				}
			}

		}

		public static void RemoveAllAndMarkAsDeleted<T>(this IList<T> trackingItems) where T : IObjectWithChangeTracker
		{
			if (trackingItems != null)
			{
				while (trackingItems.Count > 0)
				{
					var item = trackingItems.First();
					if (item.ChangeTracker.State != ObjectState.Added)
						item.MarkAsDeleted();
					trackingItems.Remove(item);
				}
			}
		}

		public static void RemoveAndMarkAsDeleted<T>(this IList<T> trackingItems, T item) where T : IObjectWithChangeTracker
		{
			if (trackingItems != null)
			{
				if (item.ChangeTracker.State != ObjectState.Added)
					item.MarkAsDeleted();
				trackingItems.Remove(item);
			}
		}

		public static IList<T> RemoveWhereAndMarkAsDeleted<T>(this IList<T> trackableObjects, Func<T, bool> predicate, Action<IList<T>, T> customRemoveAction = null) where T : IObjectWithChangeTracker
		{
			if (trackableObjects == null)
			{
				throw new ArgumentNullException("trackableObjects");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}

			var toRemove = new List<T>();
			foreach (var obj in trackableObjects)
			{
				if (predicate(obj))
				{
					toRemove.Add(obj);
				}
			}
			foreach (var obj in toRemove)
			{
				if (customRemoveAction != null)
				{
					customRemoveAction(trackableObjects, obj);
				}
				else
				{
					if (obj.ChangeTracker.State != ObjectState.Added)
						obj.MarkAsDeleted();
					trackableObjects.Remove(obj);
				}
			}
			return trackableObjects;
		}



		#region New prototype methods to detect and remove duplicate entities from graph. - JHE

		/// <summary>
		/// Set the Generic T to the Type of Entity in graph that is been searched for. - JHE
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="trackingItem"></param>
		/// <returns></returns>
		public static bool ContainsUnmodifiedDuplicateEntitiesInObjectGraph<T>(this IObjectWithChangeTracker trackingItem)
		{
			List<IObjectWithChangeTracker> allTrackerItems = new List<IObjectWithChangeTracker>();
			GetAllChangeTrackerItems(trackingItem, allTrackerItems, true, true);
			IList<IObjectWithChangeTracker> allSameTypeTrackerItems = allTrackerItems.ToList().RemoveWhere(i => !(i is T));

			List<IObjectWithChangeTracker> changedTrackerItems = new List<IObjectWithChangeTracker>();
			changedTrackerItems.AddRangeWhere(allSameTypeTrackerItems, a => a.ChangeTracker.State != ObjectState.Unchanged);

			var duplicateIDs = changedTrackerItems.SelectDuplicatesObjectIDs(a => (a as IListValue).ID).Where(a => a > 0); // Exclude all unsaved objects (id == 0) - JHE
			return duplicateIDs.CountSafe() > 0;
		}

		/// <summary>
		/// Set the Generic T to the Type of Entity in graph that is been searched for. - JHE
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="trackingItem"></param>
		public static void RemoveUnmodifiedDuplicateEntitiesInObjectGraph<T>(this IObjectWithChangeTracker trackingItem) where T : class, IObjectWithChangeTracker
		{
			IList<IObjectWithChangeTracker> allTrackerItems = GetAllChangeTrackerItems(trackingItem, null, true, true);
			IList<IObjectWithChangeTracker> allSameTypeTrackerItems = allTrackerItems.ToList().RemoveWhere(i => !(i is T));

			List<T> changedTrackerItems = new List<T>();
			changedTrackerItems.AddRangeWhere(allTrackerItems, a => a.ChangeTracker.State != ObjectState.Unchanged);

			var duplicateIDs = allSameTypeTrackerItems.SelectDuplicatesObjectIDs(a => (a as IListValue).ID).Where(a => a > 0); // Exclude all unsaved objects (id == 0) - JHE

			// Remove Unmodified duplicates - JHE
			(trackingItem as IObjectWithChangeTrackerBusiness).StopEntityTracking();
			EnsureRemoveUnmodifiedDuplicateEntitiesInObjectGraph<T>(trackingItem, allTrackerItems, duplicateIDs);
			(trackingItem as IObjectWithChangeTrackerBusiness).StartEntityTracking();
		}

		/// <summary>
		/// Set the Generic T to the Type of Entity in graph that is been searched for. - JHE
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="trackingItem"></param>
		/// <param name="allTrackerItems"></param>
		/// <param name="duplicateIDs"></param>
		public static void EnsureRemoveUnmodifiedDuplicateEntitiesInObjectGraph<T>(this IObjectWithChangeTracker trackingItem, IEnumerable<IObjectWithChangeTracker> allTrackerItems, IEnumerable<int> duplicateIDs) where T : class
		{
			foreach (var id in duplicateIDs)
			{
				if (id > 0)
				{
					// Find 1 unmodified entity with matching Key and remove the rest - JHE
					var allSameTypeTrackerItems = allTrackerItems.Where(i => i is T);
					T dItem = (T)allSameTypeTrackerItems.First(a => (a as IListValue).ID == id);

					// Find all collections of Generic Type - JHE
					IList<IList<T>> allLists = new List<IList<T>>();
					GetAllTrackableCollections<T>(trackingItem, allLists);

					foreach (var list in allLists)
					{
						foreach (var item in list.ToList())
						{
							if ((item as IListValue).ID > 0 && (item as IListValue).ID == (dItem as IListValue).ID && item != dItem)
								list.Remove(item);
						}
					}

					Type type = typeof(T);
					var props = type.GetPropertiesCached();
					props.RemoveWhere(p => p.PropertyType != type);
					foreach (var item in allTrackerItems)
					{
						Type eachType = item.GetType();
						var eachTypeProps = eachType.GetPropertiesCached();
						eachTypeProps.RemoveWhere(p => p.PropertyType != type);

						foreach (var p in eachTypeProps)
						{
							IObjectWithChangeTracker value = (IObjectWithChangeTracker)p.GetValue(item, null);

							if (value != null && (value as IListValue).ID > 0 && (value as IListValue).ID == (dItem as IListValue).ID && value != dItem)
								p.SetValue(item, null, null);
						}
					}

				}
			}
		}

		#endregion


		// Removes any entities from the object graph of type T - JHE
		public static void RemoveEntitiesFromObjectGraph<T>(this IObjectWithChangeTracker trackingItem, bool onlyRemoveUnsavedItems = true) where T : IObjectWithChangeTracker
		{
			if (trackingItem != null)
			{
				Type type = typeof(T);

				// Find all objects of type T - JHE
				List<IObjectWithChangeTracker> allTrackerItems = new List<IObjectWithChangeTracker>();
				trackingItem.GetAllChangeTrackerItems(allTrackerItems, true, false);
				List<IObjectWithChangeTracker> itemsToRemove = allTrackerItems.Where(i => i.GetType() == type).ToList();

				// Find all collections of Generic Type - JHE
				IList<IList<T>> allLists = new List<IList<T>>();
				GetAllTrackableCollections<T>(trackingItem, allLists);
				foreach (var list in allLists)
				{
					foreach (var item in list.ToList())
					{
						bool removeItem = false;
						if ((onlyRemoveUnsavedItems && (item is IListValue && (item as IListValue).ID <= 0)) || !onlyRemoveUnsavedItems)
							removeItem = true;

						if (removeItem && itemsToRemove.Contains(item))
							list.Remove(item);
					}
				}

				allTrackerItems.RemoveEntitiesFromChangeTrackers<T>(itemsToRemove);
			}
		}

		// This will remove all the items in the itemsToRemove list from any of the ChangeTrackers in the allTrackerItems list - JHE
		public static void RemoveEntitiesFromChangeTrackers<T>(this List<IObjectWithChangeTracker> allTrackerItems, IList<IObjectWithChangeTracker> itemsToRemove, bool onlyRemoveUnsavedItems = true) where T : IObjectWithChangeTracker
		{
			foreach (var trackerItem in allTrackerItems)
			{
				foreach (var item in trackerItem.ChangeTracker.ObjectsAddedToCollectionProperties.ToList())
				{
					if (item.Value is IEnumerable)
					{
						foreach (var item2 in item.Value.ToList())
						{
							bool removeItem = false;
							if ((onlyRemoveUnsavedItems && (item2 is IListValue && (item2 as IListValue).ID <= 0)) || !onlyRemoveUnsavedItems)
								removeItem = true;

							if (removeItem && itemsToRemove.Contains(item2))
								item.Value.Remove(item2);
						}
					}
				}

				foreach (var item in trackerItem.ChangeTracker.ObjectsRemovedFromCollectionProperties.ToList())
				{
					if (item.Value is IEnumerable)
					{
						foreach (var item2 in item.Value.ToList())
						{
							bool removeItem = false;
							if ((onlyRemoveUnsavedItems && (item2 is IListValue && (item2 as IListValue).ID <= 0)) || !onlyRemoveUnsavedItems)
								removeItem = true;

							if (removeItem && itemsToRemove.Contains(item2))
								item.Value.Remove(item2);
						}
					}
				}
			}
		}
	}
}
