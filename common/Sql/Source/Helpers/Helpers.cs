using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NetSteps.Sql
{
    public static class Helpers
    {
        #region Singularize Methods
        public static class Inflections
        {
            private readonly static List<string> _uncountable = new List<string>(new string[] { "equipment", "information", "rice", "money", "species", "series", "fish", "sheep" });
            public static List<string> Uncountable
            {
                get { return Inflections._uncountable; }
            }

            private readonly static List<string[]> _pluralPatterns = new List<string[]>(new[]{
			        new[]{"$", "s"},
			        new[]{"s$", "s"},
			        new[]{"(ax|test)is$", "$1es"},
			        new[]{"(octop|vir)us$", "$1i"},
			        new[]{"(alias|status)$", "$1es"},
			        new[]{"(bu)s$", "$1ses"},
			        new[]{"(buffal|tomat)o$", "$1oes"},
			        new[]{"([ti])um$", "$1a"},
			        new[]{"sis$", "ses"},
			        new[]{"(?:([^f])fe|([lr])f)$", "$1$2ves"},
			        new[]{"(hive)$", "$1s"},
			        new[]{"([^aeiouy]|qu)y$", "$1ies"},
			        new[]{"(x|ch|ss|sh)$", "$1es"},
			        new[]{"(matr|vert|ind)(?:ix|ex)$", "$1ices"},
			        new[]{"([m|l])ouse$", "$1ice"},
			        new[]{"^(ox)$", "$1en"},
			        new[]{"(quiz)$", "$1zes"}});
            public static List<string[]> PluralPatterns
            {
                get { return Inflections._pluralPatterns; }
            }

            private readonly static List<string[]> _singlePatterns = new List<string[]>(new[] { 
			        new[]{"s$", ""},
			        new[]{"(n)ews$", "$1ews"},
			        new[]{"([ti])a$", "$1um"},
			        new[]{"((a)naly|(b)a|(d)iagno|(p)arenthe|(p)rogno|(s)ynop|(t)he)ses$", "$1$2sis"},
			        new[]{"(^analy)ses$", "$1sis"},
			        new[]{"([^f])ves$", "$1fe"},
			        new[]{"(hive)s$", "$1"},
			        new[]{"(tive)s$", "$1"},
			        new[]{"([lr])ves$", "$1f"},
			        new[]{"([^aeiouy]|qu)ies$", "$1y"},
			        new[]{"(s)eries$", "$1eries"},
			        new[]{"(m)ovies$", "$1ovie"},
			        new[]{"(x|ch|ss|sh)es$", "$1"},
			        new[]{"([m|l])ice$", "$1ouse"},
			        new[]{"(bus)es$", "$1"},
			        new[]{"(o)es$", "$1"},
			        new[]{"(shoe)s$", "$1"},
			        new[]{"(cris|ax|test)es$", "$1is"},
			        new[]{"(octop|vir)i$", "$1us"},
			        new[]{"(alias|status)$", "$1"},
			        new[]{"(alias|status)es$", "$1"},
			        new[]{"(address)$", "$1"},
			        new[]{"^(ox)en", "$1"},
			        new[]{"(vert|ind)ices$", "$1ex"},
			        new[]{"(matr)ices$", "$1ix"},
			        new[]{"(quiz)zes$", "$1"}});
            public static List<string[]> SinglePatterns
            {
                get { return Inflections._singlePatterns; }
            }

            private readonly static List<string[]> _irregularPattern = new List<string[]>(new[] { 
			        new []{"person","people"},
			        new []{"man","men"},
			        new []{"child","children"},
			        new []{"sex","sexes"},
			        new []{"move","moves"}
        		
		        });
            public static List<string[]> IrregularPattern
            {
                get { return Inflections._irregularPattern; }
            }
            static Inflections()
            {
                _irregularPattern.ForEach(ss =>
                {
                    string s1 = ss[0];
                    string s2 = ss[1];
                    if (s1[0].ToString().ToUpper() == s2[0].ToString().ToUpper())
                    {
                        _pluralPatterns.Add(new[] { "(" + s1[0] + ")" + s1.Substring(1) + "$", "$1" + s2.Substring(1) });
                        _singlePatterns.Add(new[] { "(" + s2[0] + ")" + s2.Substring(1) + "$", "$1" + s1.Substring(1) });
                    }
                    else
                    {

                    }
                });
                _pluralPatterns.Reverse();
                _singlePatterns.Reverse();
            }
        }

        public static string Pluralize(this string word)
        {
            if (word == "" | Inflections.Uncountable.Contains(word)) { return word; }
            foreach (var ss in Inflections.PluralPatterns)
            {
                Regex r = new Regex(ss[0], RegexOptions.IgnoreCase);
                if (r.IsMatch(word))
                {
                    var m = r.Replace(word, ss[1]);
                    return m;
                }
            }
            return word;
        }

        public static string Singularize(this string word)
        {
            if (Inflections.Uncountable.Contains(word)) { return word; }
            foreach (var ss in Inflections.SinglePatterns)
            {
                Regex r = new Regex(ss[0], RegexOptions.IgnoreCase);
                if (r.IsMatch(word))
                {
                    var m = r.Replace(word, ss[1]);
                    return m;
                }
            }
            return word;
        }
        #endregion
    }
}
