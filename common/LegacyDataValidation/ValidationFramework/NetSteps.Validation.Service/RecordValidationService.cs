using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;
using System.Diagnostics;

namespace NetSteps.Validation.Service
{
    public class RecordValidationService : IRecordValidationService
    {
        private IRecordPropertyCalculationHandlerResolver _resolver;

        public RecordValidationService(IRecordPropertyCalculationHandlerResolver resolver)
        {
            _resolver = resolver;
        }

        public IRecord Validate(IRecord recordToValidate)
        {
            RecursiveValidate(recordToValidate);
            return recordToValidate;
        }

        private void RecursiveValidate(IRecord validationRecord)
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var result = validationRecord.Result;
                foreach (var prop in validationRecord.Properties)
                {
                    // property may have been calculated already from a dependent property.  If the property is yet unvalidated, validate it.
                    if (prop.Value.ResultKind.IsIn(ValidationResultKind.Unvalidated | ValidationResultKind.IsNew))
                    {
                        var handler = _resolver.GetHandler(validationRecord.RecordKind, prop.Key);
                        handler.Calculate(prop.Value);
                    }

                    if (prop.Value.ResultKind == ValidationResultKind.IsNew)
                    {
                        result = result.EscalateTo(ValidationResultKind.IsIncorrect);
                    }
                    else
                    {
                        result = result.EscalateTo(prop.Value.ResultKind);
                    }
                }

                // if there are warnings we still need to throw an "IsWithinMarginOfError" message - warnings are within margins of acceptability.
                if (validationRecord.ValidationComments.Any(x => x.CommentKind == ValidationCommentKind.Warning))
                {
                    result = result.EscalateTo(ValidationResultKind.IsWithinMarginOfError);
                }

                foreach (var child in validationRecord.ChildRecords)
                {
                    RecursiveValidate(child);
                    if (child.Result == ValidationResultKind.IsNew)
                    {
                        result = result.EscalateTo(ValidationResultKind.IsIncorrect);
                    }
                    else
                    {
                        result = result.EscalateTo(child.Result);
                    }
                }
                validationRecord.SetResult(result);
                var recordIdentifierComment = validationRecord.ValidationComments.SingleOrDefault(x => x.CommentKind.IsIn(ValidationCommentKind.AllRecordIdentifiers));
                if (recordIdentifierComment != null)
                {
                    recordIdentifierComment.AdditionalMessageComponents.Add("Result", result.ToString());
                }
                stopwatch.Stop();
                validationRecord.AddValidationComment(ValidationCommentKind.Performance, String.Format("Validation time:{0} ms", stopwatch.ElapsedMilliseconds.ToString())); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
