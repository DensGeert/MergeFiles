using NUnit.Framework;
using Protime.Bespoke.Tools.FileConfiguration;

namespace Protime.Bespoke.Tools.Test.FileConfiguration
{
    [TestFixture]
    class FileParserTest
    {
        [Test]
        public void FileParserFactory_Create_CsvFileParser()
        {
            var fileParser = FileParserFactory.Create(FileType.TextDelimited);

            Assert.AreEqual(typeof(CsvFileParser), fileParser.GetType());
        }

        [Test]
        public void FileParserFactory_Create_TextFixedPositionParser()
        {
            var fileParser = FileParserFactory.Create(FileType.TextFixedPositions);

            Assert.AreEqual(typeof(TextFixedPositionFileParser), fileParser.GetType());
        }

        [Test]
        public void FileParserFactory_Create_NoTypeDefined_ReturnNull()
        {
            var fileParser = FileParserFactory.Create(FileType.NoTypeDefined);

            Assert.IsNull(fileParser);
        }

        [Test]
        public void CsvParser_ReadLines()
        {
            FileLayout layout = new FileLayout()
            {
                FileType = FileType.TextDelimited,
                Delimiter = ';',
                FileMapping = new FileMapping()
                {
                    Fields = new[]
                    {
                        new Field() {Name = "Name", Start = 0},
                        new Field() {Name = "Value", Start = 1},
                        new Field() {Name = "Date", Start = 2},
                    }
                }
            };

            var fileRecords = new[] { "Item;500;20180101", "abc;123;000" };

            var records = new CsvFileParser().ReadRecords(fileRecords, layout);

            Assert.AreEqual(2, records.Count);
            Assert.AreEqual(3, records[0].Length);

            string[] fieldsOfRecord1 = records[0];
            Assert.AreEqual("Item", fieldsOfRecord1[0]);
            Assert.AreEqual("500", fieldsOfRecord1[1]);
            Assert.AreEqual("20180101", fieldsOfRecord1[2]);

            string[] fieldsOfRecord2 = records[1];
            Assert.AreEqual("abc", fieldsOfRecord2[0]);
            Assert.AreEqual("123", fieldsOfRecord2[1]);
            Assert.AreEqual("000", fieldsOfRecord2[2]);
        }

        [Test]
        public void CsvParser_ReadLines_TrimFields()
        {
            FileLayout layout = new FileLayout()
            {
                FileType = FileType.TextDelimited,
                Delimiter = ';',
                TrimFields = true,
                FileMapping = new FileMapping()
                {
                    Fields = new[]
                    {
                        new Field() {Name = "Name", Start = 0},
                        new Field() {Name = "Value", Start = 1},
                        new Field() {Name = "Date", Start = 2},
                    }
                }
            };

            var fileRecords = new[] { "Item     ;500     ;20180101     ", "abc;123      ;000    " };

            var records = new CsvFileParser().ReadRecords(fileRecords, layout);

            Assert.AreEqual(2, records.Count);
            Assert.AreEqual(3, records[0].Length);

            string[] fieldsOfRecord1 = records[0];
            Assert.AreEqual("Item", fieldsOfRecord1[0]);
            Assert.AreEqual("500", fieldsOfRecord1[1]);
            Assert.AreEqual("20180101", fieldsOfRecord1[2]);

            string[] fieldsOfRecord2 = records[1];
            Assert.AreEqual("abc", fieldsOfRecord2[0]);
            Assert.AreEqual("123", fieldsOfRecord2[1]);
            Assert.AreEqual("000", fieldsOfRecord2[2]);
        }

        [Test]
        public void CsvParser_ReadLines_HasHeader()
        {
            FileLayout layout = new FileLayout()
            {
                FileType = FileType.TextDelimited,
                Delimiter = ';',
                HasHeader = true,
                FileMapping = new FileMapping()
                {
                    Fields = new[]
                    {
                        new Field() {Name = "Name", Start = 0},
                        new Field() {Name = "Value", Start = 1},
                        new Field() {Name = "Date", Start = 2},
                    }
                }
            };

            var fileRecords = new[] { "ITEM-ID;VALUE;DATE", "Item;500;20180101", "abc;123;000" };

            var records = new CsvFileParser().ReadRecords(fileRecords, layout);

            Assert.AreEqual(2, records.Count);
            Assert.AreEqual(3, records[0].Length);

            string[] fieldsOfRecord1 = records[0];
            Assert.AreEqual("Item", fieldsOfRecord1[0]);
            Assert.AreEqual("500", fieldsOfRecord1[1]);
            Assert.AreEqual("20180101", fieldsOfRecord1[2]);

            string[] fieldsOfRecord2 = records[1];
            Assert.AreEqual("abc", fieldsOfRecord2[0]);
            Assert.AreEqual("123", fieldsOfRecord2[1]);
            Assert.AreEqual("000", fieldsOfRecord2[2]);
        }

        [Test]
        public void TextFixedPositionsParser_ReadLines()
        {
            FileLayout layout = new FileLayout()
            {
                FileType = FileType.TextFixedPositions,
                FileMapping = new FileMapping()
                {
                    Fields = new[]
                    {
                        new Field() {Name = "Name", Start = 0, Length = 5},
                        new Field() {Name = "Value", Start = 5, Length = 3},
                        new Field() {Name = "Date", Start = 8, Length = 8},
                    }
                }
            };

            var fileRecords = new[] { "Item 50020180101", "abc  12300000000" };

            var records = new TextFixedPositionFileParser().ReadRecords(fileRecords, layout);

            Assert.AreEqual(2, records.Count);
            Assert.AreEqual(3, records[0].Length);

            string[] fieldsOfRecord1 = records[0];
            Assert.AreEqual("Item ", fieldsOfRecord1[0]);
            Assert.AreEqual("500", fieldsOfRecord1[1]);
            Assert.AreEqual("20180101", fieldsOfRecord1[2]);

            string[] fieldsOfRecord2 = records[1];
            Assert.AreEqual("abc  ", fieldsOfRecord2[0]);
            Assert.AreEqual("123", fieldsOfRecord2[1]);
            Assert.AreEqual("00000000", fieldsOfRecord2[2]);
        }

        [Test]
        public void TextFixedPositionsParser_ReadLines_TrimFields()
        {
            FileLayout layout = new FileLayout()
            {
                FileType = FileType.TextFixedPositions,
                TrimFields = true,
                FileMapping = new FileMapping()
                {
                    Fields = new[]
                    {
                        new Field() {Name = "Name", Start = 0, Length = 5},
                        new Field() {Name = "Value", Start = 5, Length = 5},
                        new Field() {Name = "Date", Start = 10, Length = 8},
                    }
                }
            };

            var fileRecords = new[] { "Item 500  20180101", "abc  123  00000000" };

            var records = new TextFixedPositionFileParser().ReadRecords(fileRecords, layout);

            Assert.AreEqual(2, records.Count);
            Assert.AreEqual(3, records[0].Length);

            string[] fieldsOfRecord1 = records[0];
            Assert.AreEqual("Item", fieldsOfRecord1[0]);
            Assert.AreEqual("500", fieldsOfRecord1[1]);
            Assert.AreEqual("20180101", fieldsOfRecord1[2]);

            string[] fieldsOfRecord2 = records[1];
            Assert.AreEqual("abc", fieldsOfRecord2[0]);
            Assert.AreEqual("123", fieldsOfRecord2[1]);
            Assert.AreEqual("00000000", fieldsOfRecord2[2]);
        }

        [Test]
        public void TextFixedPositionsParser_ReadLines_HasHeader()
        {
            FileLayout layout = new FileLayout()
            {
                FileType = FileType.TextFixedPositions,
                HasHeader = true,
                FileMapping = new FileMapping()
                {
                    Fields = new[]
                    {
                        new Field() {Name = "Name", Start = 0, Length = 5},
                        new Field() {Name = "Value", Start = 5, Length = 5},
                        new Field() {Name = "Date", Start = 10, Length = 8},
                    }
                }
            };

            var fileRecords = new[] { "ITEM VAL  DATE", "Item 500  20180101", "abc  123  00000000" };

            var records = new TextFixedPositionFileParser().ReadRecords(fileRecords, layout);

            Assert.AreEqual(2, records.Count);
            Assert.AreEqual(3, records[0].Length);

            string[] fieldsOfRecord1 = records[0];
            Assert.AreEqual("Item ", fieldsOfRecord1[0]);
            Assert.AreEqual("500  ", fieldsOfRecord1[1]);
            Assert.AreEqual("20180101", fieldsOfRecord1[2]);

            string[] fieldsOfRecord2 = records[1];
            Assert.AreEqual("abc  ", fieldsOfRecord2[0]);
            Assert.AreEqual("123  ", fieldsOfRecord2[1]);
            Assert.AreEqual("00000000", fieldsOfRecord2[2]);
        }

        [Test]
        public void TextFixedPositionsParser_ReadLines_IgnoreDefaultFields()
        {
            FileLayout layout = new FileLayout()
            {
                FileType = FileType.TextFixedPositions,
                FileMapping = new FileMapping()
                {
                    Fields = new[]
                    {
                        new Field() {Name = "Name", Start = 0, Length = 5},
                        new Field() {Name = "Value", Start = 5, Length = 3},
                        new Field() {Name = "Date", Start = 8, Length = 8},
                        new Field() {Name = "ThisIsOnlyDefault", Start = -1, Default = "123"},
                        new Field() {Name = "ThisIsOnlyDefault2", Start = -1, Default = "Default1"},
                    }
                }
            };

            var fileRecords = new[] { "Item 50020180101", "abc  12300000000" };

            var records = new TextFixedPositionFileParser().ReadRecords(fileRecords, layout);

            Assert.AreEqual(2, records.Count);
            Assert.AreEqual(3, records[0].Length);

            string[] fieldsOfRecord1 = records[0];
            Assert.AreEqual("Item ", fieldsOfRecord1[0]);
            Assert.AreEqual("500", fieldsOfRecord1[1]);
            Assert.AreEqual("20180101", fieldsOfRecord1[2]);

            string[] fieldsOfRecord2 = records[1];
            Assert.AreEqual("abc  ", fieldsOfRecord2[0]);
            Assert.AreEqual("123", fieldsOfRecord2[1]);
            Assert.AreEqual("00000000", fieldsOfRecord2[2]);
        }
    }
}
