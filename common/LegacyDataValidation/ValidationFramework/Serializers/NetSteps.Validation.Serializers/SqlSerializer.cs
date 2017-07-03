using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;

namespace NetSteps.Validation.Serializers
{
    public class SqlSerializer : IRecordValidationSerializer
    {
        private readonly ValidationCommentKind _commentTypes;
        private readonly ValidationResultKind _resultTypes;
        private readonly ValidationResultKind _primaryRecordResultTypes;
        private readonly bool _indentChildRecords;

        public SqlSerializer() : this(ValidationResultKind.All, ValidationResultKind.All, ValidationCommentKind.All, false) { }

        public SqlSerializer(bool indentChildRecords) : this(ValidationResultKind.All, ValidationResultKind.All, ValidationCommentKind.All, indentChildRecords) { }

        public SqlSerializer(ValidationCommentKind commentTypes) : this(ValidationResultKind.All, ValidationResultKind.All, commentTypes, false) { }

        public SqlSerializer(ValidationResultKind primaryRecordResultTypes) : this(primaryRecordResultTypes, ValidationResultKind.All, ValidationCommentKind.All, false) { }

        public SqlSerializer(ValidationCommentKind commentTypes, bool indentChildRecords) : this(ValidationResultKind.All, ValidationResultKind.All, commentTypes, indentChildRecords) { }

        public SqlSerializer(ValidationResultKind primaryRecordResultTypes, bool indentChildRecords) : this(primaryRecordResultTypes, ValidationResultKind.All, ValidationCommentKind.All, indentChildRecords) { }

        public SqlSerializer(ValidationResultKind primaryRecordResultTypes, ValidationCommentKind commentTypes) : this(primaryRecordResultTypes, ValidationResultKind.All, commentTypes, false) { }

        public SqlSerializer(ValidationResultKind primaryRecordResultTypes, ValidationResultKind resultTypes, ValidationCommentKind commentTypes, bool indentChildRecords)
        {
            _commentTypes = commentTypes;
            _indentChildRecords = indentChildRecords;
            _resultTypes = resultTypes;
            _primaryRecordResultTypes = primaryRecordResultTypes;
        }

        public string Serialize(IRecord record)
        {
            if (!record.Result.IsIn(_primaryRecordResultTypes))
            {
                return String.Empty;
            }

            var builder = new StringBuilder();
            RecursiveSerialize(builder, record, 3);
            if (builder.Length > 0)
            {
                builder.Insert(0,
@"BEGIN TRY
    SELECT @starttrancount = @@TRANCOUNT
   IF @starttrancount = 0
        BEGIN TRANSACTION
");
                builder.Append(
@"  IF @starttrancount = 0
        COMMIT TRANSACTION
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 AND @starttrancount = 0
        ROLLBACK TRANSACTION
    PRINT 'ERROR FOUND IN ");
                builder.Append(record.RecordKind);
                builder.Append(":");
                builder.Append(record.RecordIdentity ?? "NULL");
                builder.Append(@"'
END CATCH");
                builder.Append(Environment.NewLine);
                builder.Append("GO");
                builder.Append(Environment.NewLine);
            }
            return builder.ToString();
        }

        private void RecursiveSerialize(StringBuilder builder, IRecord record, int tabCount)
        {
            try
            {
                if (!record.Result.IsIn(_resultTypes))
                {
                    return;
                }

                if (ValidationCommentKind.PrimaryRecordIdentifier.IsIn(_commentTypes) || ValidationCommentKind.ChildRecordIdentifier.IsIn(_commentTypes))
                {
                    WriteRecordValidatorComment(builder, record, tabCount);
                }

                var updateFields = new List<string>();
                var updateValues = new List<string>();
                foreach (var prop in record.Properties)
                {
                    switch (prop.Value.ResultKind)
                    {
                        case ValidationResultKind.IsCorrect:
                            break;
                        case ValidationResultKind.IsFactual:
                            break;
                        case ValidationResultKind.IsIncorrect:
                            updateFields.Add(prop.Key);
                            if (prop.Value.ExpectedValue == null)
                            {
                                updateValues.Add(null);
                            }
                            else
                            {
                                updateValues.Add(prop.Value.ExpectedValue.ToString());
                            }

                            break;
                        case ValidationResultKind.IsWithinMarginOfError:
                            break;
                        case ValidationResultKind.Unvalidated:
                            break;
                    }
                }

                foreach (var comment in record.ValidationComments)
                {
                    if (!comment.CommentKind.IsIn(ValidationCommentKind.AllRecordIdentifiers) && comment.CommentKind.IsIn(_commentTypes))
                    {
                        AddIndent(builder, tabCount);
                        builder.Append("--");
                        builder.Append(comment.PrimaryMessage);
                        builder.Append(Environment.NewLine);
                    }
                }

                if (record.Result != ValidationResultKind.IsNew)
                {
                    if (updateFields.Count > 0)
                    {
                        AddIndent(builder, tabCount);
                        builder.Append("UPDATE [");
                        builder.Append(record.RecordKind);
                        builder.Append("] SET ");
                        for (int i = 0; i < updateFields.Count; i++)
                        {
                            builder.Append(updateFields[i]);
                            builder.Append("=");
                            if (updateValues[i] == null)
                            {
                                builder.Append("null");
                            }
                            else
                            {
                                builder.Append(updateValues[i]);
                            }
                            if (i < updateFields.Count - 1)
                                builder.Append(',');
                        }
                        builder.Append(" WHERE ");
                        builder.Append(record.RecordIdentityField);
                        builder.Append('=');
                        builder.Append(record.RecordIdentity.ToString());
                        builder.Append(Environment.NewLine);
                    }
                }
                else
                {
                    AddIndent(builder, tabCount);
                    builder.Append("INSERT INTO [");
                    builder.Append(record.RecordKind);
                    builder.Append("] (");
                    builder.Append(string.Join(",", record.Properties.Select(x => x.Key)));
                    builder.Append(") VALUES (");
                    builder.Append(string.Join(",", record.Properties.Select(x => x.Value.ExpectedValue)));
                    builder.Append(")");
                    builder.Append(Environment.NewLine);

                }

                foreach (var child in record.ChildRecords)
                {
                    RecursiveSerialize(builder, child, _indentChildRecords ? tabCount + 1 : tabCount);
                }
            }
            catch (Exception ex)
            {
                AddIndent(builder, tabCount);
                builder.Append(ex.ToString());
                builder.Append(Environment.NewLine);
            }
        }

        private void WriteRecordValidatorComment(StringBuilder builder, IRecord record, int tabCount)
        {
            var recordIdentifierComments = record.ValidationComments.Where(x => x.CommentKind.IsIn(ValidationCommentKind.AllRecordIdentifiers) && x.CommentKind.IsIn(_commentTypes));
            foreach (var recordIdentifierComment in recordIdentifierComments)
            {
                AddIndent(builder, tabCount);
                builder.Append("--");
                builder.Append(recordIdentifierComment.PrimaryMessage);
                builder.Append(Environment.NewLine);
            }
        }

        private void AddIndent(StringBuilder builder, int tabCount)
        {
            builder.Append(new string('\t', tabCount));
        }
    }
}
