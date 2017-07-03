//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Business.Logic.Interfaces;
using NetSteps.Data.Entities.Commissions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
    [Serializable]
    public partial class DomainEventCategoryTypeBusinessLogic : BusinessLogicBase<DomainEventCategoryType, Int32, IDomainEventCategoryTypeRepository, IDomainEventCategoryTypeBusinessLogic>, IDomainEventCategoryTypeBusinessLogic, IDefaultImplementation
    {
    	public override Func<DomainEventCategoryType, Int32> GetIdColumnFunc
        {
            get
            {
                return i => i.DomainEventCategoryTypeID;
            }
        }
    	public override Action<DomainEventCategoryType, Int32> SetIdColumnFunc
        {
            get
            {
                return (DomainEventCategoryType i, Int32 id) => i.DomainEventCategoryTypeID = id;
            }
        }
        public override Func<DomainEventCategoryType, string> GetTitleColumnFunc
        {
            get
            {
                return i => i.Name;
            }
        }
    	public override Action<DomainEventCategoryType, string> SetTitleColumnFunc
        {
            get
            {
                return (DomainEventCategoryType i, string title) => i.Name = title;
            }
        }
    }
}