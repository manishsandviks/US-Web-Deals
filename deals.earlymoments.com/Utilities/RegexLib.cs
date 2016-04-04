using System;
using System.Text.RegularExpressions;

namespace deals.earlymoments.com.Utilities
{
    internal class RegexLib
    {
        static RegexLib()
        {
            PatternPhone = @"^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$";
            PatternZipcode = @"^(\d{5}-\d{4}|\d{5}|\d{9})$|^([a-zA-Z]\d[a-zA-Z] \d[a-zA-Z]\d)$";
            PatternState = @"^[a-zA-Z']{2}$";
            PatternCity = @"^[a-zA-Z.\s\-]*$";
            PatternEMail = @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$";
            PatternAddress = @"[^{}<>!@%]";
            PatternName = @"^[a-zA-Z'.\s]{1,30}$";
        }

        public static string PatternName { get; set; }
        public static string PatternAddress { get; set; }
        public static string PatternEMail { get; set; }
        public static string PatternCity { get; set; }
        public static string PatternState { get; set; }
        public static string PatternZipcode { get; set; }
        public static string PatternPhone { get; set; }

        public static string GetRegexResponse(string pattern, object value)
        {
            if (value == null)
                return "";

            value = (string) value;
            var rgx = new Regex(pattern, options: RegexOptions.IgnoreCase);
            return rgx.IsMatch(Convert.ToString(value)) ? (string) value : "";
        }
    }
}