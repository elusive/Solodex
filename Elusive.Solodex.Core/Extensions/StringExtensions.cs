using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Elusive.Solodex.Core.Extensions
{
    /// <summary>
    /// String type extension methods.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Formats the string called on with the objects passed in
        /// to the method.
        /// </summary>
        /// <param name="format">The string being extended.</param>
        /// <param name="args">The args to use in the format call.</param>
        /// <returns></returns>
        [Obsolete("Please use string.Format directly as using this method breaks IDE integration intellisense and provides no advantage.")]
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args); // returns string to maintain fluency
        }

        /// <summary>
        /// Formats the string called on with the objects passed in
        /// to the method.
        /// </summary>
        /// <param name="format">The string being extended.</param>
        /// <param name="formatProvider">FormatProvider for culture</param>
        /// <param name="args">The args to use in the format call.</param>
        /// <returns></returns>
        [Obsolete("Please use string.Format directly as using this method breaks IDE integration intellisense and provides no advantage.")]
        public static string FormatWith(this string format, IFormatProvider formatProvider, params object[] args)
        {
            return string.Format(formatProvider, format, args); // returns string to maintain fluency
        }

        /// <summary>
        /// Determines whether [is alpha numeric] [the specified s].
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        ///   <c>true</c> if the specified s is alphanumeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAlphaNumeric(this string s)
        {
            return Regex.IsMatch(s, @"^[a-zA-Z0-9]+$");
        }

        /// <summary>
        /// Determines whether [is alpha numeric] or has a [hyphen] in [the specified s].
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        ///   <c>true</c> if the specified s is alphanumeric or has a hyphen; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAlphaNumericWithHyphen(this string s)
        {
            return Regex.IsMatch(s, @"^[-a-zA-Z0-9]+$");
        }

        /// <summary>
        /// Determines whether the specified string is empty.
        /// </summary>
        /// <param name="s">The instance of string to be extended.</param>
        /// <returns>
        /// 	<c>true</c> if the specified s is empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEmpty(this string s)
        {
            return s.Length == 0;
        }

        /// <summary>
        /// Determines whether [is null or empty] [the specified s].
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        /// 	<c>true</c> if [is null or empty] [the specified s]; 
        /// 	otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        /// <summary>
        /// Determines whether [is null or white space] [the specified s].
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        ///     <c>true</c> if [is null or whitespace] [the specified s];
        ///     otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrWhiteSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// Determines whether the specified string is numeric characters only.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>
        ///   <c>true</c> if the specified s is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(this string s)
        {
            return Regex.IsMatch(s, @"^\d+$");
        }

        /// <summary>
        /// Joins the string array called on with the separator passed
        /// in between each item.
        /// </summary>
        /// <param name="join">The string array being extended.</param>
        /// <param name="separator">Separator value for the join call.</param>
        /// <returns>Joined string.</returns>
        public static string JoinWith(this string[] join, string separator)
        {
            return string.Join(separator, join);
        }

        /// <summary>
        /// Removes the quotes from a string.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>String without double quotes.</returns>
        public static string RemoveQuotes(this string s)
        {
            return s.Replace("\"", string.Empty);
        }

        /// <summary>
        /// Removes the single quotes.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>String without single quotes.</returns>
        public static string RemoveSingleQuotes(this string s)
        {
            return s.Replace("'", string.Empty);
        }

        /// <summary>
        /// Returns a new string with the specified number of occurences (from beginning)
        /// replaced with another specified string.
        /// </summary>
        /// <param name="s">The original string.</param>
        /// <param name="oldValue">The string to be replaced.</param>
        /// <param name="newValue">The string to replace with.</param>
        /// <param name="occurences">The occurences to replace from beginning of string.</param>
        /// <returns>The new string with replacements completed.</returns>
        public static string Replace(this string s, string oldValue, string newValue, int occurences)
        {
            occurences.Times(
                (x) =>
                {
                    var start = s.IndexOf(oldValue, StringComparison.CurrentCulture);
                    if (start > -1)
                    {
                        s = s.Substring(0, start) + s.Substring(start + oldValue.Length);
                    }
                });
            return s;
        }

        /// <summary>
        /// Replace a character at a specified index
        /// </summary>
        /// <param name="input">The string</param>
        /// <param name="index">Index</param>
        /// <param name="newChar">new character</param>
        /// <returns></returns>
        public static string ReplaceAt(this string input, int index, char newChar)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            char[] chars = input.ToCharArray();
            chars[index] = newChar;
            return new string(chars);
        }
    }
}
