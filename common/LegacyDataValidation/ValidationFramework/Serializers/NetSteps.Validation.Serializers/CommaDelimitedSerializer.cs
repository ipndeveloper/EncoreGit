using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Serializers
{
    public class CommaDelimitedSerializer : IRecordValidationSerializer
    {
        private readonly ValidationCommentKind _commentTypes;
        private readonly ValidationResultKind _resultTypes;
        private readonly bool _indentChildRecords;

        public CommaDelimitedSerializer() : this(ValidationResultKind.All, ValidationCommentKind.All, false){}

        public CommaDelimitedSerializer(bool indentChildRecords) : this(ValidationResultKind.All, ValidationCommentKind.All, indentChildRecords) { }

        public CommaDelimitedSerializer(ValidationCommentKind commentTypes) : this(ValidationResultKind.All, commentTypes, false) { }

        public CommaDelimitedSerializer(ValidationResultKind resultTypes) : this(resultTypes, ValidationCommentKind.All, false) { }

        public CommaDelimitedSerializer(ValidationCommentKind commentTypes, bool indentChildRecords) : this(ValidationResultKind.All, commentTypes, indentChildRecords) { }

        public CommaDelimitedSerializer(ValidationResultKind resultTypes, bool indentChildRecords) : this(resultTypes, ValidationCommentKind.All, indentChildRecords) { }

        public CommaDelimitedSerializer(ValidationResultKind resultTypes, ValidationCommentKind commentTypes) : this(resultTypes, commentTypes, false) { }

        public CommaDelimitedSerializer(ValidationResultKind resultTypes, ValidationCommentKind commentTypes, bool indentChildRecords)
        {
            _commentTypes = commentTypes;
            _indentChildRecords = indentChildRecords;
            _resultTypes = resultTypes;
        }

        public string Serialize(Common.Model.IRecord record)
        {
            var builder = new StringBuilder();
            RecursiveSerialize(record, builder, 0);
            return builder.ToString();
        }


        public void RecursiveSerialize(IRecord record, StringBuilder builder, int tabCount)
        {
            if (!record.Result.IsIn(_resultTypes))
            {
                return;
            }
            try
            {
                var errors = record.ValidationComments.Where(x => x.CommentKind.IsIn(_commentTypes));
                foreach (var comment in errors)
                {
                    AddIndent(builder, tabCount);
                    if (comment.CommentKind.IsIn(_commentTypes))
                    {
                        builder.Append(comment.PrimaryMessage);
                        foreach (var additionalComponent in comment.AdditionalMessageComponents)
                        {
                            builder.Append(",");
                            builder.Append(additionalComponent.Value);
                        }
                    }
                    builder.Append(Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                AddIndent(builder, tabCount);
                builder.Append("EXCEPTION:");
                builder.Append(ex.Message);
                builder.Append(Environment.NewLine);
            }

            if (record.ChildRecords != null)
            {
                foreach (var child in record.ChildRecords)
                {
                    RecursiveSerialize(child, builder, _indentChildRecords ? tabCount + 1 : tabCount);
                }
            }
        }

        private void AddIndent(StringBuilder builder, int tabCount)
        {
            builder.Append(new string('\t', tabCount));
        }
    }
}
