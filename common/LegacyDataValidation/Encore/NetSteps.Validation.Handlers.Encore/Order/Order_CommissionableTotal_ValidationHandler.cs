using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;
using NetSteps.Validation.Handlers.Core;
using NetSteps.Validation.Handlers.Helpers;
using NetSteps.Validation.Handlers.Encore.Common.Services;
using NetSteps.Validation.Handlers.Common.Services;

namespace NetSteps.Validation.Handlers.Order
{
    public class Order_CommissionableTotal_ValidationHandler : BaseRecordPropertyCalculationHandler
    {
        public Order_CommissionableTotal_ValidationHandler(IRecordPropertyCalculationHandlerResolver resolver)
            : base(resolver)
        {
        }

        protected virtual decimal GetMaximumAllowedDeviancy(IRecord orderRecord)
        {
            return orderRecord.ChildRecords.Where(x => x.RecordKind == EncoreFieldNames.TableSingularNames.OrderCustomer).Sum(x => x.ChildRecords.Where(y => y.RecordKind == EncoreFieldNames.TableSingularNames.OrderItem).Count() * 0.005M);
        }

        public override void CalculateExpectedValue(Validation.Common.Model.IRecordProperty propertyToCalculate)
        {
            var orderRecord = propertyToCalculate.ParentRecord;

            decimal expectedTotal = 0M;
            foreach (var orderCustomer in orderRecord.ChildRecords.Where(x => x.RecordKind == EncoreFieldNames.TableSingularNames.OrderCustomer))
            {
                expectedTotal += (decimal)CalculateDependentValue(orderCustomer, EncoreFieldNames.OrderCustomer.CommissionableTotal); 
            }
            propertyToCalculate.ExpectedValue = expectedTotal;

            decimal original = (decimal)propertyToCalculate.OriginalValue;
            decimal expected = Math.Round((decimal)propertyToCalculate.ExpectedValue, 2);
            var maximumAllowedDeviancy = GetMaximumAllowedDeviancy(orderRecord);

            if (original == expected)
            {
                propertyToCalculate.SetResult(ValidationResultKind.IsCorrect);
            }
            else if (Math.Abs(original - expected) < maximumAllowedDeviancy)
            {
                propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("Order CommissionableTotal was {0}. Should be {1} but falls within the margin of error {2}.", propertyToCalculate.OriginalValue, propertyToCalculate.ExpectedValue, maximumAllowedDeviancy));
                propertyToCalculate.SetResult(ValidationResultKind.IsWithinMarginOfError);
            }
            else
            {
                propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("Order CommissionableTotal was {0}. Should be {1}.", propertyToCalculate.OriginalValue, propertyToCalculate.ExpectedValue));
                propertyToCalculate.SetResult(ValidationResultKind.IsIncorrect);
            }

            if (orderRecord.Properties[EncoreFieldNames.Order.CommissionDateUTC].OriginalValue != null)
            {
                var commissionService = ServiceLocator.FindService<IOrderCommissionService>();
                decimal commissionTotal;
                if (commissionService.GetStoredOrderCommission((int)orderRecord.RecordIdentity, out commissionTotal))
                {
                    if (commissionTotal == expected)
                    {
                        if (commissionTotal == original)
                        {
                            propertyToCalculate.SetResult(ValidationResultKind.IsCorrect);
                        }
                        else
                        {
                            propertyToCalculate.SetResult(ValidationResultKind.IsIncorrect);
                        }
                    }
                    else if (Math.Abs(commissionTotal - expected) < maximumAllowedDeviancy)
                    {
                        propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("Calculated Order CommissionableTotal {0} does not match Commissions total {1} but falls within the margin of error {2}.", expected, commissionTotal, maximumAllowedDeviancy));
                        propertyToCalculate.SetResult(ValidationResultKind.IsWithinMarginOfError);
                    } 
                    else if (!commissionTotal.Equals(expected))
                    {
                        propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("Calculated Order CommissionableTotal {0} does not match Commissions total {1}.", expected, commissionTotal));
                        propertyToCalculate.SetResult(ValidationResultKind.IsBroken);

                        if (commissionTotal.Equals(original))
                        {
                            propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("Stored Order CommissionableTotal {0} MATCHES Commissions total {1}. Possible calculation fail.", original, commissionTotal));
                        }
                    }
                    
                }
            }
        }
    }
}
