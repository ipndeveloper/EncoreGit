using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using Fasterflect;
using NetSteps.Accounts.Downline.Common.Models;
using NetSteps.Common.Dynamic;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Hierarchy;
using NetSteps.Encore.Core.IoC;
using NetSteps.Accounts.Downline.Common.Repositories;
using NetSteps.Commissions.Common;
using NetSteps.Data.Entities.Hierarchy;

namespace NetSteps.Data.Entities.Repositories
{
	[ContainerRegister(typeof(IDownlineRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Default)]
	public partial class DownlineRepository : IDownlineRepository
	{
		private IDownlineService _downlineService = Create.New<IDownlineService>();

		public List<dynamic> GetDownline(int periodID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				try
				{
					return this._downlineService.GetDownline(periodID).ToList();
				}
				catch (Exception ex)
				{
					throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
				}
			});
		}

        public List<dynamic> GetDownline(int periodID, int sponsorID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                try
                {
                    return this._downlineService.GetDownline(periodID, sponsorID).ToList();
                }
                catch (Exception ex)
                {
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
                }
            });
        }

		public List<int> LoadOneSimple(int downlineId, int periodId)
		{
			if (!SimpleCache.ContainsKey(periodId))
				InitializeTree(periodId);

			return SimpleCache[periodId][downlineId];
		}

		private readonly static Dictionary<int, PeriodCache> SimpleCache = new Dictionary<int, PeriodCache>();

		private void InitializeTree(int periodID)
		{
			if (SimpleCache.ContainsKey(periodID))
			{
				SimpleCache[periodID] = new PeriodCache();
			}
			else
			{
				SimpleCache.Add(periodID, new PeriodCache());
			}

			var result = this._downlineService.GetSimpleDownline(periodID);
			foreach (var item in result)
			{
				SimpleCache[periodID].AddRelation(item.AccountId, item.SponsorId);
			}
		}

		#region NDCR.IDownlineRepository
		public virtual IList<IDownlineData> GetDownline(IGetDownlineContext parameters)
		{
			RaiseDownlineLoading(new DownlineLoadingEventArgs(parameters));
			IList<IDownlineData> result;

			using (var context = new NetStepsEntities())
			{
				var query = context.SponsorHierarchies
					.WhereInHierarchy(parameters.RootAccountId, maxLevels: parameters.MaxLevels)
					.Join(context.Accounts, x => x.AccountId, a => a.AccountID, (x, a) => new
					{
						x.AccountId,
						x.SponsorId,
						x.TreeLevel,
						a.AccountTypeID,
						a.AccountStatusID,
						a.AccountNumber,
						a.FirstName,
						a.LastName
					});

				if (parameters.AccountStatusIds != null
					&& parameters.AccountStatusIds.Any())
				{
					query = query
						.Where(x => parameters.AccountStatusIds.Contains(x.AccountStatusID));
				}

				if (parameters.AccountTypeIds != null
					&& parameters.AccountTypeIds.Any())
				{
					query = query
						.Where(x => parameters.AccountTypeIds.Contains(x.AccountTypeID));
				}

				result = query
					.ToArray()
					.Select(x =>
					{
						var data = Create.New<IDownlineData>();
						data.AccountId = x.AccountId;
						data.ParentAccountId = x.SponsorId;
						data.TreeLevel = x.TreeLevel;
						data.AccountTypeId = x.AccountTypeID;
						data.AccountStatusId = x.AccountStatusID;
						data.AccountNumber = x.AccountNumber;
						data.FirstName = x.FirstName;
						data.LastName = x.LastName;
						return data;
					})
					.ToList();
			}

			RaiseDownlineLoaded(new DownlineLoadedEventArgs(result));
			return result;
		}

		public virtual IList<IDownlineData> Search(ISearchDownlineContext parameters)
		{
			RaiseDownlineSearching(new DownlineSearchingEventArgs(parameters));
			IList<IDownlineData> result;

			using (var context = new NetStepsEntities())
			{
				var sqlFormat = @"
SELECT
	[TREE].[AccountId],
	[TREE].[SponsorId] AS ParentAccountId,
	[TREE].[TreeLevel],
	[A].[AccountTypeID],
	[A].[AccountStatusID],
	[A].[AccountNumber],
	[A].[FirstName],
	[A].[LastName]
FROM [Accounts].[SponsorHierarchy] AS [ROOTNODE]
INNER JOIN [Accounts].[SponsorHierarchy] AS [TREE]
	ON ([TREE].[LeftAnchor] >= [ROOTNODE].[LeftAnchor])
	AND ([TREE].[RightAnchor] <= [ROOTNODE].[RightAnchor])
INNER JOIN [dbo].[Accounts] AS [A] ON [TREE].[AccountId] = [A].[AccountID]
WHERE [ROOTNODE].[AccountId] = @RootAccountId
	AND [A].[AccountStatusID] != {1}
	{0}";
				var sqlParams = new List<object>
				{
					new SqlParameter("@RootAccountId", parameters.RootAccountId)
				};

				var searchFilterSqlFormat = @"AND CONTAINS (([A].[AccountNumber], [A].[FirstName], [A].[LastName]), {0})";
				var searchParams = parameters.Query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				var searchFilterSql = string.Empty;
				for (int i = 0; i < searchParams.Length; i++)
				{
					var paramName = "@SearchParam" + i.ToString();
					var paramValue = string.Format("\"{0}*\"", searchParams[i]);
					searchFilterSql += string.Format(searchFilterSqlFormat, paramName);
					sqlParams.Add(new SqlParameter(paramName, paramValue));
				}

				var sql = string.Format(sqlFormat, searchFilterSql, (int)ConstantsGenerated.AccountStatus.Terminated);

				result = context
					.ExecuteStoreQuery<SearchDownlineSqlResult>(sql, sqlParams.ToArray())
					.Select(x =>
					{
						var data = Create.New<IDownlineData>();
						data.AccountId = x.AccountId;
						data.ParentAccountId = x.SponsorId;
						data.TreeLevel = x.TreeLevel;
						data.AccountTypeId = x.AccountTypeID;
						data.AccountStatusId = x.AccountStatusID;
						data.AccountNumber = x.AccountNumber;
						data.FirstName = x.FirstName;
						data.LastName = x.LastName;
						return data;
					})
					.ToList();
			}

			RaiseDownlineSearched(new DownlineSearchedEventArgs(result));
			return result;
		}

		public virtual IList<int> GetUplineAccountIds(int accountId)
		{
			using (var context = new NetStepsEntities())
			{
				var uplineBytes = context.SponsorHierarchies
					.Where(x => x.AccountId == accountId)
					.Select(x => x.Upline)
					.FirstOrDefault();

				if (uplineBytes == null)
				{
					return new List<int>();
				}

				// Convert from big endian by reversing the whole array.
				// This will result in the account IDs being reversed.
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse(uplineBytes);
				}

				var accountIds = ParseUplineBytes(uplineBytes);

				// Get the account IDs back into the original order.
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse(accountIds);
				}

				return accountIds;
			}
		}

		public virtual IDownlineAccountInfo GetDownlineAccountInfo(int rootAccountId, int accountId)
		{
			using (var context = new NetStepsEntities())
			{
				var rootAccountTreeLevel = context.SponsorHierarchies
					.Where(h => h.AccountId == rootAccountId)
					.Select(h => h.TreeLevel)
					.FirstOrDefault();

                //if (rootAccountTreeLevel == 0)
                //{
                //    return null;
                //}

                /*CS.29JUL.2016.Inicio.Obtener Dirección Principal*/
                string calle = string.Empty;
                string direccion1 = string.Empty;
                string direccion2 = string.Empty;
                string zona = string.Empty;
                string Address1 = string.Empty;
                int CountryID = 0;
                Account cuenta = Account.LoadFull(accountId);
                if (cuenta != null)
                {
                    calle = cuenta.Addresses.Where(donde => donde.AddressTypeID == 1).FirstOrDefault().Street;
                    direccion1 = cuenta.Addresses.Where(donde => donde.AddressTypeID == 1).FirstOrDefault().Address1;
                    direccion2 = cuenta.Addresses.Where(donde => donde.AddressTypeID == 1).FirstOrDefault().Address2;
                    zona = cuenta.Addresses.Where(donde => donde.AddressTypeID == 1).FirstOrDefault().County;
                    Address1 = calle + " " + direccion1 + " " + direccion2 + " " + zona;
                    CountryID = cuenta.Addresses.Where(donde => donde.AddressTypeID == 1).FirstOrDefault().CountryID;
                }
                /*CS.29JUL.2016.Fin.Obtener Dirección Principal*/

                /*CS.21Sep.2016.Ajuste : colocar el left join para SponsorHierarchies */
				var query = from a in context.Accounts 
                           join i in context.AccountInfoCache on a.AccountID equals i.AccountID
                           join h in context.SponsorHierarchies.WhereInHierarchy(rootAccountId)
                           on a.AccountID equals h.AccountId into tmpGroups
                           from tmp in tmpGroups.DefaultIfEmpty()
						   where a.AccountID == accountId
							select new
							{
								a.AccountID,
                                TreeLevel= (tmp.TreeLevel == null) ? 0 : tmp.TreeLevel,
                                NodeCount = (tmp.NodeCount == null) ? 0 : tmp.NodeCount,
								a.AccountNumber,
								a.FirstName,
								a.LastName,
								i.SponsorAccountNumber,
								i.SponsorFirstName,
								i.SponsorLastName,
								i.EnrollerAccountNumber,
								i.EnrollerFirstName,
								i.EnrollerLastName,
								a.EnrollmentDateUTC,
								a.NextRenewalUTC,
								a.AccountTypeID,
								a.AccountStatusID,
								a.EmailAddress,
								i.PhoneNumber,
								i.PwsUrl,
								//i.Address1,
                                //a.Addresses.Where(donde => donde.AddressTypeID == 1).FirstOrDefault().
                                Address1,
								i.City,
								i.StateAbbreviation,
								i.PostalCode,
								//a.CountryID,
                                CountryID,
								i.LastOrderCommissionDateUTC,
								i.NextAutoshipRunDate
							};

				return query
					.ToArray()
					.Select(x =>
					{
						var data = Create.New<IDownlineAccountInfo>();
						data.AccountId = x.AccountID;
						data.RelativeTreeLevel = x.TreeLevel - rootAccountTreeLevel;
						data.DownlineCount = x.NodeCount - 1;
						data.AccountNumber = x.AccountNumber ?? string.Empty;
						data.FirstName = x.FirstName ?? string.Empty;
						data.LastName = x.LastName ?? string.Empty;
						data.SponsorAccountNumber = x.SponsorAccountNumber ?? string.Empty;
						data.SponsorFirstName = x.SponsorFirstName ?? string.Empty;
						data.SponsorLastName = x.SponsorLastName ?? string.Empty;
						data.EnrollerAccountNumber = x.EnrollerAccountNumber ?? string.Empty;
						data.EnrollerFirstName = x.EnrollerFirstName ?? string.Empty;
						data.EnrollerLastName = x.EnrollerLastName ?? string.Empty;
						data.EnrollmentDateUtc = x.EnrollmentDateUTC;
						data.NextRenewalDateUtc = x.NextRenewalUTC;
						data.AccountTypeId = x.AccountTypeID;
						data.AccountStatusId = x.AccountStatusID;
						data.EmailAddress = x.EmailAddress ?? string.Empty;
						data.PhoneNumber = x.PhoneNumber ?? string.Empty;
						data.PwsUrl = x.PwsUrl ?? string.Empty;
						data.Address1 = x.Address1 ?? string.Empty;
						data.City = x.City ?? string.Empty;
						data.StateAbbreviation = x.StateAbbreviation ?? string.Empty;
						data.PostalCode = x.PostalCode ?? string.Empty;
						data.CountryId = x.CountryID;
						data.LastOrderCommissionDateUtc = x.LastOrderCommissionDateUTC;
						data.NextAutoshipRunDate = x.NextAutoshipRunDate;
						return data;
					})
					.FirstOrDefault();
			}
		}
		#endregion

		#region Helpers
		protected class SearchDownlineSqlResult
		{
			public int AccountId { get; set; }
			public int? SponsorId { get; set; }
			public int TreeLevel { get; set; }
			public short AccountTypeID { get; set; }
			public short AccountStatusID { get; set; }
			public string AccountNumber { get; set; }
			public string FirstName { get; set; }
			public string LastName { get; set; }
		}

		public virtual int[] ParseUplineBytes(byte[] uplineBytes)
		{
			Contract.Requires<ArgumentNullException>(uplineBytes != null);
			Contract.Ensures(Contract.Result<int[]>() != null);

			var ints = new List<int>(uplineBytes.Length / 4);
			for (int i = 0; i < uplineBytes.Length - 3; i += 4)
			{
				ints.Add(BitConverter.ToInt32(uplineBytes, i));
			}
			return ints.ToArray();
		}

		public event EventHandler<DownlineLoadingEventArgs> DownlineLoading;
		public event EventHandler<DownlineLoadedEventArgs> DownlineLoaded;
		public event EventHandler<DownlineSearchingEventArgs> DownlineSearching;
		public event EventHandler<DownlineSearchedEventArgs> DownlineSearched;
		protected void RaiseDownlineLoading(DownlineLoadingEventArgs e) { if (DownlineLoading != null) { DownlineLoading(this, e); } }
		protected void RaiseDownlineLoaded(DownlineLoadedEventArgs e) { if (DownlineLoaded != null) { DownlineLoaded(this, e); } }
		protected void RaiseDownlineSearching(DownlineSearchingEventArgs e) { if (DownlineSearching != null) { DownlineSearching(this, e); } }
		protected void RaiseDownlineSearched(DownlineSearchedEventArgs e) { if (DownlineSearched != null) { DownlineSearched(this, e); } }
		#endregion


        public IList<IBusinessEntityDownlineData> SearchBusinessEntities(ISearchDownlineContext context)
        {
            throw new NotImplementedException();
        }
	}
}
