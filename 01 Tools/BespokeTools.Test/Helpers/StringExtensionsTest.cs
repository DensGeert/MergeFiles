using System;
using NUnit.Framework;
using Protime.Bespoke.Tools.Helpers;

namespace Protime.Bespoke.Tools.Test.Helpers
{
    [TestFixture]
    public class StringExtensionsTest
    {
        [Test]
        public void ParseToInt_StringIsNumber_ReturnNumber()
        {
            Assert.AreEqual(0, "0".ParseToInt());
            Assert.AreEqual(1, "1".ParseToInt());
            Assert.AreEqual(957, "957".ParseToInt());
            Assert.AreEqual(0, "-0".ParseToInt());
            Assert.AreEqual(0, "-0".ParseToInt());
        }

        [Test]
        public void ParseToInt_StringIsNotNumber_Return0()
        {
            Assert.AreEqual(0, "a".ParseToInt());
            Assert.AreEqual(0, "0-".ParseToInt());
            Assert.AreEqual(0, "nul".ParseToInt());
            Assert.AreEqual(0, "één".ParseToInt());
            Assert.AreEqual(0, "NegenVijfZeven".ParseToInt());
        }

        [Test]
        public void ParseToBool_ReturnTrue()
        {
            Assert.IsTrue("true".ParseToBool(), "true");
            Assert.IsTrue("TRUE".ParseToBool(), "TRUE");
        }

        [Test]
        public void ParseToBool_ReturnFalse()
        {
            Assert.IsFalse("false".ParseToBool(), "false");
            Assert.IsFalse("FALSE".ParseToBool(), "FALSE");
            Assert.IsFalse("0".ParseToBool(), "0");
            Assert.IsFalse("1".ParseToBool(), "1");
        }

        [Test]
        public void ParseToChar_CorrectParse_ReturnChar()
        {
            Assert.AreEqual(' ', " ".ParseToChar());
            Assert.AreEqual(';', ";".ParseToChar());
            Assert.AreEqual('|', "|".ParseToChar());
            Assert.AreEqual('.', ".".ParseToChar());
        }

        [Test]
        public void ParseToChar_InCorrectParse_ReturnDefaultSemiColon()
        {
            Assert.AreEqual(';', "abc".ParseToChar());
            Assert.AreEqual(';', "".ParseToChar());
        }

        [Test]
        public void ParseToDateTime_CorrectParse_ReturnDateTime()
        {
            Assert.AreEqual(new DateTime(2018, 1, 1), "20180101".ParseToDateTime("yyyyMMdd"), "Formaat: yyyyMMdd");
            Assert.AreEqual(new DateTime(2018, 1, 1), "2018-01-01".ParseToDateTime("yyyy-MM-dd"),
                "Formaat: yyyy-MM-dd");
            Assert.AreEqual(new DateTime(2018, 1, 1), "2018/01/01".ParseToDateTime(@"yyyy/MM/dd"),
                "Formaat: yyyy/MM/dd");
            Assert.AreEqual(new DateTime(2018, 1, 1, 16, 45, 3), "20180101 164503".ParseToDateTime("yyyyMMdd HHmmss"),
                "Formaat: yyyyMMdd HHmmss");
        }

        [Test]
        public void ParseToDateTime_InCorrectParse_ReturnDateTimeMinValue()
        {
            Assert.AreEqual(DateTime.MinValue, "2018-01-01".ParseToDateTime("yyyyMMdd"), "Formaat: yyyyMMdd");
            Assert.AreEqual(DateTime.MinValue, "20180101".ParseToDateTime("yyyy-MM-dd"), "Formaat: yyyy-MM-dd");
            Assert.AreEqual(DateTime.MinValue, "99999999".ParseToDateTime("yyyyMMdd"), "Formaat: yyyyMMdd");
        }

        [Test]
        public void ParseToDurationToMinuteInteger_ReturnInteger()
        {
            Assert.AreEqual(999, "999".ParseToDuration(""));
            Assert.AreEqual(0, "0".ParseToDuration(""));
            Assert.AreEqual(13, "13".ParseToDuration(""));
            Assert.AreEqual(999, "999".ParseToDuration("MMM"));

            Assert.AreEqual(120, "02.00".ParseToDuration("HH.MM"));
            Assert.AreEqual(120, "2.00".ParseToDuration("HH.MM"));
            Assert.AreEqual(0, "00.00".ParseToDuration("HH.MM"));
            Assert.AreEqual(0, "0.00".ParseToDuration("HH.MM"));


            Assert.AreEqual(0, "000".ParseToDuration("HH.MM"));

        }

        [Test]
        public void ParseToTimeSpan_DefaultFormat_MMM()
        {
            Assert.AreEqual(new TimeSpan(0, 999, 0), "999".ParseToTimeSpan(""), "format empty => 999");
            Assert.AreEqual(new TimeSpan(0, 0, 0), "0".ParseToTimeSpan(""), "format empty => 0");
            Assert.AreEqual(new TimeSpan(0, 13, 0), "13".ParseToTimeSpan(""), "format empty => 13");
            Assert.AreEqual(new TimeSpan(0, 999, 0), "999".ParseToTimeSpan("MMM"), "format MMM => 999");
            Assert.AreEqual(new TimeSpan(0, 0, 0), "0:50".ParseToTimeSpan(""),
                "format empty => 0:50 (incorrect format");
            Assert.AreEqual(new TimeSpan(0, 0, 0), "0.50".ParseToTimeSpan(""),
                "format empty => 0.50 (incorrect format");
            Assert.AreEqual(new TimeSpan(0, 0, 0), "XX5".ParseToTimeSpan(""), "format empty => XX5 (incorrect value");
            Assert.AreEqual(new TimeSpan(0, 0, 0), "0.50xx".ParseToTimeSpan(""),
                "format empty => 0.50xx (incorrect value/format");
        }

        [Test]
        public void ParseToTimeSpan_HHMM()
        {
            Assert.AreEqual(new TimeSpan(2, 0, 0), "02.00".ParseToTimeSpan("HH.MM"), "format HH.MM => 02.00");
            Assert.AreEqual(new TimeSpan(2, 0, 0), "2.00".ParseToTimeSpan("HH.MM"), "format HH.MM => 2.00");
            Assert.AreEqual(new TimeSpan(0, 0, 0), "00.00".ParseToTimeSpan("HH.MM"), "format HH.MM => 00.00");
            Assert.AreEqual(new TimeSpan(0, 0, 0), "0.00".ParseToTimeSpan("HH.MM"), "format HH.MM => 0.00");
            Assert.AreEqual(new TimeSpan(0, 15, 0), "0.15".ParseToTimeSpan("HH.MM"), "format HH.MM => 0.15");
            Assert.AreEqual(new TimeSpan(0, 50, 0), "0.50".ParseToTimeSpan("HH.MM"), "format HH.MM => 0.50");
            Assert.AreEqual(new TimeSpan(1, 15, 0), "0.75".ParseToTimeSpan("HH.MM"), "format HH.MM => 0.75");
            Assert.AreEqual(new TimeSpan(3, 30, 0), "03.30".ParseToTimeSpan("HH.MM"), "format HH.MM => 03.30");
            Assert.AreEqual(new TimeSpan(3, 30, 0), "3.30".ParseToTimeSpan("HH.MM"), "format HH.MM => 3.30");
            Assert.AreEqual(new TimeSpan(14, 15, 0), "14.15".ParseToTimeSpan("HH.MM"), "format HH.MM => 14.15");
            Assert.AreEqual(new TimeSpan(0, 0, 0), "0:50".ParseToTimeSpan("HH.MM"),
                "format HH.MM => 0:50 (incorrect format");
            Assert.AreEqual(new TimeSpan(0, 0, 0), "0.50xx".ParseToTimeSpan("HH.MM"),
                "format HH.MM => 0.50xx (incorrect value");

            Assert.AreEqual(new TimeSpan(2, 0, 0), "02:00".ParseToTimeSpan("HH:MM"), "format HH:MM => 02.00");
            Assert.AreEqual(new TimeSpan(2, 0, 0), "2:00".ParseToTimeSpan("HH:MM"), "format HH:MM => 2.00");
            Assert.AreEqual(new TimeSpan(0, 0, 0), "00:00".ParseToTimeSpan("HH:MM"), "format HH:MM => 00.00");
            Assert.AreEqual(new TimeSpan(0, 0, 0), "0:00".ParseToTimeSpan("HH:MM"), "format HH:MM => 0.00");
            Assert.AreEqual(new TimeSpan(0, 15, 0), "0:15".ParseToTimeSpan("HH:MM"), "format HH:MM => 0.15");
            Assert.AreEqual(new TimeSpan(0, 50, 0), "0:50".ParseToTimeSpan("HH:MM"), "format HH:MM => 0.50");
            Assert.AreEqual(new TimeSpan(1, 15, 0), "0:75".ParseToTimeSpan("HH:MM"), "format HH:MM => 0.75");
            Assert.AreEqual(new TimeSpan(3, 30, 0), "03:30".ParseToTimeSpan("HH:MM"), "format HH:MM => 03.30");
            Assert.AreEqual(new TimeSpan(3, 30, 0), "3:30".ParseToTimeSpan("HH:MM"), "format HH:MM => 3.30");
            Assert.AreEqual(new TimeSpan(14, 15, 0), "14:15".ParseToTimeSpan("HH:MM"), "format HH:MM => 14.15");
            Assert.AreEqual(new TimeSpan(0, 0, 0), "0.50".ParseToTimeSpan("HH:MM"),
                "format HH:MM => 0.50 (incorrect format");
        }

        [Test]
        public void ParseToTimeSpan_HHCC()
        {
            Assert.AreEqual(new TimeSpan(2, 0, 0), "02.00".ParseToTimeSpan("HH.CC"), "format HH.CC => 02.00");
            Assert.AreEqual(new TimeSpan(2, 0, 0), "2.00".ParseToTimeSpan("HH.CC"), "format HH.CC => 2.00");
            Assert.AreEqual(new TimeSpan(0, 0, 0), "00.00".ParseToTimeSpan("HH.CC"), "format HH.CC => 00.00");
            Assert.AreEqual(new TimeSpan(0, 0, 0), "0.00".ParseToTimeSpan("HH.CC"), "format HH.CC => 0.00");
            Assert.AreEqual(new TimeSpan(0, 36, 0), "0.60".ParseToTimeSpan("HH.CC"), "format HH.CC => 0.60");
            Assert.AreEqual(new TimeSpan(0, 30, 0), "0.50".ParseToTimeSpan("HH.CC"), "format HH.CC => 0.50");
            Assert.AreEqual(new TimeSpan(0, 45, 0), "0.75".ParseToTimeSpan("HH.CC"), "format HH.CC => 0.75");
            Assert.AreEqual(new TimeSpan(0, 1, 0), "0.01".ParseToTimeSpan("HH.CC"), "format HH.CC => 0.01 => 1min");
            Assert.AreEqual(new TimeSpan(0, 1, 0), "0.02".ParseToTimeSpan("HH.CC"), "format HH.CC => 0.02 => 1min");
            Assert.AreEqual(new TimeSpan(0, 2, 0), "0.03".ParseToTimeSpan("HH.CC"), "format HH.CC => 0.03 => 2min");
            Assert.AreEqual(new TimeSpan(0, 2, 0), "0.04".ParseToTimeSpan("HH.CC"), "format HH.CC => 0.04 => 2min");
            Assert.AreEqual(new TimeSpan(0, 3, 0), "0.05".ParseToTimeSpan("HH.CC"), "format HH.CC => 0.05 => 3min");
            Assert.AreEqual(new TimeSpan(3, 18, 0), "3.30".ParseToTimeSpan("HH.CC"), "format HH.CC => 3.30 => 3u18m");
            Assert.AreEqual(new TimeSpan(7, 36, 0), "07.60".ParseToTimeSpan("HH.CC"), "format HH.CC => 07.60 => 7u36m");
            Assert.AreEqual(new TimeSpan(7, 36, 0), "7.60".ParseToTimeSpan("HH.CC"), "format HH.CC => 7.60 => 7u36m");
            Assert.AreEqual(new TimeSpan(7, 36, 0), "7.6".ParseToTimeSpan("HH.CC"), "format HH.CC => 7.6");
            Assert.AreEqual(new TimeSpan(0, 0, 0), "0:50".ParseToTimeSpan("HH.CC"),
                "format HH.CC => 0:50 (incorrect format");
            Assert.AreEqual(new TimeSpan(0, 0, 0), "0.50xx".ParseToTimeSpan("HH.CC"),
                "format HH.CC => 0.50xx (incorrect value");

            Assert.AreEqual(new TimeSpan(2, 0, 0), "02:00".ParseToTimeSpan("HH:CC"), "format HH:CC => 02:00");
            Assert.AreEqual(new TimeSpan(2, 0, 0), "2:00".ParseToTimeSpan("HH:CC"), "format HH:CC => 2:00");
            Assert.AreEqual(new TimeSpan(0, 0, 0), "00:00".ParseToTimeSpan("HH:CC"), "format HH:CC => 00:00");
            Assert.AreEqual(new TimeSpan(0, 0, 0), "0:00".ParseToTimeSpan("HH:CC"), "format HH:CC => 0:00");
            Assert.AreEqual(new TimeSpan(0, 36, 0), "0:60".ParseToTimeSpan("HH:CC"), "format HH:CC => 0:60");
            Assert.AreEqual(new TimeSpan(0, 30, 0), "0:50".ParseToTimeSpan("HH:CC"), "format HH:CC => 0:50");
            Assert.AreEqual(new TimeSpan(0, 45, 0), "0:75".ParseToTimeSpan("HH:CC"), "format HH:CC => 0:75");
            Assert.AreEqual(new TimeSpan(0, 1, 0), "0:01".ParseToTimeSpan("HH:CC"), "format HH:CC => 0:01 => 1min");
            Assert.AreEqual(new TimeSpan(0, 1, 0), "0:02".ParseToTimeSpan("HH:CC"), "format HH:CC => 0:02 => 1min");
            Assert.AreEqual(new TimeSpan(0, 2, 0), "0:03".ParseToTimeSpan("HH:CC"), "format HH:CC => 0:03 => 2min");
            Assert.AreEqual(new TimeSpan(0, 2, 0), "0:04".ParseToTimeSpan("HH:CC"), "format HH:CC => 0:04 => 2min");
            Assert.AreEqual(new TimeSpan(0, 3, 0), "0:05".ParseToTimeSpan("HH:CC"), "format HH:CC => 0:05 => 3min");
            Assert.AreEqual(new TimeSpan(3, 18, 0), "3:30".ParseToTimeSpan("HH:CC"), "format HH:CC => 3:30 => 3u18m");
            Assert.AreEqual(new TimeSpan(7, 36, 0), "07:60".ParseToTimeSpan("HH:CC"), "format HH:CC => 07:60 => 7u36m");
            Assert.AreEqual(new TimeSpan(7, 36, 0), "7:60".ParseToTimeSpan("HH:CC"), "format HH:CC => 7:60 => 7u36m");
            Assert.AreEqual(new TimeSpan(7, 36, 0), "7:6".ParseToTimeSpan("HH:CC"), "format HH:CC => 7:6 => 7u36m");
            Assert.AreEqual(new TimeSpan(7, 4, 0), "7:06".ParseToTimeSpan("HH:CC"), "format HH:CC => 7:06 => 7u04m");
            Assert.AreEqual(new TimeSpan(0, 0, 0), "0.50".ParseToTimeSpan("HH:CC"),
                "format HH:CC => 0.50 (incorrect format");
            Assert.AreEqual(new TimeSpan(0, 0, 0), "0:50xx".ParseToTimeSpan("HH:CC"),
                "format HH:CC => 0:50xx (incorrect value");
        }

        [Test]
        public void ParseToDouble_Return0()
        {
            double number = 0;
            Assert.AreEqual(0, "0".ParseToDouble("0.00"));
            Assert.AreEqual(0, "0".ParseToDouble("0,00"));
            Assert.AreEqual(0, "0.00".ParseToDouble("0.00"));
            Assert.AreEqual(0, "0,00".ParseToDouble("0,00"));

            number = 12.45;
            Assert.AreEqual(number, "12.45".ParseToDouble("0.00"));
            Assert.AreEqual(number, "12,45".ParseToDouble("0,00"));

            number = 12.45;
            Assert.AreEqual(number, "12.45".ParseToDouble("€ 0.00"));
            Assert.AreEqual(number, "12,45".ParseToDouble("€ 0,00"));
        }
    }
}
