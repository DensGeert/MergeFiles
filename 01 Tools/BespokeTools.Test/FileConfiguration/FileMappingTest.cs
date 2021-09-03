using System;
using NUnit.Framework;
using Protime.Bespoke.Tools.FileConfiguration;

namespace Protime.Bespoke.Tools.Test.FileConfiguration
{
    [TestFixture]
    public class FileMappingTest
    {
        private FileMapping _fileMapping;

        [SetUp]
        public void Init()
        {

        }

        [TestCase]
        public void FileMapping_GetFieldValue()
        {
            _fileMapping = new FileMapping()
            {
                Fields = new[]
                {
                    new Field() {Name = "OrgGroupExtId", Start = 0},
                    new Field() {Name = "OrgGroupName", Start = 1},
                    new Field() {Name = "StartDate", Start = 2, Format = "yyyy-MM-dd"},
                    new Field() {Name = "ParentOrgGroupExtId", Start = 3,},
                    new Field() {Name = "ParentOrgGroupName", Start = 4,},
                    new Field() {Name = "CategoryName", Start = 5,}
                },
            };

            string[] values = { "ORG12", "OrgGroup 1.2", "2018-09-28", "ORG1", "Parent OrgGroup", "CAT123" };

            Assert.AreEqual("ORG12", _fileMapping.GetFieldValue(values, "OrgGroupExtId"));
            Assert.AreEqual("OrgGroup 1.2", _fileMapping.GetFieldValue(values, "OrgGroupName"));
            Assert.AreEqual("2018-09-28", _fileMapping.GetFieldValue(values, "StartDate"));
            Assert.AreEqual("ORG1", _fileMapping.GetFieldValue(values, "ParentOrgGroupExtId"));
            Assert.AreEqual("Parent OrgGroup", _fileMapping.GetFieldValue(values, "ParentOrgGroupName"));
            Assert.AreEqual("CAT123", _fileMapping.GetFieldValue(values, "CategoryName"));
        }

        [TestCase]
        public void FileMapping_GetFieldValue_WithMerge()
        {
            _fileMapping = new FileMapping()
            {
                Fields = new[]
                {
                    new Field() {Name = "OrgGroupExtId", Start = 0, Merge = "{0} {2}"},
                    new Field() {Name = "OrgGroupName", Start = 1},
                    new Field() {Name = "StartDate", Start = 2, Format = "yyyy-MM-dd"},
                    new Field() {Name = "ParentOrgGroupExtId", Start = 3,},
                    new Field() {Name = "ParentOrgGroupName", Start = 4,},
                    new Field() {Name = "CategoryName", Start = 5,},
                    new Field() {Name = "CategoryExtId", Start = -1, Merge = "CAT_{3}"}
                },
            };

            string[] values = { "ORG12", "OrgGroup 1.2", "2018-09-28", "ORG1", "Parent OrgGroup", "CAT123" };

            Assert.AreEqual("ORG12 2018-09-28", _fileMapping.GetFieldValue(values, "OrgGroupExtId"));
            Assert.AreEqual("CAT_ORG1", _fileMapping.GetFieldValue(values, "CategoryExtId"));
        }

        [TestCase]
        public void FileMapping_GetFieldValueDateTime()
        {
            _fileMapping = new FileMapping()
            {
                Fields = new[]
                {
                    new Field() {Name = "OrgGroupExtId", Start = 0, Merge = "{0} {2}"},
                    new Field() {Name = "DateInOtherFormat", Start = 1, Format = "yyyy-MM-dd"},
                    new Field() {Name = "CorrectDate", Start = 2, Format = "yyyy-MM-dd"},
                    new Field() {Name = "StartDateTime", Start = 3, Merge = "{3} {4}", Format = "yyyy-MM-dd HH:mm"},
                    new Field() {Name = "StartTime", Start = 4,},
                    new Field() {Name = "DateFormatNotExists", Start = 5, Format = "lblba"},
                    new Field() {Name = "IncorrectDate", Start = -1, Format = "yyyy-MM-dd"}
                },
            };

            string[] values = { "ORG12", "2018/09/05", "2018-09-28", "2018-08-08", "08:00", "CAT123" };

            Assert.AreEqual(new DateTime(2018, 9, 28), _fileMapping.GetFieldValueDateTime(values, "CorrectDate"));
            Assert.AreEqual(DateTime.MinValue, _fileMapping.GetFieldValueDateTime(values, "IncorrectDate"));
            Assert.AreEqual(DateTime.MinValue, _fileMapping.GetFieldValueDateTime(values, "DateInOtherFormat"), "Date value is in other format");
            Assert.AreEqual(DateTime.MinValue, _fileMapping.GetFieldValueDateTime(values, "DateFormatNotExists"), "Date format not exists");
            Assert.AreEqual(new DateTime(2018, 8, 8, 8, 0, 0), _fileMapping.GetFieldValueDateTime(values, "StartDateTime"), "Date value is merged before");

        }

        [TestCase]
        public void FileMapping_GetFieldValueInteger()
        {
            _fileMapping = new FileMapping()
            {
                Fields = new[]
                {
                    new Field() {Name = "CorrectInteger", Start = 0},
                    new Field() {Name = "IncorrectInteger", Start = 1},
                    new Field() {Name = "DateAsInteger", Start = 2, Format = "yyyy-MM-dd"},
                    new Field() {Name = "IntegerValueMerged", Start = 3, Merge = "{3}{4}"},
                    new Field() {Name = "toMerge", Start = 4,},
                    new Field() {Name = "MergeIsFixedValue", Start = 5, Merge = "115"},
                    new Field() {Name = "NotInFile", Start = -1, Merge = "CAT_{3}"}
                },
            };

            string[] values = { "15", "blbaal", "2018/09/05", "12", "500", "CAT123" };

            Assert.AreEqual(15, _fileMapping.GetFieldValueInteger(values, "CorrectInteger"));
            Assert.AreEqual(0, _fileMapping.GetFieldValueInteger(values, "IncorrectInteger"));
            Assert.AreEqual(0, _fileMapping.GetFieldValueInteger(values, "DateAsInteger"));
            Assert.AreEqual(0, _fileMapping.GetFieldValueInteger(values, "NotInFile"), "record is not in file");
            Assert.AreEqual(12500, _fileMapping.GetFieldValueInteger(values, "IntegerValueMerged"), "integer value is merged before");
            Assert.AreEqual(115, _fileMapping.GetFieldValueInteger(values, "MergeIsFixedValue"), "integer value is merged before with fixed value");
        }

        [TestCase]
        public void FileMapping_GetFieldValue_WithIncorrectMergeFormat_ReturnsStringEmpty()
        {
            _fileMapping = new FileMapping()
            {
                Fields = new[]
                {
                    new Field() {Name = "OrgGroupExtId", Start = 0, Merge = "{0} {33}"},
                    new Field() {Name = "OrgGroupName", Start = 1},
                    new Field() {Name = "StartDate", Start = 2, Format = "yyyy-MM-dd"},
                    new Field() {Name = "ParentOrgGroupExtId", Start = 3,},
                    new Field() {Name = "ParentOrgGroupName", Start = 4,},
                    new Field() {Name = "CategoryName", Start = 5,},
                    new Field() {Name = "CategoryExtId", Start = -1, Merge = "CAT_{33}"}
                },
            };

            string[] values = { "ORG12", "OrgGroup 1.2", "2018-09-28", "ORG1", "Parent OrgGroup", "CAT123" };

            Assert.AreEqual(string.Empty, _fileMapping.GetFieldValue(values, "OrgGroupExtId"));
            Assert.AreEqual(string.Empty, _fileMapping.GetFieldValue(values, "CategoryExtId"));
        }

        [TestCase]
        public void FileMapping_GetFieldDefaultValue()
        {
            _fileMapping = new FileMapping()
            {
                Fields = new[]
                {
                    new Field() {Name = "OrgGroupId", Start = 0, Default = "blbla"},
                    new Field() {Name = "OrgGroupExtId", Start = 1},
                    new Field() {Name = "OrgGroupName", Start = 2},
                    new Field() {Name = "StartDate", Start = 3},
                    new Field() {Name = "ParentOrgGroupId", Start = 4, Default = "32"},
                    new Field() {Name = "ParentOrgGroupExtId", Start = 5},
                    new Field() {Name = "ParentOrgGroupName", Start = 6},
                    new Field() {Name = "CategoryName", Start = 7},
                    new Field() {Name = "CategoryId", Start = -1, Default = "15"}
                },
            };

            Assert.AreEqual(32, _fileMapping.GetFieldDefaultValue("ParentOrgGroupId"), "Correct integer value - record in fileLayout");
            Assert.AreEqual(15, _fileMapping.GetFieldDefaultValue("CategoryId"), "Correct integer value - record not in fileLayout");
            Assert.AreEqual(0, _fileMapping.GetFieldDefaultValue("OrgGroupId"), "isn't integer value, result is 0");
            Assert.AreEqual(0, _fileMapping.GetFieldDefaultValue("OrgGroupExtId"), "No default value, result is 0");
        }

        [TestCase]
        public void FileMapping_GetFieldIdentifier()
        {
            _fileMapping = new FileMapping()
            {
                Fields = new[]
                {
                    new Field() {Name = "OrgGroupId", Start = 0},
                    new Field() {Name = "OrgGroupExtId", Start = 1, Identifier = "Code"},
                    new Field() {Name = "EmployeeExtId", Start = 2, Identifier = "RegisterNumber"},
                    new Field() {Name = "StartDate", Start = 3},
                    new Field() {Name = "ParentOrgGroupId", Start = 4, Default = "32"},
                    new Field() {Name = "ParentOrgGroupExtId", Start = 5},
                    new Field() {Name = "ParentOrgGroupName", Start = 6},
                    new Field() {Name = "CategoryName", Start = 7},
                    new Field() {Name = "CategoryId", Start = -1, Default = "15"}
                },
            };

            Assert.AreEqual("Code", _fileMapping.GetFieldIdentifier("OrgGroupExtId"));
            Assert.AreEqual("RegisterNumber", _fileMapping.GetFieldIdentifier("EmployeeExtId"));
            Assert.AreEqual(string.Empty, _fileMapping.GetFieldIdentifier("OrgGroupId"));
        }

        [TestCase]
        public void FileMapping_MaxFieldsInRecord_AllRecordsInFile()
        {
            _fileMapping = new FileMapping()
            {
                Fields = new[]
                {
                    new Field() {Name = "OrgGroupId", Start = 0},
                    new Field() {Name = "OrgGroupExtId", Start = 1, Identifier = "Code"},
                    new Field() {Name = "EmployeeExtId", Start = 2, Identifier = "RegisterNumber"},
                    new Field() {Name = "StartDate", Start = 3},
                    new Field() {Name = "ParentOrgGroupId", Start = 4, Default = "32"},
                    new Field() {Name = "ParentOrgGroupExtId", Start = 5},
                    new Field() {Name = "ParentOrgGroupName", Start = 6},
                    new Field() {Name = "CategoryName", Start = 7},
                },
            };

            Assert.AreEqual(8, _fileMapping.MaxFieldsInRecord());
        }

        [TestCase]
        public void FileMapping_MaxFieldsInRecord_SomeRecordsAreNotInFile()
        {
            _fileMapping = new FileMapping()
            {
                Fields = new[]
                {
                    new Field() {Name = "OrgGroupId", Start = 0},
                    new Field() {Name = "OrgGroupExtId", Start = 1, Identifier = "Code"},
                    new Field() {Name = "EmployeeExtId", Start = 2, Identifier = "RegisterNumber"},
                    new Field() {Name = "StartDate", Start = 3},
                    new Field() {Name = "ParentOrgGroupExtId", Start = 4},
                    new Field() {Name = "ParentOrgGroupName", Start = 5},
                    new Field() {Name = "CategoryExtId", Start = 6},
                    new Field() {Name = "CategoryName", Start = 7},
                    new Field() {Name = "CategoryId", Start = -1, Default = "15"},
                    new Field() {Name = "ParentOrgGroupId", Start = -1, Default = "32"}
                },
            };

            Assert.AreEqual(8, _fileMapping.MaxFieldsInRecord(), "10 records in mapping, but just 8 in file");
        }

        [TestCase]
        public void FileMapping_GetFieldValueWithFormat()
        {
            _fileMapping = new FileMapping()
            {
                Fields = new[] { new Field() {Name = "DurationValue", Start = 0, Format = "HH.MM"} }
            };

            Assert.AreEqual(0, _fileMapping.GetFieldValueDuration(new []{"-1"}, "DurationValue"));
            Assert.AreEqual(0, _fileMapping.GetFieldValueDuration(new []{"00.00"}, "DurationValue"));
            Assert.AreEqual(480, _fileMapping.GetFieldValueDuration(new []{"08.00"}, "DurationValue"));
            Assert.AreEqual(-60, _fileMapping.GetFieldValueDuration(new []{"-1.00"}, "DurationValue"));
            Assert.AreEqual(-1, _fileMapping.GetFieldValueDuration(new[] {"0.-1"}, "DurationValue"));
        }
    }
}
