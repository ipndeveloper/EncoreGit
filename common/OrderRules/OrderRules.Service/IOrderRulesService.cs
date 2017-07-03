using System;
using System.Collections.Generic;
using OrderRules.Core.Model;

namespace OrderRules.Service.Interface
{
    public interface IOrderRulesService
    {
        void Commit();
        void CreateRule(Rules rules);
        void DeleteRule(int id);
        Rules GetRuleById(int id);
        IEnumerable<Rules> GetRules();
        void UpdateRule(Rules rules);
    }
}
