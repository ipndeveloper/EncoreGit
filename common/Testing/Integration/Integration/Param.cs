using System;
using WatiN.Core.Constraints;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration
{
    public class Param
    {
        private Constraint _constraint;

        public Param()
        {
            _constraint = AnyConstraint.Instance;
        }

        public Param(int index)
        {
            _constraint = new IndexConstraint(index);
        }

        public Param(string attributeValue, string attributeName)
        {
            _constraint = new AttributeConstraint(attributeName, attributeValue);
        }

        public Param(string attributeValue, AttributeName.ID attributeName = AttributeName.ID.Id)
            : this(attributeValue, attributeName.ToString())
        {
        }

        public Param(string attributeValue, string attributeName, RegexOptions options)
        {
            _constraint = new AttributeConstraint(attributeName, new Regex(attributeValue, options));
        }

        public Param(string attributeValue, AttributeName.ID attributeName, RegexOptions options)
            : this(attributeValue, attributeName.ToString(), options)
        {
        }

        public Constraint Constraint
        {
            get { return _constraint; }
        }

        public Param Or(Param param)
        {
            _constraint = new OrConstraint(_constraint, param.Constraint);
            return this;
        }

        public Param And(Param param)
        {
            _constraint = new AndConstraint(_constraint, param.Constraint);
            return this;
        }

        [Obsolete("Use 'And(Param)'", true)]
        public Param And(int index)
        {
            _constraint = new AndConstraint(_constraint, new IndexConstraint(index));
            return this;
        }

        [Obsolete("Use 'And(Param)'", true)]
        public Param And(string attributeValue, AttributeName.ID attributeName = AttributeName.ID.Id)
        {
            _constraint = new AndConstraint(_constraint, new AttributeConstraint(attributeName.ToString(), attributeValue));
            return this;
        }

        [Obsolete("Use 'And(Param)'", true)]
        public Param And(string attributeValue, AttributeName.ID attributeName, RegexOptions options)
        {
            _constraint = new AndConstraint(_constraint, new AttributeConstraint(attributeName.ToString(), new Regex(attributeValue, options)));
            return this;
        }

        [Obsolete("Use 'Or(Param)'", true)]
        public Param Or(int index)
        {
            _constraint = new OrConstraint(_constraint, new IndexConstraint(index));
            return this;
        }

        [Obsolete("Use 'Or(Param)'", true)]
        public Param Or(string attributeValue, AttributeName.ID attributeName = AttributeName.ID.Id)
        {
            _constraint = new OrConstraint(_constraint, new AttributeConstraint(attributeName.ToString(), attributeValue));
            return this;
        }

        [Obsolete("Use 'Or(Param)'", true)]
        public Param Or(string attributeValue, AttributeName.ID attributeName, RegexOptions options)
        {
            _constraint = new OrConstraint(_constraint, new AttributeConstraint(attributeName.ToString(), new Regex(attributeValue, options)));
            return this;
        }

        [Obsolete("Use 'Or(Param)'", true)]
        public Param Or(string attributeValue, string attributeName)
        {
            _constraint = new OrConstraint(_constraint, new AttributeConstraint(attributeName, attributeValue));
            return this;
        }

        [Obsolete("Use 'Or(Param)'", true)]
        public Param Or(string attributeValue, string attributeName, RegexOptions options)
        {
            _constraint = new OrConstraint(_constraint, new AttributeConstraint(attributeName, new Regex(attributeValue, options)));
            return this;
        }
    }
}
