using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;

namespace NetSteps.Data.Entities
{
    public class ReportResults
    {
        public int TotalCount { get; set; }

        public Downline Downline { get; set; }
        public DownlineNode CurrentNode { get; set; }
        public IEnumerable<DownlineNode> Results { get; set; }
        private IEnumerable _resultsDetails = null;
        public IEnumerable ResultsDetails
        {
            get
            {
                if (_resultsDetails == null)
                {
                    List<dynamic> data = new List<dynamic>();
                    foreach (var child in Results)
                    {
                        var lookup = Downline.Lookup[child.AccountID];
                        if (lookup != null)
                            data.Add(lookup);
                    }
                    _resultsDetails = data;
                }
                return _resultsDetails;
            }
            set
            {
                _resultsDetails = value;
            }
        }

        private IEnumerable<dynamic> _currentNodeFlatChildrenDetails = null;
        public IEnumerable<dynamic> CurrentNodeFlatChildrenDetails
        {
            get
            {
                if (_currentNodeFlatChildrenDetails == null)
                {
                    List<dynamic> data = new List<dynamic>();
                    if (CurrentNode != null)
                    {
                        foreach (var child in CurrentNode.FlatChildren)
                        {
                            var lookup = Downline.Lookup[child.AccountID];
                            if (lookup != null)
                                data.Add(lookup);
                        }
                    }
                    _currentNodeFlatChildrenDetails = data;
                }
                return _currentNodeFlatChildrenDetails;
            }
        }


        public IEnumerable<dynamic> OrderBySponsor(int accountID, int periodID, int maxLevels)
        {
            List<dynamic> data = new List<dynamic>();

            Downline downline = DownlineCache.GetDownline(periodID, accountID);

            var parentNodeDetails = downline.Lookup[accountID];
            var parentNode = downline.LookupNode[accountID];

            data.Add(parentNodeDetails);

            List<DownlineNode> childernNodes = parentNode.Children.ToList();

            OrderByTreee(data, downline, childernNodes, 1, maxLevels);

            return data;
        }

        private void OrderByTreee(List<dynamic> data, Downline downline, List<DownlineNode> childNodes, int level, int maxLevels)
        {
            if (childNodes.Count > 0)
            {
                foreach (DownlineNode child in childNodes)
                {
                    var childNodeDetails = downline.Lookup[child.AccountID];
                    data.Add(childNodeDetails);

                    if (level < maxLevels)
                        OrderByTreee(data, downline, child.Children.ToList(), level + 1, maxLevels);
                }
            }
        }
    }
}