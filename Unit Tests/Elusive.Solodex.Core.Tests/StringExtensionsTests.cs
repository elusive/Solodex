using Elusive.Solodex.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Elusive.Solodex.Core.Tests
{
    /// <summary>
    /// Test fixture for string extension methods.
    /// </summary>
    [TestClass]
    public class StringExtensionsTests
    {
        #region Public Methods and Operators

        /// <summary>
        /// Tests the string extension format with.
        /// </summary>
        [TestMethod]
        public void TestStringExtensionFormatWith()
        {
            // Arrangements
            const string Before = "A, B, {0}, D";
            const string After = "A, B, C, D";

            // Actions
            var actual = Before.FormatWith("C");

            // Assertions
            Assert.AreEqual(After, actual, "FormatWith result is not correct.");
        }

        /// <summary>
        /// Tests the string extension is empty.
        /// </summary>
        [TestMethod]
        public void TestStringExtensionIsEmpty()
        {
            // Arrangements
            const string Empty = "";

            // Actions
            var actual = Empty.IsEmpty();

            // Assertions
            Assert.IsTrue(actual, "Empty string did not return true.");
        }

        /// <summary>
        /// Tests the string extension is null or empty empty.
        /// </summary>
        [TestMethod]
        public void TestStringExtensionIsNullOrEmptyEmpty()
        {
            // Arrangements
            const string Empty = "";

            // Actions
            var actual = Empty.IsNullOrEmpty();

            // Assertions
            Assert.IsTrue(actual, "Empty string did not return true.");
        }

        /// <summary>
        /// Tests the string extension is null or empty null.
        /// </summary>
        [TestMethod]
        public void TestStringExtensionIsNullOrEmptyNull()
        {
            // Arrangements
            string Null = null;

            // Actions
            var actual = Null.IsNullOrEmpty();

            // Assertions
            Assert.IsTrue(actual, "Null string did not return true.");
        }

        /// <summary>
        /// Tests the string extension join with.
        /// </summary>
        [TestMethod]
        public void TestStringExtensionJoinWith()
        {
            // Arrangements
            var list = new[] {"a", "b", "c"};
            const string Expected = "a-b-c";

            // Actions
            var actual = list.JoinWith("-");

            // Assertions
            Assert.AreEqual(Expected, actual, "Joined string is not correct.");
        }

        /// <summary>
        /// Tests the string extension remove quotes.
        /// </summary>
        [TestMethod]
        public void TestStringExtensionRemoveQuotes()
        {
            // Arrangements
            const string Before = "\"In quotes\"";
            const string Expected = "In quotes";

            // Actions
            var actual = Before.RemoveQuotes();

            // Assertions
            Assert.AreEqual(Expected, actual, "De-quoted string is not correct.");
        }

        /// <summary>
        /// Tests the string extension remove single quotes.
        /// </summary>
        [TestMethod]
        public void TestStringExtensionRemoveSingleQuotes()
        {
            // Arrangements
            const string Before = "\'In quotes\'";
            const string Expected = "In quotes";

            // Actions
            var actual = Before.RemoveSingleQuotes();

            // Assertions
            Assert.AreEqual(Expected, actual, "De-quoted string is not correct.");
        }

        #endregion
    }
}