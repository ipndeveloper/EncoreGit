using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Serializers
{
    public class FieldReportCommaDelimitedSerializer : IRecordValidationSerializer
    {
        private readonly ValidationResultKind _resultTypes;
        
        public FieldReportCommaDelimitedSerializer(ValidationResultKind resultTypes)
        {
            _resultTypes = resultTypes;
        }

        public string Serialize(Common.Model.IRecord record)
        {
            var countDictionary = new Dictionary<string, IDictionary<ValidationResultKind, int>>();
            var builder = new StringBuilder();
            RecursiveCounter(record, countDictionary);
            foreach (var entry in countDictionary)
            {
                foreach (var resultKindSet in entry.Value)
                {
                    builder.Append(record.RecordIdentity);
                    builder.Append(",");
                    builder.Append(entry.Key);
                    builder.Append(",");
                    builder.Append(resultKindSet.Key);
                    builder.Append(",");
                    builder.Append(resultKindSet.Value);
                    builder.Append(Environment.NewLine);
                }
            }
            return builder.ToString();
        }


        public void RecursiveCounter(IRecord record, IDictionary<string, IDictionary<ValidationResultKind, int>> countDictionary)
        {
            if (!record.Result.IsIn(_resultTypes))
            {
                return;
            }
            foreach (var property in record.Properties.Values)
            {
                switch (property.ResultKind)
                {
                    case ValidationResultKind.IsBroken:
                    case ValidationResultKind.IsIncorrect:
                    case ValidationResultKind.IsNew:
                    case ValidationResultKind.IsWithinMarginOfError:
                        RecordCount(property, countDictionary);
                        break;
                    default:
                        break;
                }
            }
            if (record.ChildRecords != null)
            {
                foreach (var child in record.ChildRecords)
                {
                    RecursiveCounter(child, countDictionary);
                }
            }
        }

        private void RecordCount(IRecordProperty property, IDictionary<string, IDictionary<ValidationResultKind, int>> countDictionary)
        {
            var propertyKey = String.Format("{0}.{1}", property.ParentRecord.RecordKind, property.Name);
            if (!countDictionary.ContainsKey(propertyKey))
            {
                countDictionary.Add(propertyKey, new Dictionary<ValidationResultKind, int>());
            }
            if (!countDictionary[propertyKey].ContainsKey(property.ResultKind))
            {
                countDictionary[propertyKey].Add(property.ResultKind, 0);
            }
            countDictionary[propertyKey][property.ResultKind]++;
        }
    }
}
