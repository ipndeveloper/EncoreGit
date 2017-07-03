using System;
using System.Data.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using OrderRules.Core.Model;
using OrderRules.Data.UnitOfWork;
using OrderRules.Data.UnitOfWork.Interface;
using OrderRules.Data.Repository;
using OrderRules.Data.Repository.Interface;
using OrderRules.Service;
using OrderRules.Service.Interface;


// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(OrderRules.Service.ModuleWireup))]

namespace OrderRules.Service
{
    /// <summary>
    /// Wireup command called at bootstrap time by the wireup coordinator.
    /// </summary>
    [WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {

        /// <summary>
        /// Wires this module.
        /// </summary>
        /// <param name="coordinator">the coordinator</param>
        /// <seealso cref="IWireupCoordinator"/>
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            Container.Root.ForType<CoreEntities>().Register<CoreEntities>().ResolveAsSingleton().End();

            Container.Root
                .ForType<IUnitOfWork>()
                .Register<UnitOfWork>(Param.Resolve<CoreEntities>())
                .ResolveAsSingleton()
                .End();

            Container.Root
                 .ForType<IRuleStatusesRepository>()
                 .Register<RuleStatusesRepository>(Param.Resolve<CoreEntities>())
                 .ResolveAnInstancePerRequest()
                 .End();

            Container.Root
                 .ForType<IRulesRepository>()
                 .Register<RulesRepository>(Param.Resolve<CoreEntities>())
                 .ResolveAnInstancePerRequest()
                 .End();

            Container.Root
                 .ForType<IRuleValidationsRepository>()
                 .Register<RuleValidationsRepository>(Param.Resolve<CoreEntities>())
                 .ResolveAnInstancePerRequest()
                 .End();

            Container.Root
                .ForType<IOrderRulesService>()
                .Register<OrderRulesService>
                    (
                        Param.Resolve<IRulesRepository>(),
                        Param.Resolve<IUnitOfWork>()
                    )
                .ResolveAsSingleton()
                .End();
        }
    }
}
