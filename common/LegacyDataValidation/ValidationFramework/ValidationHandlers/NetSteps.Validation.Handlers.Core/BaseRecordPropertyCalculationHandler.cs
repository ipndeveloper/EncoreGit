using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;

namespace NetSteps.Validation.Handlers.Core
{
    public abstract class BaseRecordPropertyCalculationHandler : IRecordPropertyCalculationHandler
    {
        public BaseRecordPropertyCalculationHandler(IRecordPropertyCalculationHandlerResolver resolver)
        {
            HandlerResolver = resolver;
        }
        protected IRecordPropertyCalculationHandlerResolver HandlerResolver { get; private set; }

        public void Calculate(IRecordProperty propertyToCalculate)
        {
            try
            {
                CalculateExpectedValue(propertyToCalculate);
            }
            catch(Exception ex)
            {
                propertyToCalculate.SetResult(ValidationResultKind.IsBroken);
                propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.Error, String.Format("{0}({1}).{2} Calculation Exception: {3} StackTrace: {4}", propertyToCalculate.ParentRecord.RecordKind, propertyToCalculate.ParentRecord.RecordIdentity, propertyToCalculate.Name, ex.Message, ex.StackTrace));
            }
        }

        public virtual void CalculateExpectedValue(IRecordProperty propertyToCalculate)
        {
            propertyToCalculate.ExpectedValue = propertyToCalculate.OriginalValue;
            propertyToCalculate.SetResult(ValidationResultKind.IsFactual);
        }

        public object CalculateDependentValue(IRecord record, string propertyName)
        {
            if (!record.Properties.ContainsKey(propertyName))
                return null;
            if (record.Properties[propertyName].ResultKind == ValidationResultKind.IsNew)
            {
                if (record.Properties[propertyName].ExpectedValue == null)
                {
                    var handler = HandlerResolver.GetHandler(record.RecordKind, propertyName);
                    handler.Calculate(record.Properties[propertyName]);
                }
            }
            else if (record.Properties[propertyName].ResultKind == ValidationResultKind.Unvalidated)
            {
                var handler = HandlerResolver.GetHandler(record.RecordKind, propertyName);
                handler.Calculate(record.Properties[propertyName]);
            }
            return record.Properties[propertyName].ExpectedValue;
        }
    }
}
