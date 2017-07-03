using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestMasterHelpProvider.Extensions
{
   public static class RegularExpressions
    {
        public static string Email
        {
            get
            {
                return @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            }
        }

        public static string AlphaCharacters
        {
            get
            {
                return @"[^a-zA-Z]";
            }
        }
        public static string AlphaNumbericCharacters
        {
            get
            {
                return @"[^a-zA-Z0-9]";
            }
        }
        public static string NumericCharacters
        {
            get
            {
                return @"^\s*\d*\s*$";
            }
        }
        public static string HexColor
        {
            get
            {
                return @"^#?([a-f]|[A-F]|[0-9]){3}(([a-f]|[A-F]|[0-9]){3})?$ ";
            }
        }

        /// <summary>
        /// Validates UNC Paths, with or without files. Does not validate on shares ($) or local files (c:\xxx). 
        /// http://regexlib.com/REDetails.aspx?regexp_id=865
        /// </summary>
        public static string UncPath
        {
            get { return @"^\\(\\[\w-]+){1,}(\\[\w-()]+(\s[\w-()]+)*)+(\\(([\w-()]+(\s[\w-()]+)*)+\.[\w]+)?)?$"; }
        }

        /// <summary>
        /// c:\b_card.jpg
        /// </summary>
        public static string LocalDriveInPath
        {
            get { return @"^([a-zA-Z]:)(\\).*$"; }
        }

        // http://www.regexlib.com/Search.aspx?k=po+box&c=-1&m=-1&ps=20
        public static string PoBox
        {
            get { return @"[p|P][\s]*[o|O][\s]*[b|B][\s]*[o|O][\s]*[x|X][\s]*[a-zA-Z0-9]*|\b[P|p]+(OST|ost|o|O)?\.?\s*[O|o|0]+(ffice|FFICE)?\.?\s*[B|b][O|o|0]?[X|x]+\.?\s+[#]?(\d+)*(\D+)*\b"; }
        }

        public static string Ssn
        {
            get { return @"(^|\s)(00[1-9]|0[1-9]0|0[1-9][1-9]|[1-6]\d{2}|7[0-6]\d|77[0-2])(-?|[\. ])([1-9]0|0[1-9]|[1-9][1-9])\3(\d{3}[1-9]|[1-9]\d{3}|\d[1-9]\d{2}|\d{2}[1-9]\d)($|\s|[;:,!\.\?])"; }
        }
    }
}
