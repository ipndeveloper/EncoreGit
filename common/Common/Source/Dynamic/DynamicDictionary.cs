using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NetSteps.Common.Dynamic
{
    /// <summary>
    /// A dynamic <see cref="Dictionary{TKey, TValue}"/> that returns null if a requested value is not present (rather than throwing an exception).
    /// </summary>
    [Serializable]
    public class DynamicDictionary : DynamicObject, ISerializable
    {
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!_dictionary.TryGetValue(binder.Name, out result))
            {
                // Instead of throwing an exception, return a null value.
                result = null;
            }
            // Always return true to prevent unwanted exceptions.
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _dictionary[binder.Name] = value;
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            // A convenient way of retrieving the inner dictionary.
            if (binder.Name == "AsDictionary")
            {
                result = _dictionary;
                return true;
            }
            
            return base.TryInvokeMember(binder, args, out result);
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DynamicDictionary()
        {

        }

        /// <summary>
        /// Serializes the inner <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Add items from the dictionary to be serialized.
            foreach (var item in _dictionary)
            {
                info.AddValue(item.Key, item.Value);
            }
        }

        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        protected DynamicDictionary(SerializationInfo info, StreamingContext context)
        {
            // Add the deserialized items to the dictionary.
            foreach (var item in info)
            {
                _dictionary.Add(item.Name, item.Value);
            }
        } 
    }
}
