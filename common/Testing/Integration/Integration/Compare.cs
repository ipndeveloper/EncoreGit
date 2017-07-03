using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration
{
    public enum CompareID
    {
        Contains,
        Equal,
        NotEqual,
        Match
    }

    public static class Compare
    {

        public static bool CustomCompare<T>(T obj1, CompareID method, T obj2, string msg)
        {
            bool result = false;
            if ( obj1 == null && obj2 == null)
                result = true;
            else if (obj1 != null && obj2 != null)
            {
                switch (method)
                {
                    case CompareID.Equal:
                        result = obj1.Equals(obj2);
                        break;
                    case CompareID.NotEqual:
                        result = !obj1.Equals(obj2);
                        break;
                    case CompareID.Match:
                        {
                            string pat = obj1.ToString()+"+";
                            Match match = Regex.Match(obj2.ToString(), pat);
                            if (match.Success)
                                result = true;
                            break;
                        }
                    default:
                        result = obj1.ToString().Contains(obj2.ToString());
                        break;
                }
            }
            if (result)
                Util.LogPass(string.Format("Compare {0}", msg));
            else
            {
                msg = string.Format("Compare {0}: '{1}' {2} '{3}'", msg, obj1, method, obj2);
                Util.LogFail(msg);
            }
            return result;
        }
    }
}
