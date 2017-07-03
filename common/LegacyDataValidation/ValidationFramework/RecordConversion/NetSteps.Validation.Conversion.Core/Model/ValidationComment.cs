using NetSteps.Validation.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Conversion.Core.Model
{
    public class ValidationComment : IValidationComment
    {
        public ValidationComment(ValidationCommentKind commentKind, string primaryMessage)
        {
            PrimaryMessage = primaryMessage;
            CommentKind = commentKind;
            AdditionalMessageComponents = new Dictionary<string, string>();
        }

        public string PrimaryMessage { get; private set; }

        public ValidationCommentKind CommentKind { get; private set; }

        public IDictionary<string, string> AdditionalMessageComponents { get; private set; }
    }
}
