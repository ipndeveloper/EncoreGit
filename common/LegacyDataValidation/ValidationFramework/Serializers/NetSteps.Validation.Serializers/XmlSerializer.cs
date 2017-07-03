using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;

namespace NetSteps.Validation.Serializers
{
    public class XmlSerializer : IRecordValidationSerializer
    {
        private readonly ValidationCommentKind _commentTypes;
        private readonly bool _indentChildRecords;

        public XmlSerializer()
        {
            _commentTypes = ValidationCommentKind.None;
            _indentChildRecords = false;
        }

        public XmlSerializer(bool indentChildRecords)
        {
            _indentChildRecords = indentChildRecords;
        }

        public XmlSerializer(ValidationCommentKind commentTypes)
        {
            _commentTypes = commentTypes;
            _indentChildRecords = false;
        }

        public XmlSerializer(ValidationCommentKind commentTypes, bool indentChildRecords)
        {
            _commentTypes = commentTypes;
            _indentChildRecords = indentChildRecords;
        }

        public string Serialize(IRecord validation)
        {
            throw new NotImplementedException();
        }
    }
}
