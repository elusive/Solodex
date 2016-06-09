using System;
using System.Collections.Generic;
using System.IO;
using Elusive.Solodex.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Elusive.Solodex.Core.Tests
{
    [TestClass]
    public class ValidationExtensionsTests
    {
        [TestMethod]
        public void TestValidationExtensionsRequireThatDoesNotRequireParameterName()
        {
            // Arrangements
            var argument = string.Empty;

            // Actions
            var argumentEx = argument.RequireThat();

            // Assertions
            Assert.IsNotNull(argumentEx, "Return value from RequireThat is null.");
            Assert.IsInstanceOfType(
                argumentEx,
                typeof (ValidationExtensions.ArgumentEx<string>),
                "Return value from RequireThat is wrong type.");
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestValidationExtensionsRequireThatExistsIn()
        {
            // Arrangements
            const int Argument = 4;
            var list = new List<int> {1, 2, 3};

            // Actions
            var argumentEx = Argument.RequireThat().ExistsIn(list);

            // Assertions - Expected exception
        }

        [TestMethod]
        [ExpectedException(typeof (IOException))]
        public void TestValidationExtensionsRequireThatFileDoesExistAtPath()
        {
            // Arrangements
            const string Argument = @"Z:\Iknowthatthisfiledoesnotexist\atthispath.foo";

            // Actions
            var argumentEx = Argument.RequireThat().FileDoesExistAtPath();

            // Assertions - Expected exception
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestValidationExtensionsRequireThatIsNotEmpty()
        {
            // Arrangements
            var argument = string.Empty;

            // Actions
            var argumentEx = argument.RequireThat().IsNotEmpty();

            // Assertions - Expected exception
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void TestValidationExtensionsRequireThatIsNotNull()
        {
            // Arrangements
            object argument = null;

            // Actions
            var argumentEx = argument.RequireThat().IsNotNull();

            // Assertions - Expected exception
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestValidationExtensionsRequireThatIsNotNullOrEmptyWithEmpty()
        {
            // Arrangements
            var argument = string.Empty;

            // Actions
            var argumentEx = argument.RequireThat().IsNotNullOrEmpty();

            // Assertions - Expected exception
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void TestValidationExtensionsRequireThatIsNotNullOrEmptyWithNull()
        {
            // Arrangements
            string argument = null;

            // Actions
            var argumentEx = argument.RequireThat().IsNotNullOrEmpty();

            // Assertions - Expected exception
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestValidationExtensionsRequireThatMeetsCriteriaInt()
        {
            // Arrangements
            const int Argument = 5;

            // Actions
            var argumentEx = Argument.RequireThat().MeetsCriteria(x => x > 10);

            // Assertions - Expected exception
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestValidationExtensionsRequireThatMeetsCriteriaString()
        {
            // Arrangements
            const string Argument = "123456";

            // Actions
            var argumentEx = Argument.RequireThat().MeetsCriteria(s => s.Length < 5);

            // Assertions - Expected exception
        }

        [TestMethod]
        public void TestValidationExtensionsRequireThatReturnsArgumentEx()
        {
            // Arrangements
            var argument = string.Empty;

            // Actions
            var argumentEx = argument.RequireThat("argument");

            // Assertions
            Assert.IsNotNull(argumentEx, "Return value from RequireThat is null.");
            Assert.IsInstanceOfType(
                argumentEx,
                typeof (ValidationExtensions.ArgumentEx<string>),
                "Return value from RequireThat is wrong type.");
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestValidationExtensionsRequireThatStartsWith()
        {
            // Arrangements
            const string Argument = "Scooby";
            var prefix = "Jo";

            // Actions
            var argumentEx = Argument.RequireThat().StartsWith(prefix);

            // Assertions - Expected exception
        }
    }
}