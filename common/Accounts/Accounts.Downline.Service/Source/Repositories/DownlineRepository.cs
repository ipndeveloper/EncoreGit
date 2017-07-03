using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Accounts.Downline.Common.Models;
using NetSteps.Accounts.Downline.Common.Repositories;
using NetSteps.Accounts.Downline.Service.Context;
using NetSteps.Accounts.Downline.Service.Hierarchy;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Accounts.Downline.Service.Repositories
{
	public class DownlineRepository : IDownlineRepository
	{
		protected readonly Func<IDownlineDbContext> _dbFactory;

		public DownlineRepository(
			Func<IDownlineDbContext> dbFactory)
		{
			Contract.Requires<ArgumentNullException>(dbFactory != null);

			_dbFactory = dbFactory;
		}

		public virtual IList<IDownlineData> GetDownline(IGetDownlineContext context)
		{
			RaiseDownlineLoading(new DownlineLoadingEventArgs(context));
			IList<IDownlineData> result;

			using (var db = _dbFactory())
			{
				var query = db.SponsorHierarchies
					.WhereInHierarchy(context.RootAccountId, maxLevels: context.MaxLevels)
					.Join(db.Accounts, x => x.AccountId, a => a.AccountID, (x, a) => new
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

				if (context.AccountStatusIds != null
					&& context.AccountStatusIds.Any())
				{
					query = query
						.Where(x => context.AccountStatusIds.Contains(x.AccountStatusID));
				}

				if (context.AccountTypeIds != null
					&& context.AccountTypeIds.Any())
				{
					query = query
						.Where(x => context.AccountTypeIds.Contains(x.AccountTypeID));
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

		public virtual IDownlineAccountInfo GetDownlineAccountInfo(int rootAccountId, int accountId)
		{
			using (var db = _dbFactory())
			{
				var rootAccountTreeLevel = db.SponsorHierarchies
					.Where(h => h.AccountId == rootAccountId)
					.Select(h => h.TreeLevel)
					.FirstOrDefault();

				if (rootAccountTreeLevel == 0)
				{
					return null;
				}

				var query = from h in db.SponsorHierarchies.WhereInHierarchy(rootAccountId)
							join a in db.Accounts on h.AccountId equals a.AccountID
							join i in db.AccountInfoCache on h.AccountId equals i.AccountID
							where h.AccountId == accountId
							select new
							{
								h.AccountId,
								h.TreeLevel,
								h.NodeCount,
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
								i.Address1,
								i.City,
								i.StateAbbreviation,
								i.PostalCode,
								i.CountryID,
								i.LastOrderCommissionDateUTC,
								i.NextAutoshipRunDate
							};

				return query
					.ToArray()
					.Select(x =>
					{
						var data = Create.New<IDownlineAccountInfo>();
						data.AccountId = x.AccountId;
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

		public virtual IList<int> GetUplineAccountIds(int accountId)
		{
			using (var db = _dbFactory())
			{
				var uplineBytes = db.SponsorHierarchies
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

		public virtual IList<IDownlineData> Search(ISearchDownlineContext context)
		{
			RaiseDownlineSearching(new DownlineSearchingEventArgs(context));
			IList<IDownlineData> result;

			using (var db = _dbFactory())
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
	{0}";
				var sqlParams = new List<object>
				{
					new SqlParameter("@RootAccountId", context.RootAccountId)
				};

				var searchFilterSqlFormat = @"AND CONTAINS (([A].[AccountNumber], [A].[FirstName], [A].[LastName]), {0})";
				var searchParams = context.Query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				var searchFilterSql = string.Empty;
				for (int i = 0; i < searchParams.Length; i++)
				{
					var paramName = "@SearchParam" + i.ToString();
					var paramValue = string.Format("\"{0}*\"", searchParams[i]);
					searchFilterSql += string.Format(searchFilterSqlFormat, paramName);
					sqlParams.Add(new SqlParameter(paramName, paramValue));
				}

				var sql = string.Format(sqlFormat, searchFilterSql);

				result = db.Database
					.SqlQuery<SearchDownlineSqlResult>(sql, sqlParams.ToArray())
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

		public virtual IList<IBusinessEntityDownlineData> SearchBusinessEntities(ISearchDownlineContext context)
		{
			IList<IBusinessEntityDownlineData> result;

			using (var db = _dbFactory())
			{
				var sqlFormat = @"
SELECT
	[TREE].[AccountId],
	[TREE].[SponsorId] AS ParentAccountId,
	[TREE].[TreeLevel],
	[A].[AccountTypeID],
	[A].[AccountStatusID],
	[A].[AccountNumber],
	[A].[EntityName]
FROM [Accounts].[SponsorHierarchy] AS [ROOTNODE]
INNER JOIN [Accounts].[SponsorHierarchy] AS [TREE]
	ON ([TREE].[LeftAnchor] >= [ROOTNODE].[LeftAnchor])
	AND ([TREE].[RightAnchor] <= [ROOTNODE].[RightAnchor])
INNER JOIN [dbo].[Accounts] AS [A] ON [TREE].[AccountId] = [A].[AccountID]
WHERE [ROOTNODE].[AccountId] = @RootAccountId
AND [A].[IsEntity] = 1
	{0}";
				var sqlParams = new List<object>
				{
					new SqlParameter("@RootAccountId", context.RootAccountId)
				};

				var searchFilterSqlFormat = @"AND CONTAINS (([A].[AccountNumber], [A].[FirstName], [A].[LastName], [A].[EntityName]), {0})";
				var searchParams = context.Query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				var searchFilterSql = string.Empty;
				for (int i = 0; i < searchParams.Length; i++)
				{
					var paramName = "@SearchParam" + i.ToString();
					var paramValue = string.Format("\"{0}*\"", searchParams[i]);
					searchFilterSql += string.Format(searchFilterSqlFormat, paramName);
					sqlParams.Add(new SqlParameter(paramName, paramValue));
				}

				var sql = string.Format(sqlFormat, searchFilterSql);

				result = ((DownlineDbContext)db).Database
					.SqlQuery<EntitySearchDownlineSqlResult>(sql, sqlParams.ToArray())
					.Select(x =>
					{
						var data = Create.New<IBusinessEntityDownlineData>();
						data.AccountId = x.AccountId;
						data.ParentAccountId = x.SponsorId;
						data.TreeLevel = x.TreeLevel;
						data.AccountTypeId = x.AccountTypeID;
						data.AccountStatusId = x.AccountStatusID;
						data.AccountNumber = x.AccountNumber;
						data.FirstName = x.FirstName;
						data.LastName = x.LastName;
						data.EntityName = x.EntityName;
						return data;
					})
					.ToList();
			}

			return result;
		}

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

		protected class EntitySearchDownlineSqlResult : SearchDownlineSqlResult
		{
			public string EntityName { get; set; }
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
	}
}
