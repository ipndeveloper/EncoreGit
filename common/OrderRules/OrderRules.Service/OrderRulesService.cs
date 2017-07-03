using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OrderRules.Core.Model;
using OrderRules.Data.Repository.Interface;
using OrderRules.Data.UnitOfWork.Interface;
using OrderRules.Service.Interface;

namespace OrderRules.Service
{
    public class OrderRulesService : IOrderRulesService
    {
        private readonly IRulesRepository _rulesRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OrderRulesService(IRulesRepository rulesRepository, IUnitOfWork unitOfWork)
        {
            this._rulesRepository = rulesRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Rules> GetRules()
        {
            var rules = _rulesRepository.Get();
            return rules;
        }
        public Rules GetRuleById(int id)
        {
            var rules = _rulesRepository.GetByID(id);
            return rules;
        }
        public void CreateRule(Rules rules)
        {
            _rulesRepository.Insert(rules);
            //_unitOfWork.Save();
        }

        public void DeleteRule(int id)
        {
            _rulesRepository.Delete(id);
            //_unitOfWork.Save();
        }

        public void UpdateRule(Rules rules)
        {
            _rulesRepository.Update(rules);
            //_unitOfWork.Save();
        }
        public void Commit()
        {
            _unitOfWork.Commit();
        } 

    }
}
