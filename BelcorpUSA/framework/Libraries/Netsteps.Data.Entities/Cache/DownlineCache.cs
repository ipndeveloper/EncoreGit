using System.Reflection;

namespace NetSteps.Data.Entities.Cache
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Timers;
    using Business;
    using Commissions.Common;
    using Core.Cache;
    using Encore.Core.IoC;
    using Exceptions;
    using Fasterflect;
    using NetSteps.Common.Base;
    using NetSteps.Common.Configuration;
    using NetSteps.Common.Extensions;
    using Repositories;
    using Resolvers;

    public class DownlineCache
    {
        #region Fields
        class ParameterExtension
        {
            public int PeriodID { get; set; }
            public int SponsorID { get; set; }

            public ParameterExtension(int periodID, int sponsorID)
            {
                this.PeriodID = periodID;
                this.SponsorID = sponsorID;
            }
        }

        private static readonly ICache<int, Downline> DownlinesById = new ActiveMruLocalMemoryCache<int, Downline>("DownlinesById", new DelegatedDemuxCacheItemResolver<int, Downline>(LoadDownline));

        private static readonly ICache<ParameterExtension, Downline> DownlinesByIdAndSponsor = new ActiveMruLocalMemoryCache<ParameterExtension, Downline>("DownlinesById", new DelegatedDemuxCacheItemResolver<ParameterExtension, Downline>(LoadDownline));

        #endregion

        private static bool LoadDownline(int periodId, out Downline downline)
        {
            try
            {
                List<dynamic> result = new DownlineRepository().GetDownline(periodId);
                return (downline = BuildDownlineForPeriodFromResult(periodId, -1, result)) != null;
            }
            catch (Exception ex)
            {
                EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                downline = null;
                return false;
            }
        }

        private static bool LoadDownline(ParameterExtension parameters, out Downline downline)
        {
            try
            {
                //List<dynamic> result = new DownLine().Query("EXEC usp_get_downlineMLM @0, @1", new object[] { parameters.PeriodID, parameters.SponsorID }).ToList();
                List<dynamic> result = new DownlineRepository().GetDownline(parameters.PeriodID, parameters.SponsorID);
                return (downline = BuildDownlineForPeriodFromResult(parameters.PeriodID, parameters.SponsorID, result)) != null;
            }
            catch (Exception ex)
            {
                EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                downline = null;
                return false;
            }
        }
        public static Downline GetDownline(int periodId)
        {
            Downline downline = DownlinesById.Get(periodId);
            if (downline != null)
            {
                downline.LastAccessed = DateTime.Now;
            }
            return downline;
        }

        public static Downline GetDownline(int periodId, int sponsorId)
        {
            Downline downline = DownlinesByIdAndSponsor.Get(new ParameterExtension(periodId, sponsorId));
            if (downline != null)
            {
                downline.LastAccessed = DateTime.Now;
            }
            return downline;
        }

        public static IEnumerable<dynamic> SearchDownline(int accountID, string query, int? periodID = null)
        {
            if (!periodID.HasValue)
            {
                var commissionsService = Create.New<ICommissionsService>();
                var period = commissionsService.GetCurrentPeriod();
                if (period == null)
                {
                    return new List<dynamic>();
                }
                periodID = period.PeriodId;
            }

            var downline = GetDownline(periodID.Value);
            if (downline != null)
            {
                var results = new ReportResults
                {
                    Downline = downline,
                    CurrentNode = downline.GetTree(accountID)
                };

                var newList = results.CurrentNodeFlatChildrenDetails.ToGenericList();

                var matchingItems = newList.AsQueryable();

                // Search all string columns for SearchValue - JHE
                if (!query.IsNullOrEmpty() && matchingItems.Any())
                {
                    query = query.ToCleanString();
                    var searchWords = query.Split(' ').ToList();

                    foreach (var searchWord in searchWords)
                    {
                        var stringProps = results.Downline.DynamicType.GetPropertiesCached().Where(p => p.PropertyType == typeof(string));

                        var stringPropertyQuery = string.Empty;
                        foreach (var property in stringProps)
                        {
                            stringPropertyQuery += string.Format("{0} != null && {0}.ToLower().Contains(\"{1}\") || ", property.Name, searchWord.ToLower());
                        }

                        if (stringPropertyQuery.EndsWith(" || "))
                        {
                            stringPropertyQuery = stringPropertyQuery.Substring(0, stringPropertyQuery.Length - 4);
                        }

                        matchingItems = matchingItems.Where(string.Format("({0})", stringPropertyQuery));
                    }
                }

                return matchingItems as IEnumerable<dynamic>;
            }
            else return Enumerable.Empty<dynamic>();
        }

        public static void ExpireCache()
        {
            DownlinesById.FlushAll();
        }

        private static Downline BuildDownlineForPeriodFromResult(int periodId, int SponsorID, List<dynamic> result)
        {
            var treeVisits = new Dictionary<int, int>();
            var downline = new Downline
            {
                PeriodID = periodId
            };
            AddNodesToDownline(result, downline);

            //TODO: Figure out how we're actually going to find out who's the top of the tree - DES
            int topOfTreeAccountID = SponsorID == -1 ? ConfigurationManager.GetAppSetting("TopOfTreeAccountID", 2) : SponsorID;//ConfigurationManager.GetAppSetting("TopOfTreeAccountID", 2);
            downline.Root = BuildNode(treeVisits, topOfTreeAccountID, downline);

            // Set Type of dynamically generated objects and cache it's setters. - JHE
            SetTypeOfDynamicallyGeneratedObjects(downline);

            // Set All FlatChildren for All nodes - JHE
            SetAllFlatChildrenForAllNodes(downline);

            // Calculated data to row data - JHE
            downline.Root.VisitTree(c =>
            {
                foreach (var n in c)
                {
                    dynamic data = downline.Lookup[n.AccountID];
                    data.FlatDownlineCount = n.TotalDownlineCount;
                    data.Level = n.Level;
                }
            });
            downline.LastUpdated = DateTime.Now; // TODO: Change this to pull a value from the DataBase later - JHE
            return downline;
        }
        private static void SetTypeOfDynamicallyGeneratedObjects(Downline downline)
        {
            downline.DynamicType = downline.Lookup.Values.GetTypeCheckSingle();
            var dynamicTypeGetters = downline.DynamicType.GetPropertiesCached().ToDictionary(prop => prop.Name, prop => prop.DelegateForGetPropertyValue());

            if (dynamicTypeGetters.Any())
            {
                downline.DynamicTypeGetters = dynamicTypeGetters;
            }
        }

        private static void SetAllFlatChildrenForAllNodes(Downline downline)
        {
            downline.Root.VisitTree(c => downline.Root.FlatChildren.AddRange(c));
            var nodesAdded = new ConcurrentDictionary<int, int>();

            foreach (var key in downline.LookupNode.Keys)
            {
                DownlineNode node = downline.LookupNode[key];

                Dictionary<int, int> childIds = DownlineNode.GetChildIDsRecursive(node);
                foreach (var key2 in childIds.Keys)
                {
                    var node2 = downline.LookupNode[key2];
                    if (!nodesAdded.ContainsKey(node2.AccountID))
                    {
                        node.FlatChildren.Add(node2);
                        nodesAdded.Add(node2.AccountID, node2.AccountID);
                    }
                    node.FlatChildren.AddRange(node2.Children);
                    foreach (var childNode in node2.Children)
                    {
                        nodesAdded.Add(childNode.AccountID, childNode.AccountID);
                    }
                }
            }
        }

        private static void AddNodesToDownline(IEnumerable<dynamic> result, Downline downline)
        {
            foreach (dynamic data in result)
            {
                if (!downline.Lookup.ContainsKey(data.AccountID))
                {
                    downline.Lookup.Add(data.AccountID, data);
                }

                if (data.SponsorID != null)
                {
                    if (!downline.SponsorshipTree.ContainsKey(data.SponsorID))
                    {
                        downline.SponsorshipTree.Add(data.SponsorID, new List<dynamic>());
                    }

                    downline.SponsorshipTree[data.SponsorID].Add(data);
                }
            }
        }

        private static DownlineNode BuildNode(Dictionary<int, int> treeVisits, int accountId, Downline downline, int? parentId = null, DownlineNode parentNode = null, int level = 1, int? accountTypeID = null, int? accountStatusID = null)
        {
            if (!parentId.HasValue)
            {
                treeVisits = new Dictionary<int, int>();
            }

            var newNode = new DownlineNode
            {
                AccountID = accountId,
                ParentID = parentId,
                Parent = parentNode,
                Level = level,
                AccountTypeID = accountTypeID,
                AccountStatusID = accountStatusID
            };

            var children = new List<dynamic>();
            if (downline.SponsorshipTree.ContainsKey(accountId))
            {
                children = downline.SponsorshipTree[accountId];
            }

            if (!parentId.HasValue)
            {
                treeVisits.Add(accountId, accountId);
            }

            ++level;
            newNode.Children = children.Where(d => !treeVisits.ContainsKey(d.AccountID)).Select<dynamic, DownlineNode>(d =>
            {
                treeVisits.Add(d.AccountID, d.AccountID);
                PropertyInfo[] propertyInfo = d.GetType().GetProperties();
                var node = BuildNode(treeVisits, d.AccountID, downline, accountId, newNode, level, d.AccountTypeID, PropertyInfoContainsName(propertyInfo, "AccountStatusID") ? d.AccountStatusID : null);
                downline.LookupNode.Add(d.AccountID, node);
                return node;
            }).ToList();

            // Add root node to LookupNode dictionary.
            if (!parentId.HasValue)
            {
                downline.LookupNode[newNode.AccountID] = newNode;
            }

            return newNode;
        }

        private static bool PropertyInfoContainsName(IEnumerable<PropertyInfo> info, string propertyName)
        {
            bool success = info.Any(p => p.Name == propertyName);
            return success;
        }
    }
}