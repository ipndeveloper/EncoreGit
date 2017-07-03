using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Miche.Orders.Converters;
using Miche.Orders.DataModel;
using Miche.Orders.Query.Order;
using Miche.Orders.Repository;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Validation.BatchProcess.Common;
using NetSteps.Validation.BatchProcess.LogWriters.Common;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;
using NetSteps.Validation.Conversion.Core;
using NetSteps.Validation.Handlers.Encore.Common.Services;
using NetSteps.Validation.BatchProcess.TempTableWriters.Common;
using System.Configuration;
using NetSteps.Foundation.Common;
using NetSteps.Validation.Handlers.Common.Services;
using System.Diagnostics;
using System.Threading;

namespace Miche.OrderValidation.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Kickin....");

            WireupCoordinator.SelfConfigure();

            runProgram();

            Thread.Sleep(10000);
        }

        private static void validateComponents()
        {
            validateRegistration<IRecordPropertyCalculationHandlerResolver>();
            validateRegistration<IRecordValidationService>();
            validateRegistration<IBatchValidationService>();

            validateHandlerRegistration<NetSteps.Validation.Handlers.Order.Order_ParentOrderID_ValidationHandler>("Order.ParentOrderID");

            validateHandlerRegistration<NetSteps.Validation.Handlers.OrderItemPrice.OrderItemPrice_OriginalUnitPrice_ValidationHandler>("OrderItemPrice.OriginalUnitPrice");
            validateHandlerRegistration<NetSteps.Validation.Handlers.OrderItemPrice.OrderItemPrice_UnitPrice_ValidationHandler>("OrderItemPrice.UnitPrice");
        }

        private static void runProgram()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("Create repository.");
            var repository = new OrderRepository((typename) => { return Create.New<IRecordConverter<Order>>(); });

            Console.WriteLine("Create query.");
            //var query = new SingleOrderQuery(579850);
            //var query = new DateRangeOrderQuery(new DateTime(2013, 5, 19, 4, 0, 0), new DateTime(2013, 5, 19, 8, 0, 0)) { MaximumBufferRecords = 500 };
            var query = new DateRangeOrderQuery(DateTime.Today.AddDays(-60), DateTime.Today) { MaximumBufferRecords = 2000 };

            Console.WriteLine("Create loggers.");
            var resultManagers = new List<IRecordValidationResultManager>();

            var path = String.Format("Output\\{0}",DateTime.Now.ToString("yyyyMMdd_HHmmss"));
            #region Broken SQL logger
            {
                var sqlSerializer = Container.Current.NewNamedWithParams<IRecordValidationSerializer>
                    (
                        "SqlSerializer",
                        Param.Named<ValidationResultKind>("primaryRecordResultTypes", ValidationResultKind.IsBroken),
                        Param.Named<ValidationResultKind>("resultTypes", ValidationResultKind.All),
                        Param.Named<ValidationCommentKind>("commentTypes", ValidationCommentKind.All),
                        Param.Named<bool>("indentChildRecords", true)
                    );

                var sqlLogWriter = Create.NewWithParams<ISingleValidationFileLogWriter>(
                                                                                        LifespanTracking.Default,
                                                                                        Param.Named("serializer", sqlSerializer),
                                                                                        Param.Named("filePath", path),
                                                                                        Param.Named("filePrefix", "Broken Orders (SQL)"),
                                                                                        Param.Named("fileExtension", "sql")
                                                                                        );

                var manager = Container.Current.NewWithParams<IRecordValidationResultManager>(
                                                                                    Param.Named("writer", sqlLogWriter)
                                                                                );

                resultManagers.Add(manager);
            }
            #endregion

            #region Incorrect Order SQL logger
            {
                var sqlSerializer = Container.Current.NewNamedWithParams<IRecordValidationSerializer>
                    (
                        "SqlSerializer",
                        Param.Named<ValidationResultKind>("primaryRecordResultTypes", ValidationResultKind.IsIncorrect | ValidationResultKind.IsNew | ValidationResultKind.IsWithinMarginOfError),
                        Param.Named<ValidationResultKind>("resultTypes", ValidationResultKind.All),
                        Param.Named<ValidationCommentKind>("commentTypes", ValidationCommentKind.All),
                        Param.Named<bool>("indentChildRecords", true)
                    );

                var sqlLogWriter = Create.NewWithParams<ISingleValidationFileLogWriter>(
                                                                                        LifespanTracking.Default,
                                                                                        Param.Named("serializer", sqlSerializer),
                                                                                        Param.Named("filePath", path),
                                                                                        Param.Named("filePrefix", "Incorrect Order Log (SQL)"),
                                                                                        Param.Named("fileExtension", "sql")
                                                                                        );

                var manager = Container.Current.NewWithParams<IRecordValidationResultManager>(
                                                                                    Param.Named("writer", sqlLogWriter)
                                                                                );

                resultManagers.Add(manager);
            }
            #endregion

            #region Margin of Error Order SQL logger
            {
                var sqlSerializer = Container.Current.NewNamedWithParams<IRecordValidationSerializer>
                    (
                        "SqlSerializer",
                        Param.Named<ValidationResultKind>("primaryRecordResultTypes", ValidationResultKind.IsWithinMarginOfError),
                        Param.Named<ValidationResultKind>("resultTypes", ValidationResultKind.All),
                        Param.Named<ValidationCommentKind>("commentTypes", ValidationCommentKind.All),
                        Param.Named<bool>("indentChildRecords", true)
                    );

                var sqlLogWriter = Create.NewWithParams<ISingleValidationFileLogWriter>(
                                                                                        LifespanTracking.Default,
                                                                                        Param.Named("serializer", sqlSerializer),
                                                                                        Param.Named("filePath", path),
                                                                                        Param.Named("filePrefix", "Margin Of Error Log (SQL)"),
                                                                                        Param.Named("fileExtension", "sql")
                                                                                        );

                var manager = Container.Current.NewWithParams<IRecordValidationResultManager>(
                                                                                    Param.Named("writer", sqlLogWriter)
                                                                                );

                resultManagers.Add(manager);
            }
            #endregion

            #region Warning logger
            {
                var warningSerializer = Container.Current.NewNamedWithParams<IRecordValidationSerializer>
                    (
                        "FlatTextSerializer",
                        Param.Named<ValidationResultKind>("resultTypes", ValidationResultKind.IsWithinMarginOfError),
                        Param.Named<ValidationCommentKind>("commentTypes", ValidationCommentKind.All),
                        Param.Named<bool>("indentChildRecords", true)
                    );
                
                var warningLogWriter = Create.NewWithParams<ISingleValidationFileLogWriter>(
                                                                                        LifespanTracking.Default,
                                                                                        Param.Named("serializer", warningSerializer),
                                                                                        Param.Named("filePath", path),
                                                                                        Param.Named("filePrefix", "Warning"),
                                                                                        Param.Named("fileExtension", "txt")
                                                                                        );

                var manager = Container.Current.NewWithParams<IRecordValidationResultManager>(
                                                                                    Param.Named("writer", warningLogWriter)
                                                                                );

                resultManagers.Add(manager);
            }
            #endregion

            #region Error logger
            {
                var errorSerializer = Container.Current.NewNamedWithParams<IRecordValidationSerializer>
                    (
                        "FlatTextSerializer",
                        Param.Named<ValidationCommentKind>("commentTypes", ValidationCommentKind.Error),
                        Param.Named<bool>("indentChildRecords", true)
                    );
                
                var errorLogger = Create.NewWithParams<ISingleValidationFileLogWriter>(
                                                                                        LifespanTracking.Default,
                                                                                        Param.Named("serializer", errorSerializer),
                                                                                        Param.Named("filePath", path),
                                                                                        Param.Named("filePrefix", "Error"),
                                                                                        Param.Named("fileExtension", "txt")
                                                                                        );

                var manager = Container.Current.NewWithParams<IRecordValidationResultManager>(
                                                                                      Param.Named("writer", errorLogger)
                                                                                );


                resultManagers.Add(manager);
            }
            #endregion

            #region Record Identifier logger
            {
                var errorSerializer = Container.Current.NewNamedWithParams<IRecordValidationSerializer>
                    (
                        "CommaDelimitedSerializer",
                        Param.Named<ValidationResultKind>("resultTypes", ValidationResultKind.All),
                        Param.Named<ValidationCommentKind>("commentTypes", ValidationCommentKind.PrimaryRecordIdentifier),
                        Param.Named<bool>("indentChildRecords", false)
                    );

                var errorLogger = Create.NewWithParams<ISingleValidationFileLogWriter>(
                                                                                        LifespanTracking.Default,
                                                                                        Param.Named("serializer", errorSerializer),
                                                                                        Param.Named("filePath", path),
                                                                                        Param.Named("filePrefix", "Record Identifiers"),
                                                                                        Param.Named("fileExtension", "csv")
                                                                                        );

                var manager = Container.Current.NewWithParams<IRecordValidationResultManager>(
                                                                                      Param.Named("writer", errorLogger)
                                                                                );


                resultManagers.Add(manager);
            }
            #endregion


            #region Field Report logger
            {
                var errorSerializer = Container.Current.NewNamedWithParams<IRecordValidationSerializer>
                    (
                        "FieldReportCommaDelimitedSerializer",
                        Param.Named<ValidationResultKind>("resultTypes", ValidationResultKind.All)
                    );

                var errorLogger = Create.NewWithParams<ISingleValidationFileLogWriter>(
                                                                                        LifespanTracking.Default,
                                                                                        Param.Named("serializer", errorSerializer),
                                                                                        Param.Named("filePath", path),
                                                                                        Param.Named("filePrefix", "Record Property Report"),
                                                                                        Param.Named("fileExtension", "csv")
                                                                                        );

                var manager = Container.Current.NewWithParams<IRecordValidationResultManager>(
                                                                                      Param.Named("writer", errorLogger)
                                                                                );


                resultManagers.Add(manager);
            }
            #endregion

            //#region SQL Temp Table logger
            //{
            //    var tempTableLogger = Create.NewWithParams<ITempTableOutputWriter>(
            //                                                                            LifespanTracking.Default,
            //                                                                            Param.Named("connectionString", ConfigurationManager.ConnectionStrings[ConnectionStringNames.Core].ConnectionString),
            //                                                                            Param.Named("maxRowsBeforeCommit", 1000),
            //                                                                            Param.Named("validationSchemaName", "validation"),
            //                                                                            Param.Named("filePath", path)
            //                                                                             );

            //    var manager = Container.Current.NewWithParams<IRecordValidationResultManager>(
            //                                                                        Param.Named("writer", tempTableLogger)
            //                                                                    );


            //    resultManagers.Add(manager);
            //}
            //#endregion

            Func<IEnumerable<IDependentDataService>> dependentServiceCollector = () =>
                {
                    var dependentServices = new List<IDependentDataService>
                        {
                            Container.Current.New<IOrderItemPricingService>(),
                            Container.Current.New<IPaymentService>(),
                            Container.Current.New<IOrderCommissionService>()
                        };
                    return dependentServices;
                };

            Console.WriteLine("Get batch service.");
            var service = Create.New<IBatchValidationService>();
            service.ProcessBatch(repository, query, resultManagers, dependentServiceCollector);

            stopwatch.Stop();
            Console.WriteLine(String.Format("Process completed in {0}.", stopwatch.Elapsed.ToString(@"hh\:mm\:ss")));
        }

        private static void validateRegistration<TRegistered>()
        {
            var obj = Create.New<TRegistered>();
            if (obj != null)
            {
                Console.WriteLine(String.Format("Success: {0}", typeof(TRegistered).Name));
            }
            else
            {
                Console.WriteLine(String.Format("Fail: {0} ", typeof(TRegistered).Name));
            }
        }

        private static void validateHandlerRegistration<TConcreteHandler>(string recordKind)
        {
            var handler = Create.NewNamed<IRecordPropertyCalculationHandler>(recordKind);
            if (handler != null)
            {
                if (handler is TConcreteHandler)
                {
                    Console.WriteLine(String.Format("Success: {0}", typeof(TConcreteHandler).Name));
                }
                else
                {
                    Console.WriteLine(String.Format("Fail-WrongType: RecordKind {0}", recordKind, handler.GetType().Name, typeof(TConcreteHandler).Name));
                }
            }
            else
            {
                Console.WriteLine(String.Format("Fail-NotRegistered: RecordKind {0}.", recordKind));
            }
        }
    }
}
