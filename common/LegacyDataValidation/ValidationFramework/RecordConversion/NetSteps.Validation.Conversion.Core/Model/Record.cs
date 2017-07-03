using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;

namespace NetSteps.Validation.Conversion.Core.Model
{
    public class Record : IRecord
    {
        public Record(IRecord parent, string sourceSchema, string sourceTableName)
        {
            Result = ValidationResultKind.Unvalidated;
            Properties = new Dictionary<string, IRecordProperty>();
            ChildRecords = new List<IRecord>();
            Parent = parent;
            _validationComments = new List<IValidationComment>();
            Source = new RecordSource(sourceSchema, sourceTableName);
        }

        public string RecordKind { get; set; }

        public object RecordIdentity
        {
            get
            {
                if (Properties.ContainsKey(RecordIdentityField))
                {
                    return Properties[RecordIdentityField].OriginalValue;
                }
                else
                {
                    return null;
                }
            }
        }

        public string RecordIdentityField { get; set; }

        public IDictionary<string, IRecordProperty> Properties { get; internal set; }

        public IRecord Parent { get; private set; }

        public ValidationResultKind Result { get; private set; }

        public ICollection<IRecord> ChildRecords { get; private set; }

        public void SetResult(ValidationResultKind result)
        {
            Result = Result.EscalateTo(result);
        }

        private IList<IValidationComment> _validationComments;

        public IEnumerable<IValidationComment> ValidationComments
        {
            get { return _validationComments; }
        }

        public IValidationComment AddValidationComment(ValidationCommentKind commentKind, string comment)
        {
            var newComment = new ValidationComment(commentKind, comment);
            _validationComments.Add(newComment);
            return newComment;
        }

        public IRecordSource Source { get; private set; }
    }
}
