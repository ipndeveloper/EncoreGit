using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Common.Model
{
    [ContractClass(typeof(ValidationCommentContract))]
    public interface IValidationComment
    {
        string PrimaryMessage { get; }
        IDictionary<string, string> AdditionalMessageComponents { get; }
        ValidationCommentKind CommentKind { get; }
    }

    [ContractClassFor(typeof(IValidationComment))]
    public abstract class ValidationCommentContract : IValidationComment
    {
        public string PrimaryMessage
        {
            get { throw new NotImplementedException(); }
        }

        public ValidationCommentKind CommentKind
        {
            get { throw new NotImplementedException(); }
        }


        public IDictionary<string, string> AdditionalMessageComponents
        {
            get { throw new NotImplementedException(); }
        }
    }
}
