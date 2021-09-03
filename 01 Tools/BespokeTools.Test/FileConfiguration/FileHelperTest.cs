using System;
using NUnit.Framework;
using Protime.Bespoke.Tools.FileConfiguration;

namespace Protime.Bespoke.Tools.Test.FileConfiguration
{
    [TestFixture]
    public class FileHelperTest
    {
        [Test]
        public void ParseToFileType_CorrectParse_ReturnFileType()
        {
            Assert.AreEqual(FileType.NoTypeDefined, "0".ParseToFileType(), "No type defined");
            Assert.AreEqual(FileType.TextDelimited, "1".ParseToFileType(), "TextDelimited");
            Assert.AreEqual(FileType.TextFixedPositions, "2".ParseToFileType(), "TextFixedPositions");
        }

        [Test]
        public void ParseToFileType_InCorrectNumberParse_ReturnNoTypeDefined()
        {
            Assert.AreEqual(FileType.NoTypeDefined, "999".ParseToFileType());
        }

        [Test]
        public void ParseToFileType_NoNumberParse_throwsApplicationException()
        {
            var ex = Assert.Throws<ApplicationException>(() => "abc".ParseToFileType());
            Assert.AreEqual("abc is not an underlying value of the FileType enumeration.", ex.Message);

            ex = Assert.Throws<ApplicationException>(() => "".ParseToFileType());
            Assert.AreEqual(" is not an underlying value of the FileType enumeration.", ex.Message);
        }
    }
}
