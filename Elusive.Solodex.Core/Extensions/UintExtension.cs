using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elusive.Solodex.Core.Extensions
{
    /// <summary>
    /// This class adds extensions to the Uint struct
    /// </summary>
    public static class UintExtension
    {
        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A Hex String of the Uint</returns>
        public static string ConvertToString(this uint value)
        {
            return value.ToString("X8", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Creates a range of numbers of the size specified.
        /// </summary>
        /// <example>For example:
        /// <code>var x = 5.Range();</code>
        /// Populates x with a collection of the numbers: 0,1,2,3,4
        /// </example>
        /// <param name="size">The size.</param>
        /// <returns>Range of numbers of the specified size.</returns>
        public static IEnumerable<int> Range(this int size)
        {
            for (var x = 0; x < size; x++)
            {
                yield return x;
            }
        }

        /// <summary>
        /// Executes the provided action a certain number of times,
        /// specified by the integer upon which the method is called.
        /// </summary>
        /// <param name="i">The integer value used to determine how
        /// many times to execute the provided action.</param>
        /// <param name="action">The action to execute.</param>
        public static void Times(this int i, Action<int> action)
        {
            i.Range().Each(action);
        }
    }
}