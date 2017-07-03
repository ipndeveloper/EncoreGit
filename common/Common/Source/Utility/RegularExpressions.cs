
namespace NetSteps.Common
{
    public class RegularExpressions
    {
        public static string USZipOptionalPlusFour
        {
            get
            {
                return @"^\d{5}(-?\d{4})?$";
            }
        }

        public static string Email
        {
            get
            {
                return @"^([\w\-\.]+)@((\[([0-9]{1,3}\.){3}[0-9]{1,3}\])|(([\w\-]+\.)+)([a-zA-Z]{2,4}))$";
            }
        }

        public static string EmailOrEmpty
        {
            get
            {
                return @"(^$|^([\w\-\.]+)@((\[([0-9]{1,3}\.){3}[0-9]{1,3}\])|(([\w\-]+\.)+)([a-zA-Z]{2,4}))$)";
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

        public static string NumericCharactersAndEmptyStrings
        {
            get
            {
                return @"^\d*$";
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

        //http://regexlib.com/Search.aspx?k=url
        public static string URL
        {
            get { return @"^(?=[^&])(?:(?<scheme>[^:/?#]+):)?(?://(?<authority>[^/?#]*))?(?<path>[^?#]*)(?:\?(?<query>[^#]*))?(?:#(?<fragment>.*))?"; }
        }

        public static string DateTime_MMDDYYYY
        {
            get { return @"(?=\d)^(?:(?!(?:10\D(?:0?[5-9]|1[0-4])\D(?:1582))|(?:0?9\D(?:0?[3-9]|1[0-3])\D(?:1752)))((?:0?[13578]|1[02])|(?:0?[469]|11)(?!\/31)(?!-31)(?!\.31)|(?:0?2(?=.?(?:(?:29.(?!000[04]|(?:(?:1[^0-6]|[2468][^048]|[3579][^26])00))(?:(?:(?:\d\d)(?:[02468][048]|[13579][26])(?!\x20BC))|(?:00(?:42|3[0369]|2[147]|1[258]|09)\x20BC))))))|(?:0?2(?=.(?:(?:\d\D)|(?:[01]\d)|(?:2[0-8])))))([-.\/])(0?[1-9]|[12]\d|3[01])\2(?!0000)((?=(?:00(?:4[0-5]|[0-3]?\d)\x20BC)|(?:\d{4}(?!\x20BC)))\d{4}(?:\x20BC)?)(?:$|(?=\x20\d)\x20))?((?:(?:0?[1-9]|1[012])(?::[0-5]\d){0,2}(?:\x20[aApP][mM]))|(?:[01]\d|2[0-3])(?::[0-5]\d){1,2})?$"; }
        }

        public static string DateTime_DDMMYYYY
        {
            get { return @"^(?=\d)(?:(?!(?:(?:0?[5-9]|1[0-4])(?:\.|-|\/)10(?:\.|-|\/)(?:1582))|(?:(?:0?[3-9]|1[0-3])(?:\.|-|\/)0?9(?:\.|-|\/)(?:1752)))(31(?!(?:\.|-|\/)(?:0?[2469]|11))|30(?!(?:\.|-|\/)0?2)|(?:29(?:(?!(?:\.|-|\/)0?2(?:\.|-|\/))|(?=\D0?2\D(?:(?!000[04]|(?:(?:1[^0-6]|[2468][^048]|[3579][^26])00))(?:(?:(?:\d\d)(?:[02468][048]|[13579][26])(?!\x20BC))|(?:00(?:42|3[0369]|2[147]|1[258]|09)\x20BC))))))|2[0-8]|1\d|0?[1-9])([-.\/])(1[012]|(?:0?[1-9]))\2((?=(?:00(?:4[0-5]|[0-3]?\d)\x20BC)|(?:\d{4}(?:$|(?=\x20\d)\x20)))\d{4}(?:\x20BC)?)(?:$|(?=\x20\d)\x20))?((?:(?:0?[1-9]|1[012])(?::[0-5]\d){0,2}(?:\x20[aApP][mM]))|(?:[01]\d|2[0-3])(?::[0-5]\d){1,2})?$"; }
        }

        public static string DateTime_YYYYMMDD
        {
            get { return @"^(?=\d)(?:(?!(?:1582(?:\.|-|\/)10(?:\.|-|\/)(?:0?[5-9]|1[0-4]))|(?:1752(?:\.|-|\/)0?9(?:\.|-|\/)(?:0?[3-9]|1[0-3])))(?=(?:(?!000[04]|(?:(?:1[^0-6]|[2468][^048]|[3579][^26])00))(?:(?:\d\d)(?:[02468][048]|[13579][26]))\D0?2\D29)|(?:\d{4}\D(?!(?:0?[2469]|11)\D31)(?!0?2(?:\.|-|\/)(?:29|30))))(\d{4})([-\/.])(0?\d|1[012])\2((?!00)[012]?\d|3[01])(?:$|(?=\x20\d)\x20))?((?:(?:0?[1-9]|1[012])(?::[0-5]\d){0,2}(?:\x20[aApP][mM]))|(?:[01]\d|2[0-3])(?::[0-5]\d){1,2})?$"; }
        }

        #region Credit Cards
        public static string Visa
        {
            get { return @"^4(\d{12}$|\d{15}$)"; }
        }

        public static string MasterCard
        {
            get { return @"^5[1-5]\d{14}$"; }
        }

        public static string AmericanExpress
        {
            get { return @"^3[47]\d{13}$"; }
        }

        public static string Discover
        {
            get { return @"^6(011|5\d{2})\d{12}$"; }
        }
        #endregion
    }
}
