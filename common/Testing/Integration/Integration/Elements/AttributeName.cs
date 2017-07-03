using System;

namespace NetSteps.Testing.Integration
{
    public static class AttributeName
    {
        public enum ID
        {
            Action,
            //[Obsolete("Avoid using 'Alt'")]
            Alt,
            Id,
            Name,
            ClassId,
            ClassName,
            //[Obsolete("Avoid using 'InnerText'")]
            InnerText,
            Value,
            Title,
            Href,
            Src,
            Onclick
        }
    }
}
