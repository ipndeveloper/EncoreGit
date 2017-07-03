using System;
using System.Collections.Generic;
using Fasterflect;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class Downline
    {
        private static Type _dynamicType = null;
        public Type DynamicType
        {
            get
            {
                return _dynamicType;
            }
            internal set
            {
                _dynamicType = value;
            }
        }

        // Cached Getters for the dynamically created Type - JHE
        private static Dictionary<string, MemberGetter> _dynamicTypeGetters = null;
        public Dictionary<string, MemberGetter> DynamicTypeGetters
        {
            get
            {
                return _dynamicTypeGetters;
            }
            internal set
            {
                _dynamicTypeGetters = value;
            }
        }

        private Dictionary<int, List<dynamic>> _sponsorshipTree = new Dictionary<int, List<dynamic>>();
        public Dictionary<int, List<dynamic>> SponsorshipTree
        {
            get { return _sponsorshipTree; }
        }

        private Dictionary<int, dynamic> _lookup = new Dictionary<int, dynamic>();
        public virtual Dictionary<int, dynamic> Lookup
        {
            get { return _lookup; }
        }

        // Use this in the search method for quicker lookups - JHE
        private Dictionary<int, DownlineNode> _lookupNode = new Dictionary<int, DownlineNode>();
        public Dictionary<int, DownlineNode> LookupNode
        {
            get { return _lookupNode; }
        }

        public DownlineNode Root { get; set; }

        public DateTime LastUpdated { get; set; }
        public DateTime LastAccessed { get; set; }

        public int PeriodID { get; set; }

        public DownlineNode GetTree(int accountId)
        {
	        if(LookupNode.ContainsKey(accountId))
	        {
		        return LookupNode[accountId];
	        }
            
			return null;
        }
    }
}