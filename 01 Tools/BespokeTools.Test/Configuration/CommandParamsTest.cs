using System;
using NUnit.Framework;
using Protime.Bespoke.Tools.Configuration;

namespace Protime.Bespoke.Tools.Test.Configuration
{
    [TestFixture]
    public class CommandParamsTest
    {
        [Test]
        public void GetStringValueByKey_ReturnStringValue()
        {
            string[] args = { @"\KEY=123ABC" };

            var cmdParams = new CommandParams(args);

            Assert.AreEqual("123ABC", cmdParams.GetStringValueByKey(@"\KEY"));
        }

        [Test]
        public void GetStringValueByKey_NoValue_ReturnEmptyValue()
        {
            string[] args = { @"\KEY=" };

            var cmdParams = new CommandParams(args);

            Assert.AreEqual(string.Empty, cmdParams.GetStringValueByKey(@"\KEY"));
        }

        [Test]
        public void GetStringValueByKey_KeyNotExists_ReturnEmptyValue()
        {
            string[] args = { @"\KEY=123" };

            var cmdParams = new CommandParams(args);

            Assert.AreEqual(string.Empty, cmdParams.GetStringValueByKey(@"\KEYNOTEXISTS"));
        }

        [Test]
        public void GetStringValueByKey_KeyNotExistNoArgs_ReturnEmptyValue()
        {
            string[] args = new string[] { };

            var cmdParams = new CommandParams(args);

            Assert.AreEqual(string.Empty, cmdParams.GetStringValueByKey(@"\KEYNOTEXISTS"));
        }

        [Test]
        public void GetStringValueByKey_OptionRequiredAndKeyNotExists_ThrowCommandLineArgsException()
        {
            string[] args = { @"\KEY=13" };

            var cmdParams = new CommandParams(args);

            Assert.Throws(typeof(CommandLineArgsException), () => cmdParams.GetStringValueByKey(@"\KEYNOTEXISTS", true));
        }

        [Test]
        public void GetStringValueByKey_OptionRequiredAndKeyHasNoValue_ThrowCommandLineArgsException()
        {
            string[] args = { @"\KEY=" };

            var cmdParams = new CommandParams(args);

            Assert.Throws(typeof(CommandLineArgsException), () => cmdParams.GetStringValueByKey(@"\KEY", true));
        }

        [Test]
        public void GetStringValueByKey_OptionRequiredAndKeyHasValue_ReturnValue()
        {
            string[] args = { @"\KEY=123" };

            var cmdParams = new CommandParams(args);

            Assert.AreEqual("123", cmdParams.GetStringValueByKey(@"\KEY", true));
        }


        [Test]
        public void GetBoolValueByKey_KeyExists_ReturnTrue()
        {
            string[] args = { @"\KEY" };

            var cmdParams = new CommandParams(args);

            Assert.IsTrue(cmdParams.GetBoolValueByKey(@"\KEY"));
        }

        [Test]
        public void GetBoolValueByKey_KeyExistsWithValueTrue_ReturnTrue()
        {
            string[] args = { @"\KEY=true" };

            var cmdParams = new CommandParams(args);

            Assert.IsTrue(cmdParams.GetBoolValueByKey(@"\KEY"));
        }

        [Test]
        public void GetBoolValueByKey_KeyNotExists_ReturnFalse()
        {
            string[] args = { @"\KEY" };

            var cmdParams = new CommandParams(args);

            Assert.IsFalse(cmdParams.GetBoolValueByKey(@"\OTHERKEY"));
        }

        [Test]
        public void GetBoolValueByKey_KeyExistsWithValueFalse_ReturnFalse()
        {
            string[] args = { @"\KEY=false" };

            var cmdParams = new CommandParams(args);

            Assert.IsFalse(cmdParams.GetBoolValueByKey(@"\KEY"));
        }

        [Test]
        public void GetStringValueByKey_FileString()
        {
            string[] args = { @"\FILE=C:\premium_bespoke\Input\OrgGroups.txt" };

            var cmdParams = new CommandParams(args);

            Assert.AreEqual(@"C:\premium_bespoke\Input\OrgGroups.txt", cmdParams.GetStringValueByKey(@"\FILE"));
        }

        [Test]
        public void GetStringValueByKey_FileStringWithSpacesBetweenQuotes()
        {
            string[] args = { @"\FILE=""C:\premium bespoke\Input\OrgGroups.txt""", @"\TYPE=1" };

            var cmdParams = new CommandParams(args);

            Assert.AreEqual(@"""C:\premium bespoke\Input\OrgGroups.txt""", cmdParams.GetStringValueByKey(@"\FILE"));
        }

        [Test]
        public void GetDateTimeValueByKey_CorrectDate_ReturnDateTime()
        {
            var args = new[] { "\\DATE=20180525" };

            var cmdParams = new CommandParams(args);

            Assert.AreEqual(new DateTime(2018, 5, 25), cmdParams.GetDateTimeValueByKey(@"\DATE", "yyyyMMdd"));
        }

        [Test]
        public void GetDateTimeValueByKey_InCorrectDate_ReturnDateMinValue()
        {
            var args = new[] { "\\DATE=99999999" };

            var cmdParams = new CommandParams(args);

            Assert.AreEqual(DateTime.MinValue, cmdParams.GetDateTimeValueByKey(@"\DATE", "yyyyMMdd"));
        }

        [Test]
        public void GetDateTimeValueByKey_OptionRequiredWithCorrectDate_ReturnDateTime()
        {
            var args = new[] { "\\DATE=20180525" };

            var cmdParams = new CommandParams(args);

            Assert.AreEqual(new DateTime(2018, 5, 25), cmdParams.GetDateTimeValueByKey(@"\DATE", "yyyyMMdd", true));
        }

        [Test]
        public void GetDateTimeValueByKey_OptionRequiredWithInCorrectDate_ThrowCommandLineArgsException()
        {
            var args = new[] { "\\DATE=20180525" };

            var cmdParams = new CommandParams(args);

            Assert.Throws(typeof(CommandLineArgsException), () => cmdParams.GetDateTimeValueByKey(@"\KEYNOTEXISTS", "yyyyMMdd", true));

        }

        [Test]
        public void KeyExists_Exists_ReturnTrue()
        {
            var args = new[] { @"\KEY" };

            var cmdParams = new CommandParams(args);

            Assert.IsTrue(cmdParams.ParamExists(@"\KEY"));
        }

        [Test]
        public void KeyExists_NotExists_ReturnFalse()
        {
            var args = new[] { @"\KEY" };

            var cmdParams = new CommandParams(args);

            Assert.IsFalse(cmdParams.ParamExists(@"\ANOTHERKEY"));
        }
    }
}
