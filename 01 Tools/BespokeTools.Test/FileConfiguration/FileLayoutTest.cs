using NUnit.Framework;
using Protime.Bespoke.Tools.FileConfiguration;

namespace Protime.Bespoke.Tools.Test.FileConfiguration
{
    [TestFixture]
    public class FileLayoutTest
    {
        [Test]
        public void FileLayoutFactoryCreate()
        {
            string jsonString = @"{""FileType"":1,""Delimiter"":"";"",""HasHeader"":true,""FileMapping"":{""Fields"":[{ ""Name"":""EmployeeId"", ""Start"":0 },{ ""Name"":""EmployeeExtId"", ""Start"":1, ""Identifier"":""EmployerAndEmployeeNumber"", ""Merged"":""{2}-{3}"" },{ ""Name"":""EmployeeEmployerCode"", ""Start"":2 },{ ""Name"":""EmployeeCode"", ""Start"":3 },{ ""Name"":""EmployeeName"", ""Start"":4 }]}}";

            FileLayout layout = FileLayoutFactory.Create(jsonString);

            Assert.AreEqual(FileType.TextDelimited, layout.FileType);
            Assert.AreEqual(';', layout.Delimiter);
            Assert.IsTrue(layout.HasHeader);
            Assert.IsFalse(layout.TrimFields, "Default value: not in configuration");
            Assert.AreEqual("EmployeeId", layout.FileMapping.Fields[0].Name);
            Assert.AreEqual(0, layout.FileMapping.Fields[0].Start);
            Assert.AreEqual("EmployeeExtId", layout.FileMapping.Fields[1].Name);
            Assert.AreEqual(1, layout.FileMapping.Fields[1].Start);
            Assert.AreEqual("EmployeeEmployerCode", layout.FileMapping.Fields[2].Name);
            Assert.AreEqual(2, layout.FileMapping.Fields[2].Start);
            Assert.AreEqual("EmployeeCode", layout.FileMapping.Fields[3].Name);
            Assert.AreEqual(3, layout.FileMapping.Fields[3].Start);
            Assert.AreEqual("EmployeeName", layout.FileMapping.Fields[4].Name);
            Assert.AreEqual(4, layout.FileMapping.Fields[4].Start);
        }
    }
}
