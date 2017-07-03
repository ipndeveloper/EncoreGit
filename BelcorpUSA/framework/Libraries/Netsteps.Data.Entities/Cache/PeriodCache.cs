namespace NetSteps.Data.Entities.Cache
{
    using System.Collections.Generic;

    public class PeriodCache
    {
        private readonly Dictionary<int, List<int>> _accountRelations;

        public PeriodCache()
        {
            _accountRelations = new Dictionary<int, List<int>>();
        }

        public void AddRelation(int accountId, int sponsorId)
        {
            if (_accountRelations.ContainsKey(sponsorId))
                _accountRelations[sponsorId].Add(accountId);
            else
                _accountRelations.Add(sponsorId, new List<int> { accountId });
        }

        public List<int> this[int i] { get { return _accountRelations[i]; } }

    }
}