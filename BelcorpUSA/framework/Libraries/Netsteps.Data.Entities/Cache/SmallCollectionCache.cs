using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NetSteps.Common.Base;
using NetSteps.Common.Interfaces;
using NetSteps.Core.Cache;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Cache
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Class to cache all 'small collections' with so the frequent connections/queries to the DB for small lookups 
	/// of values the change infrequently are avoided. This is the class to put 'manually added' SmallCollectionCache classes. 
	/// (Most of these are generated in a T4 template in the project.)
	/// Created: 03-01-2010
	/// </summary>
	public partial class SmallCollectionCache
	{
		#region Singleton
		public static SmallCollectionCache Instance
		{
			get { return Singleton.instance; }
		}

		private SmallCollectionCache() { }

		private abstract class Singleton
		{
			static Singleton() { } // DO NOT REMOVE: Static constructor required to prevent beforefieldinit flag from being set
			public static readonly SmallCollectionCache instance = new SmallCollectionCache();
		}
		#endregion

		public abstract class SmallCollectionBase<T, TKey> : ActiveLocalMemoryCachedListBase<T>, IExpireCache
		{
			private ConcurrentDictionary<TKey, int> _idIndexMap = new ConcurrentDictionary<TKey, int>();
			private ReaderWriterLockSlim _idIndexMapLock = new ReaderWriterLockSlim();

			public SmallCollectionBase(string name)
				: base(name)
			{ }

			#region Events

			public event EventHandler DataChanged;
			protected virtual void OnDataChanged(object sender, EventArgs e)
			{
				if (DataChanged != null)
					DataChanged(this, e);
			}

			#endregion

			protected override IList<T> PerformRefresh()
			{
				return PerformInitializeList();
			}

			protected override void AfterListRefreshed()
			{
				_idIndexMapLock.EnterWriteLock();
				try
				{
					var c = Count;
					_idIndexMap.Clear();
					for (int i = 0; i < c; i++)
					{
						_idIndexMap.AddOrUpdate(PerformGetKey(this[i]), i, (k, l) => i);
					}
				}
				finally
				{
					_idIndexMapLock.ExitWriteLock();
				}

				OnDataChanged(this, EventArgs.Empty);
			}

			protected abstract List<T> PerformInitializeList();
			protected abstract TKey PerformGetKey(T item);

			public T GetById(TKey id)
			{
				var result = default(T);
				if (Count > 0)
				{
					int index = -1;
					_idIndexMapLock.EnterReadLock();
					try
					{
						if (_idIndexMap.TryGetValue(id, out index))
						{
							result = this[index];
						}
					}
					finally
					{
						_idIndexMapLock.ExitReadLock();
					}
				}
				return result;
			}

			public virtual void ExpireCache()
			{
				Refresh();
			}
		}

		public CorporateAccountListValueCache CorporateAccountListValues = new CorporateAccountListValueCache();
		public class CorporateAccountListValueCache : SmallCollectionBase<AccountListValue, Int32>
		{
			public CorporateAccountListValueCache()
				: base("CorporateAccountListValueCacheList")
			{ }

			protected override List<AccountListValue> PerformInitializeList()
			{
				var result = AccountListValue.LoadAllCorporateListValues();
				return result.ToList();
			}

			protected override int PerformGetKey(AccountListValue item)
			{
				return item.AccountListValueID;
			}
		}

		public ShippingMethodTranslationCache ShippingMethodTranslations = new ShippingMethodTranslationCache();
		public class ShippingMethodTranslationCache : SmallCollectionBase<DescriptionTranslation, Int32>
		{
			Dictionary<int, Dictionary<int, string>> shippingMethodNames = new Dictionary<int, Dictionary<int, string>>();
			Dictionary<int, Dictionary<int, string>> shippingMethodShortDescriptions = new Dictionary<int, Dictionary<int, string>>();

			public ShippingMethodTranslationCache()
				: base("ShippingMethodTranslationCacheList")
			{ }

			protected override List<DescriptionTranslation> PerformInitializeList()
			{
				List<DescriptionTranslation> listResults;

				List<int> ids = ShippingMethod.LoadAllTranslationIds();
				var result = DescriptionTranslation.LoadBatch(ids);

				var shippingMethods = ShippingMethod.LoadAllFull();

				if (result != null)
				{
					listResults = result.ToList();

					foreach (var item in shippingMethods)
					{
						foreach (var item2 in item.Translations)
						{
							if (!shippingMethodNames.ContainsKey(item.ShippingMethodID))
							{
								shippingMethodNames.Add(item.ShippingMethodID, new Dictionary<int, string>());
							}

							if (!shippingMethodShortDescriptions.ContainsKey(item.ShippingMethodID))
							{
								shippingMethodShortDescriptions.Add(item.ShippingMethodID, new Dictionary<int, string>());
							}

							shippingMethodNames[item.ShippingMethodID].Add(item2.LanguageID, item2.Name);
							shippingMethodShortDescriptions[item.ShippingMethodID].Add(item2.LanguageID, item2.ShortDescription);
						}
					}
				}
				else
					listResults = new List<DescriptionTranslation>();

				return listResults;
			}

			protected override int PerformGetKey(DescriptionTranslation item)
			{
				return item.DescriptionTranslationID;
			}

			public string GetTranslatedName(int shippingMethodID, string defaultName, int? languageID = null)
			{
				if (languageID == null)
					languageID = ApplicationContext.Instance.CurrentLanguageID;

				if (shippingMethodNames.ContainsKey(shippingMethodID))
				{
					if (shippingMethodNames[shippingMethodID].ContainsKey(languageID.Value))
					{
						return shippingMethodNames[shippingMethodID][languageID.Value];
					}
				}

				return defaultName;
			}


			public string GetTranslatedShortDescription(int shippingMethodID, string defaultDescription, int? languageID = null)
			{
				if (languageID == null)
					languageID = ApplicationContext.Instance.CurrentLanguageID;

				if (shippingMethodShortDescriptions.ContainsKey(shippingMethodID))
				{
					if (shippingMethodShortDescriptions[shippingMethodID].ContainsKey(languageID.Value))
					{
						return shippingMethodShortDescriptions[shippingMethodID][languageID.Value];
					}
				}

				return defaultDescription;
			}

			public override void ExpireCache()
			{
				shippingMethodNames = null;
				shippingMethodShortDescriptions = null;
				base.ExpireCache();
			}
		}



		private static AsyncReloadObject<ReportCategoryCollection> _reportCategories = new AsyncReloadObject<ReportCategoryCollection>(()=>{
			ReportCategoryCollection result = new ReportCategoryCollection();
			try 
			{
				result = ReportCategoryCollection.LoadAll();
			}
			catch(Exception e)
			{
				try
				{
					ExceptionLogger.LogException(e, "something went wrong while trying to load all categories in a background thread", false);
				}
				catch(Exception i)
				{
//If this is thrown in production it will crash the application pool
#if DEBUG
					throw i;
#endif
				}
			}
			return result;
		}, TimeSpan.FromDays(1));

		public static ReportCategoryCollection ReportCategories
		{
			get { return _reportCategories.Value; }
		}

		public static void ExpireReportsCategories()
		{
			_reportCategories.InvalidateData();
		}


		/// <summary>
		/// Reflects over the current class to find all properties that implement IExpireCache
		/// and calls ExpireCache(); - JHE
		/// </summary>
		public void ExpireAllCache()
		{
			Type type = this.GetType();
			foreach (var member in type.GetMembers())
			{
				if (member.MemberType == System.Reflection.MemberTypes.Field)
				{
					object obj = ((System.Reflection.FieldInfo)member).GetValue(this);
					if (obj is IExpireCache)
						(obj as IExpireCache).ExpireCache();
				}
			}
		}
	}
}
