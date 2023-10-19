using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeederAnalysis.Business
{
    public static class StringExtension
    {
        /// <summary>
        /// Returns the last few characters of the string with a length
        /// specified by the given parameter. If the string's length is less than the 
        /// given length the complete string is returned. If length is zero or 
        /// less an empty string is returned
        /// </summary>
        /// <param name="s">the string to process</param>
        /// <param name="length">Number of characters to return</param>
        /// <returns></returns>
        public static string Right(this string s, int length)
        {
            length = Math.Max(length, 0);

            if (s.Length > length)
            {
                return s.Substring(s.Length - length, length);
            }
            else
            {
                return s;
            }
        }
        /// <summary>
        /// Return the remainder of a string s after a separator c.
        /// </summary>
        /// <param name="s">String to search in.</param>
        /// <param name="c">Separator</param>
        /// <returns>The right part of the string after the character c, or the string itself when c isn't found.</returns>
        public static string RightOf(this string s, char c)
        {
            int ndx = s.IndexOf(c);
            if (ndx == -1)
                return s;
            return s.Substring(ndx + 1);
        }
        /// <summary>
        /// Returns the first few characters of the string with a length
        /// specified by the given parameter. If the string's length is less than the 
        /// given length the complete string is returned. If length is zero or 
        /// less an empty string is returned
        /// </summary>
        /// <param name="s">the string to process</param>
        /// <param name="length">Number of characters to return</param>
        /// <returns></returns>
        public static string Left(this string s, int length)
        {
            length = Math.Max(length, 0);

            if (s.Length > length)
            {
                return s.Substring(0, length);
            }
            else
            {
                return s;
            }
        }
        /// <summary>
        /// Returns the first part of the strings, up until the character c. If c is not found in the
        /// string the whole string is returned.
        /// </summary>
        /// <param name="s">String to truncate</param>
        /// <param name="c">Character to stop at.</param>
        /// <returns>Truncated string</returns>
        public static string LeftOf(this string s, char c)
        {
            int ndx = s.IndexOf(c);
            if (ndx >= 0)
            {
                return s.Substring(0, ndx);
            }

            return s;
        }
        public static bool IsNullOrBlank(this String text)
        {
            return text == null || text.Trim().Length == 0;
        }

        public static List<T> Join<T>(this List<T> first, List<T> second)
        {
            if (first == null)
            {
                return second;
            }
            if (second == null)
            {
                return first;
            }

            return first.Concat(second).ToList();
        }
    }
}
