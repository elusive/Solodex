using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elusive.Solodex.Core.Extensions
{
    public static class ValidationExtensions
    {
        [DebuggerHidden]
        public static ArgumentEx<T> ExistsIn<T>(this ArgumentEx<T> arg, IEnumerable<T> list)
        {
            if (!list.Contains(arg.Value))
            {
                throw new ArgumentException(
                    "The argument {0} does not exist in the list provided.".FormatWith(arg.Name),
                    arg.Name);
            }

            return arg;
        }

        [DebuggerHidden]
        public static ArgumentEx<string> FileDoesExistAtPath(this ArgumentEx<string> arg)
        {
            if (!File.Exists(arg.Value))
            {
                throw new IOException("File does not exist at path: {0}".FormatWith(arg.Value));
            }

            return arg;
        }

        [DebuggerHidden]
        public static ArgumentEx<float> IsBetween(
            this ArgumentEx<float> x,
            float lowerBound,
            float upperBound,
            bool inclusive = true)
        {
            if (!inclusive)
            {
                if (x > lowerBound && x < upperBound)
                {
                    return x;
                }
            }

            if (x >= lowerBound && x <= upperBound)
            {
                return x;
            }

            throw new ArgumentException(
                "The argument {0} is not between {1} and {2}.".FormatWith(x.Name, lowerBound, upperBound));
        }

        [DebuggerHidden]
        public static ArgumentEx<uint> IsBetween(
            this ArgumentEx<uint> x,
            uint lowerBound,
            uint upperBound,
            bool inclusive = true)
        {
            if (!inclusive)
            {
                if (x > lowerBound && x < upperBound)
                {
                    return x;
                }
            }

            if (x >= lowerBound && x <= upperBound)
            {
                return x;
            }

            throw new ArgumentException(
                "The argument {0} is not between {1} and {2}.".FormatWith(x.Name, lowerBound, upperBound));
        }

        [DebuggerHidden]
        public static ArgumentEx<byte> IsBetween(
            this ArgumentEx<byte> x,
            byte lowerBound,
            byte upperBound,
            bool inclusive = true)
        {
            if (!inclusive)
            {
                if (x > lowerBound && x < upperBound)
                {
                    return x;
                }
            }

            if (x >= lowerBound && x <= upperBound)
            {
                return x;
            }

            throw new ArgumentException(
                "The argument {0} is not between {1} and {2}.".FormatWith(x.Name, lowerBound, upperBound));
        }

        [DebuggerHidden]
        public static ArgumentEx<string> IsNotEmpty(this ArgumentEx<string> arg)
        {
            if (arg == "")
            {
                throw new ArgumentException("The argument {0} may not be empty.".FormatWith(arg.Name), arg.Name);
            }

            return arg;
        }

        [DebuggerHidden]
        public static ArgumentEx<T> IsNotNull<T>(this ArgumentEx<T> arg) where T : class
        {
            if (arg.Value == null)
            {
                throw new ArgumentNullException(arg.Name);
            }

            return arg; // for fluency
        }

        [DebuggerHidden]
        public static ArgumentEx<string> IsNotNullOrEmpty(this ArgumentEx<string> arg)
        {
            arg.IsNotNull();
            arg.IsNotEmpty();

            return arg;
        }

        [DebuggerHidden]
        public static ArgumentEx<string> MeetsCriteria(this ArgumentEx<string> x, Func<string, bool> predicate)
        {
            if (predicate(x))
            {
                return x;
            }

            throw new ArgumentException("The argument {0} does not meet the required criteria.", x.Name);
        }

        [DebuggerHidden]
        public static ArgumentEx<int> MeetsCriteria(this ArgumentEx<int> x, Func<int, bool> predicate)
        {
            if (predicate(x))
            {
                return x;
            }

            throw new ArgumentException("The argument {0} does not meet the required criteria.", x.Name);
        }

        [DebuggerHidden]
        public static ArgumentEx<float> MeetsCriteria(this ArgumentEx<float> x, Func<float, bool> predicate)
        {
            if (predicate(x))
            {
                return x;
            }

            throw new ArgumentException("The argument {0} does not meet the required criteria.", x.Name);
        }

        [DebuggerHidden]
        public static ArgumentEx<byte> MeetsCriteria(this ArgumentEx<byte> x, Func<byte, bool> predicate)
        {
            if (predicate(x))
            {
                return x;
            }

            throw new ArgumentException("The argument {0} does not meet the required criteria.", x.Name);
        }

        /// <summary>
        /// Static extension constructor for the generic type that
        /// is in our ArgumentEx class.  This makes the method 
        /// available to all arguments.  The method returns a new 
        /// instance of our arg wrapper that is itself extended with
        /// all of our validation methods.
        /// </summary>
        /// <example>
        /// <para>To use this technique, simply call the method on any
        /// argument you need to validate.  It returns to you an instance
        /// of the <see cref="ArgumentEx{T}"/> class off of which you can 
        /// call its validation methods.  The validation methods all return
        /// the arg wrapper instance so you can chain validation calls
        /// as well.</para>
        /// <code language="C#">
        /// public void MyMethod(int myNum, string myString)
        /// {
        ///     // validate arguments
        ///     myNum.RequireThat("myNum")
        ///         .IsNotNull()
        ///         .IsInRange(0, 10);        
        ///     myString.RequireThat("myString").IsNotNullOrEmpty();
        /// }
        /// </code>
        /// </example>
        /// <typeparam name="T">Argument type.</typeparam>
        /// <param name="arg">The argument instance.</param>
        /// <param name="name">The name of the argument.</param>
        /// <returns>Instance of the <see cref="ArgumentEx{T}"/> </returns>
        public static ArgumentEx<T> RequireThat<T>(this T arg, string name)
        {
            return new ArgumentEx<T>(arg, name);
        }

        /// <summary>
        /// Static extension constructor for the Generic Argument Extension 
        /// class.
        /// </summary>
        /// <typeparam name="T">Argument type.</typeparam>
        /// <param name="arg">The argument instance.</param>
        /// <returns>Instance of <see cref="ArgumentEx{T}"/></returns>
        public static ArgumentEx<T> RequireThat<T>(this T arg)
        {
            // no name provide so pass default arg name value
            return new ArgumentEx<T>(arg, "argument");
        }

        [DebuggerHidden]
        public static ArgumentEx<string> StartsWith(this ArgumentEx<string> arg, string prefix)
        {
            if (!arg.Value.StartsWith(prefix))
            {
                throw new ArgumentException("Parameter {0} must start with {1}.".FormatWith(arg.Name, prefix), arg.Name);
            }

            return arg;
        }

        /// <summary>
        /// Class to represent a generic argument and as a central 
        /// extension point for validation methods for method args.
        /// </summary>
        /// <typeparam name="T">Generic type of the argument.</typeparam>
        public class ArgumentEx<T>
        {
            public ArgumentEx(T value, string name)
            {
                this.Value = value;
                this.Name = name;
            }

            public string Name { get; set; }

            public T Value { get; set; }

            public static implicit operator T(ArgumentEx<T> arg)
            {
                return arg.Value;
            }
        }
    }
}